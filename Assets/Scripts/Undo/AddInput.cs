public class AddInput : InputChange
{
    public AddInput(ModelController modelController, string input) : base(modelController, input) { }

    public override Change Execute()
    {
        InputBuffer.Append(Input);
        return this;
    }

    public override Change Rollback()
    {
        new RemoveInput(ModelController, Input).Execute();
        return this;
    }
}
