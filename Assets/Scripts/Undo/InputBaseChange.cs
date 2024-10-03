
using MathLib;
using System;

public class InputBaseChange : Change
{
    public int OldBase { get; }
    public int NewBase { get; }

    public InputBaseChange(ModelController modelController, int oldBase, int newBase) : base(modelController)
    {
        this.OldBase = oldBase;
        this.NewBase = newBase;
    }

    public override Change Execute()
    {
        ModelController.InputBuffer.ChangeBase(NewBase);
        return this;
    }

    //public Change ChangeInputBase(int newBase)
    //{
    //    if (ModelController.InputEmpty)
    //        return this.SetUndoPoint(false).FollowedBy(new InputBaseChange(ModelController, ModelController.InputBase, newBase)).Execute();

    //    Q q = ModelController.InputBuffer.AsQ();
    //    return this.SetUndoPoint(false)
    //        .FollowedBy(new InputBaseChange(ModelController, ModelController.InputBase, newBase)).Execute()
    //        .ClearInput()
    //        .HasFiniteExpansion(q, newBase)
    //            ? AddInput(q.ToStringFinite(newBase))
    //            : AddOutput(new NumberEntry(q));

    //}


    public override Change Rollback()
    {
        new InputBaseChange(ModelController, NewBase, OldBase).Execute();
        return Previous;
    }


}

