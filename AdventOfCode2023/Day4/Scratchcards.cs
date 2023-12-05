namespace AdventOfCode2023.Day4;

public sealed class Scratchcards
{
	private readonly List<Scratchcard> _cards;

	public Scratchcards()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day4", "ScratchcardsInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

		_cards = Parse(rawData.ToList()).ToList();
    }

	public int[] Results()
	{
		int[] results = new int[2];

		results[0] = TotalPoints(_cards);

		results[1] = TotalCount(_cards);

		return results;
	}

	private static int TotalCount(List<Scratchcard> cards)
	{
		Dictionary<int, int> cardsCount = cards.
			ToDictionary(key => key.GetNumber(), value => 1);

        for (int num = 0; num < cards.Count; num++)
			for (int i = 0; i < cardsCount[num + 1]; i++)
				for (int count = 1; count <= cards[num].MatchingNumbers(); count++)
					cardsCount[num + 1 + count] += 1;

		return cardsCount.Sum(c => c.Value);
	}

	private static int TotalPoints(List<Scratchcard> cards) =>
		cards.Select(c => c.Points()).Sum();

	private static IEnumerable<Scratchcard> Parse(List<string?> data)
	{
		foreach (string? line in data)
		{
			if (string.IsNullOrEmpty(line))
				continue;

			string[] cardAndNums = line.Split(':');
            string[] allNums = cardAndNums[1].Trim().Split('|');
			string[] winningNums = allNums[0].Trim().Split(' ').
				Where(n => !string.IsNullOrEmpty(n)).ToArray();
			string[] cardNums = allNums[1].Trim().Split(' ').
				Where(n => !string.IsNullOrEmpty(n)).ToArray();

            yield return new Scratchcard(
				int.Parse(cardAndNums[0].Split(' ').Last()),
                winningNums.Select(int.Parse).ToList(),
                cardNums.Select(int.Parse).ToList());
        }
	}
}