
public class AddOutput : OutputChange
{
    public AddOutput(ModelController modelController, NumberEntry numberEntry) : base(modelController, numberEntry) { }

    public override Change Execute()
    {
        OutputEntries.Add(NumberEntry);
        return this;
    }

    public override Change Rollback()
    {
        new RemoveOutput(ModelController).Execute();
        return Previous;
    }
}
