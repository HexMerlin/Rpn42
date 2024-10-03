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
        if (ModelController.InputEmpty)
        {
            return this.FollowedBy(new InputBaseChange(ModelController, ModelController.InputBase, newBase));
        }
        else
        {
            Q q = ModelController.InputBuffer.AsQ();
            if (HasFiniteExpansion(q, newBase))
            {
                return ReplaceInput(q.ToStringFinite(newBase))
                    .FollowedBy(new InputBaseChange(ModelController, ModelController.InputBase, newBase))
                    .Prepend(this.SetUndoPoint(false));
            }
            else
            {
                return ClearInput()
                    .AddOutput(new NumberEntry(q))
                    .FollowedBy(new InputBaseChange(ModelController, ModelController.InputBase, newBase))
                    .Execute()
                    .Prepend(this.SetUndoPoint(false));
                
            }
        }
    }

    public Change AddInput(string input) 
        => new AddInput(ModelController, input).Execute().Prepend(this);

    public Change RemoveInputChar() 
        => new RemoveInput(ModelController, 1).Execute().Prepend(this);

    public Change RemoveInputChars(int count) 
        => new RemoveInput(ModelController, count).Execute().Prepend(this);

    public Change ClearInput() 
          => new RemoveInput(ModelController).Execute().Prepend(this);
   
    public Change AddOutput(NumberEntry numberEntry) 
        => new AddOutput(ModelController, numberEntry).Execute().Prepend(this);

    public Change RemoveOutput() 
        => new RemoveOutput(ModelController).Execute().Prepend(this);

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

    public Change Prepend(Change previous)
    {
        previous.Next = this;
        this.Previous = previous;
        return this;
    }

    public Change FollowedBy(Change next)
    {
        next.Previous = this;
        this.Next = next;
        return next;
    }


    public static bool HasFiniteExpansion(Q q, int base_)
    {
        if (base_ is not (2 or 3 or 5 or 7 or 10))
            throw new ArgumentOutOfRangeException(nameof(base_), base_, "Base must be 2, 3, 5, 7, or 10");

        if (base_ != 10)
            return q.Denominator.Abs().IsPowerOf(base_);


        var d = q.Denominator.Abs();
        while (d > 1)
        {
            if (d % 2 == 0)
                d /= 2;
            else if (d % 5 == 0)
                d /= 5;
            else
                return false;
        }

        return true;
    }
}

