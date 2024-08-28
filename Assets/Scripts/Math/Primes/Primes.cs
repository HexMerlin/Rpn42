//#nullable enable
//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Numerics;
//using System.Threading;
//using System.Threading.Tasks;

//public class Primes
//{
//    private static readonly Lazy<Primes> instance = new Lazy<Primes>(() => new Primes(MaxSupportedPrime), LazyThreadSafetyMode.ExecutionAndPublication);

//    // Lock to enforce only one factorization method running at a time
//    private static readonly object factorizationLock = new object();

//    private readonly int[][] primeArrays;

//    private const int MaxSupportedPrime = 2000000000; //2000000000;
        
//    private Primes(int maxPrime, int threadCount = -1)
//    {
//        if (threadCount <= 0) 
//            threadCount = Environment.ProcessorCount;
//        maxPrime = Math.Min(maxPrime, MaxSupportedPrime);
     
//        this.primeArrays = PrimeArrayChunks(maxPrime, threadCount);
//    }

//    public static bool IsReady => instance.IsValueCreated;

//    private static int[][] PrimeArrayChunks(int maxPrime, int chunkCount)
//    {
//        List<int>[] primeLists = Enumerable.Range(0, chunkCount).Select(_ => new List<int>()).ToArray();

//        int primeCount = 0;

//        foreach (int prime in PrimeGenerator.GeneratePrimes())
//        {
//            primeCount++;
//            primeLists[primeCount % chunkCount].Add(prime);
//            if (prime >= maxPrime) //stop when we have a prime >= maxPrime
//                break;
//        }

//        int[][] primeArrays = new int[chunkCount][];
//        for (int i = 0; i < chunkCount; i++)
//            primeArrays[i] = primeLists[i].ToArray();
//        return primeArrays;
//    }
//    public static void Prepare(Action? instanceReadyCallback = null)
//    {
//        if (instance.IsValueCreated)
//            instanceReadyCallback?.Invoke();
//        else
//            _ = Task.Run(() => {
//            Primes _ = instance.Value; // Force creation of the instance
//            instanceReadyCallback?.Invoke(); // Signal that creation is complete
//            });
//    }

//    public static Factorization Factorization(BigInteger integer)
//    {
//        lock (factorizationLock)
//        {
//            Factorization factorization = instance.Value.InstanceFactorization(integer);
//            return factorization;
//        }
//    }

//    private Factorization InstanceFactorization(BigInteger dividend)
//    {
//        if (dividend == 0)
//            return new Factorization(Array.Empty<int>(), 0);

//        int sign = dividend.Sign;
//        dividend = BigInteger.Abs(dividend);

//        List<int> primeFactors = new List<int>();

//        using ReaderWriterLockSlim integerAndPrimeFactorsLock = new ReaderWriterLockSlim();
   
//        Parallel.ForEach(primeArrays, primeArray =>
//        {
//            BigInteger localDividend = -1; //will be updated in the loop
//            List<int> localPrimeFactors = new List<int>();

//            for (int index = 0; index < primeArray.Length; index++)
//            {
//                if (index % 1024 == 0)
//                {
//                    integerAndPrimeFactorsLock.EnterReadLock();
//                    localDividend = dividend; //update dividend integer occasionally
//                    integerAndPrimeFactorsLock.ExitReadLock();
//                }
//                int prime = primeArray[index];
//                if (prime > localDividend)
//                    break; //no need to continue

//                if (localDividend % prime == 0) //found a prime factor
//                {
//                    integerAndPrimeFactorsLock.EnterWriteLock();
//                    dividend /= prime; 
//                    localDividend = dividend;
//                    integerAndPrimeFactorsLock.ExitWriteLock();

//                    localPrimeFactors.Add(prime);
//                    index--; //stay on the same prime, as it may divide multiple times
//                    continue;
//                }
              
//            }
//            integerAndPrimeFactorsLock.EnterWriteLock();
//            primeFactors.AddRange(localPrimeFactors);
//            integerAndPrimeFactorsLock.ExitWriteLock();


//        });

//        return new Factorization(primeFactors.OrderBy(p => p).ToArray(), dividend * sign);
//    }
//}

