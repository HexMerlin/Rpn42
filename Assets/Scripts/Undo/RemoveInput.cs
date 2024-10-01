public class RemoveInput : InputChange
{
    public RemoveInput(ModelController modelController, string input) : base(modelController, input) { }

    public override Change Execute(InputBuffer inputBuf)
    {
        inputBuf.RemoveChars(inputBuf.Length - Input.Length, Input.Length);
        return this;
    }

    public override Change Rollback(InputBuffer inputBuf)
    {
        new AddInput(ModelController, Input).Execute(inputBuf);
        return this;
    }
}
