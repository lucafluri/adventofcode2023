using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day00 : BaseDay
{
    private List<string> _input;
    
    
    public Day00()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
       
        return new (0.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        
        return new (0.ToString());
    } 
}