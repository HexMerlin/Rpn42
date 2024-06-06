using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OperationController
{
    private List<NumberEntry> outputEntries;

    private readonly StringBuilder input = new StringBuilder();

    public IReadOnlyList<NumberEntry> OutputEntries => outputEntries;

    Action<string> OnInputUpdate;
    Action OnOutputUpdate;
    
    private Change CurrentChange = Change.None.SetCheckPoint(true);

    public OperationController(Action<string> onInputUpdate, Action onOutputUpdate)
    {
        outputEntries = new List<NumberEntry>();
        OnInputUpdate = onInputUpdate;
        OnOutputUpdate = onOutputUpdate;
    }

    public NumberEntry this[int index] => outputEntries[index];

    public int OutputCount => outputEntries.Count;

    private bool OutputEmpty => outputEntries.Count == 0;
    
    private bool InputEmpty => input.Length == 0;
     
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
                CurrentChange = CurrentChange.AddInput(buttonValue, input);
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
                    CurrentChange = CurrentChange.RemoveInputChar(input);
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
                    CurrentChange = CurrentChange.ClearInput(input);
                if (!OutputEmpty)
                    CurrentChange = CurrentChange.ClearAllOutputs(outputEntries);
                break;
            case "mod":
                PerformBinaryOperation((a, b) => a % b);
                break;
            case "div-ones":
                PerformUnaryOperation((a) => a.DivideByMersenneCeiling());
                break;
            default:
                Debug.LogWarning($"Unhandled button: {buttonValue}");
                return;
        }
        CurrentChange.SetCheckPoint();
        OnInputUpdate(input.ToString());
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
            CurrentChange.ClearInput(input).AddOutput(new NumberEntry(result), outputEntries);
    }

    private void PerformBinaryOperation(Func<Rational, Rational, Rational> operation)
    {
        (bool success, Rational leftOperand, Rational RightOperand) = PeekOperands();
        if (!success) return; //need 2 operands to perform operation: abort operation

        Rational result = operation(leftOperand, RightOperand);

        if (result.IsInvalid) return;

        CurrentChange = InputEmpty ?
            CurrentChange.RemoveOutput(outputEntries).ReplaceOutput(new NumberEntry(result), outputEntries) :
            CurrentChange.ClearInput(input).ReplaceOutput(new NumberEntry(result), outputEntries);
    }



    private (bool success, Rational operand) PeekOperand()
    {
        if (OutputCount + (InputEmpty ? 0 : 1) >= 1)
        {
            Rational operand = InputEmpty ? LastOutput.Rational : new Rational(input.ToString());
            if (!operand.IsInvalid)
                return (true, operand);
        }
        return (false, Rational.Invalid);
    }

    private (bool success, Rational leftOperand, Rational rightOperand) PeekOperands()
    {
        if (OutputCount + (InputEmpty ? 0 : 1) >= 2)
        {
            Rational rightOperand = InputEmpty ? LastOutput.Rational : new Rational(input.ToString());
            if (!rightOperand.IsInvalid)
            {
                Rational leftOperand = InputEmpty ? SecondLastOutput.Rational : LastOutput.Rational;
                return (true, leftOperand, rightOperand);
            }
        }
        return (false, Rational.Invalid, Rational.Invalid);
    }
}
