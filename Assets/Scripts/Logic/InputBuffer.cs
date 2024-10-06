#nullable enable
using System.Numerics;
using System;
using System.Text;
using MathLib;

public class InputBuffer
{
    public int Base { 
        get; 
        private set; 
    }

    private Q? _q;

    public Q Q
    {
        get
        {
            _q ??= ParseNumber(sb, Base);
            return _q;
        }
    } 

    private readonly StringBuilder sb;

    public InputBuffer(int base_)
    {
        this.sb = new StringBuilder();
        Base = base_;
    }
  
    public int Length => this.sb.Length;

    public bool IsEmpty => this.sb.Length == 0;

    private void InvalidateQ() => this._q = null;

    public void ChangeBase(int newBase)
    {
        if (newBase == Base) return;  
        Base = newBase;
    }

   
    public void Clear()
    {
        this.sb.Clear();
        InvalidateQ();
    }

    public void Append(string input)
    {
        this.sb.Append(input);
        InvalidateQ();
    }
      
    public void RemoveChars(int startIndex, int count)
    {
        sb.Remove(startIndex, count);
        InvalidateQ();
    }

    /// <summary>
    /// Parses a number from a string denoting its positional representation.
    /// The string can contain an optional radix point.
    /// </summary>
    /// <remarks>
    /// The method handles numbers with arbitrary magnitude and precision.
    /// </remarks>
    /// <param name="sb">A <see cref="StringBuilder"/> containing a string of the number.</param>
    private static Q ParseNumber(StringBuilder sb, int base_)
    {
        if (sb.Length == 0) return Q.NaN;
        string input = sb.ToString();
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

