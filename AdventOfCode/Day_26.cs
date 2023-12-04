using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day_26 : BaseDay
{
    private readonly string[] _input;
    
    private int total = 0;
    
    
    public Day_26()
    {
        _input = File.ReadAllLines(InputFilePath);
    }
    
    

    public override ValueTask<string> Solve_1()
    {
       
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        total = 0;
        return new ValueTask<string>(total.ToString());
    } 
}