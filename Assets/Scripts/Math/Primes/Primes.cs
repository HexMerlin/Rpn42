
using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

public class Primes
{
    private static Primes instance = new Primes();

    private static readonly object instanceLock = new object();

    private readonly int[][] primeArrays;

    public const int MaxSupportedPrime = 2000000000;

    private Primes()
    {
        this.primeArrays = Array.Empty<int[]>();

    }

    private int CurrentMaxPrime => primeArrays.Length == 0 ? 0 : primeArrays.Max(primeArray => primeArray.LastOrDefault());

    private Primes(int maxPrime, int threadCount = -1)
    {
        if (threadCount <= 0) 
            threadCount = Environment.ProcessorCount;
        maxPrime = Math.Min(maxPrime, MaxSupportedPrime);
      
        this.primeArrays = PrimeArrayChunks(maxPrime, threadCount);
    }

    private static int[][] PrimeArrayChunks(int maxPrime, int chunkCount)
    {
        List<int>[] primeLists = Enumerable.Range(0, chunkCount).Select(_ => new List<int>()).ToArray();

        int primeCount = 0;

        foreach (int prime in PrimeGenerator.GeneratePrimes())
        {
            primeCount++;
            primeLists[primeCount % chunkCount].Add(prime);
            if (prime >= maxPrime) //stop when we have a prime >= maxPrime
                break;
        }

        int[][] primeArrays = new int[chunkCount][];
        for (int i = 0; i < chunkCount; i++)
            primeArrays[i] = primeLists[i].ToArray();
        return primeArrays;
    }

    public static Factorization Factorization(BigInteger integer)
    {
        lock (instanceLock)
        {
            Factorization factorization = instance.InstanceFactorization(integer);
            if (factorization.IsComplete)
                return factorization;

            
            int maxPrime = (int) BigInteger.Min(MaxSupportedPrime, factorization.RemainderFactor.Abs());
            if (maxPrime <= instance.CurrentMaxPrime) //maxPrime is already calculated, return the partial factorization
                return factorization;

            instance = new Primes(maxPrime);  

            Factorization remFact = instance.InstanceFactorization(factorization.RemainderFactor); //redo factorization of the remainder

            return new Factorization(factorization.PrimeFactors.Concat(remFact.PrimeFactors).ToArray(), remFact.RemainderFactor);

        }
    }

    private Factorization InstanceFactorization(BigInteger dividend)
    {
        if (dividend == 0)
            return new Factorization(Array.Empty<int>(), 0);

        int sign = dividend.Sign;
        dividend = BigInteger.Abs(dividend);

        List<int> primeFactors = new List<int>();

        using ReaderWriterLockSlim integerAndPrimeFactorsLock = new ReaderWriterLockSlim();
   
        Parallel.ForEach(primeArrays, primeArray =>
        {
            BigInteger localDividend = -1; //will be updated in the loop
            List<int> localPrimeFactors = new List<int>();

            for (int index = 0; index < primeArray.Length; index++)
            {
                if (index % 1024 == 0)
                {
                    integerAndPrimeFactorsLock.EnterReadLock();
                    localDividend = dividend; //update dividend integer occasionally
                    integerAndPrimeFactorsLock.ExitReadLock();
                }
                int prime = primeArray[index];
                if (prime > localDividend)
                    break; //no need to continue

                if (localDividend % prime == 0) //found a prime factor
                {
                    integerAndPrimeFactorsLock.EnterWriteLock();
                    dividend /= prime; 
                    localDividend = dividend;
                    integerAndPrimeFactorsLock.ExitWriteLock();

                    localPrimeFactors.Add(prime);
                    index--; //stay on the same prime, as it may divide multiple times
                    continue;
                }
              
            }
            integerAndPrimeFactorsLock.EnterWriteLock();
            primeFactors.AddRange(localPrimeFactors);
            integerAndPrimeFactorsLock.ExitWriteLock();


        });

        return new Factorization(primeFactors.OrderBy(p => p).ToArray(), dividend * sign);
    }
}
