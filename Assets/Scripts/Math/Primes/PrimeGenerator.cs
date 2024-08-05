#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;


/// <summary>
/// Provides a method for generating prime numbers using a segmented sieve algorithm.
/// </summary>
/// <remarks>
/// The <see cref="PrimeGenerator"/> class uses a segmented sieve algorithm to efficiently generate prime numbers. 
/// The algorithm is designed to be cache-friendly by processing primes in segments or "pages", which helps in 
/// reducing memory usage and improving performance. This implementation is particularly useful for generating 
/// a large number of primes.
/// </remarks>
public static class PrimeGenerator
{

    /// <summary>
    /// Generates an enumerable sequence of prime numbers.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of prime numbers.</returns>
    /// <remarks>
    /// This method uses a segmented sieve algorithm to generate prime numbers. The algorithm divides the range 
    /// of numbers into smaller segments and processes each segment individually. This approach helps in keeping 
    /// the memory footprint low and improves cache performance.
    /// 
    /// The first prime number (2) is yielded separately, and subsequent primes are generated and yielded as the 
    /// algorithm processes each segment.
    /// </remarks>
    public static IEnumerable<int> GeneratePrimes()
    {
        const int PageSize = 1 << 14; // typical L1 CPU cache size in bytes
        const int BitsPerBuffer = PageSize * 8; // in bits
        const int BufferRange = BitsPerBuffer * 2;

        IEnumerator<int> basePrimeStream = Enumerable.Empty<int>().GetEnumerator();
        List<int> basePrimes = new List<int>();
        uint[] compositeBuffer = new uint[PageSize / 4]; // 4 byte words
        yield return 2; // The first prime number

        for (int lowerLimit = 0; ; lowerLimit += BitsPerBuffer)
        {
            for (var bufferIndex = 0; ; ++bufferIndex)
            {
                if (bufferIndex < 1)
                {
                    if (bufferIndex < 0)
                    {
                        bufferIndex = 0;
                        yield return 2; // Yield the first prime number again if necessary
                    }

                    int nextLimit = 3 + lowerLimit + lowerLimit + BufferRange;

                    if (lowerLimit <= 0)
                    {
                        // Cull the very first page
                        for (int i = 0, prime = 3, square = 9; square < nextLimit; i++, prime += 2, square = prime * prime)
                        {
                            if ((compositeBuffer[i >> 5] & (1 << (i & 31))) == 0)
                            {
                                for (int j = (square - 3) >> 1; j < BitsPerBuffer; j += prime)
                                {
                                    compositeBuffer[j >> 5] |= 1u << j;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Cull for the rest of the pages
                        Array.Clear(compositeBuffer, 0, compositeBuffer.Length);
                     
                        if (basePrimes.Count == 0)
                        {
                            // Initialize secondary base primes stream
                            basePrimeStream = GeneratePrimes().GetEnumerator();
                            basePrimeStream.MoveNext();
                            basePrimeStream.MoveNext();
                            basePrimes.Add(basePrimeStream.Current);
                            basePrimeStream.MoveNext();
                        }

                        // Ensure base primes array contains enough primes
                        for (int prime = basePrimes[^1], square = prime * prime; square < nextLimit;)
                        {
                            prime = basePrimeStream.Current;
                            basePrimeStream.MoveNext();
                            square = prime * prime;
                            basePrimes.Add(prime);
                        }

                        for (int i = 0, limit = basePrimes.Count - 1; i < limit; i++)
                        {
                            int prime = basePrimes[i];
                            int start = (prime * prime - 3) >> 1;

                            // Adjust start index based on page lower limit
                            if (start >= lowerLimit)
                                start -= lowerLimit;
                            else
                            {
                                int remainder = (lowerLimit - start) % prime;
                                start = (remainder != 0) ? prime - remainder : 0;
                            }

                            for (int j = start; j < BitsPerBuffer; j += prime)
                                compositeBuffer[j >> 5] |= 1u << j;
                            
                        }
                    }
                }

                while (bufferIndex < BitsPerBuffer && (compositeBuffer[bufferIndex >> 5] & (1 << (bufferIndex & 31))) != 0)
                    ++bufferIndex;


                if (bufferIndex < BitsPerBuffer)
                    yield return 3 + ((bufferIndex + lowerLimit) << 1);
                else break; // Outer loop for the next page segment

            }
        }
    }
}

