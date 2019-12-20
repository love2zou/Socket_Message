using System.Text;

namespace Globalegrow.Toolkit
{
    public static class CharExtensions
    {
        /// <summary>
        /// <para>Takes a System.Char and returns a System.String of that System.Char</para>
        /// <para>repeated [n] number of times</para>
        /// </summary>
        /// <param name="c">The Char</param>
        /// <param name="count">The number of times to repeat the System.Char</param>
        /// <returns>System.String of the specified System.Char repeated [n] number of times</returns>
        public static string Repeat(this char c, byte count)
        {
            if (count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(byte.MaxValue);
            for (int i = 0; i < count; i++)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}