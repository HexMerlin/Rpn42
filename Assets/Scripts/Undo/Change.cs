#nullable enable

public abstract class Change
{
    public ModelController ModelController { get; }

    public bool IsUndoPoint { get; set; } = false;

    public Change? Previous { get; private set; } = null;

    public Change? Next { get; private set; } = null;

    protected Change(ModelController modelController)
    {
        this.ModelController = modelController;
    }

    public abstract Change Execute();

    public abstract Change Rollback();

    public static Change CreateStart(ModelController modelController) => new NoChange(modelController);

    public Change AddInput(string input) 
        => new AddInput(ModelController, input).Execute().AppendTo(this);

    public Change RemoveInputChar() 
        => new RemoveInput(ModelController, 1).Execute().AppendTo(this);

    public Change RemoveInputChars(int count) 
        => new RemoveInput(ModelController, count).Execute().AppendTo(this);

    public Change ClearInput() 
          => new RemoveInput(ModelController).Execute().AppendTo(this);
   
    public Change AddOutput(NumberEntry numberEntry) 
        => new AddOutput(ModelController, numberEntry).Execute().AppendTo(this);

    public Change RemoveOutput() 
        => new RemoveOutput(ModelController).Execute().AppendTo(this);

    //we remove them one by one, so they all are tracked by the undo system
    public Change ClearAllOutputs()
    {
        Change change = RemoveOutput();
        while (ModelController.OutputEntries.Count > 0)
            change = change.RemoveOutput();
        return change;
    }

    public Change ReplaceInput(string input) => ClearInput().AddInput(input);

    public Change ReplaceOutput(NumberEntry numberEntry) => RemoveOutput().AddOutput(numberEntry);

    public Change AppendTo(Change previous)
    {
        previous.Next = this;
        this.Previous = previous;
        return this;
    }

}

