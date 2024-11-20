
using System.Globalization;

namespace BT.Shared.Helpers
{
    public static class StringHelpers
    {
        static TextInfo TextInfo = new CultureInfo("en-GB", false).TextInfo;

        public static string ToTitleCase(string text) => TextInfo.ToTitleCase(text);

        /// <summary>
        /// Truncate a string to a set size.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        /// public static string Truncate(this string value, int maxLength)
        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="maxlen"></param>
        /// <returns></returns>
        /// public static string ShortenAndFormatText(this string val, int maxlen) with trailing ...
        public static string ShortenAndFormatText(string val, int maxlen)
        {
            const string postFix = "...";

            if (string.IsNullOrEmpty(val)) return val;

            if (val.Length > maxlen)
            {
                var truncateFirst = Truncate(val, (maxlen - postFix.Length));
                var truncateLast = truncateFirst + postFix;
                return truncateLast;
            }
            else
            {
                return val;
            }
        }
    }
}
