using System.Runtime.CompilerServices;

namespace RomajiConverter
{
    /// <summary>
    /// Discards high 8 bit and converts character to byte.
    /// </summary>
    internal static class Parser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this char character) => (byte)character;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToUshort(this (char, char) tuple) 
            => (ushort)((tuple.Item1 & 255) | ((tuple.Item2 & 255) << 8));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUint(this (char, char, char, char) tuple)
            => (uint)((tuple.Item1 & 255) | ((tuple.Item2 & 255) << 8) | (tuple.Item3 & 255) << 16 | (tuple.Item4 & 255) << 24);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (char, char) ToRomaji(this ushort romaji)
        {
            var first = (char)(romaji & 255);
            var second = (char)((romaji >> 8) & 255);
            return (first, second);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (char, char, char, char) ToRomaji(this uint romaji)
        {
            var first = (char)(romaji & 255);
            var second = (char)((romaji >> 8) & 255);
            var third = (char)((romaji >> 16) & 255);
            var fourth = (char)((romaji >> 24) & 255);
            return (first, second, third, fourth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToHiragana(this byte hiragana) => hiragana != 0 ? (char)(12288 | hiragana) : char.MinValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (char, char) ToHiragana(this ushort hiragana)
        {
            var first = (hiragana & 255) != 0 ? (char)(12288 | hiragana & 255) : char.MinValue;
            var second = ((hiragana >> 8) & 255) != 0 ? (char)(12288 | (hiragana >> 8) & 255) : char.MinValue;
            return (first, second);
        }
    }
}