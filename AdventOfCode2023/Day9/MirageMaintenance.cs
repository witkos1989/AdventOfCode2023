using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day9;

public sealed class MirageMaintenance
{
    private readonly List<List<long>> _values;

	public MirageMaintenance()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day9", "MirageMaintenanceInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _values = Parse(rawData).ToList();
        ExtrapolateAll(_values);
    }

    public long[] Results() => new[] { ExtrapolateAll(_values),
                                       ExtrapolateAllBackwards(_values) };

    private static long ExtrapolateAll(List<List<long>> values) =>
        values.Select(v => Extrapolate(CountDifferencesToZero(v))).Sum();

    private static long ExtrapolateAllBackwards(List<List<long>> values) =>
        values.Select(v => ExtrapolateBackwards(CountDifferencesToZero(v))).Sum();

    private static long Extrapolate(List<List<long>> differences)
    {
        long value = 0;

        for (int i = differences.Count - 1; i >= 0; i--)
            value = differences[i].Last() + value; 

        return value;
    }

    private static long ExtrapolateBackwards(List<List<long>> differences)
    {
        long value = 0;

        for (int i = differences.Count - 1; i >= 0; i--)
            value = differences[i].First() - value;

        return value;
    }

    private static List<List<long>> CountDifferencesToZero(List<long> list)
    {
        List<List<long>> results = new() { list };

        while (!results.Last().All(r => r == 0))
            results.Add(CountDifferences(results.Last()).ToList());

        return results;
    }

    private static IEnumerable<long> CountDifferences(List<long> values)
    {
        for (int i = 1; i < values.Count; i++)
            yield return values[i] - values[i - 1];
    }

    private static IEnumerable<List<long>> Parse(IEnumerable<string?> data) =>
        data.Where(line => !string.IsNullOrEmpty(line)).
        Select(line => Regex.Matches(line!, "[-0-9]{1,}").Select(m => long.Parse(m.ToString())).ToList());
}