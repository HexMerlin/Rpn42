
using System;

/// <summary>
/// Represents the starting point of change history that cannot be undone.
/// </summary>
public class NoChange : Change
{
    public NoChange(ModelController modelController) : base(modelController) => IsUndoPoint = true;

    public override Change Execute() => this;
    public override Change Rollback() => throw new NotSupportedException();
}