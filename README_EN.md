# RomajiTyping | RomajiConverter

English | [日本語](README.md)

## Overview

RomajiTyping is a repository that provides the Romaji typing game system for C#.  
Using the repository, you can easily create a typing game just by programming the UI separately.  
These classes are written in C#9.0, you can use them in Unity as it is.

RomajiTyping depends on RomajiConverter, which is provided this repository.

## Usage

> [!NOTE]
> See also [the sandbox for an example of a console typing game.](sandbox/ConsoleApp)

Check Japanese document for more information.

## Feature

Check Japanese document for more information.

## Reference

### class Typing

Constructors

| constructor                                     | description                                                       |
|-------------------------------------------------|-------------------------------------------------------------------|
| Typing(ReadOnlySpan\<char\>, RomanizationStyle) | Initialize a new instance that has target and romanization style. |

Properties

| property        | type                 | description                                                              |
|-----------------|----------------------|--------------------------------------------------------------------------|
| Input           | ReadOnlySpan\<char\> | Gets the input characters.                                               |
| IsFullMatch     | bool                 | Gets whether the input matches the entire target.                        |
| IsMatch         | bool                 | Gets whether the input matches the target from start to middle (or end). |
| MatchedHiragana | ReadOnlySpan\<char\> | Gets the target Hiragana that matches the input.                         |
| MatchedInput    | ReadOnlySpan\<char\> | Gets the input characters that matches the target.                       |
| RemainHiragana  | ReadOnlySpan\<char\> | Gets the remain of target Hiragana.                                      |
| RemainRomaji    | ReadOnlySpan\<char\> | Gets the remain of target Romaji.                                        |
| UnmatchedInput  | ReadOnlySpan\<char\> | Gets the unmatched input character.                                      |

Methods

| method          | return | description                                  |
|-----------------|--------|----------------------------------------------|
| Add(char input) | void   | Adds a character to the instance.            |
| Remove()        | void   | Remove the last character from the instance. |

## License

This repository is licensed under the MIT License.
