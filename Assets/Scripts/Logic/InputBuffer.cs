using System.Numerics;
using System;
using System.Text;
using MathLib;

public class InputBuffer
{
    public int Base { 
        get; 
        internal set; //TODO: Throw exception if set when sb is not empty - or change the base of sb?
    }

    private readonly StringBuilder sb;

    public InputBuffer(int base_)
    {
        this.sb = new StringBuilder();
        Base = base_;
    }

    public int Length => this.sb.Length;

    public void Clear() => this.sb.Clear();

    public void Append(string input) => this.sb.Append(input);

    public void RemoveChars(int startIndex, int count)
    {
        sb.Remove(startIndex, count);
    }

    public Q AsQ() => ParseNumber(sb.ToString(), Base);

    /// <summary>
    /// Parses a number from a string denoting its positional representation.
    /// The string can contain an optional radix point.
    /// </summary>
    /// <remarks>
    /// The method handles numbers with arbitrary magnitude and precision.
    /// </remarks>
    /// <param name="input">The string representation of the number.</param>
    private static Q ParseNumber(string input, int base_)
    {
        input = input.Trim();
        int pointIndex = input.IndexOf('.');
        if (pointIndex == -1)
            return new Q(BigIntegerExtensions.Parse(input, base_));

        if (input.LastIndexOf('.') != pointIndex)
            throw new ArgumentException("Invalid number format: multiple radix points", nameof(input));
        input = input.Remove(pointIndex, 1);
        if (input.Length == 0)
            return Q.Zero;

        return new Q(BigIntegerExtensions.Parse(input, base_), BigInteger.Pow(base_, input.Length - pointIndex));
    }

    public bool ContainsRadixPoint() => sb.ToString().Contains('.', StringComparison.InvariantCulture);

    public string SubString(int startIndex, int length) => sb.ToString(startIndex, length);

    public string String() => sb.ToString(); 

    public override string ToString() => throw new InvalidOperationException("No ToString() method for InputBuffer");

}

