
public class NoChange : Change
{
    public NoChange() : base() => IsUndoPoint = true;
}