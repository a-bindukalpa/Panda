using System.Text.RegularExpressions;

namespace Panda.Utils
{
    public static class PostcodeUtil
    {
        // Regex patterns
        public static readonly Regex DISTRICT_SPLIT_REGEX = new(@"^([a-z]{1,2}\d)([a-z])$", RegexOptions.IgnoreCase);
        public static readonly Regex UNIT_REGEX = new(@"[a-z]{2}$", RegexOptions.IgnoreCase);
        public static readonly Regex INCODE_REGEX = new(@"\d[a-z]{2}$", RegexOptions.IgnoreCase);
        public static readonly Regex OUTCODE_REGEX = new(@"^[a-z]{1,2}\d[a-z\d]?$", RegexOptions.IgnoreCase);
        public static readonly Regex POSTCODE_REGEX = new(@"^[a-z]{1,2}\d[a-z\d]?\s*\d[a-z]{2}$", RegexOptions.IgnoreCase);
        public static readonly Regex POSTCODE_CORPUS_REGEX = new(@"[a-z]{1,2}\d[a-z\d]?\s*\d[a-z]{2}", RegexOptions.IgnoreCase);
        public static readonly Regex AREA_REGEX = new(@"^[a-z]{1,2}", RegexOptions.IgnoreCase);
        public static readonly Regex FIXABLE_REGEX = new(@"^\s*[a-z01]{1,2}[0-9oi][a-z\d]?\s*[0-9oi][a-z01]{2}\s*$", RegexOptions.IgnoreCase);
        private static readonly Regex SPACE_REGEX = new(@"\s+", RegexOptions.IgnoreCase);

        // Sanitizes string: removes spaces and uppercases
        public static string Sanitize(string s) => SPACE_REGEX.Replace(s ?? "", "").ToUpperInvariant();

        // Validation
        public static bool IsValid(string postcode) => POSTCODE_REGEX.IsMatch(Sanitize(postcode));

        public static bool ValidOutcode(string outcode) => OUTCODE_REGEX.IsMatch(Sanitize(outcode));

        // Normalization
        public static string ToNormalised(string postcode)
        {
            var outcode = ToOutcode(postcode);
            var incode = ToIncode(postcode);
            if (outcode == null || incode == null) return null;
            return $"{outcode} {incode}";
        }

        public static string ToOutcode(string postcode)
        {
            if (!IsValid(postcode)) return null;
            return INCODE_REGEX.Replace(Sanitize(postcode), "");
        }

        public static string ToIncode(string postcode)
        {
            if (!IsValid(postcode)) return null;
            var match = INCODE_REGEX.Match(Sanitize(postcode));
            return match.Success ? match.Value : null;
        }

        public static string ToArea(string postcode)
        {
            if (!IsValid(postcode)) return null;
            var match = AREA_REGEX.Match(Sanitize(postcode));
            return match.Success ? match.Value : null;
        }

        public static string ToSector(string postcode)
        {
            var outcode = ToOutcode(postcode);
            var incode = ToIncode(postcode);
            if (outcode == null || incode == null) return null;
            return $"{outcode} {incode[0]}";
        }

        public static string ToUnit(string postcode)
        {
            if (!IsValid(postcode)) return null;
            var match = UNIT_REGEX.Match(Sanitize(postcode));
            return match.Success ? match.Value : null;
        }

        public static string ToDistrict(string postcode)
        {
            var outcode = ToOutcode(postcode);
            if (outcode == null) return null;
            var match = DISTRICT_SPLIT_REGEX.Match(outcode);
            return match.Success ? match.Groups[1].Value : outcode;
        }

        public static string ToSubDistrict(string postcode)
        {
            var outcode = ToOutcode(postcode);
            if (outcode == null) return null;
            var match = DISTRICT_SPLIT_REGEX.Match(outcode);
            return match.Success ? outcode : null;
        }

        // Parse result types
        public record ValidPostcode(
            bool Valid,
            string Postcode,
            string Incode,
            string Outcode,
            string Area,
            string District,
            string SubDistrict,
            string Sector,
            string Unit
        );

        public record InvalidPostcode(
            bool Valid,
            string Postcode,
            string Incode,
            string Outcode,
            string Area,
            string District,
            string SubDistrict,
            string Sector,
            string Unit
        );

        // Parse
        public static object Parse(string postcode)
        {
            if (!IsValid(postcode))
            {
                return new InvalidPostcode(false, null, null, null, null, null, null, null, null);
            }
            return new ValidPostcode(
                true,
                ToNormalised(postcode),
                ToIncode(postcode),
                ToOutcode(postcode),
                ToArea(postcode),
                ToDistrict(postcode),
                ToSubDistrict(postcode),
                ToSector(postcode),
                ToUnit(postcode)
            );
        }

        // Find all postcodes in text
        public static string[] Match(string corpus)
        {
            var matches = POSTCODE_CORPUS_REGEX.Matches(corpus ?? "");
            var result = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
                result[i] = matches[i].Value;
            return result;
        }

        // Replace all postcodes in text
        public static (string[] Match, string Result) Replace(string corpus, string replaceWith = "")
        {
            var matches = Match(corpus);
            var result = POSTCODE_CORPUS_REGEX.Replace(corpus ?? "", replaceWith);
            return (matches, result);
        }

        // Fix common errors and normalize
        public static string Fix(string s)
        {
            if (s == null) return null;
            if (!FIXABLE_REGEX.IsMatch(s)) return s;
            s = s.ToUpperInvariant().Trim().Replace(" ", "");
            int l = s.Length;
            if (l < 5) return s;
            var inward = s.Substring(l - 3, 3);
            var outcode = s.Substring(0, l - 3);
            return $"{CoerceOutcode(outcode)} {Coerce("NLL", inward)}";
        }

        private static readonly System.Collections.Generic.Dictionary<char, char> ToLetter = new()
        {
            ['0'] = 'O',
            ['1'] = 'I'
        };

        private static readonly System.Collections.Generic.Dictionary<char, char> ToNumber = new()
        {
            ['O'] = '0',
            ['I'] = '1'
        };

        private static string CoerceOutcode(string i)
        {
            if (i.Length == 2) return Coerce("LN", i);
            if (i.Length == 3) return Coerce("L??", i);
            if (i.Length == 4) return Coerce("LLN?", i);
            return i;
        }

        private static string Coerce(string pattern, string input)
        {
            var result = new char[input.Length];
            for (int idx = 0; idx < input.Length && idx < pattern.Length; idx++)
            {
                char c = input[idx];
                char target = pattern[idx];
                if (target == 'N')
                    result[idx] = ToNumber.TryGetValue(c, out var n) ? n : c;
                else if (target == 'L')
                    result[idx] = ToLetter.TryGetValue(c, out var l) ? l : c;
                else
                    result[idx] = c;
            }
            return new string(result);
        }

        public static string CoerceOrInvalid(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
                return "INVALID";

            // Try to fix and normalize
            var fixedPostcode = Fix(postcode);

            // Validate the fixed postcode
            if (IsValid(fixedPostcode))
                return ToNormalised(fixedPostcode);

            return "INVALID";
        }
    }
}