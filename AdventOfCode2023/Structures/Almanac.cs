namespace AdventOfCode2023.Structures;

public class Almanac
{
	private readonly List<long> _seeds;
	private readonly Dictionary<(long, long), (long, long)> _seedToSoilMap;
    private readonly Dictionary<(long, long), (long, long)> _soilToFertilizerMap;
    private readonly Dictionary<(long, long), (long, long)> _fertilizerToWaterMap;
    private readonly Dictionary<(long, long), (long, long)> _waterToLightMap;
    private readonly Dictionary<(long, long), (long, long)> _lightToTemperatureMap;
    private readonly Dictionary<(long, long), (long, long)> _temperatureToHumidityMap;
    private readonly Dictionary<(long, long), (long, long)> _humidityToLocationMap;

    public Almanac(List<long> seeds,
                   Dictionary<(long, long), (long, long)> seedToSoilMap,
                   Dictionary<(long, long), (long, long)> soilToFertilizerMap,
                   Dictionary<(long, long), (long, long)> fertilizerToWaterMap,
                   Dictionary<(long, long), (long, long)> waterToLightMap,
                   Dictionary<(long, long), (long, long)> lightToTemperatureMap,
                   Dictionary<(long, long), (long, long)> temperatureToHumidityMap,
                   Dictionary<(long, long), (long, long)> humidityToLocationMap)
	{
        _seeds = seeds;
        _seedToSoilMap = seedToSoilMap;
        _soilToFertilizerMap = soilToFertilizerMap;
        _fertilizerToWaterMap = fertilizerToWaterMap;
        _waterToLightMap = waterToLightMap;
        _lightToTemperatureMap = lightToTemperatureMap;
        _temperatureToHumidityMap = temperatureToHumidityMap;
        _humidityToLocationMap = humidityToLocationMap;
    }

    public long LowestSeedsLocation() =>
        CorrespondingSoil(_seeds).Min();

    public long LowestSeedsRangeLocation()
    {
        List<long> seeds = SeedsRange().ToList();
        Dictionary<int, long> correspondingVals = new();
        int sqrtLength = (int)Math.Sqrt(seeds.Count);

        for (int i = 0; i < seeds.Count; i += sqrtLength)
            correspondingVals.Add(i, CorrespondingSoil(seeds[i]));

        int tempMinimal = correspondingVals.MinBy(v => v.Value).Key;
        List<long> results = new();

        for (int i = tempMinimal - sqrtLength; i < tempMinimal + sqrtLength; i ++)
            results.Add(CorrespondingSoil(seeds[i]));

        return results.Min();
    }

    private IEnumerable<long> CorrespondingSoil(List<long> seeds) =>
        seeds.Select(seed => CorrespondingFertilizer(Convert(Find(_seedToSoilMap, seed), seed)));

    private long CorrespondingSoil(long seed) =>
        CorrespondingFertilizer(Convert(Find(_seedToSoilMap, seed), seed));

    private long CorrespondingFertilizer(long seed) =>
        CorrespondingWater(Convert(Find(_soilToFertilizerMap, seed), seed));

    private long CorrespondingWater(long seed) =>
        CorrespondingLight(Convert(Find(_fertilizerToWaterMap, seed), seed));

    private long CorrespondingLight(long seed) =>
        CorrespondingTemperature(Convert(Find(_waterToLightMap, seed), seed));

    private long CorrespondingTemperature(long seed) =>
        CorrespondingHumidity(Convert(Find(_lightToTemperatureMap, seed), seed));

    private long CorrespondingHumidity(long seed) =>
        CorrespondingLocation(Convert(Find(_temperatureToHumidityMap, seed), seed));

    private long CorrespondingLocation(long seed) =>
        Convert(Find(_humidityToLocationMap, seed), seed);

    private static KeyValuePair<(long, long), (long, long)> Find(Dictionary<(long, long), (long, long)> map, long needle) =>
        map.FirstOrDefault(s => s.Value.Item1 <= needle && s.Value.Item2 >= needle);

    private static long Convert(KeyValuePair<(long, long), (long, long)> map, long needle) =>
        map.Key == (0, 0) && map.Value == (0, 0) ? needle : map.Key.Item1 + (needle - map.Value.Item1);

    private IEnumerable<long> SeedsRange()
    {
        for (int i = 0; i < _seeds.Count; i += 2)
            for (long j = _seeds[i]; j < _seeds[i] + _seeds[i + 1]; j++)
                yield return j;
    }
}