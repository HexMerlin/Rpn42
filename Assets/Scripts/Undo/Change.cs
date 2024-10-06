#nullable enable

using MathLib;
using System;

public abstract class Change
{
    public ModelController ModelController { get; }

    public bool IsUndoPoint { get; private set; } = false;

    public Change? Previous { get; private set; } = null;

    public Change? Next { get; private set; } = null;

    protected Change(ModelController modelController)
    {
        this.ModelController = modelController;
    }

    public abstract Change Execute();

    public abstract Change Rollback();

    public static Change CreateStart(ModelController modelController) => new NoChange(modelController);

    public virtual Change SetUndoPoint(bool isUndoPoint = true)
    {
        IsUndoPoint = isUndoPoint;
        return this;
    }

    public Change ChangeInputBase(int newBase)
    {
        Q q = ModelController.InputEmpty ? Q.NaN : ModelController.InputBuffer.Q;

        Change result = this.FollowedBy(new InputBaseChange(ModelController, ModelController.InputBase, newBase));

        return q.IsNaN
            ? result
            : q.HasFiniteExpansion(newBase)
                ? result.ClearInput().AddInput(q.ToStringFinite(newBase))
                : result.ClearInput().AddOutput(new NumberEntry(q));
    }

    public Change AddInput(string input) 
        => this.FollowedBy(new AddInput(ModelController, input));

    public Change RemoveInputChar() 
        => this.FollowedBy(new RemoveInput(ModelController, 1));

    public Change RemoveInputChars(int count) 
        => this.FollowedBy(new RemoveInput(ModelController, count));

    public Change ClearInput()
          => ModelController.InputBuffer.IsEmpty
            ? this
            : this.FollowedBy(new RemoveInput(ModelController));
   
    public Change AddOutput(NumberEntry numberEntry) 
        => this.FollowedBy(new AddOutput(ModelController, numberEntry));

    public Change RemoveOutput() 
        => ModelController.OutputEmpty ? this : this.FollowedBy(new RemoveOutput(ModelController));

    //we remove them one by one, so they all are tracked by the undo system
    public Change ClearAllOutputs()
    {
        Change change = this;
        while (ModelController.OutputEntries.Count > 0)
            change = change.RemoveOutput();
        return change;
    }

    public Change FollowedBy(Change next)
    {
        this.Next = next.Execute();
        next.Previous = this;
        return next;
    }


}

