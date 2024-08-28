//#nullable enable
//using System;
//using System.Collections.Generic;
//using System.Numerics;


//public partial class Q : IEquatable<Q>, IComparable<Q>
//{
//    public BigInteger Numerator { get; } 
    
//    public BigInteger Denominator { get; }

//    private int computedLength;

//    private int computedPeriod;
      

//    public static Q Invalid => new Q(0, 0, false);

//    public static Q RadixPoint => new Q(0, -1, false);

//    public static Q RepetendStart => new Q(0, -2, false);

//    public static Q RepetendEnd => new Q(0, -3, false);

//    public static Q Zero => new Q(0, 1, false);
//    public static Q One => new Q(1, 1, false);

//    public static Q Half => new Q(1, 2, false);

//    public bool IsInteger => Denominator == 1;
   
//    public bool IsTerminating => Denominator >= 1 && Denominator.IsPowerOfTwo;

//    public bool IsRadixPoint => Denominator == RadixPoint.Denominator;

//    public bool IsRepetendStart => Denominator == RepetendStart.Denominator;

//    public bool IsRepetendEnd => Denominator == RepetendEnd.Denominator;

//    public bool IsSpecialDelimiter => Denominator < 0;
//    public bool IsInvalid => Denominator == 0;

//    private const int UninitializedInt = 0;

   
//    public Q() : this(0, 1, false) { }

//    public Q(BigInteger numerator, BigInteger denominator) : this(numerator, denominator, true) { }

//    private Q(BigInteger numerator, BigInteger denominator, bool normalize)
//    {
//        Numerator = numerator;
//        Denominator = denominator;
//        computedLength = UninitializedInt;
//        computedPeriod = UninitializedInt;

//        if (normalize)
//        {
//            if (Denominator <= 0)
//            {
//                Numerator = Denominator = 0;
//                return;
//            }

//            if (denominator.Sign == -1)
//            {
//                Numerator = -Numerator;
//                Denominator = -Denominator;
//            }

//            BigInteger gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(Numerator), BigInteger.Abs(Denominator));
//            if (gcd > 1)
//            {
//                Numerator /= gcd;
//                Denominator /= gcd;
//            }
//        }
//    }


//    public Q(BigInteger numerator) : this(numerator, 1, false) { }

//    public Q(string input)
//    {
//        computedLength = UninitializedInt;
//        computedPeriod = UninitializedInt;
//        Denominator = 1;

//        int pointIndex = input.IndexOf('.');
//        if (pointIndex != -1) //not currently supporting point notation
//        {
//            Numerator = 0;
//            Denominator = 0;
//            return;
//            //denominator <<= (input.Length - pointIndex - 1); //compute the denominator
//            //input = input.Remove(pointIndex, 1); // remove the point
//        }


//        Numerator = BigInteger.Parse(input);

//    }

//    public BigInteger IntegerPart => Numerator / Denominator;

//    public Q FractionalPart => this - IntegerPart;

//    public int IntegerLength => (int)BigInteger.Abs(IntegerPart).GetBitLength();

//    public bool TryCastToInt32(out int result)
//    {
//        if (Denominator == 1 && Numerator <= int.MaxValue && Numerator >= int.MinValue)
//        {
//            result = (int)Numerator;
//            return true;
//        }
//        result = int.MinValue;
//        return false;
//    }

//    public Q this[int index] => (this << (index - IntegerLength)).FractionalPart;

//    public Q Weight(int index) => One << (IntegerLength - index - 1);

//    public Q Term(int index)
//    {
//        int repetendStart = Length - Period;
//        bool bit = this[index] >= Half;

//        if (index < repetendStart)
//            return bit ? Weight(index) : Zero;
//        else
//            return bit ? (Weight(index) << Period) / ((BigInteger.One << Period) - BigInteger.One) : Q.Zero;
         
//    }

//    public IEnumerable<Q> RotationsBin
//    {
//        get
//        {            
//            if (this < 0) yield break;
//            Q firstInRepetend = Invalid;
            
//            int integerLength = this.IntegerLength;
//            int radixPointIndex = IsInteger ? -1 : integerLength; //do not output radix point for integers

//            for (int i = 0; ; i++)
//            {
//                Q r = this[i];

//                if (i == radixPointIndex)
//                    yield return RadixPoint;

//                if ((r == firstInRepetend || r.IsZero) && i >= integerLength)
//                    break;

//                if (firstInRepetend.IsInvalid && r.Denominator.IsOdd() && r.Denominator > 1)
//                {
//                    firstInRepetend = r;
//                    yield return RepetendStart;
//                }
             

//                yield return r;

//            }
//            if (!firstInRepetend.IsInvalid)
//                yield return RepetendEnd;
//        }
//    }

//    public IEnumerable<Q> RotationsBalBin
//    {
//        get
//        {
//            Q r = (this << 1).FractionalPart;
//            return r.RotationsBin;

//        }
//    }
//    public IEnumerable<Q> Partition
//    {
//        get
//        {
//            for (int i = 0; i < Length; i++)
//                yield return Term(i);
//        }
//    }

//    /// <summary>
//    /// Returns the rational number as a mixed number
//    /// </summary>
//    /// <remarks>
//    /// For any non-zero fractional part, the numerator is always <b>Odd</b>
//    /// </remarks>
//    /// <example>
//    /// <code>7/3   →  2 + 1/3</code>
//    /// <code>2/5   →  1 - 3/5</code>
//    /// <code>-5/3  → -2 + 1/3</code>
//    /// <code>13/6  →  2 + 1/6</code>
//    /// </example>
//    public (BigInteger Integer, Q Fraction) Mixed
//    {
//        get
//        {
//            var numerator = BigInteger.Abs(Numerator);
//            BigInteger integer = numerator / Denominator;
//            BigInteger fractionNumerator = numerator % Denominator;

//            if (fractionNumerator.IsEven)
//            {
//                if (fractionNumerator.IsZero)
//                    return (Numerator / Denominator, Q.Zero);
//                integer++;
//                fractionNumerator = fractionNumerator - Denominator;
//            }
//            if (Numerator.Sign < 0)
//            {
//                integer = -integer;
//                fractionNumerator = -fractionNumerator;
//            }

//            return (integer, new Q(fractionNumerator, Denominator));
//        }
//    }


//    public int Length
//    {
//        get
//        {
//            ComputeLengthAndPeriod();
//            return computedLength;
//        }
//    }

//    public int Period
//    {
//        get
//        {
//            ComputeLengthAndPeriod();
//            return computedPeriod;
//        }
//    }

//    private void ComputeLengthAndPeriod()
//    {
//        if (computedLength != UninitializedInt)
//            return;

//        computedLength = 0;
//        computedPeriod = -1;
//        foreach (Q r in RotationsBin)
//        {
//            if (!r.IsSpecialDelimiter)
//            {
//                computedLength++;
//                if (computedPeriod >= 0)
//                    computedPeriod++;
//            }
//            else if (r == RepetendStart)
//                computedPeriod = 0;

            
//        }
//        if (computedPeriod == -1)
//            computedPeriod = 0;
//    }


//    public Q WeightFloor
//    {
//        get
//        {
//            Q w = Q.One;
//            while (w < this.Abs)
//                w <<= 1;
//            return w >> 1;
//        }
//    }

//    public BigInteger RepetendAsInteger
//    {
//        get
//        {
//            BigInteger repInt = 0;
//            bool inRepetend = false;

//            foreach (Q r in RotationsBin)
//            {
//                if (r.IsRepetendStart)
//                {
//                    inRepetend = true;
//                    continue;
//                }
//                if (!inRepetend)
//                    continue;
//                if (r.IsSpecialDelimiter)
//                    continue;
              
//                repInt <<= 1;
//                if (r >= Half)
//                    repInt++;
//            }

//            return repInt;
//        }
//    }
//}
