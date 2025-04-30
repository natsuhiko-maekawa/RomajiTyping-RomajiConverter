using BenchmarkDotNet.Attributes;
using RomajiConverter;

namespace Benchmark;

[MemoryDiagnoser]
public class ConvertRomajiBenchmark
{
    private string _romaji = null!;

    [GlobalSetup]
    public void SetUp()
    {
        // しーしゃーぷでつくったはいぱふぉーまんすなへんかんあるごりずむ
        _romaji = "shi-sha-pudetsukuttahaipafo-mansunahenkannarugorizumu";
    }

    [Benchmark]
    public void ConvertRomaji()
    {
        Span<char> hiragana = stackalloc char[_romaji.Length];
        var length = Romaji.SentenceToHiragana(_romaji, hiragana, out _);
        var str = hiragana[..length].ToString();
    }
}