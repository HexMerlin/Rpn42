﻿
using System;
using System.Linq;
using System.Numerics;
using System.Text;

public partial struct Rational
{
    private static char? ToSeparatorChar(Separator separator) => separator switch
    {
        Separator.RadixPoint => '⠄',
        Separator.RepetendBegin => '⠁',
        Separator.RadixPointAndRepetendBegin => '⠅',
        //Separator.RepetendEnd => '…',
        _ => null
    };

    public string ToStringFraction() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";


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

    public string ToStringBalBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach ((Rational r, Separator s) in RotationsBalBin)
        {
            if (s != Separator.None)
                sb.Append(ToSeparatorChar(s));
            sb.Append(r.Numerator.IsOdd() ? '1' : '0');
        }
        return sb.ToString();
    }

    public string ToStringRotationsBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach ((Rational r, Separator s) in RotationsBin)
        {
            if (s != Separator.None)
                sb.Append(ToSeparatorChar(s));
            sb.Append(r.Denominator == Denominator ? (r.Numerator + "/") : r.ToString());
            sb.Append(' ');
        }
        return sb.ToString();

    }


    public string ToStringRotationsBalBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach ((Rational r, Separator s) in RotationsBalBin)
        {
            if (s != Separator.None)
                sb.Append(ToSeparatorChar(s));
            sb.Append(r.Denominator == Denominator ? (r.Numerator + "/") : r.ToString());
            sb.Append(' ');
        }
        return sb.ToString();

    }


    public readonly string ToStringDecimal(int maxDecimalDigits = 50)
    {
      
        BigInteger integerPart = BigInteger.DivRem(Numerator.Abs(), Denominator, out BigInteger remainder);
        
        // Handle the fractional part
        StringBuilder result = new StringBuilder();
        if (Numerator < 0) result.Append("-");

        result.Append(integerPart);
        if (remainder.IsZero)
            return result.ToString();

        result.Append('.');

        for (int i = 0; i < maxDecimalDigits; i++)
        {
            remainder *= 10;
            BigInteger digit = BigInteger.Divide(remainder, Denominator);
            result.Append(digit);
            remainder = BigInteger.Remainder(remainder, Denominator);
            // If the remainder is zero, we can stop early
            if (remainder == 0)
                break;
        }

        return result.ToString();
    }

    public string ToStringRepInfo() => $"P={Period}";

    public override readonly string ToString() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";


}