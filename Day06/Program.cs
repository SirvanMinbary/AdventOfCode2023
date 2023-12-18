using System;
using System.Collections.Generic;
using Utility;

namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CalculateWin(TestInput()));
            Console.WriteLine(CalculateWin(InputHelper.ReadInput("06.txt")));

            Console.WriteLine(CalculateSingleRace(TestInput()));
            Console.WriteLine(CalculateSingleRace(InputHelper.ReadInput("06.txt")));
        }

        static int CalculateWin(string[] lines)
        {
            var winAmount = new List<int>();
            var times = FindNumbers(lines[0]);
            var distances = FindNumbers(lines[1]);

            for (int i = 0; i < times.Count; i++)
            {
                var time = times[i];
                var record = distances[i];
                var wins = 0;

                for (int ms = 1; ms < time; ms++)
                {
                    var distance = (time - ms) * ms;
                    if (distance > record)
                    {
                        wins++;
                    }
                }

                winAmount.Add(wins);
            }

            var sum = 1;
            foreach (var w in winAmount)
            {
                sum = w * sum;
            }
            return sum;
        }

        static long CalculateSingleRace(string[] lines)
        {
            var time = FindSingleNumber(lines[0]);
            var record = FindSingleNumber(lines[1]);
            long wins = 0;

            for (int ms = 1; ms < time; ms++)
            {
                var distance = (time - ms) * ms;
                if (distance > record)
                {
                    wins++;
                }
            }

            return wins;
        }

        static string[] TestInput() =>
        [
            "Time:      7  15   30",
            "Distance:  9  40  200"
        ];

        static List<int> FindNumbers(string line)
        {
            var result = new List<int>();
            var split = line.Split(' ');
            foreach (var s in split)
            {
                if (int.TryParse(s, out int i))
                {
                    result.Add(i);
                }
            }

            return result;
        }

        static long FindSingleNumber(string line)
        {
            var number = "";
            var split = line.Split(' ');
            foreach (var s in split)
            {
                if (long.TryParse(s, out long i))
                {
                    number += s;
                }
            }

            return long.Parse(number);
        }
    }
}
