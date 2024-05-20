using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;



public enum Separator
{
    None = 1,
    RadixPoint = -1,
    RepetendBegin = -2,
    RadixPointAndRepetendBegin = -3,
}

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

 
    //public static implicit operator Rational(Delimiter type)
    //    => new Rational(0, (int)type, false);

    //public Separator Separator => Denominator > BigInteger.Zero
    //   ? Separator.None
    //   : Enum.IsDefined(typeof(Separator), (int)Denominator)
    //       ? (Separator)(int)Denominator
    //       : throw new ArgumentOutOfRangeException(nameof(Denominator), "Invalid denominator value");


    //public Rational() : this(0, 1, false) { }   //use when there is support for C# 10 

    public Rational(BigInteger numerator, BigInteger denominator) : this(numerator, denominator, true) { }

    private Rational(BigInteger numerator, BigInteger denominator, bool checkAndNormalize)
    {
        computedLength = uninitializedInt;
        computedPeriod = uninitializedInt;

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
        computedLength = uninitializedInt;
        computedPeriod = uninitializedInt;
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
            
            for (int len = 0; ; len++)
            {
                Rational r = this[len];

                if (r == firstInRepetend || r.IsZero)
                   break;
                bool emitRadixPoint = len == integerLength;
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


    private static char? ToSeparatorChar(Separator separator) => separator switch
    { 
        Separator.RadixPoint => '⠄',
        Separator.RepetendBegin => '⠁',
        Separator.RadixPointAndRepetendBegin => '⠅',
        //Separator.RepetendEnd => '…',
        _ => null
    };

    public string ToStringNormal() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";


    public string ToStringPartition() => $"{string.Join(" ", Partition.Select(r => r.ToString()))}";

    public string ToStringBin()
    {
        StringBuilder sb = new StringBuilder(); 
        foreach ((Rational r, Separator s) in RotationsBin)
        {
            if (s != Separator.None)
                sb.Append(ToSeparatorChar(s));
            sb.Append(r >= Half ? '1' : '0');
        }
        return sb.ToString();
    }

    public string ToStringBalBin() => "Not implemented";

    public string ToStringRotationsBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach ((Rational r, Separator s) in RotationsBin)
        {
            if (s != Separator.None)
                sb.Append(ToSeparatorChar(s));
            sb.Append(r.Denominator == r.Denominator ? (r.Numerator + "/") : r.ToString());
            sb.Append(' ');
        }
        return sb.ToString();

    }


    public string ToStringRotationsBalBin() => "Not implemented";

    public string ToStringRepInfo()
    {

        return $"P={Period}";
    }

    public override string ToString() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";



}
