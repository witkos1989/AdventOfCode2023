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

    public int[] Results() => new int[] { CountStepsToEnd(_directions, _navigation), 0 };

    private static int CountStepsToEnd(char[] directions, Dictionary<string, (string, string)> navigation)
    {
        int count = 0;
        string current = "AAA";

        for (int i = 0; i < directions.Length; i++)
        {
            if (current == "ZZZ")
                break;

            (string left, string right) = navigation[current];

            current = directions[i] == 'L' ? left : right;

            count += 1;

            i = i + 1 == directions.Length ? -1 : i;
        }
        return count;
    }

    private static char[] ParseDirections(IEnumerable<string?> data) =>
        data.First()!.ToArray();

    private static Dictionary<string, (string, string)> ParseNavigation(IEnumerable<string?> data)
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