namespace AdventOfCode2023.Day10;

public sealed class PipeMaze
{
    private record Point(sbyte X, sbyte Y);
    private readonly (Point, Point)[][] _directions;
    private readonly (int, int) _start;

    public PipeMaze()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day10", "PipeMazeInput.txt");
        StreamReader file = new(currentDirectory);
        List<string?> rawData = file.ImportData().ToList();

        _directions = Parse(rawData).ToArray();
        _start = (FindX(rawData), FindY(rawData));
    }

    public int[] Results() =>
        new int[] { GoThroughPipes(_directions, _start), 0 };


    private static int GoThroughPipes((Point, Point)[][] directions, (int x, int y) start)
    {
        int steps = 1;
        (int x, int y) next = Select(directions, start);
        (int, int) prev = start;

        while (next != start)
        {
            (prev, next) = (next, NextStep(directions[next.y][next.x], next, prev));

            steps += 1;
        }

        return steps / 2;
    }

    private static (int, int) NextStep((Point, Point) pipe, (int x, int y) curr, (int x, int y) prev) =>
        (curr.x + pipe.Item1.X, curr.y + pipe.Item1.Y) == prev ?
        (curr.x + pipe.Item2.X, curr.y + pipe.Item2.Y) :
        (curr.x + pipe.Item1.X, curr.y + pipe.Item1.Y);

    private static (int, int) Select((Point, Point)[][] directions, (int x, int y) start) =>
        start.x + 1 < directions.Length && CheckDirections(directions[start.y][start.x + 1], start, (start.x + 1, start.y)) ?
        (start.x + 1, start.y) :
        start.y + 1 < directions.Length && CheckDirections(directions[start.y + 1][start.x], start, (start.x, start.y + 1)) ?
        (start.x, start.y + 1) :
        start.x - 1 >= 0 && CheckDirections(directions[start.y][start.x - 1], start, (start.x - 1, start.y)) ?
        (start.x - 1, start.y) :
        (start.x, start.y - 1);

    private static bool CheckDirections((Point, Point) pipe, (int x, int y) prev, (int x, int y) curr) =>
        (curr.x + pipe.Item1.X, curr.y + pipe.Item1.Y) == prev ||
        (curr.x + pipe.Item2.X, curr.y + pipe.Item2.Y) == prev;

    private static int FindY(List<string?> data) =>
        data.IndexOf(LineWithStart(data));

    private static int FindX(List<string?> data) =>
        LineWithStart(data).IndexOf('S');

    private static string LineWithStart(List<string?> data) =>
        data.Where(l => l!.Contains('S')).First()!;

    private static IEnumerable<(Point, Point)[]> Parse(List<string?> data) =>
        data.Where(l => !string.IsNullOrEmpty(l)).
        Select(line => line!.Select(point => Convert(point)).
        ToArray());

    private static (Point, Point) Convert(char sign) =>
        sign switch
        {
            '-' => (new Point(-1, 0), new Point(1, 0)),
            '|' => (new Point(0, 1), new Point(0, -1)),
            'L' => (new Point(0, -1), new Point(1, 0)),
            'J' => (new Point(0, -1), new Point(-1, 0)),
            '7' => (new Point(0, 1), new Point(-1, 0)),
            'F' => (new Point(0, 1), new Point(1, 0)),
            'S' => (new Point(-1, -1), new Point(-1, -1)),
            _ => (new Point(0, 0), new Point(0, 0))
        };
}