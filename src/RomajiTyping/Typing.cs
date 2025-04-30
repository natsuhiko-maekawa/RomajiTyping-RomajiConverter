using System;
using RomajiConverter;

namespace RomajiTyping
{
    public class Typing
    {
        public ReadOnlySpan<char> Input => _romaji.AsSpan()[.._romajiLength];
        public ReadOnlySpan<char> MatchedHiragana => _target.Hiraganas[.._current];
        public ReadOnlySpan<char> RemainHiragana => _target[^(_target.Count - _current)..];
        public ReadOnlySpan<char> MatchedInput => Input[.._matchedEnd];
        public ReadOnlySpan<char> UnmatchedInput => Input[^(_romajiLength - _matchedEnd)..];
        public ReadOnlySpan<char> RemainRomaji => _remain.AsSpan()[.._remainLength];
        public bool IsMatch => UnmatchedInput.IsEmpty;
        public bool IsFullMatch => _current >= _target.Count;
        private ReadOnlySpan<char> LastInput => Input[_ranges[_rangesLength - 1]];
        private readonly Range[] _ranges;
        private readonly char[] _remain;
        private readonly char[] _romaji;
        private int _current;
        private int _matchedEnd;
        private int _rangesLength;
        private int _remainLength;
        private int _romajiLength;
        private readonly RomanizationStyle _style;
        private readonly Target _target;

        public Typing(ReadOnlySpan<char> target, RomanizationStyle style = RomanizationStyle.Default)
        {
            _target = new Target(target);
            _style = style;
            _ranges = new Range[target.Length];
            _romaji = new char[target.Length * 2];
            _remain = new char[target.Length * 2];
            _remainLength = GetRemain(ReadOnlySpan<char>.Empty, _remain);
        }

        public void Add(char input)
        {
            if (IsFullMatch || !IsMatch) return;
            _romaji[_romajiLength] = input;
            ++_romajiLength;
            SplitRomajis(Input, _ranges);
            int remainLength;
            Span<char> hiragana = stackalloc char[3];
            var hiraganaLength = Romaji.MoraToHiragana(LastInput, hiragana, out var remainStartIndex);

            if (hiraganaLength > 0 && _target[_current].StartsWith(hiragana[..hiraganaLength]))
            {
                _current += hiraganaLength;
                if (_current >= _target.Count && remainStartIndex == LastInput.Length)
                {
                    _remain.AsSpan().Clear();
                    ++_matchedEnd;
                    return;
                }
            }
            else if (hiraganaLength > 0)
            {
                remainLength = GetRemain(LastInput[^1..], _remain);
                if (remainLength != 0) _remainLength = remainLength;
                return;
            }

            remainLength = GetRemain(LastInput[remainStartIndex..], _remain);
            if (remainLength > 0 && _romajiLength - 1 == _matchedEnd)
            {
                _remainLength = remainLength;
                ++_matchedEnd;
            }
        }

        public void Remove()
        {
            if (_romajiLength == 0) return;
            if (IsMatch) --_matchedEnd;
            _romaji[_romajiLength - 1] = char.MinValue;
            --_romajiLength;
            SplitRomajis(Input, _ranges);
            Span<char> hiraganas = stackalloc char[_target.Count];
            var length = Romaji.SentenceToHiragana(Input, hiraganas, out var remainStartIndex);
            _current = Math.Min(length, _current);
            _remainLength = GetRemain(Input[remainStartIndex..], _remain);
        }

        private int GetRemain(ReadOnlySpan<char> inputsRemain, Span<char> remain)
        {
            if (_current >= _target.Count) return 0;

            Span<char> romaji = stackalloc char[4];
            var romajiLength = Hiragana.MoraToRomajiStartsWith(_target[_current], romaji, start: inputsRemain, _style);
            if (romajiLength == 0) return 0;

            remain.Clear();
            Span<char> hiragana = stackalloc char[3];
            var hiraganaLength = Romaji.MoraToHiragana(romaji[..romajiLength], hiragana, out _);
            romaji[inputsRemain.Length..romajiLength].CopyTo(remain);
            var endIndex = _target.Count - (_current + hiraganaLength);
            var remainLength
                = Hiragana.SentenceToRomaji(_target[^endIndex..], remain[(romajiLength - inputsRemain.Length)..], _style)
                  + (romajiLength - inputsRemain.Length);
            return remainLength;
        }

        /// <summary>
        /// Splits a Romaji sentence per mora and adds ranges to the argument.
        /// </summary>
        /// <remarks>
        /// For example, split "sinzyuku" into "si", "n", "zyu", "ku".
        /// </remarks>
        /// <param name="romajis">A Romaji sentence.</param>
        /// <param name="ranges">List to store ranges.</param>
        private void SplitRomajis(ReadOnlySpan<char> romajis, Span<Range> ranges)
        {
            ranges.Clear();
            Span<char> hiragana = stackalloc char[2];
            var index = 0;
            for (var start = 0; start < romajis.Length; ++start)
            {
                var end = RomajiMoraEndIndex(romajis, start);
                var range = new Range(start, end + 1);
                var length = Romaji.MoraToHiragana(Input[range], hiragana, out _);
                if (length >= 2 && hiragana[0] == 'っ')
                {
                    ranges[index] = new Range(start, start);
                    ++index;
                }

                ranges[index] = range;
                ++index;

                if (length == 2 && Hiragana.IsYouon(hiragana) && start + end < romajis.Length - 1)
                {
                    ranges[index] = new Range(end, end);
                    ++index;
                }

                start = end;
            }

            _rangesLength = index;
        }

        /// <summary>
        /// Returns an index it is an end of mora.
        /// </summary>
        /// <remarks>
        /// In this case returns the index: The mora ends with a vowel, the first character
        /// is "n" and ends with consonant except "y" and "n", or the second character is
        /// "n" and the first character is "n".
        /// </remarks>
        /// <param name="romajis">Romaji characters.</param>
        /// <param name="startIndex">The index it is a start of mora.</param>
        /// <returns>An index it is an end of mora.</returns>
        private static int RomajiMoraEndIndex(ReadOnlySpan<char> romajis, int startIndex)
        {
            for (var i = startIndex; i < romajis.Length; ++i)
                if (Romaji.IsVowel(romajis[i])
                    || romajis[i] == '-'
                    || (romajis[i] == 'n' && i > 0 && romajis[i - 1] == 'n')
                    || (romajis[i] == 'n' && i + 1 < romajis.Length - 1 && IsGeneralConsonant(romajis[i + 1])))
                    return i;
            return romajis.Length - 1;

            bool IsGeneralConsonant(char romaji)
            {
                return !Romaji.IsVowel(romaji) && romaji != 'y' && romaji != 'n';
            }
        }
    }
}