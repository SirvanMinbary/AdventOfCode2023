using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Day03
{
    internal class Program
    {
        static string[] Input => InputHelper.ReadInput("03.txt");
        //static string[] Input => new[]
        //{
        //    "467..114..",
        //    "...*......",
        //    "..35..633.",
        //    "......#...",
        //    "617*......",
        //    ".....+.58.",
        //    "..592.....",
        //    "......755.",
        //    "...$.*....",
        //    ".664.598.."
        //};
        static int Width => Input.First().Length;
        static int Height => Input.Length - 1;

        static void Main(string[] args)
        {
            var sum = 0;
            var sum2 = 0;

            for (int y = 0; y < Input.Length; y++)
            {
                var line = Input[y];
                var currentNumber = "";
                for (int x = 0; x < line.Length; x++)
                {
                    var symbol = line[x];

                    if (char.IsDigit(symbol))
                    {
                        currentNumber += symbol;
                    }
                    else if (currentNumber.Length > 0)
                    {
                        if (IsValidSymbol(symbol) || IsAdjacentToSymbol(x - currentNumber.Length, y, currentNumber.Length))
                        {
                            sum += int.Parse(currentNumber);
                        }

                        currentNumber = "";
                    }

                    if (symbol == '*')
                    {
                        sum2 += FindGearRatio(x, y);
                    }
                }

                if (currentNumber.Length > 0)
                {
                    if (IsAdjacentToSymbol(Width - currentNumber.Length, y, currentNumber.Length))
                    {
                        sum += int.Parse(currentNumber);
                    }
                }
            }

            Console.WriteLine(sum);
            Console.WriteLine(sum2);
        }

        static bool IsAdjacentToSymbol(int x, int y, int length)
        {
            var symbols = "";

            var adjustedStart = x == 0 ? x : x - 1;
            var adjustedLength = x + length >= Width ? length + 1 : length + 2;

            if (x != 0)
            {
                symbols += Input[y][x - 1];
            }

            if (y != 0)
            {
                var lineAbove = Input[y - 1];
                symbols += lineAbove.Substring(adjustedStart, adjustedLength);
            }

            if (y < Height)
            {
                var lineBelow = Input[y + 1];
                symbols += lineBelow.Substring(adjustedStart, adjustedLength);
            }

            return symbols.Any(IsValidSymbol);
        }

        static bool IsValidSymbol(char c)
        {
            return !char.IsLetterOrDigit(c) && c != '.';
        }

        static int FindGearRatio(int x, int y)
        {
            var adjustedLength = x + 1 >= Width ? x + 1 : x + 2;
            var partNumbers = new List<int>();

            if (y != 0)
            {
                var aboveLine = Input[y - 1];
                var candidates = new List<int>();

                var aboveLeft = FindNumber(x - 1, aboveLine, true);
                if (aboveLeft != null)
                {
                    candidates.Add(aboveLeft.Value);
                    var aboveLeftBackward = FindNumber(x - 1, aboveLine, false);
                    candidates.Add(aboveLeftBackward.Value);
                }

                candidates.Add(FindNumber(x, aboveLine, true).GetValueOrDefault());
                candidates.Add(FindNumber(x, aboveLine, false).GetValueOrDefault());
                candidates.Add(FindNumber(x + 1, aboveLine, true).GetValueOrDefault());
                candidates.Add(FindNumber(x + 1, aboveLine, false).GetValueOrDefault());
                partNumbers.AddRange(candidates.OrderDescending().Where(c => c > 0).Take(2));
                //partNumbers.Add(candidates.Max());
            }

            if (x != 0)
            {
                var left = FindNumber(x - 1, Input[y], false);
                if (left.HasValue)
                {
                    partNumbers.Add(left.Value);
                }
            }

            if (x + 1 < Width)
            {
                var right = FindNumber(x + 1, Input[y], true);
                if (right.HasValue)
                {
                    partNumbers.Add(right.Value);
                }
            }

            if (y + 1 <= Height)
            {
                var candidates = new List<int>();
                var line = Input[y + 1];

                var belowLeft = FindNumber(x - 1, line, true);
                if (belowLeft != null)
                {
                    candidates.Add(belowLeft.Value);
                    var aboveLeftBackward = FindNumber(x - 1, line, false);
                    candidates.Add(aboveLeftBackward.Value);

                }
                // om det finns två värden på en rad så inkluderas de inte här. Måste fixa
                // se rad 10, index 49 (575.970)
                candidates.Add(FindNumber(x, line, true).GetValueOrDefault());
                candidates.Add(FindNumber(x, line, false).GetValueOrDefault());
                candidates.Add(FindNumber(x + 1, line, true).GetValueOrDefault());
                candidates.Add(FindNumber(x + 1, line, false).GetValueOrDefault());
                partNumbers.AddRange(candidates.OrderDescending().Where(c => c > 0).Take(2));
                //partNumbers.Add(candidates.Max());
            }

            partNumbers = partNumbers.Where(n => n != 0).ToList();

            if (partNumbers.Count == 2)
            {
                var part1 = partNumbers[0];
                var part2 = partNumbers[1];
                return part1 * part2;
            }

            return 0;
        }

        static int? FindNumber(int start, string line, bool forward)
        {
            var digits = new StringBuilder();
            var index = start;
            while (true && index >= 0 && index < Width)
            {
                var symbol = line[index];
                if (char.IsDigit(symbol))
                {
                    if (forward)
                    {
                        digits.Append(symbol);
                        index++;
                    }
                    else
                    {
                        digits.Insert(0, symbol);
                        index--;
                    }
                }
                else
                {
                    break;
                }
            }

            return digits.Length > 0 ? int.Parse(digits.ToString()) : null;
        }
    }
}