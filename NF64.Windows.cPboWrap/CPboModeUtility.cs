using System;


namespace NF64.Windows.cPboWrap
{
    internal static class CPboModeUtility
    {
        public static string GetCommandString(CPboMode mode)
        {
            switch (mode)
            {
            case CPboMode.Extract: return "-e";
            case CPboMode.Make: return "-p";
            default: throw new ArgumentException($"'{mode}' is undefined.", nameof(mode));
            }
        }


        public static CPboMode GetMode(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException($"'{s}' is null or empty", nameof(s));

            switch (s)
            {
            case "-e": return CPboMode.Extract;
            case "-p": return CPboMode.Make;
            default: throw new ArgumentException($"'{s}' is undefined.", nameof(s));
            }
        }
    }
}
