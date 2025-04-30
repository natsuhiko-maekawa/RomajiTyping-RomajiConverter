using System;
using RomajiConverter;
using RomajiTyping;

var target = "しーしゃーぷでつくったはいぱふぉーまんすなへんかんあるごりずむ";
var typing = new Typing(target, RomanizationStyle.HepburnStyle);

while (true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(typing.MatchedHiragana.ToString());
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(typing.RemainHiragana.ToString());

    if (typing.IsMatch)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(typing.MatchedInput.ToString());
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(typing.RemainRomaji.ToString());

        if (typing.IsFullMatch)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Perfect Match");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(typing.MatchedInput.ToString());
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(typing.UnmatchedInput.ToString());
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(typing.MatchedInput.ToString());
        Console.WriteLine(typing.RemainRomaji.ToString());
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("No Match");
    }

    var key = Console.ReadKey(false);
    if (key.Key == ConsoleKey.Enter)
    {
        Console.Clear();
        Console.WriteLine("Please input target text");
        Console.CursorVisible = true;

        target = Console.ReadLine() ?? "";
        typing = new Typing(target);

        Console.Clear();
    }
    else if (key.Key == ConsoleKey.Backspace)
    {
        typing.Remove();
    }
    else if (key.Key == ConsoleKey.Escape)
    {
        break;
    }
    else if (key.KeyChar is >= 'a' and <= 'z' or '-')
    {
        typing.Add(key.KeyChar);
    }
}
