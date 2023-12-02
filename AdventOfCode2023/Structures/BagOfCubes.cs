namespace AdventOfCode2023.Structures;

public record BagOfCubes
{
	private readonly int _redCubes;
	private readonly int _greenCubes;
	private readonly int _blueCubes;

	public BagOfCubes(int red, int green, int blue) =>
		(_redCubes, _greenCubes, _blueCubes) = (red, green, blue);

	public bool CheckIfBelowZero() =>
		_redCubes < 0 || _greenCubes < 0 || _blueCubes < 0;

	public int PowerOfCubes() =>
		_redCubes * _greenCubes * _blueCubes;

	public static BagOfCubes MinimumCubeBag(List<BagOfCubes> bags) =>
		new(bags.Max(c => c._redCubes),
			bags.Max(c => c._greenCubes),
			bags.Max(c => c._blueCubes));

	public static BagOfCubes operator -(BagOfCubes cubesA, BagOfCubes cubesB) =>
		new(cubesA._redCubes - cubesB._redCubes,
			cubesA._greenCubes - cubesB._greenCubes,
			cubesA._blueCubes - cubesB._blueCubes);
}