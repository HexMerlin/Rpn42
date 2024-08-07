#nullable enable

using System;
using System.Linq;
using System.Numerics;

/// <summary>
/// Represents the result of factoring an integer into its prime factors.
/// </summary>
public sealed class Factorization
{
    /// <summary>
    /// The prime factors of the integer in increasing order.
    /// </summary>
    public readonly int[] PrimeFactors;

    /// <summary>
    /// The remainder factor of the integer after factoring.
    /// If the input integer was not fully factored, a remainder factor > 1 is left.
    /// If the input integer was negative, the remainder factor is negative.
    /// </summary>
    public readonly BigInteger RemainderFactor;

    /// <summary>
    /// A value indicating whether the factorization is complete to only primes.
    /// Returns true if the factorization is complete, meaning there is no remainder factor other than 1.
    /// Returns false if the factorization is partial, meaning there is a remainder factor that could be potentially composite.
    /// </summary>
    public bool IsComplete => RemainderFactor.IsZero || RemainderFactor.Abs().IsOne;

    /// <summary>
    /// Gets a value indicating whether the factored number is 0.
    /// </summary>
    public bool IsZero => RemainderFactor.IsZero;
    
    /// <summary>
    /// Gets a value indicating whether the factored number is 1.
    /// </summary>
    public bool IsOne => PrimeFactors.Length == 0 && RemainderFactor.IsOne;

    /// <summary>
    /// The count of factors in the factorization.
    /// </summary>
    public int FactorCount => Math.Max(1, PrimeFactors.Length + (RemainderFactor.IsOne ? 0 : 1));

    /// <summary>
    /// Initializes a new instance of the <see cref="Factorization"/> class.
    /// </summary>
    /// <param name="primeFactors">The prime factors of the integer in increasing order.</param>
    /// <param name="remainderFactor">The remainder factor of the integer after factoring.</param>
    public Factorization(int[] primeFactors, BigInteger remainderFactor)
    {
        PrimeFactors = primeFactors;
        RemainderFactor = remainderFactor;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Factorization"/> object is equal to the current object.
    /// </summary>
    /// <param name="other">The <see cref="Factorization"/> object to compare against.</param>
    /// <returns>true if the objects are equal; otherwise, false.</returns>
    public bool Equals(Factorization? other)
    => other != null
       && RemainderFactor == other.RemainderFactor
       && PrimeFactors.SequenceEqual(other.PrimeFactors);

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is Factorization other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = RemainderFactor.GetHashCode();
            for (int i = 0; i < PrimeFactors.Length;)
                hash ^= PrimeFactors[i].GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Gets the factored integer by multiplying all the prime factors with the remainder factor.
    /// </summary>
    public BigInteger FactoredInteger => PrimeFactors.Aggregate(RemainderFactor, (acc, prime) => acc * prime);

    /// <summary>
    /// Returns a string representation of the <see cref="Factorization"/> object.
    /// </summary>
    /// <returns>A string representation of the <see cref="Factorization"/> object.</returns>
    public override string ToString()
    {
        string remainderString = PrimeFactors.Length == 0
            ? RemainderFactor.ToString()
            : RemainderFactor == 1
                ? string.Empty
                : " · " + RemainderFactor;
        if (!IsComplete)
            remainderString += '?';

        return string.Join(" · ", PrimeFactors) + remainderString;
    }
}

