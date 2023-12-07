namespace AdventOfCode2023.Structures;

public class Cards
{
	private readonly char[] _deck;
	private readonly int _bid;
    private readonly CardStrength _cardStrength;

	public Cards(char[] deck, int bid) =>
		(_deck, _bid, _cardStrength) = (deck, bid, Select(deck));

    public CardStrength GetCardStrength() =>
        _cardStrength;

    public int GetCardsWinning(int rank) =>
        _bid * rank;

    public int DeckCard(int index) =>
        Array.FindIndex(CardTypes(), c => c == _deck[index]);

    private static CardStrength Select(char[] deck) =>
        CalculateCardStrength(CardTypes().Select(t => deck.Count(d => d == t)).ToArray());

	private static char[] CardTypes() =>
		new[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

    private static char[] CardTypesWithJoker() =>
        new[] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

    private static CardStrength CalculateCardStrength(int[] countCards) => countCards switch
	{
        int[] count when count.Any(c => c == 5) => CardStrength.Five_Of_A_Kind,
		int[] count when count.Any(c => c == 4) => CardStrength.Four_Of_A_Kind,
        int[] count when count.Any(c => c == 3) && count.Any(c => c == 2) => CardStrength.Full_House,
        int[] count when count.Any(c => c == 3) => CardStrength.Three_Of_A_Kind,
        int[] count when count.Count(c => c == 2) == 2 => CardStrength.Two_Pair,
        int[] count when count.Count(c => c == 2) == 1 => CardStrength.One_Pair,
        _ => CardStrength.High_Card
    };
}