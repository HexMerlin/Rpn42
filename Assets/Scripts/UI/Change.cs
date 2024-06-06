#nullable enable

using System.Collections.Generic;
using System.Text;

public enum ChangeType
{
    AddInput,
    RemoveInput,
    AddOutput,
    RemoveOutput,
    None,
}

public class Change
{
    private bool IsCheckPoint = false;

    public Change SetCheckPoint(bool isCheckPoint = true) { this.IsCheckPoint = isCheckPoint; return this; }

    public readonly ChangeType Type;
    public string Input { get; }

    readonly NumberEntry NumberEntry;

    public Change? Previous { get; private set; } = null;

    public Change? Next { get; private set; } = null;

    private Change(ChangeType type, string input, NumberEntry numberEntry)
    {
        this.Type = type;
        this.Input = input;
        this.NumberEntry = numberEntry;
    }

    public static readonly Change None = new Change(ChangeType.None, string.Empty, NumberEntry.Invalid);

    public Change AddInput(string input, StringBuilder inputBuf)
    {
        inputBuf.Append(input);
        return new Change(ChangeType.AddInput, input, NumberEntry.Invalid).AddAfter(this);
    }

    public Change ClearInput(StringBuilder inputBuf) => RemoveInputChars(inputBuf.Length, inputBuf);

    public Change RemoveInputChar(StringBuilder inputBuf) => RemoveInputChars(1, inputBuf);

    public Change RemoveInputChars(int count, StringBuilder inputBuf)
    {
        int start = inputBuf.Length - count;
        string input = inputBuf.ToString(start, count);
        inputBuf.Remove(start, count);
        return new Change(ChangeType.RemoveInput, input, NumberEntry.Invalid).AddAfter(this);
    }

    public Change ReplaceInput(string input, StringBuilder inputBuf) => ClearInput(inputBuf).AddInput(input, inputBuf);

    public Change AddOutput(NumberEntry numberEntry, List<NumberEntry> outputItems)
    {
        outputItems.Add(numberEntry);
        return new Change(ChangeType.AddOutput, string.Empty, numberEntry).AddAfter(this);
    }

    public Change RemoveOutput(List<NumberEntry> outputItems)
    {
        NumberEntry numberEntry = outputItems[^1];
        outputItems.RemoveAt(outputItems.Count - 1);
        return new Change(ChangeType.RemoveOutput, string.Empty, numberEntry).AddAfter(this);
    }

    public Change ClearAllOutputs(List<NumberEntry> outputItems)
    {
        Change change = RemoveOutput(outputItems);
        while (outputItems.Count > 0)
            change = change.RemoveOutput(outputItems);
        return change;
    }


    public Change ReplaceOutput(NumberEntry numberEntry, List<NumberEntry> outputItems) => RemoveOutput(outputItems).AddOutput(numberEntry, outputItems);
    
    private Change AddAfter(Change previous)
    {
        previous.Next = this;
        this.Previous = previous;
        return this;
    }

}

