using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string[] _input;
    
    private int total = 0;
    private int total2 = 0;
    
    
    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }
    
    

    public override ValueTask<string> Solve_1()
    {
       
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        
        return new ValueTask<string>(total2.ToString());
    } 
}
