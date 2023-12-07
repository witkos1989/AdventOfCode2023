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
        new long[] { CalculateHandsRank(_camelCards, false),
                     CalculateHandsRank(_camelCards, true) };

    private static long CalculateHandsRank(List<Cards> camelCards, bool joker) =>
        camelCards.OrderBy(c => c.GetCardStrength(joker)).
        ThenBy(c => c.DeckCard(0, joker)).
        ThenBy(c => c.DeckCard(1, joker)).
        ThenBy(c => c.DeckCard(2, joker)).
        ThenBy(c => c.DeckCard(3, joker)).
        ThenBy(c => c.DeckCard(4, joker)).
        Select((c, i) => c.GetCardsWinning(i + 1)).
        Sum();

    private static IEnumerable<Cards> Parse(IEnumerable<string?> data) =>
        data.Select(line => new Cards(line!.Split(' ')[0].ToArray(), int.Parse(line!.Split(' ')[1])));
}