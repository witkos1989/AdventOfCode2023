using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day6;

public sealed class WaitForIt
{
    private readonly List<long> _times;
    private readonly List<long> _distances;
    private readonly long _longTime;
    private readonly long _longDistance;

	public WaitForIt()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day6", "WaitForItInput.txt");
        StreamReader file = new(currentDirectory);
        List<string?> rawData = file.ImportData().ToList();

        _times = Parse(rawData[0]!).ToList();
        _distances = Parse(rawData[1]!).ToList();
        _longTime = IgnoreWhiteSpaceAndParse(rawData[0]!);
        _longDistance = IgnoreWhiteSpaceAndParse(rawData[1]!);
    }

    public long[] Results()
    {
        long[] results = new long[2];

        results[0] = MultiplyRacesWins(_times, _distances);

        results[1] = Race(_longTime, _longDistance);

        return results;
    }

    private static long MultiplyRacesWins(List<long> times, List<long> distances) =>
        times.Select((time, i) => Race(time, distances[i])).Aggregate((a, b) => a * b);

    private static long Race(long time, long distance) =>
        Enumerable.Range(0, (int)time).Where(t => t * (time - t) > distance).Count();

    private static IEnumerable<long> Parse(string data) =>
        Regex.Matches(data, "[0-9]{1,}").Select(match => long.Parse(match.ToString()));

    private static long IgnoreWhiteSpaceAndParse(string data) =>
        long.Parse(Regex.Match(data.Replace(" ", ""), "[0-9]{1,}").ToString());
}