using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Advent of Code/2023/1.txt");
            var input = File.ReadAllLines(path);

            int sum1 = CalculateCalibrationValue(input);
            Console.WriteLine(sum1);

            int testSum = CalculateCalibrationWithLetters(TestInput());
            Console.WriteLine(testSum);

            int sum2 = CalculateCalibrationWithLetters(input);
            Console.WriteLine(sum2);
        }

        private static int CalculateCalibrationValue(string[] input)
        {
            int sum = 0;

            foreach (var line in input)
            {
                var matches = Regex.Matches(line, @"\d{1}");
                var calibrationValue = int.Parse(matches.First().Value + matches.Last().Value);
                sum += calibrationValue;
            }

            return sum;
        }

        private static int CalculateCalibrationWithLetters(string[] input)
        {
            int sum = 0;

            foreach (var line in input)
            {
                var matches = Regex.Matches(line, @"(\d{1})|(?=(one))|(?=(two))|(?=(three))|(?=(four))|(?=(five))|(?=(six))|(?=(seven))|(?=(eight))|(?=(nine))");

                var digits = new StringBuilder();
                digits.Append(GetDigit(matches.First()));
                digits.Append(GetDigit(matches.Last()));
                sum += int.Parse(digits.ToString());
            }

            return sum;
        }

        private static string GetDigit(Match match)
        {
            var value = match.Groups.Values.First(v => v.Length > 0).Value;

            if (int.TryParse(value, out int _))
            {
                return value;
            }
            else
            {
                return value switch
                {
                    "one" => "1",
                    "two" => "2",
                    "three" => "3",
                    "four" => "4",
                    "five" => "5",
                    "six" => "6",
                    "seven" => "7",
                    "eight" => "8",
                    "nine" => "9",
                    _ => throw new Exception($"Could not parse value: {value}"),
                };
            }
        }

        private static string[] TestInput()
        {
            return new[]
            {
                "two1nine",
                "eightwothree",
                "abcone2threexyz",
                "xtwone3four",
                "4nineeightseven2",
                "zoneight234",
                "7pqrstsixteen"
            };
        }
    }
}