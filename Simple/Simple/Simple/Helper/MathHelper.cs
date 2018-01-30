using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple
{
    public class MathHelper
    {
    
        //http://blog.csdn.net/arvonzhang/article/details/8564836

        //1.费马小定理：N^P-N≡0(mod P)，P为素数，N不能被P整除,≡同余表示等式两边mod p的余数相等,或者写作N^P≡N(mod P)
        //N*（N^(P-1) - 1） %  P = 0，说明N*（N^(P-1) - 1）可以被P整除，显然也可以被N整除
        //公倍数：两个不同的自然数A和B，若有自然数C可以被A整除也可以被B整除，那么C就是A和B的公倍数。
        //则存在整数M满足N*（N^(P-1) - 1） = M * N * P 
        //则N^(P-1) - 1 = M * P，说明N^(P-1) - 1是P的倍数
        //即(N^(P-1) - 1) % P = 0,即N^(P-1) % P = 1，这个可以结合下面的Montgomery来判断是否是“素数”
        //存在伪素数满足费马小定理，伪素数其实是合数，伪素数有无穷多个

        //2.积模分解公式：如果有：X%Z=0，即X能被Z整除，则有：(X+Y)%Z=Y%Z
        //设有X、Y和Z三个正整数，则必有：(X*Y)%Z=((X%Z)*(Y%Z))%Z

        //http://blog.csdn.net/jiange_zh/article/details/50684528
        //快速幂算法
        public static uint Power(uint n, uint power)
        {
            uint result = 1;
            while(power > 0)
            {
                if ((power & 1) != 0)
                    result *= n;
                n *= n;
                power >>= 1;
            }
            return result;
        }

        //http://blog.csdn.net/ltyqljhwcm/article/details/53043646
        //计算n ^ power % divisor,蒙哥马利快速幂模算法
        public static uint Montgomery(uint n, uint power, uint divisor)
        {
            n = n % divisor;
            uint result = 1;
            while(power > 0)
            {
                if((power & 1) != 0)
                    result = (result * n) % divisor;
                n = (n * n) % divisor;
                power >>= 1;
            }
            return result;
        }

        //https://opensource.apple.com/source/libcpp/libcpp-31/src/hash.cpp?txt   
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

        //不小于threshold的数的index，内部函数，array升序排列，threshold小于等于array里面最大的数
        private static int LowerBoundIndex(uint[] array, uint threshold)
        {
            return LowerBoundIndex(array, 0, array.Length - 1, threshold);
        }

        private static int LowerBoundIndex(uint[] array, int begin, int end, uint threshold)
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
                return LowerBoundIndex(array, begin, half, threshold);
            return LowerBoundIndex(array, half, end, threshold);
        }

        //返回最接近n的素数(不小于n)
        public static uint NextPrime(uint n)
        {
            uint L = 210;
            int N = small_primes.Length;
            // If n is small enough, search in small_primes
            if (n <= small_primes[N - 1])
                return small_primes[LowerBoundIndex(small_primes, n)];
            // Else n > largest small_primes
            // Start searching list of potential primes: L * k0 + indices[in]
            int M = indices.Length; ;
            // Select first potential prime >= n
            //   Known a-priori n >= L
            uint k0 = n / L;
            int index = LowerBoundIndex(indices, n - k0 * L);
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
