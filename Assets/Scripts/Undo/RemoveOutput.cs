using System.Collections.Generic;

public class RemoveOutput : OutputChange
{
    public RemoveOutput(ModelController modelController, NumberEntry numberEntry) : base(modelController, numberEntry) { }

    public override Change Execute(List<NumberEntry> outputItems)
    {
        outputItems.RemoveAt(outputItems.Count - 1);
        return this;
    }

    public override Change Rollback(List<NumberEntry> outputItems)
    {
        new AddOutput(ModelController, NumberEntry).Execute(outputItems);
        return this;
    }
}
