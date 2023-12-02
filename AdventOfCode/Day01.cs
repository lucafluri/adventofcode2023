using System.Diagnostics;

namespace AdventOfCode;


//SOLVED IN PYTHON

public class Day01 : BaseDay
{
    private readonly string _input;
    private readonly string[] _inputLines;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
        _inputLines = File.ReadAllLines(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        Console.WriteLine("");
        Console.WriteLine(_inputLines.Length);

        foreach (var line in _inputLines)
        {
        }
        
        return new ValueTask<string>("RESULT");
    }
    // public override ValueTask<string> Solve_1() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 1");

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
