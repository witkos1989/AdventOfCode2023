namespace AdventOfCode2023.Structures;

public class Scratchcard
{
	private readonly int _number;
	private readonly List<int> _winningNumbers;
	private readonly List<int> _cardNumbers;

	public Scratchcard(int number, List<int> winningNumbers, List<int> cardNumbers) =>
		(_number, _winningNumbers, _cardNumbers) = (number, winningNumbers, cardNumbers);

	public int MatchingNumbers() =>
		_winningNumbers.Intersect(_cardNumbers).Count();

    public int Points() =>
        (int)Math.Pow(2, MatchingNumbers() - 1);

	public int GetNumber() => _number;
}