using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLib;

public class ModelController 
{
   
    private List<NumberEntry> outputEntries;

    private readonly InputBuffer inputBuf;

    public Change CurrentChange = Change.CreateStart();

    public IReadOnlyList<NumberEntry> OutputEntries => outputEntries;

    public Format NumberFormat => new Format(NumberMode, NumberBase);

    public Mode NumberMode
    {
        get; set;
    }

    public int NumberBase
    {
        get; set;
    }

    public int InputBase
    {
        get => inputBuf.Base;
        set => inputBuf.Base = value;
    }
    //private int _numberBase;   
    //public int NumberBase
    //{
    //    get => _numberBase;
    //    set
    //    {
    //        if (value == NumberBase) return;
    //        if (!InputEmpty)
    //            PerformUnaryOperation((a) => a);
    //        if (value == 10 && NumberMode is Mode.PAdic)
    //            NumberMode = Mode.Normal;
    //        this.CurrentChange = this.CurrentChange.ChangeBase(_numberBase, value);
    //        _numberBase = value;

    //    }
    //}

    //public void PerformChangeBase(int newBase)
    //{
    //    if (newBase == NumberBase) return;
    //    if (!InputEmpty)
    //        PerformUnaryOperation((a) => a);
    //    if (newBase == 10 && NumberMode is Mode.PAdic)
    //        NumberMode = Mode.Normal;

    //    this.CurrentChange = this.CurrentChange.AddInput(input, inputBuf);
    //}

    public ModelController()
    {
        this.outputEntries = new List<NumberEntry>();
        this.NumberBase = 10;
        this.NumberMode = Mode.Normal;
        this.inputBuf = new InputBuffer(base_: 10);

    }
    public NumberEntry this[int index] => this.outputEntries[index];

    public int OutputCount => this.outputEntries.Count;

    private bool OutputEmpty => this.outputEntries.Count == 0;

    public string Input => this.inputBuf.ToString();

    public bool InputEmpty => this.inputBuf.Length == 0;
     
    private NumberEntry LastOutput => this.outputEntries[^1];

    private NumberEntry SecondLastOutput => this.outputEntries[^2];


    public void LoadSavedData()
    {
        SavedData savedData = PersistenceManager.LoadData(); //load the saved data from disk
        ReadFrom(savedData); //read the saved data into the model
    }


    public void SaveData()
    {
        SavedData savedData = new SavedData(); //create an empty instance of SavedData
        WriteTo(savedData); //write the current state of the model to the instance of SavedData
        PersistenceManager.SaveData(savedData); //save the instance of SavedData to disk
    }


    private void ReadFrom(SavedData savedData)
    {
        this.outputEntries.Clear();
        this.outputEntries.AddRange(savedData.numberEntries);
        this.inputBuf.Clear();
        this.inputBuf.Append(savedData.input);
    }

    private void WriteTo(SavedData savedData)
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

    public void PerformUnaryOperation(Func<Q, Q> operation, bool retainOperand = false)
    { 
        (Q _, Q operand) = PeekOperands();
        if (operand.IsNaN) return; //need 1 operand to perform operation: abort operation

        Q result = operation(operand);

        if (result.IsNaN) return;

        if (retainOperand)
        {
            this.CurrentChange = this.CurrentChange.AddOutput(new NumberEntry(result), outputEntries);
        } 
        else 
        this.CurrentChange = InputEmpty ?
            this.CurrentChange.ReplaceOutput(new NumberEntry(result), outputEntries) :
            this.CurrentChange.ClearInput(inputBuf).AddOutput(new NumberEntry(result), outputEntries);
    }

    public void PerformBinaryOperation(Func<Q, Q, Q> operation)
    {
        (Q leftOperand, Q rightOperand) = PeekOperands();
        if (leftOperand.IsNaN || rightOperand.IsNaN) return; //need 2 operands to perform operation: abort operation

        Q result = operation(leftOperand, rightOperand);

        if (result.IsNaN) return;

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


    public (Q leftOperand, Q rightOperand) PeekOperands()
    {
        Q lastOutput = OutputCount > 0 ? LastOutput.Q : Q.NaN;
      
        return InputEmpty ?
            (OutputCount > 1 ? SecondLastOutput.Q : Q.NaN, lastOutput)
            : (lastOutput, inputBuf.AsQ());
    }

}
