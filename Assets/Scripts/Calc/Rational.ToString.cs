﻿
using System;
using System.Linq;
using System.Numerics;
using System.Text;

public partial struct Rational
{

    public const char RadixPointChar = '.';

    private static bool AddDelimiterChars(Rational r, StringBuilder sb)
    {
        if (r.IsRadixPoint)
            sb.Append(RadixPointChar);
        else if (r.IsRepetendStart)
            sb.Append("<color=lightBlue>");
        else if (r.IsRepetendEnd)
            sb.Append("</color>");
        
        return r.IsSpecialDelimiter;
    }

    public string ToStringFraction() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";


    public string ToStringPartition() => $"{string.Join(" ", Partition.Select(r => r.ToString()))}";

    public string ToStringRotationsBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Rational r in RotationsBin)
        {
            if (!AddDelimiterChars(r, sb))
            {
                sb.Append(r.Denominator == Denominator ? (r.Numerator + "/") : r.ToString());
                sb.Append(' ');
            }
        }
  
        return sb.ToString();

    }

    public string ToStringRotationsBalBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Rational r in RotationsBalBin)
        {
            if (!AddDelimiterChars(r, sb))
            {
                sb.Append(r.Denominator == Denominator ? (r.Numerator + "/") : r.ToString());
                sb.Append(' ');
            }
        }
      
        return sb.ToString();

    }

    public string ToStringBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Rational r in RotationsBin)
        {
            if (!AddDelimiterChars(r, sb))
                sb.Append(r >= Half ? '1' : '0');
        }

        return sb.ToString();
    }

    public string ToStringBalBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Rational r in RotationsBalBin)
        {
            if (!AddDelimiterChars(r, sb))
            {
                sb.Append(r.Denominator == Denominator ? (r.Numerator + "/") : r.ToString());
                sb.Append(' ');
            }
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

        result.Append(RadixPointChar);

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