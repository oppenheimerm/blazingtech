using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace BT.Admin.Helpers
{
    public static class FileHelpers
    {
        private static string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private static readonly List<string> ReservedCharacters = new List<string>() { "\"!\", \"#\", \"$\", \"&\", \"'\", \"(\", \")\", \"*\", \",\", \"/\", \":\", \";\", \"=\", \"?\", \"@\", \"[\", \"]\", \"\\\"\", \"%\", \".\", \"<\", \">\", \"\\\\\", \"^\", \"_\", \"'\", \"{\", \"}\", \"|\", \"~\", \"`\", \"+\"" };
        public static readonly string RootDirectory = "img\\ProductImages";
        public static readonly string Space = " ";
        public static readonly string Dash = "-";

        public static bool ValidImageFile(IBrowserFile file)
        {
            var ext = Path.GetExtension(file.Name.ToLowerInvariant());
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "The name should be lower case.")]
        public static string SanitizeName(string name)
        {
            name = name?.ToLowerInvariant().Replace(
                Space, Dash, StringComparison.OrdinalIgnoreCase) ?? string.Empty;
            name = RemoveDiacritics(name);
            name = RemoveReservedUrlCharacters(name);

            return name.ToLowerInvariant();
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string RemoveReservedUrlCharacters(string text)
        {
            foreach (var chr in ReservedCharacters)
            {
                text = text.Replace(chr, string.Empty, StringComparison.OrdinalIgnoreCase);
            }
            return text;
        }

    }
}
