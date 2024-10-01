public class AddInput : InputChange
{
    public AddInput(ModelController modelController, string input) : base(modelController, input) { }

    public override Change Execute(InputBuffer inputBuf)
    {
        inputBuf.Append(Input);
        return this;
    }

    public override Change Rollback(InputBuffer inputBuf)
    {
        new RemoveInput(ModelController, Input).Execute(inputBuf);
        return this;
    }
}
