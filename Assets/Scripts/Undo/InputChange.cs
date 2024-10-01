public abstract class InputChange : Change
{
    public string Input { get; }

    public InputChange(string input) : base() => Input = input;

    public abstract Change Execute(InputBuffer inputBuf);

    public abstract Change Rollback(InputBuffer inputBuf);
}
