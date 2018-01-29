using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple
{
    public class MathHelper
    {
        //https://opensource.apple.com/source/libcpp/libcpp-31/src/hash.cpp?txt   
        //积模分解公式 （(A%C)*(B%C)）%C=(A*B)%C  
        //对于指定的数Value，如果事先建立一张足够大的素数表PrimeSet, 
        //对于PrimeSet每个数N都有 N * (Value - 1) % Value == 1,则可以粗略的认为Value是素数，否则不是 
        //handle all next_prime(i) for i in [1, 210), special case 0 
        static uint[] small_primes ={
                              0,   2,   3,   5,   7,   11,  13,  17,  19,  23,
                              29,  31,  37,  41,  43,  47,  53,  59,  61,  67,
                              71,  73,  79,  83,  89,  97,  101, 103, 107, 109,
                              113, 127, 131, 137, 139, 149, 151, 157, 163, 167,
                              173, 179, 181, 191, 193, 197, 199, 211
                          };
        //potential primes = 210*k + indices[i], k >= 1 
        // these numbers are not divisible by 2, 3, 5 or 7 
        //(or any integer 2 <= j <= 10 for that matter).
        static uint[] indices ={
                        1,   11,  13,  17,  19,  23,  29,  31,  37,  41,
                        43,  47,  53,  59,  61,  67,  71,  73,  79,  83,
                        89,  97,  101, 103, 107, 109, 113, 121, 127, 131,
                        137, 139, 143, 149, 151, 157, 163, 167, 169, 173,
                        179, 181, 187, 191, 193, 197, 199, 209
                        };

        //return the index
        private static int LowerBound(uint[] array, uint threshold)
        {
            return LowerBound(array, 0, array.Length - 1, threshold);
        }

        private static int LowerBound(uint[] array, int begin, int end, uint threshold)
        {
            int length = end - begin + 1;
            if (length < 3)
            {
                if (array[begin] >= threshold)
                    return begin;
                return end;
            }
            int half = begin + length / 2;
            if (array[half] >= threshold)
                return LowerBound(array, begin, half, threshold);
            return LowerBound(array, half, end, threshold);
        }

        public static uint NextPrime(uint n)
        {
            uint L = 210;
            int N = small_primes.Length;
            // If n is small enough, search in small_primes
            if (n <= small_primes[N - 1])
                return small_primes[LowerBound(small_primes, n)];
            // Else n > largest small_primes
            // Start searching list of potential primes: L * k0 + indices[in]
            int M = indices.Length; ;
            // Select first potential prime >= n
            //   Known a-priori n >= L
            uint k0 = n / L;
            int index = LowerBound(indices, n - k0 * L);
            n = L * k0 + indices[index];
            while (true)
            {
                // Divide n by all primes or potential primes (i) until:
                //    1.  The division is even, so try next potential prime.
                //    2.  The i > sqrt(n), in which case n is prime.
                // It is known a-priori that n is not divisible by 2, 3, 5 or 7,
                //    so don't test those (j == 5 ->  divide by 11 first).  And the
                //    potential primes start with 211, so don't test against the last
                //    small prime.
                for (uint j = 5; j < N - 1; ++j)
                {
                    uint p = small_primes[j];
                    uint q = n / p;
                    if (q < p)
                        return n;
                    if (n == q * p)
                        goto next;
                }
                // n wasn't divisible by small primes, try potential primes
                {
                    uint i = 211;
                    while (true)
                    {
                        uint q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 10;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 8;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 8;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 6;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 4;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 2;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        i += 10;
                        q = n / i;
                        if (q < i)
                            return n;
                        if (n == q * i)
                            break;

                        // This will loop i to the next "plane" of potential primes
                        i += 2;
                    }
                }
            next:
                // n is not prime.  Increment n to next potential prime.
                if (++index == M)
                {
                    ++k0;
                    index = 0;
                }
                n = L * k0 + indices[index];
            }
        }
    }
}
