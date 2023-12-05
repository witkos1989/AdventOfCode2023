namespace AdventOfCode2023.Day5;

public sealed class IfYouGiveASeedAFertilizer
{
    private readonly Almanac _almanac;

	public IfYouGiveASeedAFertilizer()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day5", "IfYouGiveASeedAFertilizerInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _almanac = Parse(rawData.ToList());
    }

    public long[] Results()
    {
        long[] results = new long[2];

        results[0] = _almanac.LowestSeedsLocation();

        results[1] = _almanac.LowestSeedsRangeLocation();

        return results;
    }

    private static Almanac Parse(List<string?> data)
    {
        byte saveDataTo = 0;
        List<long> seeds = new();
        Dictionary<(long, long), (long, long)> seedToSoil = new();
        Dictionary<(long, long), (long, long)> soilToFertilizer = new();
        Dictionary<(long, long), (long, long)> fertilizerToWater = new();
        Dictionary<(long, long), (long, long)> waterToLight = new();
        Dictionary<(long, long), (long, long)> lightToTemperature = new();
        Dictionary<(long, long), (long, long)> temperatureToHumidity = new();
        Dictionary<(long, long), (long, long)> humidityToLocation = new();

        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
            {
                saveDataTo = 0;
                continue;
            }

            string[] map = line.Split(' ');

            switch (saveDataTo)
            {
                case 1:
                    seedToSoil.Add(
                        ConvertSource(map[0], map[2]),
                        ConvertDestination(map[1], map[2]));
                    break;
                case 2:
                    soilToFertilizer.Add(
                        ConvertSource(map[0], map[2]),
                        ConvertDestination(map[1], map[2]));
                    break;
                case 3:
                    fertilizerToWater.Add(
                        ConvertSource(map[0], map[2]),
                        ConvertDestination(map[1], map[2]));
                    break;
                case 4:
                    waterToLight.Add(
                        ConvertSource(map[0], map[2]),
                        ConvertDestination(map[1], map[2]));
                    break;
                case 5:
                    lightToTemperature.Add(
                        ConvertSource(map[0], map[2]),
                        ConvertDestination(map[1], map[2]));
                    break;
                case 6:
                    temperatureToHumidity.Add(
                        ConvertSource(map[0], map[2]),
                        ConvertDestination(map[1], map[2]));
                    break;
                case 7:
                    humidityToLocation.Add(
                        ConvertSource(map[0], map[2]),
                        ConvertDestination(map[1], map[2]));
                    break;
            }

            switch (line)
            {
                case string l when l.Contains("seeds"):
                    string[] seedsInput = l.Split(':')[1].Trim().Split(' ');

                    foreach (string seedInput in seedsInput)
                        seeds.Add(long.Parse(seedInput));
                    break;
                case string l when l.Contains("seed-to-soil map:"):
                    saveDataTo = 1;
                    break;
                case string l when l.Contains("soil-to-fertilizer map:"):
                    saveDataTo = 2;
                    break;
                case string l when l.Contains("fertilizer-to-water map:"):
                    saveDataTo = 3;
                    break;
                case string l when l.Contains("water-to-light map:"):
                    saveDataTo = 4;
                    break;
                case string l when l.Contains("light-to-temperature map:"):
                    saveDataTo = 5;
                    break;
                case string l when l.Contains("temperature-to-humidity map:"):
                    saveDataTo = 6;
                    break;
                case string l when l.Contains("humidity-to-location map:"):
                    saveDataTo = 7;
                    break;
            }
        }

        return new Almanac(seeds,
                           seedToSoil,
                           soilToFertilizer,
                           fertilizerToWater,
                           waterToLight,
                           lightToTemperature,
                           temperatureToHumidity,
                           humidityToLocation);
    }

    private static (long, long) ConvertSource(string source, string range) =>
        (long.Parse(source), long.Parse(source) + long.Parse(range) - 1);
    
    private static (long, long) ConvertDestination(string destination, string range) =>
        (long.Parse(destination), long.Parse(destination) + long.Parse(range) - 1);
}