using System.Collections.Generic;

public abstract class OutputChange : Change
{
    public NumberEntry NumberEntry { get; }

    public OutputChange(NumberEntry numberEntry) : base() => NumberEntry = numberEntry;

    public abstract Change Execute(List<NumberEntry> outputItems);

    public abstract Change Rollback(List<NumberEntry> outputItems);
}
