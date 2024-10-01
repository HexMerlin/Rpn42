public class RemoveInput : InputChange
{
    public RemoveInput(string input) : base(input) { }

    public override Change Execute(InputBuffer inputBuf)
    {
        inputBuf.RemoveChars(inputBuf.Length - Input.Length, Input.Length);
        return this;
    }

    public override Change Rollback(InputBuffer inputBuf)
    {
        new AddInput(Input).Execute(inputBuf);
        return this;
    }
}
