using System;
using System.Collections.Generic;
using System.Numerics;

public partial struct Rational : IEquatable<Rational>, IComparable<Rational>
{
    public readonly BigInteger Numerator { get; }
    public readonly BigInteger Denominator { get; }

    private int computedLength;

    private int computedPeriod;
      

    public static Rational Invalid => new Rational(0, 0, false);
    public static Rational Zero => new Rational(0, 1, false);
    public static Rational One => new Rational(1, 1, false);

    public static Rational Half => new Rational(1, 2, false);

    public bool IsSpecialDelimiter => Denominator < 0;
    public bool IsInvalid => Denominator == 0;

    private const int uninitializedInt = int.MinValue + 1;

    //public Rational() : this(0, 1, false) { }   //use when there is support for C# 10 

    public Rational(BigInteger numerator, BigInteger denominator) : this(numerator, denominator, true) { }

    private Rational(BigInteger numerator, BigInteger denominator, bool checkAndNormalize)
    {
        Numerator = numerator;
        Denominator = denominator;
        computedLength = uninitializedInt;
        computedPeriod = uninitializedInt;

        if (checkAndNormalize)
        {
            if (Denominator <= 0)
            {
                Numerator = Denominator = 0;
                return;
            }

            if (denominator.Sign == -1)
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }

            BigInteger gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(Numerator), BigInteger.Abs(Denominator));
            if (gcd > 1)
            {
                Numerator /= gcd;
                Denominator /= gcd;
            }
        }
    }


    public Rational(BigInteger numerator) : this(numerator, 1, false) { }

    public Rational(string input)
    {
        computedLength = uninitializedInt;
        computedPeriod = uninitializedInt;
        Denominator = 1;

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

    public Rational Weight(int index) => One << (IntegerLength - index - 1);

    public Rational Term(int index)
    {
        int repetendStart = Length - Period;
        bool bit = this[index] >= Half;

        if (index < repetendStart)
            return bit ? Weight(index) : Zero;
        else
            return bit ? (Weight(index) << Period) / ((BigInteger.One << Period) - BigInteger.One) : Rational.Zero;
         
    }
   

    public IEnumerable<(Rational, Separator)> RotationsBalBin
    {
        get
        {
            yield break;
            //Rational r = this;
            //Rational w = WeightFloor;

            //while (w >= 1)
            //{
            //    yield return r;
            //    if (r > 0)
            //        r -= w;
            //    else
            //        r += w;

            //    w >>= 1;
            //}
            //Debug.Assert(w == Half);
            //static bool IsDoubleOdd(Rational r) => !r.Numerator.IsEven && !r.Denominator.IsEven; //used to determine when the repetend starts
            //Rational repetendStart = IsDoubleOdd(r) ? r : Invalid; //used for bookkeeping when the repetend started

            //if (repetendStart.IsInvalid)
            //    yield return Delimiter.RadixPoint; //no repetend detected yet, return only the radix point
            //else
            //    yield return Delimiter.RadixPointAndRepetendBegin; //repetend detected, return the combined radix point and the repetend begin

            //while (true)
            //{
            //    if (r.IsZero)
            //        yield break;
            //    if (repetendStart.IsInvalid && IsDoubleOdd(r))
            //    {
            //        repetendStart = r;
            //        yield return Delimiter.RepetendBegin;
            //    }

            //    yield return r;

            //    r <<= 1;

            //    if (r > 0)
            //        r--;
            //    else
            //        r++;

            //    if (r == repetendStart)
            //        yield break;
            //}

        }
    }
   
    public IEnumerable<(Rational rational, Separator separator)> RotationsBin
    {
        get
        {            
            if (this < 0) yield break;
            Rational firstInRepetend = Invalid;
            
            int integerLength = this.IntegerLength;
            bool InFractionPart(int i) => i >= integerLength;

            for (int i = 0; ; i++)
            {
                Rational r = this[i];

                if (InFractionPart(i) && (r == firstInRepetend || r.IsZero))
                    break;
                bool emitRadixPoint = i == integerLength;
                bool emitRepetendBegin = firstInRepetend.IsInvalid && r.Denominator.IsOdd();
                Separator separator = emitRepetendBegin && emitRadixPoint ? Separator.RadixPointAndRepetendBegin : emitRepetendBegin ? Separator.RepetendBegin : emitRadixPoint ? Separator.RadixPoint : Separator.None;
                if (emitRepetendBegin)
                    firstInRepetend = r;

                yield return (r, separator);
            }
         
        }
    }

    public IEnumerable<Rational> Partition
    {
        get
        {
            for (int i = 0; i < Length; i++)
                yield return Term(i);
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
            BigInteger fractionNumerator = numerator % Denominator;

            if (fractionNumerator.IsEven)
            {
                if (fractionNumerator.IsZero)
                    return (Numerator / Denominator, Rational.Zero);
                integer++;
                fractionNumerator = fractionNumerator - Denominator;
            }
            if (Numerator.Sign < 0)
            {
                integer = -integer;
                fractionNumerator = -fractionNumerator;
            }

            return (integer, new Rational(fractionNumerator, Denominator));
        }
    }


    public int Length
    {
        get
        {
            ComputeLengthAndPeriod();
            return computedLength;
        }
    }

    public int Period
    {
        get
        {
            ComputeLengthAndPeriod();
            return computedPeriod;
        }
    }

    private void ComputeLengthAndPeriod()
    {
        if (computedPeriod != uninitializedInt)
            return;

        computedLength = 0;
        computedPeriod = 0;
        foreach ((_, Separator separator) in RotationsBin)
        {
            if (computedPeriod >= 1)
                computedPeriod++;
            else if (separator == Separator.RepetendBegin || separator == Separator.RadixPointAndRepetendBegin)
                computedPeriod = 1;

            computedLength++;
        }
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
}
