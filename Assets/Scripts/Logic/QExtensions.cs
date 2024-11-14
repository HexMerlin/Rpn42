using MathLib;
using MathLib.Misc;
using MathLib.Prime;
using MathLib.BalMult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


public static class QExtensions
{

    public static string ToStringNonAdjacentForm(this Q q)
    {
        static char ToC(int b) => b == 0 ? '0' : b == 1 ? '+' : '-';
        if (!q.IsInteger) return "";
        return Forms.ToNonAdjacentForm(q.Numerator).Select(ToC).Str();
    }

    public static string ToStringBalancedBinary(this Q q)
    {
        static char ToC(int b) => b == 1 ? '+' : '-';
        if (!q.IsInteger || q.Numerator.IsEven) return "";
        return Forms.ToBalancedBits(q.Numerator).Select(ToC).Str();
    }

    public static string ToStringBalDigitsCorrect(this Q q)
    {
        if (!q.IsInteger || q.Numerator.IsEven) return "";
        var (x, y) = Factors(q.Numerator);
        Product product = new Product(x, y);
        return product.Coeffs.Select(c => c.ToString()).Str(" ");
    }

    public static string ToStringBalDigitsPredicted(this Q q)
    {
        if (!q.IsInteger || q.Numerator.IsEven) return "";
        var (x, y) = Factors(q.Numerator);
        Product product = new Product(x, y);

        int[] balDigits = BalDigits.ToBalancedDigits(x * y, product.XLength, product.YLength);
        return balDigits.Select(c => c.ToString()).Str(" ");
    }

    private static (BigInteger x, BigInteger y) Factors(BigInteger integer)
    {
        var factorization = Primes.Factorization(integer);
        
        if (factorization.PrimeFactors.Length == 1)
            return (integer, 1);
        var x = factorization.PrimeFactors[0];
        var y = integer / x;
        return BigInteger.Abs(x) >= BigInteger.Abs(y) ? (x, y) : (y, x);
    }


    public static Q PadicGenerator(this Q q, int base_)
        => new Qp(q, base_).Generator;

    /// <summary>
    /// Divides the given rational number by the next Mersenne number greater than or equal to the numerator.
    /// </summary>
    /// <param name="mustBeCoprime">Indicates whether the Mersenne number must be coprime with the numerator.</param>
    /// <returns>A new <see cref="Q"/> instance resulting from the division.</returns>
    public static Q DivideByNextMersenneNumber(this Q q, bool mustBeCoprime = false)
        => q / NextMersenneNumber(q.Numerator, mustBeCoprime);

    /// <summary>
    /// Finds the next Mersenne number that is greater than or equal to the specified number.
    /// If <paramref name="mustBeCoprime"/> is true, the method continues searching until a Mersenne number is found
    /// that is coprime with the input number.
    /// </summary>
    /// <param name="num">The number for which the next Mersenne number is to be found.</param>
    /// <param name="mustBeCoprime">Specifies whether the Mersenne number must be coprime with <paramref name="num"/>.</param>
    /// <returns>The next Mersenne number that is equal to or larger than <paramref name="num"/>.</returns>
    public static BigInteger NextMersenneNumber(BigInteger num, bool mustBeCoprime)
    {
        num = BigInteger.Abs(num);

        BigInteger mersenne = 1;
        while (mersenne <= num + 1)
            mersenne <<= 1;

        if (mustBeCoprime)
            while (!BigInteger.GreatestCommonDivisor(num, mersenne - 1).IsOne)
                mersenne <<= 1;

        return mersenne - 1;
    }

    public static bool HasFiniteExpansion(this Q q, int base_)
    {
        if (base_ is not (2 or 3 or 5 or 7 or 10))
            throw new ArgumentOutOfRangeException(nameof(base_), base_, "Base must be 2, 3, 5, 7, or 10");

        if (base_ != 10)
            return q.Denominator.Abs().IsPowerOf(base_);


        var d = q.Denominator.Abs();
        while (d > 1)
        {
            if (d % 2 == 0)
                d /= 2;
            else if (d % 5 == 0)
                d /= 5;
            else
                return false;
        }

        return true;
    }
}

