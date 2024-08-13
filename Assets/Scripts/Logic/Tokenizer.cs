using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class Tokenizer
{
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

