using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using static RomajiConverter.Romaji;

namespace RomajiConverter
{
    public static class Hiragana
    {
        private static readonly char[] Vowels = { 'a', 'i', 'u', 'e', 'o'};

        /// <summary>
        /// Hiragana-Romaji dictionary.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static readonly IGrouping<ushort, (char consonant, char youon, char vowel, RomanizationStyle)>[] Romajis = Consonants
            .SelectMany(
                collectionSelector: static _ => Vowels,
                resultSelector: static (consonant, vowel) => (consonant, vowel))
            .Select(static x => (tuple: x, hiragana: x.consonant.Value[x.vowel]))
            .Where(static x => x.hiragana.Item1 != Non)
            .Select(static x =>
            {
                var key = x.hiragana.ToUshort();
                var consonant = x.tuple.consonant.Key.ToRomaji().Item1;
                var youon = x.tuple.consonant.Key.ToRomaji().Item2;
                var vowel = x.tuple.vowel;
                var system = x.tuple.consonant.Value.Style;
                return (key, consonant, youon, vowel, system);
            })
            .Concat(SpecialRomajis
                .Select(static x =>
                {
                    var key = x.Value;
                    var romaji = x.Key.ToRomaji();
                    var consonant = romaji.Item2 != Non ? romaji.Item1 : Non;
                    var youon = romaji.Item3 != Non ? romaji.Item2 : Non;
                    var vowel = romaji.Item2 != Non ? romaji.Item2 : romaji.Item1;
                    const RomanizationStyle system = RomanizationStyle.None;
                    return (key, consonant, youon, vowel, system);
                }))
            .GroupBy(
                keySelector: static x => x.key,
                elementSelector: static x => (x.consonant, x.youon, x.vowel, x.system))
            .ToArray();

        private static readonly Dictionary<ushort, (char consonant, char youon, char vowel)[]> JapanStyle = Romajis
            .Select(static x => (x.Key, romaji: x
                .OrderBy(static y => y, HiraganaComparer.JapanStyle)
                .Select(static y => (y.consonant, y.youon, y.vowel))
                .ToArray()))
            .ToDictionary(static x => x.Key, static x => x.romaji);

        private static readonly Dictionary<ushort, (char consonant, char youon, char vowel)[]> HepburnStyle = Romajis
            .Select(static x => (x.Key, romaji: x
                .OrderBy(static y => y, HiraganaComparer.HepburnStyle)
                .Select(static y => (y.consonant, y.youon, y.vowel))
                .ToArray()))
            .ToDictionary(static x => x.Key, static x => x.romaji);

        private static readonly Dictionary<byte, (char consonant, char youon, char vowel)> SmallerHiraganas = SmallerRomajis
            .Where(static x => x.Value != 'っ'.ToByte())
            .ToDictionary(
                keySelector: static x => x.Value,
                elementSelector: static x =>
                {
                    var (first, second, _, _) = x.Key.ToRomaji();
                    return (first, Non, second);
                });

        /// <summary>
        /// Convert a Hiragana sentence to a Romaji.
        /// </summary>
        /// <param name="hiraganas">A hiragana sentence.</param>
        /// <param name="romajis">
        /// Span&lt;char&gt; struct to store the converted Hiragana.
        /// It's length must be greater than twice the length of a Hiragana sentence.
        /// </param>
        /// <param name="style">The romanization style.</param>
        /// <returns>A length of the romaji.</returns>
        public static int SentenceToRomaji(ReadOnlySpan<char> hiraganas, Span<char> romajis, RomanizationStyle style = RomanizationStyle.Default)
        {
            Span<char> romaji = stackalloc char[4];
            var totalRomajiLength = 0;
            for (var i = 0; i < hiraganas.Length; ++i)
            {
                var endIndex = MoraEndIndex(hiraganas[i..]);
                romaji.Clear();
                var romajiLength = MoraToRomaji(hiraganas[i..(i + endIndex + 1)], romaji, style);
                if (romajiLength == 0) break;
                romaji[..romajiLength].CopyTo(romajis[totalRomajiLength..]);
                totalRomajiLength += romajiLength;
                i += endIndex;
            }

            return totalRomajiLength;
        }

        /// <summary>
        /// Convert a Hiragana mora to a Romaji.
        /// </summary>
        /// <param name="hiragana">A hiragana mora.</param>
        /// <param name="romaji">
        /// Span&lt;char&gt; struct to store the converted Hiragana.
        /// It's length must be greater than 3.
        /// </param>
        /// <param name="style">The romanization style.</param>
        /// <returns>A length of the romaji.</returns>
        public static int MoraToRomaji(ReadOnlySpan<char> hiragana, Span<char> romaji, RomanizationStyle style = RomanizationStyle.Default)
        {
            return MoraToRomajiStartsWith(hiragana, romaji, ReadOnlySpan<char>.Empty, style);
        }

        public static int MoraToRomajiStartsWith(ReadOnlySpan<char> hiragana, Span<char> romaji, ReadOnlySpan<char> start, RomanizationStyle style = RomanizationStyle.Default)
        {
            romaji.Clear();
            // Try to convert Hiragana as youon or chokuon.
            var hiragana1 = hiragana.Length >= 2 ? hiragana[1] : char.MinValue;
            var romanization = GetRomanization(style);
            if (romanization.TryGetValue((hiragana[0], hiragana1).ToUshort(), out var romajis))
            {
                foreach (var romajiTuple in romajis)
                {
                    var length = ConvertToRomaji(romajiTuple, romaji);
                    if (romaji[..length].StartsWith(start)) return length;
                }
            }

            if (hiragana[0] == 'っ') return TryConvertSokuon(hiragana, romaji, start, style);
            if (hiragana[0] == 'ん') return TryConvertHatsuon(hiragana, romaji, start);
            if (SmallerHiraganas.ContainsKey(hiragana[0].ToByte())) return TryConvertSmaller(hiragana[0], romaji, start, style);

            if (romanization.TryGetValue((hiragana[0], Non).ToUshort(), out romajis))
            {
                foreach (var romajiTuple in romajis)
                {
                    var length = ConvertToRomaji(romajiTuple, romaji);
                    if (romaji[..length].StartsWith(start)) return length;
                }
            }

            return 0;
        }

        private static int TryConvertSokuon(ReadOnlySpan<char> hiragana, Span<char> romaji, ReadOnlySpan<char> start, RomanizationStyle style)
        {
            if (hiragana.Length >= 2 && !IsVowel(hiragana[1]) && hiragana[1] != 'ん')
            {
                Span<char> moraWithSokuonRomaji = stackalloc char[4];
                var stripStart = start.Length >= 2 ? start[1..] : start;
                var length = MoraToRomajiStartsWith(hiragana[1..], moraWithSokuonRomaji[1..], stripStart) + 1;
                moraWithSokuonRomaji[0] = moraWithSokuonRomaji[1];
                if (moraWithSokuonRomaji[..length].StartsWith(start))
                {
                    moraWithSokuonRomaji.CopyTo(romaji);
                    return length;
                }
            }

            if (!(start.Length <= 0 || start[0] is 'l' or 'x')) return 0;

            var lOrX = start.Length >= 1 ? start[0] : GetLOrX(style);
            var romanization = GetRomanization(style);
            var romajis = romanization[('つ', Non).ToUshort()];
            foreach (var romajiTuple in romajis)
            {
                var length = ConvertToRomaji(romajiTuple, romaji[1..]);
                if (start.Length <= 1 || romaji[1..(length + 1)].StartsWith(start[1..]))
                {
                    romaji[0] = lOrX;
                    return length + 1;
                }
            }

            romaji.Clear();
            return 0;
        }

        private static int TryConvertHatsuon(ReadOnlySpan<char> hiragana, Span<char> romaji, ReadOnlySpan<char> start)
        {
            Span<char> hatsuonRomaji = stackalloc char[2] { 'n', 'n' };
            Span<char> moraWithHatsuonRomaji = stackalloc char[4];
            if (hiragana.Length >= 2 && (start.Length >= 2 && hatsuonRomaji.SequenceEqual(start[..2]) || IsVowel(hiragana[1]) || IsYGyo(hiragana[1]) || IsNGyo(hiragana[1])))
            {
                var length = MoraToRomaji(hiragana[1..], moraWithHatsuonRomaji[2..]) + 2;
                hatsuonRomaji.CopyTo(moraWithHatsuonRomaji);
                moraWithHatsuonRomaji[..length].CopyTo(romaji);
                return length;
            }

            if (hiragana.Length >= 2 && (start.Length == 0 || start[0] == 'n'))
            {
                var length = MoraToRomaji(hiragana[1..], moraWithHatsuonRomaji[1..]) + 1;
                hatsuonRomaji[..1].CopyTo(moraWithHatsuonRomaji);
                moraWithHatsuonRomaji[..length].CopyTo(romaji);
                return length;
            }

            if (hiragana.Length == 1 && hatsuonRomaji.StartsWith(start))
            {
                hatsuonRomaji.CopyTo(romaji);
                return 2;
            }

            return 0;
        }

        private static int TryConvertSmaller(char hiragana, Span<char> romaji, ReadOnlySpan<char> start, RomanizationStyle style)
        {
            if (!(start.Length == 0 || start.Length >= 1 && start[0] is 'l' or 'x')) return 0;
            var lOrX = start.Length >= 1 ? start[0] : GetLOrX(style);
            if (SmallerHiraganas.TryGetValue(hiragana.ToByte(), out var romajiTuple))
            {
                var length = ConvertToRomaji(romajiTuple, romaji[1..]);
                if (start.Length <= 1 || romaji[1..length].StartsWith(start[1..]))
                {
                    romaji[0] = lOrX;
                    return length + 1;
                }
            }

            romaji.Clear();
            return 0;
        }

        private static int ConvertToRomaji((char consonant, char youon, char vowel) romajiTuple, Span<char> romaji)
        {
            // Convert singular vowel.
            if (romajiTuple.consonant == char.MinValue)
            {
                romaji[0] = romajiTuple.vowel;
                return 1;
            }

            // Convert chokuon.
            if (romajiTuple.youon == char.MinValue)
            {
                romaji[0] = romajiTuple.consonant;
                romaji[1] = romajiTuple.vowel;
                return 2;
            }

            // Convert youon.
            romaji[0] = romajiTuple.consonant;
            romaji[1] = romajiTuple.youon;
            romaji[2] = romajiTuple.vowel;
            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsVowel(char hiragana)
        {
            return hiragana is 'あ' or 'い' or 'う' or 'え' or 'お';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNGyo(char hiragana)
        {
            return hiragana is 'な' or 'に' or 'ぬ' or 'ね' or 'の';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsYGyo(char hiragana)
        {
            return hiragana is 'や' or 'ゆ' or 'よ';
        }

        public static bool IsYouon(ReadOnlySpan<char> hiragana)
        {
            return hiragana.Length >= 2 && hiragana[1] is 'ぁ' or 'ぃ' or 'ぅ' or 'ぇ' or 'ぉ' or 'ゃ' or 'ゅ' or 'ょ';
        }

        public static int MoraEndIndex(ReadOnlySpan<char> hiraganas)
        {
            var index = 0;
            while (true)
            {
                if (hiraganas[index] is not ('っ' or 'ん') && !IsYouon(hiraganas[index..])) return index;
                ++index;
                if (index >= hiraganas.Length) return index - 1;
                if (hiraganas[index - 1] == 'っ' && (IsVowel(hiraganas[index]) || hiraganas[index] == 'ん'))
                    return index - 1;
            }
        }

        private static Dictionary<ushort, (char consonant, char youon, char vowel)[]> GetRomanization(RomanizationStyle style)
        {
            return (style & (RomanizationStyle.JapanStyle | RomanizationStyle.HepburnStyle)) switch
            {
                RomanizationStyle.JapanStyle => JapanStyle,
                RomanizationStyle.HepburnStyle => HepburnStyle,
                _ => JapanStyle
            };
        }

        private static char GetLOrX(RomanizationStyle style)
        {
            return (style & (RomanizationStyle.StartsWithL | RomanizationStyle.StartsWithX)) switch
            {
                RomanizationStyle.StartsWithL => 'l',
                RomanizationStyle.StartsWithX => 'x',
                _ => 'x'
            };
        } 
    }
}