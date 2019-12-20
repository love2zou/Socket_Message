using System;

namespace Globalegrow.Toolkit
{
    //TODO: Test
    public static class Int32Extensions
    {
        public static bool IsMultipleOf(this int i, int numberToCompare)
        {
            return i % numberToCompare == 0;
        }

        /// <summary>
        /// <para>Divide by given divisor and round UP to the next whole number.</para>
        /// <para>For example, 10001 / 9000 = 1.111222 and will be rounded UP to 2</para>
        /// </summary>
        /// <param name="dividend">The dividend</param>
        /// <param name="divisor">The divisor</param>
        /// <returns>Quotient</returns>
        public static int DivideRoundUp(int dividend, int divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            if (divisor == -1 && dividend == int.MinValue)
            {
                throw new ArithmeticException(string.Concat("divisor = -1 and dividend = ", int.MinValue));
            }

            int roundedTowardsZeroQuotient = dividend / divisor;
            bool dividedEvenly = (dividend % divisor) == 0;
            if (dividedEvenly)
            {
                return roundedTowardsZeroQuotient;
            }

            // At this point we know that divisor was not zero
            // (because we would have thrown) and we know that
            // dividend was not zero (because there would have been no remainder)
            // Therefore both are non-zero.  Either they are of the same sign,
            // or opposite signs. If they're of opposite sign then we rounded
            // UP towards zero so we're done. If they're of the same sign then
            // we rounded DOWN towards zero, so we need to add one.
            bool wasRoundedDown = ((divisor > 0) == (dividend > 0));
            if (wasRoundedDown)
            {
                return roundedTowardsZeroQuotient + 1;
            }
            else
            {
                return roundedTowardsZeroQuotient;
            }
        }

        public static int LowestCommonMultiple(int lhs, int rhs)
        {
            int max = lhs > rhs ? rhs : lhs;

            int lcm = -1;
            for (int i = 2; i <= max; i++)
            {
                if (lhs.IsMultipleOf(i) && rhs.IsMultipleOf(i))
                {
                    lcm = i;
                    break;
                }
            }

            return lcm;
        }

        public static int GreatestCommonDivisor(int lhs, int rhs)
        {
            int i = 0;
            while (true)
            {
                i = lhs % rhs;
                if (i == 0)
                {
                    return rhs;
                }

                lhs = rhs;
                rhs = i;
            }
        }

        public static string ToFraction(this int i)
        {
            return ((double)i).ToFraction();
        }
    }
}