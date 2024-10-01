using System.Collections.Generic;

public class RemoveOutput : OutputChange
{
    public RemoveOutput(NumberEntry numberEntry) : base(numberEntry) { }

    public override Change Execute(List<NumberEntry> outputItems)
    {
        outputItems.RemoveAt(outputItems.Count - 1);
        return this;
    }

    public override Change Rollback(List<NumberEntry> outputItems)
    {
        new AddOutput(NumberEntry).Execute(outputItems);
        return this;
    }
}
