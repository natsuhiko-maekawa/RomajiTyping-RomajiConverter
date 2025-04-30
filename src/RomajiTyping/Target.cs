using System;
using System.Collections.Generic;
using RomajiConverter;

namespace RomajiTyping
{
    internal readonly struct Target
    {
        public ReadOnlySpan<char> Hiraganas => _hiraganas;

        // _hiraganas and _ranges are the same length.
        public int Count => _hiraganas.Length;
        public ReadOnlySpan<char> this[Index index] => Hiraganas[_ranges[index]];
        public ReadOnlySpan<char> this[Range range] => Hiraganas[_ranges.Start(range.Start).._ranges.End(range.End)];
        private readonly char[] _hiraganas;
        private readonly Range[] _ranges;

        public Target(ReadOnlySpan<char> hiraganas)
        {
            _hiraganas = hiraganas.ToArray();
            _ranges = SplitHiraganas(hiraganas);
        }

        /// <summary>
        /// Splits a Hiragana sentence and returns ranges. See remarks fo details.
        /// </summary>
        /// <remarks>
        /// For example, split "しんじゅく" into "し", "んじゅ", "じゅ", "ゅ", "く".
        /// </remarks>
        /// <param name="hiraganas">A Hiragana sentence.</param>
        /// <returns>Ranges.</returns>
        private static Range[] SplitHiraganas(ReadOnlySpan<char> hiraganas)
        {
            var ranges = new Range[hiraganas.Length];
            for (var i = 0; i < ranges.Length; ++i)
            {
                var end = Hiragana.MoraEndIndex(hiraganas[i..]);
                var range = new Range(i, i + end + 1);
                ranges[i] = range;
            }

            return ranges;
        }
    }

    internal static class RangesEx
    {
        public static Index Start(this IList<Range> ranges, Index start)
        {
            var index = start.GetOffset(ranges.Count);
            return index < ranges.Count ? ranges[index].Start : ranges[^1].End;
        }

        public static Index End(this IList<Range> ranges, Index end)
        {
            var index = end.GetOffset(ranges.Count) - 1;
            return index >= 0 ? ranges[index].End : 0;
        }
    }
}