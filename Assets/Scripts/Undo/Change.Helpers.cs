using System.Collections.Generic;
using System.Text;

public class AddInput : InputChange
{
    public AddInput(string input) : base(input) { }

    public override Change Execute(InputBuffer inputBuf)
    {
        inputBuf.Append(Input);
        return this;
    }

    public override Change Rollback(InputBuffer inputBuf)
    {
        new RemoveInput(Input).Execute(inputBuf);
        return this;
    }
}

public class RemoveInput : InputChange
{
    public RemoveInput(string input) : base(input) { }

    public override Change Execute(InputBuffer inputBuf)
    {
        inputBuf.RemoveChars(inputBuf.Length - Input.Length, Input.Length);
        return this;
    }

    public override Change Rollback(InputBuffer inputBuf)
    {
        new AddInput(Input).Execute(inputBuf);
        return this;
    }
}

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

public abstract class InputChange : Change
{
    public string Input { get; }

    public InputChange(string input) : base() => Input = input;

    public abstract Change Execute(InputBuffer inputBuf);

    public abstract Change Rollback(InputBuffer inputBuf);
}

public abstract class OutputChange : Change
{
    public NumberEntry NumberEntry { get; }

    public OutputChange(NumberEntry numberEntry) : base() => NumberEntry = numberEntry;

    public abstract Change Execute(List<NumberEntry> outputItems);

    public abstract Change Rollback(List<NumberEntry> outputItems);
}

public class NoChange : Change
{
    public NoChange() : base() => IsUndoPoint = true;
}