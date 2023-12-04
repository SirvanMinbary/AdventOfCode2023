using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var testSum = CountPoints(TestInput());
            Console.WriteLine(testSum);

            var sum = CountPoints(InputHelper.ReadInput("04.txt"));
            Console.WriteLine(sum);

            var testWonCards = CountWiningCopies(TestInput());
            Console.WriteLine(testWonCards);

            var wonCards = CountWiningCopies(InputHelper.ReadInput("04.txt"));
            Console.WriteLine(wonCards);
        }

        private static int CountPoints(string[] cards)
        {
            var sum = 0;
            foreach (var card in cards)
            {
                var numbers = FindWinningNumbers(card);
                if (numbers.Any())
                {
                    var points = 1;
                    for (int i = 0; i < numbers.Count - 1; i++)
                    {
                        points = points * 2;
                    }
                    sum += points;
                }
            }
            return sum;
        }

        private static int CountWiningCopies(string[] cards)
        {
            var indexedCards = new List<(int, string)>();
            for (int i = 0; i < cards.Length; i++)
            {
                indexedCards.Add((i, cards[i]));
            }

            for (int i = 0; i < cards.Length; i++)
            {
                var card = cards[i];
                var matches = FindWinningNumbers(card).Count;
                var cardsToCheck = indexedCards.Where(c => c.Item1 == i).ToList();
                foreach (var _ in cardsToCheck)
                {
                    for (int k = 1; k < matches + 1; k++)
                    {
                        var wonCardIndex = k + i;
                        if (wonCardIndex < cards.Length)
                        {
                            indexedCards.Add(new(wonCardIndex, cards[wonCardIndex]));
                        }
                    }
                }
            }

            return indexedCards.Count;
        }

        private static List<int> FindWinningNumbers(string card)
        {
            var start = card[(card.IndexOf(':') + 1)..];
            var split = start.Split('|');
            var numbers1 = split[0].Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            var numbers2 = split[1].Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();

            var common = numbers1
                .Intersect(numbers2)
                .Select(s => int.Parse(s))
                .ToList();
            return common;
        }

        static string[] TestInput()
        {
            return new string[]
            {
                "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
                "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
                "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
                "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
                "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
                "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
            };
        }
    }
}