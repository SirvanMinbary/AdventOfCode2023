using System;
using System.Linq;
using System.Text.RegularExpressions;
using Utility;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = InputHelper.ReadInput("02.txt");

            var redLimit = 12;
            var greenLimit = 13;
            var blueLimit = 14;
            var sum = 0;

            foreach (var line in input)
            {
                var gameId = int.Parse(line.Substring(5, line.IndexOf(':') - 5));

                var matches = Regex.Matches(line, @"(?<red>\d+) red|(?<green>\d+) green|(?<blue>\d+) blue");

                var red = GetMaxForColor("red", matches);
                var green = GetMaxForColor("green", matches);
                var blue = GetMaxForColor("blue", matches);

                if (red <= redLimit
                    && green <= greenLimit
                    && blue <= blueLimit)
                {
                    sum += gameId;
                }
            }

            Console.WriteLine(sum);

            var sum2 = 0;

            foreach (var line in input)
            {
                var gameId = int.Parse(line.Substring(5, line.IndexOf(':') - 5));

                var start = line.Substring(line.IndexOf(':') + 1);
                var split = start.Split(';');

                var minRed = 0;
                var minGreen = 0;
                var minBlue = 0;

                foreach (var set in split)
                {
                    var red = ReadValueForColor("red", set);
                    if (red > minRed)
                    {
                        minRed = red;
                    }

                    var green = ReadValueForColor("green", set);
                    if (green > minGreen)
                    {
                        minGreen = green;
                    };

                    var blue = ReadValueForColor("blue", set);
                    if (blue > minBlue)
                    {
                        minBlue = blue;
                    }
                }

                sum2 += minRed * minGreen * minBlue;
            }

            Console.WriteLine(sum2);
        }

        private static int GetMaxForColor(string color, MatchCollection matches)
        {
            var colorMatches = matches.SelectMany(m => m.Groups.Values.Where(v => v.Name == color && v.Success));
            return colorMatches.Max(m => int.Parse(m.Value));
        }

        private static int ReadValueForColor(string color, string set)
        {
            var split = set.Split(',');
            var colorValue = split.FirstOrDefault(s => s.EndsWith(color));
            if (string.IsNullOrEmpty(colorValue))
            {
                return 0;
            }

            return int.Parse(colorValue.TrimStart().Split(' ').First());

        }
    }
}