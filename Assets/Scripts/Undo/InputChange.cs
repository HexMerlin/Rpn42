public abstract class InputChange : Change
{
    public string Input { get; }

    public InputChange(ModelController modelController, string input) : base(modelController) => Input = input;

    public abstract Change Execute(InputBuffer inputBuf);

    public abstract Change Rollback(InputBuffer inputBuf);
}
