
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

    public override Change Rollback()
    {
        new InputBaseChange(ModelController, NewBase, OldBase).Execute();
        return Previous;
    }


}

