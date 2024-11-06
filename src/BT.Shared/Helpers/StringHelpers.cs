
using System.Globalization;

namespace BT.Shared.Helpers
{
    public static class StringHelpers
    {
        static TextInfo TextInfo = new CultureInfo("en-GB", false).TextInfo;

        public static string ToTitleCase(string text) => TextInfo.ToTitleCase(text);
    }
}
