public abstract class InputChange : Change
{
    public int Base { get; }

    public string Input { get; }

    public InputBuffer InputBuffer => ModelController.InputBuffer;

    public InputChange(ModelController modelController, string input) : base(modelController)
    {
        Base = InputBuffer.Base;
        Input = input;
    }

    public abstract Change Execute();

    public abstract Change Rollback();
}
