using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using MathLib;

public static class StringParser
{
    /// <summary>
    /// Parses a number from a string denoting its positional representation.
    /// The string can contain an optional radix point.
    /// </summary>
    /// <remarks>
    /// The method handles numbers with arbitrary magnitude and precision.
    /// </remarks>
    /// <param name="input">The string representation of the number.</param>
    public static Q ParseNumber(string input, int base_)
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

    public static BigInteger[] TokenizeDistinctIntegers(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Array.Empty<BigInteger>();
        return Regex.Matches(input, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).Distinct().OrderBy(i => i).ToArray();

    }

    //public static string[] TokenizeNumberStrings(string input)
    //{
    //    return string.IsNullOrWhiteSpace(input)
    //        ? Array.Empty<string>()
    //        : Regex.Matches(input, @"-?\d*\.?\d+").Select(m => m.Value).ToArray();
    //}
}

