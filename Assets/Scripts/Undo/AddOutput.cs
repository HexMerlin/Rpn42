using System.Collections.Generic;

public class AddOutput : OutputChange
{
    public AddOutput(NumberEntry numberEntry) : base(numberEntry) { }

    public override Change Execute(List<NumberEntry> outputItems)
    {
        outputItems.Add(NumberEntry);
        return this;
    }

    public override Change Rollback(List<NumberEntry> outputItems)
    {
        new RemoveOutput(NumberEntry).Execute(outputItems);
        return this;
    }
}
