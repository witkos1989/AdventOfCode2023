namespace AdventOfCode2023.Day8;

public class HauntedWasteland
{
    private readonly char[] _directions;
    private readonly Dictionary<string, (string, string)> _navigation;

    public HauntedWasteland()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day8", "HauntedWastelandInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _directions = ParseDirections(rawData);
        _navigation = ParseNavigation(rawData);
    }

    public long[] Results() =>
        new long[] { CountStepsToEnd("AAA", "ZZZ", _directions, _navigation),
                     CountStepsToManyEnds(_directions, _navigation) };

    private static int CountStepsToEnd(
        string start,
        string end,
        char[] directions,
        Dictionary<string, (string, string)> navigation)
    {
        int count = 0;
        string current = start;

        for (int i = 0; i < directions.Length; i++)
        {
            if (end.Length == 1 ? current.Last() == end.Last() : current == end)
                break;

            (string left, string right) = navigation[current];

            current = directions[i] == 'L' ? left : right;

            count += 1;

            i = i + 1 == directions.Length ? -1 : i;
        }
        return count;
    }

    private static long CountStepsToManyEnds(
        char[] directions,
        Dictionary<string, (string, string)> navigation) =>
        LCM(navigation.Where(n => n.Key.Last() == 'A').
            Select(n => n.Key).
            Select(start => CountStepsToEnd(start, "Z", directions, navigation)).
            ToArray());

    private static long LCM(int[] steps)
    {
        long maxSteps = steps.Max();

        for (long i = maxSteps; ; i += maxSteps)
        {
            long[] moduloResult = new long[6];
            for (int s = 0; s < 6; s++)
                moduloResult[s] = i % steps[s];

            if (moduloResult.All(r => r == 0))
            {
                return i;
            }
        }
    }

    private static char[] ParseDirections(IEnumerable<string?> data) =>
        data.First()!.ToArray();

    private static Dictionary<string, (string, string)> ParseNavigation(
        IEnumerable<string?> data)
    {
        Dictionary<string, (string, string)> result = new();

        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            string[] nodeAndDirections = line.Split('=');
            string[] directions = nodeAndDirections[1].Trim().Replace("(", "").Replace(")", "").Split(',');
            result.Add(nodeAndDirections[0].Trim(), (directions[0].Trim(), directions[1].Trim()));
        }

        return result;
    }
}