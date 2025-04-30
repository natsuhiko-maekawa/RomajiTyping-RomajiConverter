using NUnit.Framework;

namespace RomajiConverter.Tests;

public class ConvertHiraganaTests
{
    [TestCase("あ", "a")]
    [TestCase("い", "i")]
    [TestCase("か", "ka")]
    [TestCase("き", "ki")]
    [TestCase("し", "si")]
    [TestCase("しゃ", "sya")]
    [TestCase("っ", "xtu")]
    [TestCase("ゃ", "xya")]
    public void ConvertHiraganaToRomaji(string hiragana, string romaji)
    {
        Span<char> actual = stackalloc char[3];
        var length = Hiragana.MoraToRomaji(hiragana, actual);
        Assert.That(actual[..length].ToString(), Is.EqualTo(romaji));
    }

    [TestCase("あ", "a")]
    [TestCase("い", "i")]
    [TestCase("か", "ka")]
    [TestCase("き", "ki")]
    [TestCase("し", "shi")]
    [TestCase("しゃ", "sha")]
    [TestCase("っ", "ltsu")]
    public void ConvertHiraganaToRomajiByHepburnStyle(string hiragana, string romaji)
    {
        Span<char> actual = stackalloc char[4];
        var length = Hiragana.MoraToRomaji(hiragana, actual, RomanizationStyle.HepburnStyle | RomanizationStyle.StartsWithL);
        Assert.That(actual[..length].ToString(), Is.EqualTo(romaji));
    }

    [TestCase("あ", "a", "")]
    [TestCase("あ", "a", "a")]
    [TestCase("か", "ka", "")]
    [TestCase("か", "ka", "k")]
    [TestCase("か", "ka", "ka")]
    [TestCase("か", "ca", "c")]
    [TestCase("か", "ca", "ca")]
    [TestCase("しゃ", "sya", "")]
    [TestCase("しゃ", "sya", "sy")]
    [TestCase("しゃ", "sya", "sya")]
    [TestCase("しゃ", "sha", "sh")]
    [TestCase("しゃ", "sha", "sha")]
    [TestCase("じゃ", "zya", "z")]
    [TestCase("じゃ", "ja", "j")]
    [TestCase("ふぁ", "hu", "h")]
    [TestCase("っ", "", "t")]
    [TestCase("っか", "kka", "")]
    [TestCase("っか", "kka", "k")]
    [TestCase("っか", "kka", "kk")]
    [TestCase("っか", "kka", "kka")]
    [TestCase("っか", "xtu", "x")]
    [TestCase("っか", "xtu", "xt")]
    [TestCase("んか", "nka", "")]
    [TestCase("んか", "nka", "n")]
    [TestCase("んか", "nka", "nk")]
    [TestCase("んか", "nka", "nka")]
    [TestCase("んか", "nnka", "nn")]
    [TestCase("んか", "nnka", "nnk")]
    [TestCase("んか", "nnka", "nnka")]
    public void ConvertHiraganaToRomajiStartsWith(string hiragana, string romaji, string start)
    {
        Span<char> actual = stackalloc char[4];
        var length = Hiragana.MoraToRomajiStartsWith(hiragana, actual, start);
        Assert.That(actual[..length].ToString(), Is.EqualTo(romaji));
    }

    [TestCase("あ", "a", "")]
    [TestCase("あ", "a", "a")]
    [TestCase("か", "ka", "")]
    [TestCase("か", "ka", "k")]
    [TestCase("か", "ka", "ka")]
    [TestCase("か", "ca", "c")]
    [TestCase("か", "ca", "ca")]
    [TestCase("しゃ", "sha", "")]
    [TestCase("しゃ", "sya", "sy")]
    [TestCase("しゃ", "sya", "sya")]
    [TestCase("しゃ", "sha", "sh")]
    [TestCase("しゃ", "sha", "sha")]
    [TestCase("じゃ", "zya", "z")]
    [TestCase("じゃ", "ja", "j")]
    [TestCase("っか", "kka", "")]
    [TestCase("っか", "kka", "k")]
    [TestCase("っか", "kka", "kk")]
    [TestCase("っか", "kka", "kka")]
    [TestCase("んか", "nka", "")]
    [TestCase("んか", "nka", "n")]
    [TestCase("んか", "nka", "nk")]
    [TestCase("んか", "nka", "nka")]
    [TestCase("んか", "nnka", "nn")]
    [TestCase("んか", "nnka", "nnk")]
    [TestCase("んか", "nnka", "nnka")]
    public void ConvertHiraganaToRomajiStartsWithByHepburnStyle(string hiragana, string romaji, string start)
    {
        Span<char> actual = stackalloc char[4];
        var length = Hiragana.MoraToRomajiStartsWith(hiragana, actual, start, RomanizationStyle.HepburnStyle);
        Assert.That(actual[..length].ToString(), Is.EqualTo(romaji));
    }

    [TestCase("うえの", "ueno", Description = "Two consecutive vowels.")]
    [TestCase("にっぽり", "nippori", Description = "Sokuon.")]
    [TestCase("しんじゅく", "sinzyuku", Description = "Hatuon and youon.")]
    [TestCase("ちょうぜつそふとうぇあせっけいにゅうもん", "tyouzetusohutoweasekkeinyuumonn", Description = "ちょ、うぇ、っけ、ん")]
    [TestCase("しーしゃーぷでつくったはいぱふぉーまんすなへんかんあるごりずむ", "si-sya-pudetukuttahaipafo-mansunahenkannarugorizumu",
        Description = "ー、ふぉ、んす、んあ")]
    [TestCase("うっうというぽけもんのなまえはいっぱんてきなそくおんのほうそくにはんする", "uxtuutoiupokemonnnonamaehaippantekinasokuonnnohousokunihansuru",
        Description = "っう、いう、んの")]
    public void ConvertHiraganaSentenceToRomaji(string hiragana, string romaji)
    {
        Span<char> actual = stackalloc char[hiragana.Length * 2];
        var length = Hiragana.SentenceToRomaji(hiragana, actual);
        Assert.That(actual[..length].ToString(), Is.EqualTo(romaji));
    }
}