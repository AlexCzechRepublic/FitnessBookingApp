using System.Text.RegularExpressions;

namespace FitnessBookingApp.Utils
{
    public static class PhoneUtils
    {
        private static readonly Regex NonDigitPlus = new(@"[^\d+]", RegexOptions.Compiled);
        private static readonly Regex FullCz = new(@"^\+420\d{9}$", RegexOptions.Compiled);
        private static readonly Regex FullSk = new(@"^\+421\d{9}$", RegexOptions.Compiled);

        public static readonly string[] AllowedPrefixes = new[] { "+420", "+421" };

        public static bool TryNormalizeFull(string? input, out string normalized, out string error)
        {
            normalized = string.Empty;
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Telefon je povinný.";
                return false;
            }

            var s = input.Trim();
            s = NonDigitPlus.Replace(s, "");

            if (s.StartsWith("00"))
            {
                s = "+" + s[2..];
            }

            if (s.StartsWith("420") && s.Length >= 4) s = "+" + s;
            if (s.StartsWith("421") && s.Length >= 4) s = "+" + s;

            if (s.StartsWith("+4210")) s = "+421" + s.Substring(5);
            if (s.StartsWith("+4200")) s = "+420" + s.Substring(5);

            if (FullCz.IsMatch(s) || FullSk.IsMatch(s))
            {
                normalized = s;
                return true;
            }

            if (!s.StartsWith("+") && Regex.IsMatch(s, @"^\d{9}$"))
            {
                normalized = "+420" + s;
                return true;
            }

            if (Regex.IsMatch(s, @"^0\d{9}$"))
            {
                normalized = "+421" + s.Substring(1);
                return true;
            }

            error = "Telefon musí být +420/+421 a 9 číslic (např. +420 777 777 777).";
            return false;
        }

        public static bool TryNormalizeWithCountry(string? countryCode, string? localNumber, out string normalized, out string error)
        {
            normalized = string.Empty;
            error = string.Empty;

            countryCode = (countryCode ?? "").Trim();
            localNumber = (localNumber ?? "").Trim();

            if (!AllowedPrefixes.Contains(countryCode))
            {
                error = "Nepodporovaná předvolba. Použijte +420 nebo +421.";
                return false;
            }

            var digits = Regex.Replace(localNumber, @"\D", "");
            if (digits.StartsWith("0")) digits = digits[1..];

            if (!Regex.IsMatch(digits, @"^\d{9}$"))
            {
                error = "Zadejte 9 číslic bez mezer.";
                return false;
            }

            normalized = countryCode + digits;
            return true;
        }

        public static string FormatDisplay(string normalized)
        {
            if (FullCz.IsMatch(normalized) || FullSk.IsMatch(normalized))
            {
                var local = normalized.Substring(4);
                return $"{normalized.Substring(0, 4)} {local.Substring(0, 3)} {local.Substring(3, 3)} {local.Substring(6, 3)}";
            }
            return normalized;
        }

        public static bool IsAllowedNormalized(string s) => FullCz.IsMatch(s) || FullSk.IsMatch(s);
    }
}