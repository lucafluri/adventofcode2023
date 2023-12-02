namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new(_input.Length.ToString());

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();
}
