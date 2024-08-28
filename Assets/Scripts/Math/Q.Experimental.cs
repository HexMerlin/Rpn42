//#nullable enable
//using System;
//using System.Numerics;

//public partial class Q
//{

//    public Q DivideByNextMersenneNumber(bool mustBeCoprime = false) => this / NextMersenneNumber(Numerator, mustBeCoprime);

//    public static BigInteger NextMersenneNumber(BigInteger num, bool mustBeCoprime)
//    {
//        num = BigInteger.Abs(num);

//        BigInteger mersenne = 1;
//        while (mersenne <= num + 1)
//            mersenne <<= 1;

//        if (mustBeCoprime)
//            while (!BigInteger.GreatestCommonDivisor(num, mersenne - 1).IsOne)
//                mersenne <<= 1;

//        return mersenne - 1;
//    }

//    public Q RepetendShiftLeft()
//    {
//        int period = Period;
//        BigInteger num = (BigInteger.One << period) - 1;
//        BigInteger den = (BigInteger.One << (period - 1)) - 1;
//        return this * new Q(num, den, false);
//    }

//    public Q RepetendShiftRight()
//    {
//        int period = Period;
//        BigInteger num = (BigInteger.One << period) - 1;
//        BigInteger den = (BigInteger.One << (period + 1)) - 1;
//        return this * new Q(num, den, false);
//    }

//    public static Q FindUnitFractionWithRepetendFactor(Q repetendFactor)
//    {
//        if (!repetendFactor.IsInteger)
//            return Invalid;

//        BigInteger repetendFactorToFind = repetendFactor.Numerator;
//        for (int i = 3; i < 20000; i += 2)
//        {
//            Q r = new(1, i);
//            BigInteger repetendAsInt = r.RepetendAsInteger;
//            if (repetendAsInt.IsZero)
//                continue;
//            if (repetendAsInt % repetendFactorToFind == 0)
//                return r;
//        }
//        return Invalid;
//    }

//    public BigInteger AsBalanced()
//    {
//        BigInteger result = BigInteger.Zero;

//        foreach (Q r in RotationsBin)
//        {
//            if (r.IsSpecialDelimiter)
//                continue;
//            result <<= 1;
//            bool isOne = r >= Half;
//            if (isOne)
//                result++;
//            else
//                result--;
//        }
//        return result;
//    }

//}