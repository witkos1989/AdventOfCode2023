namespace AdventOfCode2023.Day1;

public sealed class Trebuchet
{
    private readonly List<string> _calibration;
    private readonly Dictionary<int, string> _numberNames;
    public Trebuchet()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day1", "TrebuchetInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _calibration = rawData.Where(d => !string.IsNullOrEmpty(d)).ToList()!;
        _numberNames = new Dictionary<int, string> {
            { 1, "one" }, { 2, "two" }, { 3, "three" },
            { 4, "four" }, { 5, "five" }, { 6, "six" },
            { 7, "seven" }, { 8, "eight" }, { 9, "nine" } };
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = GetCalibrationValueWithDigitsOnly(_calibration).Sum();

        results[1] = GetCalibrationValueWithLettersAndDigits(
            _calibration, _numberNames).Sum();

        return results;
    }

    private static IEnumerable<int> GetCalibrationValueWithDigitsOnly(
        List<string> calibrationData)
    {
        foreach (string data in calibrationData)
        {
            Dictionary<int, int> indexedValues = ExtractNumbersFromString(data);

            if (indexedValues.Count == 0)
                continue;

            yield return indexedValues.MinBy(k => k.Key).Value * 10 +
                         indexedValues.MaxBy(k => k.Key).Value;
        }
    }

    private static IEnumerable<int> GetCalibrationValueWithLettersAndDigits(
        List<string> calibrationData,
        Dictionary<int, string> numberNames)
    {
        foreach (string data in calibrationData)
        {
            Dictionary<int, int> indexedValues = ExtractNumbersFromString(data);

            AddNumbersToDictionary(data, indexedValues, numberNames);

            if (indexedValues.Count == 0)
                continue;

            yield return indexedValues.MinBy(k => k.Key).Value * 10 +
                         indexedValues.MaxBy(k => k.Key).Value;
        }
    }

    private static Dictionary<int, int> ExtractNumbersFromString(string data) =>
        data.Select((d, i) => char.IsDigit(d) ? new { d, i } : null).
        Where(v => v is not null).
        ToDictionary(key => key!.i, value => int.Parse(value!.d.ToString()));

    private static void AddNumbersToDictionary(
        string data,
        Dictionary<int,int> values,
        Dictionary<int, string> numberNames)
    {
        foreach (KeyValuePair<int, string> num in numberNames)
        {
            if (!data.Contains(num.Value))
                continue;

            int occurs = data.Split(num.Value).Length;

            values.Add(data.IndexOf(num.Value), num.Key);

            if (occurs > 2)
                values.Add(data.LastIndexOf(num.Value), num.Key);
        }
    }
}