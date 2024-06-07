using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OperationController
{
    private List<NumberEntry> outputEntries;

    private readonly StringBuilder inputBuf = new StringBuilder();

    public IReadOnlyList<NumberEntry> OutputEntries => outputEntries;

    Action<string> OnInputUpdate;
    Action OnOutputUpdate;
    
    private Change CurrentChange = Change.CreateStart();

    public OperationController(Action<string> onInputUpdate, Action onOutputUpdate)
    {
        outputEntries = new List<NumberEntry>();
        OnInputUpdate = onInputUpdate;
        OnOutputUpdate = onOutputUpdate;
    }

    public NumberEntry this[int index] => outputEntries[index];

    public int OutputCount => outputEntries.Count;

    private bool OutputEmpty => outputEntries.Count == 0;
    
    private bool InputEmpty => inputBuf.Length == 0;
     
    private NumberEntry LastOutput => outputEntries[^1];

    private NumberEntry SecondLastOutput => outputEntries[^2];

    public void InputButtonPressed(string buttonValue)
    {
        bool outputChanged = true;

        switch (buttonValue)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                CurrentChange = CurrentChange.AddInput(buttonValue, inputBuf);
                outputChanged = false;
                break;

            case "enter":

                if (InputEmpty)
                {
                    if (OutputEmpty)
                        return;
                    else
                        CurrentChange = CurrentChange.AddOutput(LastOutput, outputEntries); //add a copy of last output
                }
                else
                    PerformUnaryOperation((a) => a);

                break;
            case "back-drop":
                if (InputEmpty)
                {
                    if (OutputEmpty) return;
                        CurrentChange = CurrentChange.RemoveOutput(outputEntries);
                }
                else
                {
                    CurrentChange = CurrentChange.RemoveInputChar(inputBuf);
                }
                break;
            case "swap":
                //implement this
                break;
            case "neg":
                PerformUnaryOperation((a) => -a);
                break;
            case "reciprocal":
                PerformUnaryOperation((a) => a.Reciprocal);
                break;
            case "square":
                PerformUnaryOperation((a) => a * a);
                break;
            case "sum":
                PerformBinaryOperation((a, b) => a + b);
                break;
            case "diff":
                PerformBinaryOperation((a, b) => a - b);
                break;
            case "prod":
                PerformBinaryOperation((a, b) => a * b);
                break;
            case "quotient":
                PerformBinaryOperation((a, b) => a / b);
                break;
            case "clear":
                if (!InputEmpty)
                    CurrentChange = CurrentChange.ClearInput(inputBuf);
                if (!OutputEmpty)
                    CurrentChange = CurrentChange.ClearAllOutputs(outputEntries);
                break;
            case "mod":
                PerformBinaryOperation((a, b) => a % b);
                break;
            case "div-ones":
                PerformUnaryOperation((a) => a.DivideByMersenneCeiling());
                break;
            case "undo":
                PerformUndoOperation();
                break;

            case "redo":
                PerformRedoOperation();
                break;
            default:
                Debug.LogWarning($"Unhandled button: {buttonValue}");
                return;
        }
        CurrentChange.IsCheckPoint = true;
        OnInputUpdate(inputBuf.ToString());
        if (outputChanged) OnOutputUpdate();
    }


    private void PerformUnaryOperation(Func<Rational, Rational> operation)
    {
        (bool success, Rational operand) = PeekOperand();
        if (!success) return; //need 1 operand to perform operation: abort operation

        Rational result = operation(operand);

        if (result.IsInvalid) return;

        CurrentChange = InputEmpty ?
            CurrentChange.ReplaceOutput(new NumberEntry(result), outputEntries) :
            CurrentChange.ClearInput(inputBuf).AddOutput(new NumberEntry(result), outputEntries);
    }

    private void PerformBinaryOperation(Func<Rational, Rational, Rational> operation)
    {
        (bool success, Rational leftOperand, Rational RightOperand) = PeekOperands();
        if (!success) return; //need 2 operands to perform operation: abort operation

        Rational result = operation(leftOperand, RightOperand);

        if (result.IsInvalid) return;

        CurrentChange = InputEmpty ?
            CurrentChange.RemoveOutput(outputEntries).ReplaceOutput(new NumberEntry(result), outputEntries) :
            CurrentChange.ClearInput(inputBuf).ReplaceOutput(new NumberEntry(result), outputEntries);
    }

    public void PerformUndoOperation()
    {
        if (CurrentChange is NoChange)
            return;

        while (true)
        {
            CurrentChange = CurrentChange switch
            {
                InputChange inputChange => inputChange.Rollback(inputBuf).Previous,
                OutputChange outputChange => outputChange.Rollback(outputEntries).Previous,
                _ => throw new ArgumentOutOfRangeException($"Unknown ChangeType {CurrentChange.GetType().Name}")
            };
            if (CurrentChange.IsCheckPoint)
                return;
        }
    }

    public void PerformRedoOperation()
    {
             
        while (true)
        {
            if (CurrentChange.Next is null)
                return;
            CurrentChange = CurrentChange.Next;

            CurrentChange = CurrentChange switch
            {
                InputChange inputChange => inputChange.Execute(inputBuf),
                OutputChange outputChange => outputChange.Execute(outputEntries),
                _ => throw new ArgumentOutOfRangeException($"Unknown ChangeType {CurrentChange.GetType().Name}")
            };
            if (CurrentChange.IsCheckPoint)
                return;
        }
    }

    private (bool success, Rational operand) PeekOperand()
    {
        if (OutputCount + (InputEmpty ? 0 : 1) >= 1)
        {
            Rational operand = InputEmpty ? LastOutput.Rational : new Rational(inputBuf.ToString());
            if (!operand.IsInvalid)
                return (true, operand);
        }
        return (false, Rational.Invalid);
    }

    private (bool success, Rational leftOperand, Rational rightOperand) PeekOperands()
    {
        if (OutputCount + (InputEmpty ? 0 : 1) >= 2)
        {
            Rational rightOperand = InputEmpty ? LastOutput.Rational : new Rational(inputBuf.ToString());
            if (!rightOperand.IsInvalid)
            {
                Rational leftOperand = InputEmpty ? SecondLastOutput.Rational : LastOutput.Rational;
                return (true, leftOperand, rightOperand);
            }
        }
        return (false, Rational.Invalid, Rational.Invalid);
    }



}
