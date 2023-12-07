using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var testInput = TestInput().ToList();
            var input = InputHelper.ReadInput("05.txt").ToList();

            var testSeeds = MapSeeds(testInput);
            Console.WriteLine(GetLowestLocationValue(testSeeds));

            var part1 = MapSeeds(input);
            Console.WriteLine(GetLowestLocationValue(part1));

            var part2Test = MapSeedPairs(TestInput().ToList());
            Console.WriteLine(GetLowestLocationValue(part2Test));

            var part2 = MapSeedPairs(input);
            Console.WriteLine(GetLowestLocationValue(part2));
        }

        static List<SeedMap> MapSeedPairs(List<string> input)
        {
            var seeds = new List<SeedMap>();
            var seedLine = input[0][7..];
            var pairs = seedLine.Split(' ').Select(s => long.Parse(s)).ToList();
            for (int i = 0; i < pairs.Count; i += 2)
            {
                var start = pairs[i];
                var range = pairs[i + 1];
                for (int x = 0; x < range; x++)
                {
                    seeds.Add(new SeedMap { Seed = start + x });
                }
            }

            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().Order().ToList();

            for (int i = 0; i < categories.Count - 1; i++)
            {
                MapSourceToDestination(seeds, categories[i], categories[i + 1], input);
            }

            return seeds;
        }

        static List<SeedMap> MapSeeds(List<string> input)
        {
            var seeds = new List<SeedMap>();
            var seedLine = input[0][7..];
            foreach (var seed in seedLine.Split(' '))
            {
                seeds.Add(new SeedMap { Seed = long.Parse(seed) });
            }

            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().Order().ToList();

            for (int i = 0; i < categories.Count - 1; i++)
            {
                MapSourceToDestination(seeds, categories[i], categories[i + 1], input);
            }

            return seeds;
        }

        static void MapSourceToDestination(List<SeedMap> seeds, Category sourceCategory, Category destinationCategory, List<string> input)
        {
            var indexName = $"{sourceCategory.ToString().ToLower()}-to-{destinationCategory.ToString().ToLower()} map:";
            var indexStart = input.IndexOf(indexName) + 1;
            var loopIndex = 0;

            while (indexStart + loopIndex < input.Count)
            {
                var map = input[indexStart + loopIndex];

                var split = map.Split(' ');
                if (!long.TryParse(split[0], out long destinationStart))
                {
                    break;
                }
                var sourceStart = long.Parse(split[1]);
                var range = long.Parse(split[2]);

                Parallel.ForEach(seeds, seed =>
                {
                    var sourceValue = seed.GetCategoryValue(sourceCategory).Value;
                    if (sourceValue >= sourceStart && (sourceStart + range) > sourceValue)
                    {
                        var diff = sourceValue - sourceStart;
                        var destinationValue = diff + destinationStart;
                        seed.UpdateCategoryValue(destinationCategory, destinationValue);
                    }
                });

                loopIndex++;
            }

            while (true)
            {
                var seed = GetSeedMapOnCategory(destinationCategory, null, seeds);
                if (seed == null)
                {
                    break;
                }

                seed.UpdateCategoryValue(destinationCategory, seed.GetCategoryValue(sourceCategory).Value);
            }
        }

        static SeedMap GetSeedMapOnCategory(Category category, long? value, List<SeedMap> seedMaps)
        {
            return category switch
            {
                Category.Seed => seedMaps.FirstOrDefault(s => s.Seed == value),
                Category.Soil => seedMaps.FirstOrDefault(s => s.Soil == value),
                Category.Fertilizer => seedMaps.FirstOrDefault(s => s.Fertilizer == value),
                Category.Water => seedMaps.FirstOrDefault(s => s.Water == value),
                Category.Light => seedMaps.FirstOrDefault(s => s.Light == value),
                Category.Temperature => seedMaps.FirstOrDefault(s => s.Temperature == value),
                Category.Humidity => seedMaps.FirstOrDefault(s => s.Humidity == value),
                Category.Location => seedMaps.FirstOrDefault(s => s.Location == value),
                _ => throw new Exception($"Could not find Seed map with {category} {value}"),
            };
        }

        static string[] TestInput()
        {
            return new string[]
            {
                "seeds: 79 14 55 13",
                "seed-to-soil map:",
                "50 98 2",
                "52 50 48",
                "soil-to-fertilizer map:",
                "0 15 37",
                "37 52 2",
                "39 0 15",
                "fertilizer-to-water map:",
                "49 53 8",
                "0 11 42",
                "42 0 7",
                "57 7 4",
                "water-to-light map:",
                "88 18 7",
                "18 25 70",
                "light-to-temperature map:",
                "45 77 23",
                "81 45 19",
                "68 64 13",
                "temperature-to-humidity map:",
                "0 69 1",
                "1 0 69",
                "humidity-to-location map:",
                "60 56 37",
                "56 93 4",
            };
        }

        static long GetLowestLocationValue(List<SeedMap> seedMaps)
        {
            return seedMaps.Select(s => s.Location.Value).Min();
        }
    }

    class SeedMap
    {
        public long Seed { get; set; }
        public long? Soil { get; set; }
        public long? Fertilizer { get; set; }
        public long? Water { get; set; }
        public long? Light { get; set; }
        public long? Temperature { get; set; }
        public long? Humidity { get; set; }
        public long? Location { get; set; }

        public override string ToString()
        {
            return $"{Seed};{Soil};{Fertilizer};{Water};{Light};{Temperature};{Humidity};{Location}";
        }

        public void UpdateCategoryValue(Category category, long value)
        {
            switch (category)
            {
                case Category.Seed:
                    Seed = value;
                    break;
                case Category.Soil:
                    Soil = value;
                    break;
                case Category.Fertilizer:
                    Fertilizer = value;
                    break;
                case Category.Water:
                    Water = value;
                    break;
                case Category.Light:
                    Light = value;
                    break;
                case Category.Temperature:
                    Temperature = value;
                    break;
                case Category.Humidity:
                    Humidity = value;
                    break;
                case Category.Location:
                    Location = value;
                    break;
                default:
                    break;
            }
        }

        public long? GetCategoryValue(Category category)
        {
            return category switch
            {
                Category.Seed => Seed,
                Category.Soil => Soil,
                Category.Fertilizer => Fertilizer,
                Category.Water => Water,
                Category.Light => Light,
                Category.Temperature => Temperature,
                Category.Humidity => Humidity,
                Category.Location => Location,
                _ => throw new Exception("Could not find Category"),
            };
        }
    }

    enum Category
    {
        Seed,
        Soil,
        Fertilizer,
        Water,
        Light,
        Temperature,
        Humidity,
        Location
    }
}