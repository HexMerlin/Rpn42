using System.Collections.Generic;
using System;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Diagnostics;


public enum RationalType
{
    Normal = 1,
    Invalid = 0,
    RadixPoint = -1,
    RepetendBegin = -2,
    RadixPointAndRepetendBegin = -3,
    RepetendEnd = -4,
}

public readonly partial struct Rational : IEquatable<Rational>, IComparable<Rational>
{
    public readonly BigInteger Numerator { get; }
    public readonly BigInteger Denominator { get; }

    public static Rational Zero => new Rational(0, 1, false);
    public static Rational One => new Rational(1, 1, false);

    public static Rational Half => new Rational(1, 2, false);

    public bool IsSpecialDelimiter => Denominator < 0;
    public bool IsInvalid => Type.Equals(RationalType.Invalid);

    public static implicit operator Rational(RationalType type)
        => new Rational(0, (int)type, false);

    public RationalType Type => Denominator > BigInteger.Zero
       ? RationalType.Normal
       : Enum.IsDefined(typeof(RationalType), (int)Denominator)
           ? (RationalType)(int)Denominator
           : throw new ArgumentOutOfRangeException(nameof(Denominator), "Invalid denominator value");


    //public Rational() : this(0, 1, false) { }   //use when there is support for C# 10 

    public Rational(BigInteger numerator, BigInteger denominator) : this(numerator, denominator, true) { }

    private Rational(BigInteger numerator, BigInteger denominator, bool checkAndNormalize)
    {
        if (checkAndNormalize)
        {
            if (denominator <= 0)
            {
                Numerator = 0;
                Denominator = 0;
                return;
            }

            if (denominator.Sign == -1)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            BigInteger gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), BigInteger.Abs(denominator));
            if (gcd > 1)
            {
                numerator /= gcd;
                denominator /= gcd;
            }
        }
        this.Numerator = numerator;
        this.Denominator = denominator;
    }


    public Rational(BigInteger numerator) : this(numerator, 1, false) { }

    public Rational(string input)
    {
        Denominator = BigInteger.One;

        int pointIndex = input.IndexOf('.');
        if (pointIndex != -1) //not currently supporting point notation
        {
            Numerator = 0;
            Denominator = 0;
            return;
            //denominator <<= (input.Length - pointIndex - 1); //compute the denominator
            //input = input.Remove(pointIndex, 1); // remove the point
        }


        Numerator = BigInteger.Parse(input);

    }

    public BigInteger IntegerPart => Numerator / Denominator;

    public Rational FractionalPart => this - IntegerPart;

    public int IntegerLength => (int)BigInteger.Abs(IntegerPart).GetBitLength(); 

    public Rational this[int index] => (this << (index - IntegerLength)).FractionalPart;

    public (int Period, BigInteger Repetend) Repetend
    {
        get
        {
            int period = 0;
            BigInteger repetend = BigInteger.MinusOne;

            foreach (Rational r in RotationsBinOLD)
            {
                if (r.Type == RationalType.RepetendBegin || r.Type == RationalType.RadixPointAndRepetendBegin)
                {
                    repetend = 0; //repetend started
                    continue;
                }
                if (repetend == BigInteger.MinusOne)
                    continue;

                repetend <<= 1;

                if (r > Half)
                    repetend++;
                period++;
            }
            return (period, repetend);
        }
    }

    public Rational DivideByMersenneCeiling()
    {
        BigInteger div = MersenneCeiling(Numerator);
        return this / div;
    }



    public static BigInteger MersenneCeiling(BigInteger num)
    {
        num = BigInteger.Abs(num);
        BigInteger ceiling = 1;

        while (ceiling <= num)
            ceiling = (ceiling << 1) + 1;

        return ceiling;
    }



public Rational WeightFloor
    {
        get
        {
            Rational w = Rational.One;
            while (w < this.Abs)
                w <<= 1;
            return w >> 1;
        }
    }

    public IEnumerable<Rational> Partition
    {
        get
        {
            if (this < 0)
            {
                yield break;
            }
            Rational w = WeightFloor;

            foreach (Rational rot in RotationsBinOLD)
            {
                RationalType type = rot.Type;
                if (type == RationalType.RepetendBegin || type == RationalType.RadixPointAndRepetendBegin)
                    break;
                else if (type == RationalType.RadixPoint)
                {
                    continue;
                }
                else if (type == RationalType.Normal)
                {
                    yield return rot > w ? w : Rational.Zero;
                    w >>= 1;
                }
                else throw new InvalidOperationException($"Unexpected {nameof(RationalType)} value {type}");
            }

            (int period, BigInteger repetend) = Repetend;
            var numerator = BigInteger.One << (period - 1);
            var denominator = (BigInteger.One << period) - BigInteger.One;
            for (int i = 0; i < period; i++)
            {
                var r = new Rational(numerator, denominator);
                if (r >= w)
                {
                    yield return r;

                    numerator >>= 1;
                }
                else
                    yield return Rational.Zero;
            }
            Debug.Assert(repetend.IsZero);
        }
    }

    public IEnumerable<Rational> RotationsBalBin
    {
        get
        {
            Rational r = this;
            Rational w = WeightFloor;

            while (w >= 1)
            {
                yield return r;
                if (r > 0)
                    r -= w;
                else
                    r += w;

                w >>= 1;
            }
            Debug.Assert(w == Half);
            static bool IsDoubleOdd(Rational r) => !r.Numerator.IsEven && !r.Denominator.IsEven; //used to determine when the repetend starts
            Rational repetendStart = IsDoubleOdd(r) ? r : RationalType.Invalid; //used for bookkeeping when the repetend started

            if (repetendStart.IsInvalid)
                yield return RationalType.RadixPoint; //no repetend detected yet, return only the radix point
            else
                yield return RationalType.RadixPointAndRepetendBegin; //repetend detected, return the combined radix point and the repetend begin

            while (true)
            {
                if (r.IsZero)
                    yield break;
                if (repetendStart.IsInvalid && IsDoubleOdd(r))
                {
                    repetendStart = r;
                    yield return RationalType.RepetendBegin;
                }

                yield return r;

                r <<= 1;

                if (r > 0)
                    r--;
                else
                    r++;

                if (r == repetendStart)
                    yield break;
            }

        }
    }
    public IEnumerable<Rational> RotationsBin
    {
        get
        {
            if (this < 0) yield break;
            Rational firstInRepetend = RationalType.Invalid;

            var integerLength = this.IntegerLength;

            for (int i = 0; ; i++)
            {
                Rational r = this[i];

                if (r == firstInRepetend || r.IsZero)
                   break;
                bool emitRadixPoint = i == integerLength;
                bool emitRepetendBegin = firstInRepetend.IsInvalid && r.Denominator.IsOdd();

                if (emitRepetendBegin)
                {
                    firstInRepetend = r;
                    if (emitRadixPoint)
                        yield return RationalType.RadixPointAndRepetendBegin;
                    else
                        yield return RationalType.RepetendBegin;
                }
                if (emitRadixPoint && !emitRepetendBegin)
                    yield return RationalType.RadixPoint;

                yield return r;
            }
            if (!Denominator.IsPowerOfTwo)
                yield return RationalType.RepetendEnd;


        }
    }


    public IEnumerable<Rational> RotationsBinOLD
    {
        get
        {
            if (this < 0)
            {
                yield break;
            }
            Rational r = this;
            Rational w = WeightFloor;

            while (w >= 1)
            {
                yield return r;
                if (r > w)
                    r -= w;

                w >>= 1;
            }
            Debug.Assert(w == Half);

            Rational repetendStart = !r.Denominator.IsEven ? r : RationalType.Invalid;

            if (repetendStart.IsInvalid)
                yield return RationalType.RadixPoint;
            else
                yield return RationalType.RadixPointAndRepetendBegin;

            while (true)
            {
                if (r.IsZero)
                    yield break;
                if (repetendStart.IsInvalid && !r.Denominator.IsEven)
                {
                    repetendStart = r;
                    yield return RationalType.RepetendBegin;
                }

                yield return r;

                r <<= 1;

                if (r > 1)
                    r--;

                if (r == repetendStart)
                    yield break;
            }

        }
    }


    /// <summary>
    /// Returns the rational number as a mixed number
    /// </summary>
    /// <remarks>
    /// For any non-zero fractional part, the numerator is always <b>Odd</b>
    /// </remarks>
    /// <example>
    /// <code>7/3   →  2 + 1/3</code>
    /// <code>2/5   →  1 - 3/5</code>
    /// <code>-5/3  → -2 + 1/3</code>
    /// <code>13/6  →  2 + 1/6</code>
    /// </example>
    public (BigInteger Integer, Rational Fraction) Mixed
    {
        get
        {
            var numerator = BigInteger.Abs(Numerator);
            BigInteger integer = numerator / Denominator;
            BigInteger fractionalNumerator = numerator % Denominator;

            if (fractionalNumerator.IsEven)
            {
                if (fractionalNumerator.IsZero)
                    return (Numerator / Denominator, Rational.Zero);
                integer++;
                fractionalNumerator = fractionalNumerator - Denominator;
            }
            if (Numerator.Sign < 0)
            {
                integer = -integer;
                fractionalNumerator = -fractionalNumerator;
            }

            return (integer, new Rational(fractionalNumerator, Denominator));
        }
    }

    private char? ToSeparatorChar() => Type switch
    { 
        RationalType.RadixPoint => '⠄',
        RationalType.RepetendBegin => '⠁',
        RationalType.RadixPointAndRepetendBegin => '⠅',
        RationalType.RepetendEnd => '…',
        _ => null
    };

    public string ToStringNormal() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";

    public string ToStringMixed()
    {
        (BigInteger integer, Rational fraction) = Mixed;
        return fraction.IsZero ? integer.ToString() : $"{(integer.IsZero ? "" : integer.ToString() + " ")}{fraction.ToStringNormal()}";
    }

    public string ToStringBinOLD()
    {
        Rational weight = WeightFloor;
        StringBuilder sb = new StringBuilder();

        foreach (Rational rot in RotationsBinOLD)
        {
            if (rot.IsSpecialDelimiter)
            {
                sb.Append(rot.ToSeparatorChar());
            }
            else
            {
                sb.Append(rot > weight ? '1' : '0');
                if (weight > Half)
                    weight >>= 1;
            }
            sb.Append(' ');

        }
        if (sb.Length > 0) sb.Remove(sb.Length - 1, 1); //remove the last space
        return sb.ToString();
    }

    public string ToStringPartition() => $"{string.Join(" ", Partition.Select(r => r.ToSeparatorChar()?.ToString() ?? r.ToString()))}";

    public string ToStringBin() => $"{string.Join(" ", RotationsBin.Select(r => r.ToSeparatorChar() ?? (r >= Half ? '1' : '0')))}";

    public string ToStringBalBin() => $"{string.Join(" ", RotationsBalBin.Select(r => r.ToSeparatorChar() ?? (r > 0 ? '+' : '-')))}";

    public string ToStringRotationsBin()
    {
        var denominator = Denominator;
        return $"{string.Join(" ", RotationsBin.Select(r => r.ToSeparatorChar()?.ToString() ?? (denominator == r.Denominator ? (r.Numerator + "/") : r.ToString())))}";
    }

    public string ToStringRotationsBinOLD()
    {
        var denominator = Denominator;
        return $"{string.Join(" ", RotationsBinOLD.Select(r => r.ToSeparatorChar()?.ToString() ?? (denominator == r.Denominator ? (r.Numerator + "/") : r.ToString())))}";
    }

    public string ToStringRotationsBalBin()
    {
        var denominator = Denominator;
        return $"{string.Join(" ", RotationsBalBin.Select(r => r.ToSeparatorChar()?.ToString() ?? (denominator == r.Denominator ? (r.Numerator + "/") : r.ToString())))}";
    }

    public string ToStringRepInfo()
    {
        (int period, BigInteger repetend) = Repetend;
        return $"P={period} R={repetend}";
    }

    public override string ToString() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";

}
