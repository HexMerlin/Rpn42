using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;


public class OperationController 
{
   
    private List<NumberEntry> outputEntries;

    private StringBuilder inputBuf = new StringBuilder();

   // public ButtonCollection Buttons;

    public Change CurrentChange = Change.CreateStart();

    public IReadOnlyList<NumberEntry> OutputEntries => outputEntries;

    public Format NumberFormat { get; set; } = Format.Normal;

    public OperationController()
    {
        this.outputEntries = new List<NumberEntry>();
        this.NumberFormat = Format.Normal;

    }
    public NumberEntry this[int index] => this.outputEntries[index];

    public int OutputCount => this.outputEntries.Count;

    private bool OutputEmpty => this.outputEntries.Count == 0;

    public string Input => this.inputBuf.ToString();

    public bool InputEmpty => this.inputBuf.Length == 0;
     
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

    public void PerformAddInput(string input) => this.CurrentChange = this.CurrentChange.AddInput(input, inputBuf);

    public void PerformUnaryOperation(Func<Rational, Rational> operation, bool retainOperand = false)
    { 
        (Rational _, Rational operand) = PeekOperands();
        if (operand.IsInvalid) return; //need 1 operand to perform operation: abort operation

        Rational result = operation(operand);

        if (result.IsInvalid) return;

        if (retainOperand)
        {
            this.CurrentChange = this.CurrentChange.AddOutput(new NumberEntry(result), outputEntries);
        } 
        else 
        this.CurrentChange = InputEmpty ?
            this.CurrentChange.ReplaceOutput(new NumberEntry(result), outputEntries) :
            this.CurrentChange.ClearInput(inputBuf).AddOutput(new NumberEntry(result), outputEntries);
    }

    public void PerformBinaryOperation(Func<Rational, Rational, Rational> operation)
    {
        (Rational leftOperand, Rational rightOperand) = PeekOperands();
        if (leftOperand.IsInvalid || rightOperand.IsInvalid) return; //need 2 operands to perform operation: abort operation

        Rational result = operation(leftOperand, rightOperand);

        if (result.IsInvalid) return;

        this.CurrentChange = InputEmpty ?
            this.CurrentChange.RemoveOutput(outputEntries).ReplaceOutput(new NumberEntry(result), outputEntries) :
            this.CurrentChange.ClearInput(inputBuf).ReplaceOutput(new NumberEntry(result), outputEntries);
    }

    public void PerformBackDrop()
    {
        if (InputEmpty)
        {
            if (OutputEmpty) return;
            this.CurrentChange = this.CurrentChange.RemoveOutput(outputEntries);
        }
        else
        {
            this.CurrentChange = this.CurrentChange.RemoveInputChar(inputBuf);
        }
    }

    public void PerformClear()
    {
        if (!InputEmpty)
            this.CurrentChange = this.CurrentChange.ClearInput(inputBuf);
        if (!OutputEmpty)
            this.CurrentChange = this.CurrentChange.ClearAllOutputs(outputEntries);
    }

    public void PerformCopy2()
    {
        if (OutputCount < 2) return;
        NumberEntry secondLastOutput = this.SecondLastOutput;
        NumberEntry lastOutput = this.LastOutput;
        this.CurrentChange = this.CurrentChange.AddOutput(secondLastOutput, outputEntries);
        this.CurrentChange = this.CurrentChange.AddOutput(lastOutput, outputEntries);
    }

    public void PerformUndo()
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

    public void PerformRedo()
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


    public (Rational leftOperand, Rational rightOperand) PeekOperands()
    {
        Rational lastOutput = OutputCount > 0 ? LastOutput.Rational : Rational.Invalid;
      
        return InputEmpty ?
            (OutputCount > 1 ? SecondLastOutput.Rational : Rational.Invalid, lastOutput)
            : (lastOutput, new Rational(this.inputBuf.ToString()));
    }

}
