public class RemoveInput : InputChange
{
    public RemoveInput(ModelController modelController, string input) : base(modelController, input) { }

    public RemoveInput(ModelController modelController) : this(modelController, modelController.InputBuffer.Length) { }

    public RemoveInput(ModelController modelController, int count) : base(modelController, modelController.InputBuffer.ToString(modelController.InputBuffer.Length - count, count)) { }

    public override Change Execute()
    {
        InputBuffer.RemoveChars(InputBuffer.Length - Input.Length, Input.Length);
        return this;
    }

    public override Change Rollback()
    {
        new AddInput(ModelController, Input).Execute();
        return this;
    }
}
