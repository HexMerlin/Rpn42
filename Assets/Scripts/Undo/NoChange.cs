
using System;

/// <summary>
/// Represents the starting point of change history that cannot be undone.
/// </summary>
public class NoChange : Change
{
    public NoChange(ModelController modelController) : base(modelController) => base.SetUndoPoint(true);

    public override Change SetUndoPoint(bool isUndoPoint)
    {
        if (!isUndoPoint)
            throw new ArgumentOutOfRangeException(nameof(isUndoPoint), $"Cannot set undo point to false on {nameof(NoChange)}, it must always be true.");
        return this;
    }

    public override Change Execute() => this;
    public override Change Rollback() => throw new NotSupportedException();
}