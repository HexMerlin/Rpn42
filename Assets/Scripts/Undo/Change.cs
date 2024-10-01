#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

public abstract class Change
{
    public bool IsUndoPoint { get; set; } = false;

    public Change? Previous { get; private set; } = null;

    public Change? Next { get; private set; } = null;

    public static Change CreateStart() => new NoChange();

    public Change AddInput(string input, InputBuffer inputBuf) 
        => new AddInput(input).Execute(inputBuf).AppendTo(this);

    public Change RemoveInputChars(int count, InputBuffer inputBuf)
    {
        string input = inputBuf.ToString(inputBuf.Length - count, count);
        return new RemoveInput(input).Execute(inputBuf).AppendTo(this);
    }
    public Change AddOutput(NumberEntry numberEntry, List<NumberEntry> outputItems) 
        => new AddOutput(numberEntry).Execute(outputItems).AppendTo(this);

    public Change RemoveOutput(List<NumberEntry> outputItems) 
        => new RemoveOutput(outputItems[^1]).Execute(outputItems).AppendTo(this);

    public Change RemoveInputChar(InputBuffer inputBuf) => RemoveInputChars(1, inputBuf);

    public Change ClearInput(InputBuffer inputBuf) => RemoveInputChars(inputBuf.Length, inputBuf);

    public Change ClearAllOutputs(List<NumberEntry> outputItems)
    {
        Change change = RemoveOutput(outputItems);
        while (outputItems.Count > 0)
            change = change.RemoveOutput(outputItems);
        return change;
    }
    public Change ReplaceInput(string input, InputBuffer inputBuf) => ClearInput(inputBuf).AddInput(input, inputBuf);


    public Change ReplaceOutput(NumberEntry numberEntry, List<NumberEntry> outputItems) => RemoveOutput(outputItems).AddOutput(numberEntry, outputItems);


    public Change AppendTo(Change previous)
    {
        previous.Next = this;
        this.Previous = previous;
        return this;
    }

}

