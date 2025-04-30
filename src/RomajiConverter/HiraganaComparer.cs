using System.Collections.Generic;

namespace RomajiConverter
{
    internal class HiraganaComparer : IComparer<(char consonant, char youon, char vowel, RomanizationStyle style)>
    {
        public static HiraganaComparer JapanStyle { get; } = new(RomanizationStyle.JapanStyle);
        public static HiraganaComparer HepburnStyle { get; } = new(RomanizationStyle.HepburnStyle);
        private static readonly int[] JapanStyleOrder = { 2, 0, 1 };
        private static readonly int[] HepburnStyleOrder = { 2, 1, 0 };
        private readonly RomanizationStyle _style;

        private HiraganaComparer(RomanizationStyle style)
        {
            _style = style;
        }

        public int Compare(
            (char consonant, char youon, char vowel, RomanizationStyle style) x,
            (char consonant, char youon, char vowel, RomanizationStyle style) y)
        {
            var order = _style switch
            {
                RomanizationStyle.JapanStyle => JapanStyleOrder,
                RomanizationStyle.HepburnStyle => HepburnStyleOrder,
                _ => JapanStyleOrder
            };

            if (x.style != y.style) return order[(int)x.style] - order[(int)y.style];
            var xLength = GetRomajiLength(x.consonant, x.youon);
            var yLength = GetRomajiLength(y.consonant, y.youon);
            return xLength - yLength;
        }

        private static int GetRomajiLength(char consonant, char youon)
        {
            if (consonant == char.MinValue) return 1;
            if (youon == char.MinValue) return 2;
            return 3;
        }
    }
}