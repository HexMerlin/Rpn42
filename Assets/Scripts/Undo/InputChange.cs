public abstract class InputChange : Change
{
    public string Input { get; }

    public InputBuffer InputBuffer => ModelController.InputBuffer;

    public InputChange(ModelController modelController, string input) : base(modelController) => Input = input;

    public abstract Change Execute();

    public abstract Change Rollback();
}
