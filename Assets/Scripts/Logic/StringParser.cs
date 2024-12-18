﻿using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using MathLib;

public static class StringParser
{


    public static BigInteger[] TokenizeDistinctIntegers(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Array.Empty<BigInteger>();
        return Regex.Matches(input, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).Distinct().OrderBy(i => i).ToArray();

    }

}

