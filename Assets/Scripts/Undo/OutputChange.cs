using System.Collections.Generic;

public abstract class OutputChange : Change
{
    public NumberEntry NumberEntry { get; }

    public OutputChange(ModelController modelController, NumberEntry numberEntry) : base(modelController) => NumberEntry = numberEntry;

    public abstract Change Execute(List<NumberEntry> outputItems);

    public abstract Change Rollback(List<NumberEntry> outputItems);
}
