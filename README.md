# RomajiTyping | RomajiConverter

[English](./README_EN.md) | 日本語

## 概要

RomajiTypingはC#で書かれたローマ字のタイピングゲームのシステムを提供するリポジトリです。  
別途UIを実装するだけで簡単にタイピングゲームを作成することが可能です。  
C#9.0で書かれているため、そのままUnityで使用できます。

RomajiTypingを利用するにはこのリポジトリで提供しているRomajiConverterが必要です。

## 使い方

> [!NOTE]
> [sandboxにコンソール入力を利用したタイピングゲームの実例](sandbox/ConsoleApp)があります。  
> コンソール入力の実装は[akeit0氏のRomajiTyping](https://github.com/Akeit0/RomajiTyping)に着想を得ました。

ターゲットとローマ字の表記法を指定してインスタンスを生成します。  
ローマ字の表記法は省略可能で、省略した場合は日本式になります。

```csharp
using RomajiConverter;
using RomajiTyping;

var target = "しーしゃーぷでつくったはいぱふぉーまんすなへんかんあるごりずむ";
var romajiTyping = new Typing(target, RomanizationStyle.HepburnStyle);
```

インスタンス生成後、各プロパティから画面出力に必要な文字列を取得できます。  
文字列はすべて`ReadOnlySpan<char>`型で取得します。

```csharp
// shi-sha-pudetsukuttahaipafo-mansunahenkannarugorizumuと表示
Console.WriteLine(typing.RemainRomaji.ToString());
```

各メソッドで文字を入力・削除できます。  
文字の入力・削除により自動的にプロパティが更新されます。

```csharp
// 一文字入力
typing.Add(input);
// 一文字削除
typing.Remove();
```

入力が正しいかどうかは`IsMatch`プロパティ、`IsFullMatch`プロパティで確認します。

## 特長

ひらがなには複数のローマ字入力に対応したものがあります。
例えば、「しゃ」は「sya」「sha」「sixya」「silya」「shixya」「shilya」のいずれの入力でも変換可能です。

一般的なタイピングゲーム同様、RomajiTypingもこれらの入力に対応しており、さらに入力に応じて自動的にローマ字文字列を更新します。

例えば、ターゲットに「しゃかい」、ローマ字の表記法に日本式を指定してインスタンスを生成した場合、最初に「syakai」という文字列を生成します。
その後、順に「s」、「h」と入力された場合、ローマ字が自動的に「shakai」に更新されます。
同様に「s」「i」と入力された場合、ローマ字が自動的に「sixyakai」に更新されます。

## リファレンス

### class Typing

Constructors

| constructor                                     | description                         |
|-------------------------------------------------|-------------------------------------|
| Typing(ReadOnlySpan\<char\>, RomanizationStyle) | ターゲットとローマ字の表記法を指定してインスタンスを生成、初期化する。 |

Properties

| property        | type                 | description                           |
|-----------------|----------------------|---------------------------------------|
| Input           | ReadOnlySpan\<char\> | 入力された文字列を取得する。                        |
| IsFullMatch     | bool                 | 入力がターゲットと完全に一致しているかを取得する。             |
| IsMatch         | bool                 | 入力がターゲットの最初から途中 (最後) までと一致しているかを取得する。 |
| MatchedHiragana | ReadOnlySpan\<char\> | 入力と一致するひらがなのターゲットを取得する。               |
| MatchedInput    | ReadOnlySpan\<char\> | ターゲットと一致する入力された文字列を取得する。              |
| RemainHiragana  | ReadOnlySpan\<char\> | 残りのターゲットのひらがなを取得する。                   |
| RemainRomaji    | ReadOnlySpan\<char\> | 残りのターゲットのローマ字を取得する。                   |
| UnmatchedInput  | ReadOnlySpan\<char\> | 不一致の入力された文字を取得する。                     |

Methods

| method          | return | description         |
|-----------------|--------|---------------------|
| Add(char input) | void   | インスタンスに文字を追加する。     |
| Remove()        | void   | インスタンスから直近の文字を削除する。 |

## ライセンス

This repository is licensed under the MIT License.
