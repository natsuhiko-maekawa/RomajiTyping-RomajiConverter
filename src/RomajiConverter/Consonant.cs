using System;

namespace RomajiConverter
{
    internal readonly struct Consonant
    {
        private readonly byte _a;
        private readonly byte _i;
        private readonly byte _u;
        private readonly byte _e;
        private readonly byte _o;
        private readonly byte _firstKana;
        public RomanizationStyle Style { get; }

        public Consonant(char a, char i, char u, char e, char o, char firstKana = char.MinValue, RomanizationStyle style = RomanizationStyle.None)
        {
            _a = a.ToByte();
            _i = i.ToByte();
            _u = u.ToByte();
            _e = e.ToByte();
            _o = o.ToByte();
            _firstKana = firstKana.ToByte();
            Style = style;
        }

        /// <summary>
        /// Returns the Hiragana characters that matches the vowel of index.
        /// </summary>
        /// <param name="vowel">Vowel character any of a, i, u, e or o.</param>
        /// <exception cref="ArgumentOutOfRangeException">Index is not vowel character.</exception>
        public (char, char) this[char vowel]
        {
            get
            {
                var hiragana = vowel switch
                {
                    'a' => _a,
                    'i' => _i,
                    'u' => _u,
                    'e' => _e,
                    'o' => _o,
                    _ => throw new ArgumentOutOfRangeException(nameof(vowel), vowel, null)
                };

                return _firstKana == 0
                    ? (hiragana.ToHiragana(), char.MinValue)
                    : (_firstKana.ToHiragana(), hiragana.ToHiragana());
            }
        }
    }
}