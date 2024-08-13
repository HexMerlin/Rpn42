using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UIElements;


public class OperationController 
{
   
    private List<NumberEntry> outputEntries;

    private StringBuilder inputBuf = new StringBuilder();

    public CalcButtons CalcButtons;

    private Change CurrentChange = Change.CreateStart();

    public IReadOnlyList<NumberEntry> OutputEntries => outputEntries;

    private Format _numberFormat = Format.Normal;

    public Format NumberFormat
    {

        get => _numberFormat;
        private set
        {
            this._numberFormat = value;
            foreach ((CalcButton button, Format buttonFormat) in CalcButtons.ModeButtons)
                button.SetSelected(buttonFormat == value);
        }
    }

  
    public OperationController(VisualElement buttonGrid)
    {
        this.outputEntries = new List<NumberEntry>();
        this.CalcButtons = new CalcButtons(buttonGrid);
        this.NumberFormat = Format.Normal;

    }
    public NumberEntry this[int index] => this.outputEntries[index];

    public int OutputCount => this.outputEntries.Count;

    private bool OutputEmpty => this.outputEntries.Count == 0;

    public string Input => this.inputBuf.ToString();

    private bool InputEmpty => this.inputBuf.Length == 0;
     
    private NumberEntry LastOutput => this.outputEntries[^1];

    private NumberEntry SecondLastOutput => this.outputEntries[^2];

    public void ReadFrom(SavedData savedData)
    {
        this.outputEntries.Clear();
        this.outputEntries.AddRange(savedData.numberEntries);
        this.inputBuf.Clear();
        this.inputBuf.Append(savedData.input);
    }

    public void WriteTo(SavedData savedData)
    {
        savedData.numberEntries = this.OutputEntries.ToArray();
        savedData.input = this.Input;
    }

    public void AddOutput(NumberEntry numberEntry, bool isUndoPoint)
    {
        this.CurrentChange = this.CurrentChange.AddOutput(numberEntry, outputEntries);
        this.CurrentChange.IsUndoPoint = isUndoPoint;
    }

    public void InputButtonPressed(CalcButton calcButton)
    {
        if (CalcButtons.IsNumberFormatButton(calcButton) is Format numberFormat)
        {
            NumberFormat = numberFormat;
            return;

        }
       
        switch (calcButton.Name)
        {
            case CalcButtons.Zero:
            case CalcButtons.One:
            case CalcButtons.Two:
            case CalcButtons.Three:
            case CalcButtons.Four:
            case CalcButtons.Five:
            case CalcButtons.Six:
            case CalcButtons.Seven:
            case CalcButtons.Eight:
            case CalcButtons.Nine:
                string digit = calcButton.UnityButton.text; //note: text on digit buttons maps verbatim to input strings
                this.CurrentChange = this.CurrentChange.AddInput(digit, inputBuf); 
       
                break;

            case CalcButtons.Enter:

                if (InputEmpty)
                {
                    if (OutputEmpty)
                        return;
                    else
                        this.CurrentChange = this.CurrentChange.AddOutput(LastOutput, outputEntries); //add a copy of last output
                }
                else
                    PerformUnaryOperation((a) => a);

                break;
            case CalcButtons.BackDrop:
                if (InputEmpty)
                {
                    if (OutputEmpty) return;
                    this.CurrentChange = this.CurrentChange.RemoveOutput(outputEntries);
                }
                else
                {
                    this.CurrentChange = this.CurrentChange.RemoveInputChar(inputBuf);
                }
                break;
            case CalcButtons.Copy2:
                if (OutputCount < 2) return;
                NumberEntry secondLastOutput = this.SecondLastOutput;
                NumberEntry lastOutput = this.LastOutput;
                this.CurrentChange = this.CurrentChange.AddOutput(secondLastOutput, outputEntries);
                this.CurrentChange = this.CurrentChange.AddOutput(lastOutput, outputEntries);
                break;
            case CalcButtons.Neg:
                PerformUnaryOperation((a) => -a);
                break;
            case CalcButtons.Reciprocal:
                PerformUnaryOperation((a) => a.Reciprocal);
                break;
            case CalcButtons.Square:
                PerformUnaryOperation((a) => a * a);
                break;
            case CalcButtons.Sum:
                PerformBinaryOperation((a, b) => a + b);
                break;
            case CalcButtons.Diff:
                PerformBinaryOperation((a, b) => a - b);
                break;
            case CalcButtons.Prod:
                PerformBinaryOperation((a, b) => a * b);
                break;
            case CalcButtons.Quotient:
                PerformBinaryOperation((a, b) => a / b);
                break;
            case CalcButtons.Clear:
                if (!InputEmpty)
                    this.CurrentChange = this.CurrentChange.ClearInput(inputBuf);
                if (!OutputEmpty)
                    this.CurrentChange = this.CurrentChange.ClearAllOutputs(outputEntries);
                break;
            case CalcButtons.Mod:
                PerformBinaryOperation((a, b) => a % b);
                break;
            case CalcButtons.DivOnes:
                PerformUnaryOperation((a) => a.DivideByNextMersenneNumber(mustBeCoprime: false));
                break;
            case CalcButtons.Undo:
                PerformUndoOperation();
                break;

            case CalcButtons.Redo:
                PerformRedoOperation();
                break;
            case CalcButtons.AsRepetend:
                PerformUnaryOperation((a) => a.DivideByNextMersenneNumber(mustBeCoprime: true));
                break;                
            default:
                throw new ArgumentException($"Unhandled button name: {calcButton.Name}");
            }
        this.CurrentChange.IsUndoPoint = true;
    }


    private void PerformUnaryOperation(Func<Rational, Rational> operation)
    {
        (bool success, Rational operand) = PeekOperand();
        if (!success) return; //need 1 operand to perform operation: abort operation

        Rational result = operation(operand);

        if (result.IsInvalid) return;

        this.CurrentChange = InputEmpty ?
            this.CurrentChange.ReplaceOutput(new NumberEntry(result), outputEntries) :
            this.CurrentChange.ClearInput(inputBuf).AddOutput(new NumberEntry(result), outputEntries);
    }

    private void PerformBinaryOperation(Func<Rational, Rational, Rational> operation)
    {
        (bool success, Rational leftOperand, Rational RightOperand) = PeekOperands();
        if (!success) return; //need 2 operands to perform operation: abort operation

        Rational result = operation(leftOperand, RightOperand);

        if (result.IsInvalid) return;

        this.CurrentChange = InputEmpty ?
            this.CurrentChange.RemoveOutput(outputEntries).ReplaceOutput(new NumberEntry(result), outputEntries) :
            this.CurrentChange.ClearInput(inputBuf).ReplaceOutput(new NumberEntry(result), outputEntries);
    }

    public void PerformUndoOperation()
    {
        if (this.CurrentChange is NoChange)
            return;

        while (true)
        {
            this.CurrentChange = this.CurrentChange switch
            {
                InputChange inputChange => inputChange.Rollback(inputBuf).Previous,
                OutputChange outputChange => outputChange.Rollback(outputEntries).Previous,
                _ => throw new ArgumentOutOfRangeException($"Unknown ChangeType {CurrentChange.GetType().Name}")
            };
            if (this.CurrentChange.IsUndoPoint)
                return;
        }
    }

    public void PerformRedoOperation()
    {
             
        while (true)
        {
            if (this.CurrentChange.Next is null)
                return;
            this.CurrentChange = this.CurrentChange.Next;

            this.CurrentChange = this.CurrentChange switch
            {
                InputChange inputChange => inputChange.Execute(this.inputBuf),
                OutputChange outputChange => outputChange.Execute(this.outputEntries),
                _ => throw new ArgumentOutOfRangeException($"Unknown ChangeType {this.CurrentChange.GetType().Name}")
            };
            if (this.CurrentChange.IsUndoPoint)
                return;
        }
    }

    private (bool success, Rational operand) PeekOperand()
    {
        if (OutputCount + (InputEmpty ? 0 : 1) >= 1)
        {
            Rational operand = InputEmpty ? LastOutput.Rational : new Rational(this.inputBuf.ToString());
            if (!operand.IsInvalid)
                return (true, operand);
        }
        return (false, Rational.Invalid);
    }

    private (bool success, Rational leftOperand, Rational rightOperand) PeekOperands()
    {
        if (OutputCount + (InputEmpty ? 0 : 1) >= 2)
        {
            Rational rightOperand = InputEmpty ? LastOutput.Rational : new Rational(this.inputBuf.ToString());
            if (!rightOperand.IsInvalid)
            {
                Rational leftOperand = InputEmpty ? SecondLastOutput.Rational : LastOutput.Rational;
                return (true, leftOperand, rightOperand);
            }
        }
        return (false, Rational.Invalid, Rational.Invalid);
    }



}
