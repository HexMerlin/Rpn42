using System.Collections.Generic;

public class AddOutput : OutputChange
{
    public AddOutput(ModelController modelController, NumberEntry numberEntry) : base(modelController, numberEntry) { }

    public override Change Execute(List<NumberEntry> outputItems)
    {
        outputItems.Add(NumberEntry);
        return this;
    }

    public override Change Rollback(List<NumberEntry> outputItems)
    {
        new RemoveOutput(ModelController, NumberEntry).Execute(outputItems);
        return this;
    }
}
