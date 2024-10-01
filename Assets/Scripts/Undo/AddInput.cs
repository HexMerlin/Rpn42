public class AddInput : InputChange
{
    public AddInput(string input) : base(input) { }

    public override Change Execute(InputBuffer inputBuf)
    {
        inputBuf.Append(Input);
        return this;
    }

    public override Change Rollback(InputBuffer inputBuf)
    {
        new RemoveInput(Input).Execute(inputBuf);
        return this;
    }
}
