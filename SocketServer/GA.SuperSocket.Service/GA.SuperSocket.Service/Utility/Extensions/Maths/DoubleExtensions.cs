using System;
using System.Globalization;
using System.Linq;

namespace Globalegrow.Toolkit
{
    //TODO: Test
    public static class DoubleExtensions
    {
        /// <summary>
        /// <para>Divide by given divisor and round UP to the next whole number.</para>
        /// <para>For example, 10001 / 9000 = 1.111222 and will be rounded UP to 2</para>
        /// </summary>
        /// <param name="dividend">The dividend</param>
        /// <param name="divisor">The divisor</param>
        /// <returns>Quotient</returns>
        public static int DivideRoundUp(double dividend, double divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            if (divisor == -1 && dividend == int.MinValue)
            {
                throw new ArithmeticException(string.Concat("divisor = -1 and dividend = ", int.MinValue));
            }

            int roundedTowardsZeroQuotient = (int)(dividend / divisor);
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

        //TODO: Finish
        public static string ToFraction(this double d)
        {
            string s = d.ToString(CultureInfo.InvariantCulture);

            int length = s.Substring(s.IndexOf('.') + 1).Count();
            int numerator = int.Parse(s.Substring(s.IndexOf('.') + 1));
            int denominator = (int)System.Math.Pow((double)10.0, (double)(length));

            int gcd = Int32Extensions.GreatestCommonDivisor(numerator, denominator);

            return string.Concat(numerator / gcd, '/', denominator / gcd);
        }
    }
}