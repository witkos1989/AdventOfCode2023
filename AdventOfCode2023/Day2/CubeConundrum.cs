namespace AdventOfCode2023.Day2;

public sealed class CubeConundrum
{
    private readonly BagOfCubes _bag;
    private readonly List<(int, List<BagOfCubes>)> _games;

	public CubeConundrum()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day2", "CubeConundrumInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _bag = new BagOfCubes(12, 13, 14);
        _games = Parse(rawData.ToList()).ToList();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = SumOf(_games, _bag);

        results[1] = SumOf(_games);

        return results;
    }

    private static int SumOf(List<(int, List<BagOfCubes>)> games, BagOfCubes bag) =>
        games.Where(game => IsValid(game.Item2, bag)).Sum(g => g.Item1);

    private static int SumOf(List<(int, List<BagOfCubes>)> games) =>
        PowerOf(games).Sum();

    private static bool IsValid(List<BagOfCubes> gameSets, BagOfCubes bag) =>
        !gameSets.Where(set => (bag - set).CheckIfBelowZero()).Any();

    private static IEnumerable<int> PowerOf(List<(int, List<BagOfCubes>)> games) =>
        games.Select(g => FewestBagOfCubesFor(g.Item2).PowerOfCubes());

    private static BagOfCubes FewestBagOfCubesFor(List<BagOfCubes> game) =>
        BagOfCubes.MinimumCubeBag(game);

    private static IEnumerable<(int, List<BagOfCubes>)> Parse(List<string?> data)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            string[] dayAndCubes = line.Split(':');
            int day = int.Parse(dayAndCubes[0].Trim().Split(' ')[1]);
            string[] sets = dayAndCubes[1].Trim().Split(';');
            List<BagOfCubes> revealedCubes = new();

            foreach (string set in sets)
            {
                string[] cubes = set.Trim().Split(',');
                int redCubes = 0, greenCubes = 0, blueCubes = 0;

                foreach (string cube in cubes)
                {
                    string[] numAndName = cube.Trim().Split(' ');

                    switch (numAndName[1])
                    {
                        case "red":
                            redCubes = int.Parse(numAndName[0]);
                            break;
                        case "green":
                            greenCubes = int.Parse(numAndName[0]);
                            break;
                        case "blue":
                            blueCubes = int.Parse(numAndName[0]);
                            break;
                    }
                }

                revealedCubes.Add(new BagOfCubes(redCubes, greenCubes, blueCubes));
            }

            yield return (day, revealedCubes);
        }
    }
}