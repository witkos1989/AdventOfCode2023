namespace AdventOfCode2023.Day3;

public class GearRatios
{
    private readonly string[] _schematic;

	public GearRatios()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day3", "GearRatiosInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _schematic = rawData.Where(d => !string.IsNullOrEmpty(d)).ToArray()!;
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = FindPartNumber(_schematic);

        results[1] = FindGearRatio(_schematic);

        return results;
    }

    private static int FindPartNumber(string[] schematic)
    {
        List<int> numbers = new();

        for (int i = 0; i < schematic.Length; i++)
        {
            for (int j = 0; j < schematic[i].Length; j++)
            {
                if (!char.IsNumber(schematic[i][j]))
                    continue;

                bool isPartNumber = FindAdjacentSymbol(schematic, i, j);

                if (isPartNumber)
                {
                    numbers.Add(ReadPartNumber(schematic, i, j));

                    while (j + 1 < schematic[i].Length &&
                           char.IsNumber(schematic[i][j + 1]))
                        j += 1;
                }
            }
        }

        return numbers.Sum();
    }

    private static int FindGearRatio(string[] schematic)
    {
        int gears = 0;

        for (int i = 0; i < schematic.Length; i++)
        {
            for (int j = 0; j < schematic[i].Length; j++)
            {
                if (schematic[i][j] != '*')
                    continue;

                gears += FindTwoAdjacentNumbers(schematic, i, j);
            }
        }

        return gears;
    }

    private static int FindTwoAdjacentNumbers(string[] schematic, int x, int y)
    {
        List<int> numbers = new();
        List<int[]> adjacentPositions = new() {
            new[] { x, y - 1 }, new[] { x, y + 1 },
            new[] { x - 1, y }, new[] { x + 1, y },
            new[] { x - 1, y - 1 }, new[] { x - 1, y + 1},
            new[] { x + 1, y - 1 }, new[] { x + 1, y + 1} };

        foreach (int[] position in adjacentPositions)
        {
            int newX = position[0], newY = position[1];

            if (newX < 0 || newX >= schematic.Length ||
                newY < 0 || newY >= schematic[x].Length)
                continue;

            if (char.IsNumber(schematic[newX][newY]))
                numbers.Add(ReadPartNumber(schematic, newX, newY));
        }

        numbers = numbers.Distinct().ToList();

        return numbers.Count == 2 ? numbers.First() * numbers.Last() : 0;
    }

    private static bool FindAdjacentSymbol(string[] schematic, int x, int y)
    {
        List<int[]> adjacentPositions = new() {
            new[] { x, y - 1 }, new[] { x, y + 1 },
            new[] { x - 1, y }, new[] { x + 1, y },
            new[] { x - 1, y - 1 }, new[] { x - 1, y + 1},
            new[] { x + 1, y - 1 }, new[] { x + 1, y + 1} };

        foreach (int[] position in adjacentPositions)
        {
            int newX = position[0], newY = position[1];

            if (newX < 0 || newX >= schematic.Length ||
                newY < 0 || newY >= schematic[x].Length)
                continue;

            if (schematic[newX][newY] != '.' &&
                !char.IsNumber(schematic[newX][newY]))
                return true;
        }

        return false;
    }

    private static int ReadPartNumber(string[] schematic, int x, int y)
    {
        int result = schematic[x][y] - 48;

        while (y - 1 >= 0 && char.IsNumber(schematic[x][y - 1]))
        {
            y -= 1;

            result = schematic[x][y] - 48;
        }

        while (y + 1 < schematic[x].Length && char.IsNumber(schematic[x][y + 1]))
        {
            y += 1;

            result = result * 10 + (schematic[x][y] - 48);
        }

        return result;
    }
}