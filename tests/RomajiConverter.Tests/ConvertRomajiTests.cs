using NUnit.Framework;

namespace RomajiConverter.Tests;

public class ConvertRomajiTests
{
    [TestCase("a", "あ")]
    [TestCase("i", "い")]
    [TestCase("k", "", "k")]
    [TestCase("ka", "か")]
    [TestCase("ki", "き")]
    [TestCase("kk", "", "kk")]
    [TestCase("ca", "か")]
    [TestCase("ci", "し")]
    [TestCase("si", "し")]
    [TestCase("shi", "し")]
    [TestCase("sya", "しゃ")]
    [TestCase("sha", "しゃ")]
    [TestCase("kka", "っか")]
    [TestCase("kkya", "っきゃ")]
    [TestCase("ltu", "っ")]
    [TestCase("ltsu", "っ")]
    [TestCase("xtu", "っ")]
    [TestCase("wi", "うぃ")]
    [TestCase("wyi", "ゐ")]
    [TestCase("n", "", "n")]
    [TestCase("nk", "ん", "k")]
    [TestCase("nka", "んか")]
    [TestCase("nn", "ん")]
    [TestCase("ny", "", "ny")]
    [TestCase("-", "ー")]
    public void ConvertRomajiToHiragana(string romaji, string hiragana, string remain = "")
    {
        Span<char> actualHiragana = stackalloc char[3];
        var length = Romaji.MoraToHiragana(romaji, actualHiragana, out var remainStartIndex);
        Assert.That(actualHiragana[..length].ToString(), Is.EqualTo(hiragana), $"{nameof(hiragana)}'s test has error.");
        Assert.That(romaji.AsSpan()[remainStartIndex..].ToString(), Is.EqualTo(remain), $"{nameof(remain)}'s test has error.");
    }

    [TestCase("chouzetsusofutoweasekkeinyuumonn", "ちょうぜつそふとうぇあせっけいにゅうもん")]
    [TestCase("shi-sha-pudetsukuttahaipafo-mansunahenkannarugorizumu", "しーしゃーぷでつくったはいぱふぉーまんすなへんかんあるごりずむ")]
    [TestCase("csharpdetsukuttahighperformancenahenkannalgorithm", "cしゃrpでつくったひghぺrふぉrまんせなへんかんあlごり", "thm")]
    public void ConvertRomajiSentenceToHiragana(string romaji, string hiragana, string remain = "")
    {
        Span<char> actual = stackalloc char[romaji.Length];
        var length = Romaji.SentenceToHiragana(romaji, actual, out var remainStartIndex);
        Assert.That(actual[..length].ToString(), Is.EqualTo(hiragana));
        Assert.That(romaji.AsSpan()[remainStartIndex..].ToString(), Is.EqualTo(remain));
    }
}