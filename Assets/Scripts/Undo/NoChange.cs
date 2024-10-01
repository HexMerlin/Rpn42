
/// <summary>
/// Represents the starting point of change history that cannot be undone.
/// </summary>
public class NoChange : Change
{
    public NoChange(ModelController modelController) : base(modelController) => IsUndoPoint = true;
}