using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static RomajiConverter.RomanizationStyle;

namespace RomajiConverter
{
    public static class Romaji
    {
        public const char Non = char.MinValue;

        /// <remarks>
        /// Youon is a compound of a Hiragana and a smaller Hiragana except the parts like
        /// "f": If the vowel is "a", it is composed of "ふ" and "ぁ", but if the vowel is
        /// "u", it is composed of "ふ" only.
        /// </remarks>
        internal static readonly Dictionary<ushort, Consonant> Consonants = new()
        {
            { (Non, Non).ToUshort(), new Consonant('あ', 'い', 'う', 'え', 'お', style: JapanStyle) },
            { ('w', 'h').ToUshort(), new Consonant('ぁ', 'ぃ', Non, 'ぇ', 'ぉ', firstKana: 'う') },
            { ('c', Non).ToUshort(), new Consonant('か', 'し', 'く', 'せ', 'こ') },
            { ('k', Non).ToUshort(), new Consonant('か', 'き', 'く', 'け', 'こ', style: JapanStyle) },
            { ('k', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'き', style: JapanStyle) },
            { ('q', Non).ToUshort(), new Consonant('ぁ', 'ぃ', Non, 'ぇ', 'ぉ', firstKana: 'く') },
            { ('q', 'w').ToUshort(), new Consonant('ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', firstKana: 'く') },
            { ('q', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'く') },
            { ('g', Non).ToUshort(), new Consonant('が', 'ぎ', 'ぐ', 'げ', 'ご', style: JapanStyle) },
            { ('g', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ぎ', style: JapanStyle) },
            { ('s', Non).ToUshort(), new Consonant('さ', 'し', 'す', 'せ', 'そ', style: JapanStyle) },
            { ('s', 'h').ToUshort(), new Consonant('ゃ', Non, 'ゅ', 'ぇ', 'ょ', firstKana: 'し', style: HepburnStyle) },
            { ('s', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'し', style: JapanStyle) },
            { ('s', 'w').ToUshort(), new Consonant('ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', firstKana: 'す') },
            { ('z', Non).ToUshort(), new Consonant('ざ', 'じ', 'ず', 'ぜ', 'ぞ', style: JapanStyle) },
            { ('z', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'じ', style: JapanStyle) },
            { ('j', Non).ToUshort(), new Consonant('ゃ', Non, 'ゅ', 'ぇ', 'ょ', firstKana: 'じ', style: HepburnStyle) },
            { ('j', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'じ') },
            { ('t', Non).ToUshort(), new Consonant('た', 'ち', 'つ', 'て', 'と', style: JapanStyle) },
            { ('c', 'h').ToUshort(), new Consonant('ゃ', Non, 'ゅ', 'ぇ', 'ょ', firstKana: 'ち', style: HepburnStyle) },
            { ('c', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ち') },
            { ('t', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ち', style: JapanStyle) },
            { ('t', 's').ToUshort(), new Consonant('ぁ', 'ぃ', Non, 'ぇ', 'ぉ', firstKana: 'つ', style: HepburnStyle) },
            { ('t', 'h').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'て') },
            { ('t', 'w').ToUshort(), new Consonant('ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', firstKana: 'と') },
            { ('d', Non).ToUshort(), new Consonant('だ', 'ぢ', 'づ', 'で', 'ど', style: JapanStyle) },
            { ('d', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ぢ', style: JapanStyle) },
            { ('d', 'h').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'で') },
            { ('d', 'w').ToUshort(), new Consonant('ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', firstKana: 'ど') },
            { ('n', Non).ToUshort(), new Consonant('な', 'に', 'ぬ', 'ね', 'の', style: JapanStyle) },
            { ('n', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'に', style: JapanStyle) },
            { ('h', Non).ToUshort(), new Consonant('は', 'ひ', 'ふ', 'へ', 'ほ', style: JapanStyle) },
            { ('h', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ひ', style: JapanStyle) },
            { ('f', Non).ToUshort(), new Consonant('ぁ', 'ぃ', Non, 'ぇ', 'ぉ', firstKana: 'ふ', style: HepburnStyle) },
            { ('f', 'w').ToUshort(), new Consonant('ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ', firstKana: 'ふ') },
            { ('f', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ふ') },
            { ('b', Non).ToUshort(), new Consonant('ば', 'び', 'ぶ', 'べ', 'ぼ', style: JapanStyle) },
            { ('b', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'び', style: JapanStyle) },
            { ('p', Non).ToUshort(), new Consonant('ぱ', 'ぴ', 'ぷ', 'ぺ', 'ぽ', style: JapanStyle) },
            { ('p', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ぴ', style: JapanStyle) },
            { ('m', Non).ToUshort(), new Consonant('ま', 'み', 'む', 'め', 'も', style: JapanStyle) },
            { ('m', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'み', style: JapanStyle) },
            { ('y', Non).ToUshort(), new Consonant('や', 'い', 'ゆ', Non, 'よ', style: JapanStyle) },
            { ('r', Non).ToUshort(), new Consonant('ら', 'り', 'る', 'れ', 'ろ', style: JapanStyle) },
            { ('r', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'り', style: JapanStyle) },
            { ('w', Non).ToUshort(), new Consonant('わ', Non, 'う', Non, 'を', style: JapanStyle) },
            { ('v', Non).ToUshort(), new Consonant('ぁ', 'ぃ', Non, 'ぇ', 'ぉ', firstKana: 'ヴ') },
            { ('v', 'y').ToUshort(), new Consonant('ゃ', 'ぃ', 'ゅ', 'ぇ', 'ょ', firstKana: 'ヴ') }
        };

        internal static readonly Dictionary<uint, byte> SmallerRomajis = new()
        {
            { ('a', Non, Non, Non).ToUint(), 'ぁ'.ToByte() },
            { ('i', Non, Non, Non).ToUint(), 'ぃ'.ToByte() },
            { ('u', Non, Non, Non).ToUint(), 'ぅ'.ToByte() },
            { ('e', Non, Non, Non).ToUint(), 'ぇ'.ToByte() },
            { ('o', Non, Non, Non).ToUint(), 'ぉ'.ToByte() },
            { ('k', 'a', Non, Non).ToUint(), 'ヵ'.ToByte() },
            { ('k', 'e', Non, Non).ToUint(), 'ヶ'.ToByte() },
            { ('t', 'u', Non, Non).ToUint(), 'っ'.ToByte() },
            { ('t', 's', 'u', Non).ToUint(), 'っ'.ToByte() },
            { ('y', 'a', Non, Non).ToUint(), 'ゃ'.ToByte() },
            { ('y', 'u', Non, Non).ToUint(), 'ゅ'.ToByte() },
            { ('y', 'o', Non, Non).ToUint(), 'ょ'.ToByte() },
            { ('w', 'a', Non, Non).ToUint(), 'ゎ'.ToByte() }
        };

        internal static readonly Dictionary<uint, ushort> SpecialRomajis = new()
        {
            { ('y', 'e', Non, Non).ToUint(), ('い', 'ぇ').ToUshort() },
            { ('w', 'i', Non, Non).ToUint(), ('う', 'ぃ').ToUshort() },
            { ('w', 'e', Non, Non).ToUint(), ('う', 'ぇ').ToUshort() },
            { ('w', 'y', 'i', Non).ToUint(), ('ゐ', Non).ToUshort() },
            { ('w', 'y', 'e', Non).ToUint(), ('ゑ', Non).ToUshort() },
            { ('n', 'n', Non, Non).ToUint(), ('ん', Non).ToUshort() },
            { ('-', Non, Non, Non).ToUint(), ('ー', Non).ToUshort() }
        };

        /// <summary>
        /// Try to convert Romaji sentence to Hiragana.
        /// </summary>
        /// <param name="romajis">Romaji sentence.</param>
        /// <param name="hiraganas">
        /// Span&lt;char&gt; struct to store the converted Hiragana.
        /// It's length must be greater than the length of the romaji.
        /// </param>
        /// <param name="remainStartIndex"></param>
        /// <returns>A length of the Hiragana.</returns>
        public static int SentenceToHiragana(ReadOnlySpan<char> romajis, Span<char> hiraganas, out int remainStartIndex)
        {
            remainStartIndex = 0;
            Span<char> hiragana = stackalloc char[2];
            var totalLength = 0;
            for (var i = 0; i < romajis.Length; ++i)
            {
                hiragana.Clear();
                var endIndex = MoraEndIndex(romajis[i..]);
                var length = ToHiragana(romajis[i..(i + endIndex + 1)], hiragana, out remainStartIndex);
                if (length > 0)
                {
                    hiragana[..length].CopyTo(hiraganas[totalLength..]);
                    totalLength += length;
                    i += remainStartIndex - 1;
                }
                else if(endIndex > 0)
                {
                    romajis[(i + remainStartIndex)..(i + remainStartIndex + 1)].CopyTo(hiraganas[totalLength..]);
                    ++totalLength;
                }
                else
                {
                    remainStartIndex = i;
                    return totalLength;
                }
            }

            remainStartIndex = romajis.Length;
            return totalLength;
        }

        /// <summary>
        /// Try to convert Romaji to Hiragana (1 mora).
        /// </summary>
        /// <param name="romaji">Romaji (1 mora).</param>
        /// <param name="hiragana">
        /// Span&lt;char&gt; struct to store the converted Hiragana.
        /// It's length must be greater than 3.
        /// </param>
        /// <param name="remainStartIndex"></param>
        /// <returns>A length of the Hiragana. Return zero if it fails.</returns>
        public static int MoraToHiragana(ReadOnlySpan<char> romaji, Span<char> hiragana, out int remainStartIndex)
        {
            var endIndex = MoraEndIndex(romaji);
            if (endIndex < 0 && romaji[0] != 'n')
            {
                remainStartIndex = 0;
                return 0;
            }

            var length = ToHiragana(romaji, hiragana, out remainStartIndex);
            return length;
        }

        private static int ToHiragana(ReadOnlySpan<char> romaji, Span<char> hiragana, out int remainStartIndex)
        {
            var offset = 0;
            while (true)
            {
                ushort key;
                switch (romaji.Length - offset)
                {
                    case 1:
                        key = (Non, Non).ToUshort();
                        break;
                    case 2:
                        key = (romaji[0 + offset], Non).ToUshort();
                        break;
                    case 3:
                        key = (romaji[0 + offset], romaji[1 + offset]).ToUshort();
                        break;
                    case >= 4:
                        goto MaybeSokuon;
                    default:
                        remainStartIndex = 0;
                        return 0;
                }

                // Converts and returns Hiragana that matches consonants and a vowel.
                // For example, "あ", "か", "きゃ" and anymore.
                if (Consonants.TryGetValue(key, out var consonant) && IsVowel(romaji[^1]))
                {
                    var hiraganaTuple = consonant[romaji[^1]];
                    var hiraganaLength = ConvertTupleToSpan(hiraganaTuple, hiragana[offset..]);
                    if (hiraganaLength > 0)
                    {
                        remainStartIndex = romaji.Length;
                        return hiraganaLength + offset;
                    }
                }

                // Converts and returns Hiragana that matches special romaji.
                // For example, "うぃ", "ゐ", "ん" when typing "nn", "ー".
                var specialKey = key | (uint)((romaji[^1] & 255) << 8 * (romaji.Length - 1));
                if (SpecialRomajis.TryGetValue(specialKey, out var specialHiragana))
                {
                    var hiraganaTuple = specialHiragana.ToHiragana();
                    remainStartIndex = romaji.Length;
                    return ConvertTupleToSpan(hiraganaTuple, hiragana[offset..]) + offset;
                }

                MaybeSokuon:
                // Converts and returns sokuon like "っか", "っきゃ".
                // However, if the Hiragana is only "っ", the IsSmaller() method will convert it in the postprocessing.
                if (offset == 0 && IsSokuon(romaji))
                {
                    hiragana[0] = 'っ';
                    // Set the offset to treat sokuon as ordinary Hiragana like "kka" as "ka" and converts again.
                    offset = 1;
                }
                // Converts and returns "ん" when typing "nk", "ns" etc., except "nn".
                // "nn" is converted in the preprocessing.
                else if (offset == 0 && IsHatsuon(romaji))
                {
                    hiragana[0] = 'ん';
                    offset = 1;
                }
                else if (hiragana[0] == 'ん')
                {
                    remainStartIndex = 1;
                    return 1;
                }
                else
                {
                    break;
                }
            }

            // Converts and returns smaller Hiragana like "っ", "ヵ".
            if (IsSmaller(romaji))
            {
                // Get Hiragana that matches consonants and a vowel.
                var key = (romaji.Length - 1) switch
                {
                    1 => (romaji[1], Non, Non, Non).ToUint(),
                    2 => (romaji[1], romaji[2], Non, Non).ToUint(),
                    3 => (romaji[1], romaji[2], romaji[3], Non).ToUint(),
                    _ => uint.MinValue
                };

                if (SmallerRomajis.TryGetValue(key, out var smallerHiragana))
                {
                    hiragana[0] = smallerHiragana.ToHiragana();
                    remainStartIndex = romaji.Length;
                    return 1;
                }
            }

            remainStartIndex = 0;
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsVowel(char romaji)
        {
            return romaji is 'a' or 'i' or 'u' or 'e' or 'o';
        }

        private static bool IsSokuon(ReadOnlySpan<char> romaji)
        {
            return romaji.Length >= 3 && romaji[0] == romaji[1];
        }

        private static bool IsHatsuon(ReadOnlySpan<char> romaji)
        {
            return romaji.Length >= 2 && romaji[0] == 'n' && romaji[1] != 'y' && !IsVowel(romaji[1]);
        }

        private static bool IsSmaller(ReadOnlySpan<char> romaji)
        {
            return romaji[0] is 'l' or 'x';
        }

        private static int MoraEndIndex(ReadOnlySpan<char> romajis)
        {
            var index = 0;
            while (true)
            {
                if (IsVowel(romajis[index]) || romajis[index] == '-') return index;
                ++index;
                if (index >= romajis.Length) return -1;
                if (romajis[index - 1] == 'n' && romajis[index] == 'n') return index;
            }
        }

        /// <summary>
        /// Converts the Tuple of Hiragana to the Span&lt;char&gt; of Hiragana.
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="hiragana">
        /// Span&lt;char&gt; struct to store the converted Hiragana.
        /// It's length must be greater than 2.
        /// </param>
        /// <returns>A length of the hiragana. Return zero if it fails.</returns>
        private static int ConvertTupleToSpan((char, char) tuple, Span<char> hiragana)
        {
            if (tuple.Item1 == char.MinValue) return 0;
            hiragana[0] = tuple.Item1;
            if (tuple.Item2 == char.MinValue) return 1;
            hiragana[1] = tuple.Item2;
            return 2;
        }
    }
}