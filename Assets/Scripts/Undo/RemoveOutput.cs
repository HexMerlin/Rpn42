using System.Collections.Generic;

public class RemoveOutput : OutputChange
{
    public RemoveOutput(ModelController modelController) : base(modelController, modelController.OutputEntries[^1]) { }

    public override Change Execute()
    {
        OutputEntries.RemoveAt(OutputEntries.Count - 1);
        return this;
    }

    public override Change Rollback()
    {
        new AddOutput(ModelController, NumberEntry).Execute();
        return Previous;
    }
}
