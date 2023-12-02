namespace AdventOfCode2023.Extensions;

public static class Extensions
{
    public static IEnumerable<string?> ImportData(this StreamReader stream)
    {
        while (!stream.EndOfStream)
        {
            string? line = stream.ReadLine();

            yield return line;
        }
    }
}