using Benchmark.Config;
using BenchmarkDotNet.Attributes;
using RomajiConverter;

namespace Benchmark;

[Config(typeof(AntiVirusFriendlyConfig))]
[MemoryDiagnoser]
public class ConvertHiraganaBenchmark
{
    private string _hiragana = null!;

    [GlobalSetup]
    public void SetUp()
    {
        _hiragana = "しーしゃーぷでつくったはいぱふぉーまんすなへんかんあるごりずむ";
    }

    [Benchmark]
    public void ConvertHiragana()
    {
        Span<char> romaji = stackalloc char[_hiragana.Length * 2];
        var length = Hiragana.SentenceToRomaji(_hiragana, romaji);
        var str = romaji[..length].ToString();
    }
}