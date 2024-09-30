using MathLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


public static class QExtensions
{
    /// <summary>
    /// Divides the given rational number by the next Mersenne number greater than or equal to the numerator.
    /// </summary>
    /// <param name="mustBeCoprime">Indicates whether the Mersenne number must be coprime with the numerator.</param>
    /// <returns>A new <see cref="Q"/> instance resulting from the division.</returns>
    public static Q DivideByNextMersenneNumber(Q q, bool mustBeCoprime = false)
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
}

