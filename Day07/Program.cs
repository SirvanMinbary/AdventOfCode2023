using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var testInput = new string[]
            {
                "32T3K 765",
                "T55J5 684",
                "KK677 28",
                "KTJJT 220",
                "QQQJA 483",
            };
            var input = InputHelper.ReadInput("07.txt");

            var test1 = CalculateTotalWinnings(testInput, false);
            Console.WriteLine(test1);

            var part1 = CalculateTotalWinnings(input, false);
            Console.WriteLine(part1);

            //var test2 = CalculateTotalWinnings(testInput, true);
            //Console.WriteLine(test2);

            var part2 = CalculateTotalWinnings(input, true);
            Console.WriteLine(part2);
        }

        private static long CalculateTotalWinnings(string[] lines, bool useJokers)
        {
            var hands = new List<Hand>();

            foreach (var line in lines)
            {
                hands.Add(new Hand(line, useJokers));
            }

            hands = hands.Order().ToList();

            long sum = 0;

            for (int i = 0; i < hands.Count; i++)
            {
                sum += hands[i].Winnings * (i + 1);
            }

            return sum;
        }
    }

    enum HandType
    {
        High,
        Pair,
        TwoPair,
        ThreeOak,
        FullHouse,
        FourOak,
        FiveOak
    }

    struct Hand : IComparable<Hand>
    {
        public HandType HandType;
        public string Cards;
        public long Winnings;

        public Hand(string s, bool useJokers)
        {
            var split = s.Split(' ');
            HandType = GetHandType(split[0], useJokers);
            Cards = split[0];
            Winnings = long.Parse(split[1]);
        }

        public readonly int CompareTo(Hand other)
        {
            const string CardValues = "23456789TJQKA";

            if (HandType != other.HandType)
            {
                return HandType > other.HandType ? 1 : -1;
            }

            for (int i = 0; i < Cards.Length; i++)
            {

                if (Cards[i] == other.Cards[i])
                {
                    continue;
                }

                return CardValues.IndexOf(Cards[i]) > CardValues.IndexOf(other.Cards[i]) ? 1 : -1;
            }

            return 0;
        }

        public override readonly string ToString()
        {
            return $"{Cards} : {Winnings} : {HandType}";
        }

        private HandType GetHandType(string s, bool useJokers)
        {
            var distinct = s.Distinct().ToArray();
            var distinctCount = distinct.Length;

            if (useJokers && s.Contains('J') && distinctCount > 1)
            {
                if (distinctCount == 2)
                {
                    return HandType.FiveOak;
                }
                else if (distinctCount == 3)
                {
                    var ordered = distinct.OrderByDescending(d => s.Count(s => d == s && d != 'J'));
                    var first = GetHandType(s.Replace('J', ordered.ElementAt(0)), false);
                    var second = GetHandType(s.Replace('J', ordered.ElementAt(1)), false);
                    return first > second ? first : second;
                }
                else if (distinctCount == 4)
                {
                    var highestCount = distinct.OrderByDescending(d => s.Count(s => d == s && d != 'J')).First();
                    return GetHandType(s.Replace('J', highestCount), false);
                }
                else if (distinctCount == 5)
                {
                    var highestCount = distinct.OrderByDescending(d => s.Count(s => d == s && d != 'J')).First();
                    return GetHandType(s.Replace('J', highestCount), false);
                }

            }

            if (distinctCount == 1)
            {
                return HandType.FiveOak;
            }
            else if (distinctCount == 2)
            {
                if (distinct.Any(c => s.Count(s => s == c) == 4))
                {
                    return HandType.FourOak;
                }
                else
                {
                    return HandType.FullHouse;
                }
            }
            else if (distinctCount == 3)
            {
                if (distinct.Any(c => s.Count(s => s == c) == 3))
                {
                    return HandType.ThreeOak;
                }
                else
                {
                    return HandType.TwoPair;
                }
            }
            else if (distinctCount == 4)
            {
                return HandType.Pair;
            }
            else
            {
                return HandType.High;
            }
        }
    }
}
