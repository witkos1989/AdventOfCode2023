using System.Collections.Generic;

namespace AdventOfCode2023.Structures;

public class Cards
{
	private readonly char[] _deck;
	private readonly int _bid;
    private readonly CardStrength _cardStrength;
    private readonly CardStrength _cardStrengthWithJoker;

	public Cards(char[] deck, int bid) =>
		(_deck, _bid, _cardStrength, _cardStrengthWithJoker) =
        (deck, bid, Select(deck), Select(JokerBehavior(deck)));

    public CardStrength GetCardStrength(bool joker) =>
        !joker ? _cardStrength : _cardStrengthWithJoker;

    public int GetCardsWinning(int rank) =>
        _bid * rank;

    public int DeckCard(int index, bool joker) =>
        Array.FindIndex(!joker ? CardTypes() : JokerCardTypes(), c => c == _deck[index]);

    private static CardStrength Select(char[] deck) =>
        CalculateCardStrength(CardTypes().Select(t => deck.Count(d => d == t)).ToArray());


    private static char[] CardTypes() =>
		new[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

    private static char[] JokerCardTypes() =>
        new[] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

    private static char[] JokerBehavior(char[] deck) =>
        !deck.Any(c => c == 'J') ?
        deck :
        ChangeCards(deck, ActLike(deck.Where(c => c != 'J').ToArray(), CardTypes()));

    private static char[] ChangeCards(char[] deck, char changeTo) =>
        Enumerable.Range(0, deck.Length).
        Select((c, i) => deck[i] != 'J' ? deck[i] : changeTo).
        ToArray();

    private static char ActLike(char[] cards, char[] cardTypes)
    {
        int count = 0;
        char card = 'J';

        for (int i = 0; i < cardTypes.Length; i++)
        {
            if (cards.Count(c => c == cardTypes[i]) > count)
            {
                card = cardTypes[i];
                count = cards.Count(c => c == cardTypes[i]);
            }
            else if (cards.Count(c => c == cardTypes[i]) == count)
                card = cardTypes[i];
        }

        return card;
    }

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