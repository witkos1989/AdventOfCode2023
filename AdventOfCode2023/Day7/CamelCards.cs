namespace AdventOfCode2023.Day7;

public sealed class CamelCards
{
    private readonly List<Cards> _camelCards;

    public CamelCards()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day7", "CamelCardsInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _camelCards = Parse(rawData).ToList();
    }

    public long[] Results() =>
        new long[] { CalculateHandsRank(_camelCards), 0 };

    private static long CalculateHandsRank(List<Cards> camelCards) =>
        camelCards.OrderBy(c => c.GetCardStrength()).
        ThenBy(c => c.DeckCard(0)).
        ThenBy(c => c.DeckCard(1)).
        ThenBy(c => c.DeckCard(2)).
        ThenBy(c => c.DeckCard(3)).
        ThenBy(c => c.DeckCard(4)).
        Select((c, i) => c.GetCardsWinning(i + 1)).
        Sum();

    private static IEnumerable<Cards> Parse(IEnumerable<string?> data) =>
        data.Select(line => new Cards(line!.Split(' ')[0].ToArray(), int.Parse(line!.Split(' ')[1])));
}