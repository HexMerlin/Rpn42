using System.Collections.Generic;

public abstract class OutputChange : Change
{
    public List<NumberEntry> OutputEntries => ModelController.OutputEntries;

    public NumberEntry NumberEntry { get; }

    public OutputChange(ModelController modelController, NumberEntry numberEntry) : base(modelController) => NumberEntry = numberEntry;

    public abstract Change Execute();

    public abstract Change Rollback();
}
