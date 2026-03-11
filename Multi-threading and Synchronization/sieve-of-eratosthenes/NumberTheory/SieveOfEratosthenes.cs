namespace NumberTheory;

public static class SieveOfEratosthenes
{
    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a sequential approach.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersSequentialAlgorithm(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        bool[] isPrime = new bool[n + 1];
        for (int i = 2; i <= n; i++)
        {
            isPrime[i] = true;
        }

        for (int p = 2; p * p <= n; p++)
        {
            if (isPrime[p])
            {
                for (int i = p * p; i <= n; i += p)
                {
                    isPrime[i] = false;
                }
            }
        }

        var primes = new List<int>();
        for (int i = 2; i <= n; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a modified sequential approach.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersModifiedSequentialAlgorithm(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        int limit = (int)Math.Sqrt(n);
        var basePrimes = GetPrimeNumbersSequentialAlgorithm(limit).ToList();

        bool[] isPrime = new bool[n + 1];
        var primes = new List<int>(basePrimes);

        if (n > limit)
        {
            for (int i = limit + 1; i <= n; i++)
            {
                isPrime[i] = true;
            }

            foreach (var p in basePrimes)
            {
                int start = limit + 1;
                int firstMultiple = start + (p - (start % p)) % p;
                if (firstMultiple < p * p)
                {
                    firstMultiple = p * p;
                }

                for (int i = firstMultiple; i <= n; i += p)
                {
                    isPrime[i] = false;
                }
            }

            for (int i = limit + 1; i <= n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a concurrent approach by data decomposition.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentDataDecomposition(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        int limit = (int)Math.Sqrt(n);
        var basePrimes = GetPrimeNumbersSequentialAlgorithm(limit).ToList();

        bool[] isPrime = new bool[n + 1];
        var primes = new List<int>(basePrimes);

        if (n > limit)
        {
            for (int i = limit + 1; i <= n; i++)
            {
                isPrime[i] = true;
            }

            int rangeStart = limit + 1;
            int rangeEnd = n;
            int rangeLength = rangeEnd - rangeStart + 1;
            int numThreads = Environment.ProcessorCount;
            int chunkSize = (int)Math.Ceiling((double)rangeLength / numThreads);

            var threads = new List<Thread>();

            for (int t = 0; t < numThreads; t++)
            {
                int startIdx = rangeStart + t * chunkSize;
                if (startIdx > rangeEnd)
                {
                    break;
                }

                int endIdx = Math.Min(rangeEnd, startIdx + chunkSize - 1);

                var thread = new Thread(() =>
                {
                    foreach (var p in basePrimes)
                    {
                        int firstMultiple = startIdx + (p - (startIdx % p)) % p;
                        if (firstMultiple < p * p)
                        {
                            firstMultiple = p * p;
                        }

                        for (int i = firstMultiple; i <= endIdx; i += p)
                        {
                            isPrime[i] = false;
                        }
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            for (int i = limit + 1; i <= n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a concurrent approach by "basic" primes decomposition.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentBasicPrimesDecomposition(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        int limit = (int)Math.Sqrt(n);
        var basePrimes = GetPrimeNumbersSequentialAlgorithm(limit).ToList();

        bool[] isPrime = new bool[n + 1];
        var primes = new List<int>(basePrimes);

        if (n > limit)
        {
            for (int i = limit + 1; i <= n; i++)
            {
                isPrime[i] = true;
            }

            int numThreads = Environment.ProcessorCount;
            int numPrimes = basePrimes.Count;
            int chunkSize = numPrimes == 0 ? 1 : (int)Math.Ceiling((double)numPrimes / numThreads);

            var threads = new List<Thread>();

            for (int t = 0; t < numThreads; t++)
            {
                int startIdx = t * chunkSize;
                if (startIdx >= numPrimes)
                {
                    break;
                }

                int endIdx = Math.Min(numPrimes - 1, startIdx + chunkSize - 1);
                var threadPrimes = basePrimes.GetRange(startIdx, endIdx - startIdx + 1);

                var thread = new Thread(() =>
                {
                    foreach (var p in threadPrimes)
                    {
                        int start = limit + 1;
                        int firstMultiple = start + (p - (start % p)) % p;
                        if (firstMultiple < p * p)
                        {
                            firstMultiple = p * p;
                        }

                        for (int i = firstMultiple; i <= n; i += p)
                        {
                            isPrime[i] = false;
                        }
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            for (int i = limit + 1; i <= n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using thread pool and signaling construct.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentWithThreadPool(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        int limit = (int)Math.Sqrt(n);
        var basePrimes = GetPrimeNumbersSequentialAlgorithm(limit).ToList();

        bool[] isPrime = new bool[n + 1];
        var primes = new List<int>(basePrimes);

        if (n > limit)
        {
            for (int i = limit + 1; i <= n; i++)
            {
                isPrime[i] = true;
            }

            if (basePrimes.Count > 0)
            {
                using var countdownEvent = new CountdownEvent(basePrimes.Count);

                foreach (var p in basePrimes)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            int start = limit + 1;
                            int firstMultiple = start + (p - (start % p)) % p;
                            if (firstMultiple < p * p)
                            {
                                firstMultiple = p * p;
                            }

                            for (int i = firstMultiple; i <= n; i += p)
                            {
                                isPrime[i] = false;
                            }
                        }
                        finally
                        {
                            countdownEvent.Signal();
                        }
                    });
                }

                countdownEvent.Wait();
            }

            for (int i = limit + 1; i <= n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }
        }

        return primes;
    }
}
