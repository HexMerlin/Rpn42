using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLib;

public class ModelController 
{

    internal readonly List<NumberEntry> OutputEntries;

    internal readonly InputBuffer InputBuffer;

    public Change CurrentChange;

   
    public Format OutputFormat => new Format(NumberMode, OutputBase);

    public Mode NumberMode
    {
        get; set;
    }

    public int OutputBase
    {
        get; set;
    }

    public int InputBase
    {
        get => InputBuffer.Base;
        set => InputBuffer.Base = value;
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
        this.CurrentChange = Change.CreateStart(this);
        this.OutputEntries = new List<NumberEntry>();
        this.OutputBase = 10;
        this.NumberMode = Mode.Normal;
        this.InputBuffer = new InputBuffer(base_: 10);

    }
    public NumberEntry this[int index] => this.OutputEntries[index];

    public int OutputCount => this.OutputEntries.Count;

    private bool OutputEmpty => this.OutputEntries.Count == 0;

    public bool InputEmpty => this.InputBuffer.Length == 0;
     
    private NumberEntry LastOutput => this.OutputEntries[^1];

    private NumberEntry SecondLastOutput => this.OutputEntries[^2];


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
        this.OutputEntries.Clear();
        this.OutputEntries.AddRange(savedData.numberEntries);
        this.InputBuffer.Clear();
        this.InputBuffer.Append(savedData.input);
    }

    private void WriteTo(SavedData savedData)
    {
        savedData.numberEntries = this.OutputEntries.ToArray();
        savedData.input = this.InputBuffer.String();
    }

    public void AddOutput(NumberEntry numberEntry, bool isUndoPoint)
    {
        this.CurrentChange = this.CurrentChange.AddOutput(numberEntry);
        this.CurrentChange.IsUndoPoint = isUndoPoint;
    }

    public void PerformAddInput(string input) => this.CurrentChange = this.CurrentChange.AddInput(input);

    public void PerformUnaryOperation(Func<Q, Q> operation, bool retainOperand = false)
    { 
        (Q _, Q operand) = PeekOperands();
        if (operand.IsNaN) return; //need 1 operand to perform operation: abort operation

        Q result = operation(operand);

        if (result.IsNaN) return;

        if (retainOperand)
        {
            this.CurrentChange = this.CurrentChange.AddOutput(new NumberEntry(result));
        } 
        else 
        this.CurrentChange = InputEmpty ?
            this.CurrentChange.ReplaceOutput(new NumberEntry(result)) :
            this.CurrentChange.ClearInput().AddOutput(new NumberEntry(result));
    }

    public void PerformBinaryOperation(Func<Q, Q, Q> operation)
    {
        (Q leftOperand, Q rightOperand) = PeekOperands();
        if (leftOperand.IsNaN || rightOperand.IsNaN) return; //need 2 operands to perform operation: abort operation

        Q result = operation(leftOperand, rightOperand);

        if (result.IsNaN) return;

        this.CurrentChange = InputEmpty ?
            this.CurrentChange.RemoveOutput().ReplaceOutput(new NumberEntry(result)) :
            this.CurrentChange.ClearInput().ReplaceOutput(new NumberEntry(result));
    }

    public void PerformBackDrop()
    {
        if (InputEmpty)
        {
            if (OutputEmpty) return;
            this.CurrentChange = this.CurrentChange.RemoveOutput();
        }
        else
        {
            this.CurrentChange = this.CurrentChange.RemoveInputChar();
        }
    }

    public void PerformClear()
    {
        if (!InputEmpty)
            this.CurrentChange = this.CurrentChange.ClearInput();
        if (!OutputEmpty)
            this.CurrentChange = this.CurrentChange.ClearAllOutputs();
    }

    public void PerformCopy2()
    {
        if (OutputCount < 2) return;
        NumberEntry secondLastOutput = this.SecondLastOutput;
        NumberEntry lastOutput = this.LastOutput;
        this.CurrentChange = this.CurrentChange.AddOutput(secondLastOutput);
        this.CurrentChange = this.CurrentChange.AddOutput(lastOutput);
    }

    public void PerformUndo()
    {
        if (this.CurrentChange is NoChange)
            return;

        do
        {
            this.CurrentChange = this.CurrentChange.Rollback();

        } while (! this.CurrentChange.IsUndoPoint);
    }

    public void PerformRedo()
    {
        do
        {
            if (this.CurrentChange.Next is null)
                return;
            this.CurrentChange = this.CurrentChange.Next;
            this.CurrentChange = this.CurrentChange.Execute();

        } while (!this.CurrentChange.IsUndoPoint) ;
    }


    public (Q leftOperand, Q rightOperand) PeekOperands()
    {
        Q lastOutput = OutputCount > 0 ? LastOutput.Q : Q.NaN;
      
        return InputEmpty ?
            (OutputCount > 1 ? SecondLastOutput.Q : Q.NaN, lastOutput)
            : (lastOutput, InputBuffer.AsQ());
    }

}
