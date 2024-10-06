using System;

public class InputBaseChange : Change
{
    public int OldBase { get; }
    public int NewBase { get; }

    public InputBaseChange(ModelController modelController, int oldBase, int newBase) : base(modelController)
    {
        if (oldBase == newBase) throw new ArgumentException("Old and new base must not be the same.");
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
        ModelController.InputBuffer.ChangeBase(OldBase);
        return Previous;
    }


}

