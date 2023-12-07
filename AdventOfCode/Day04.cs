using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~8ms
// Setup: ~5ms
// Part 1: ~1ms
// Part 2: ~2ms
public class Day04 : BaseDay
{
    private readonly string[] _input;
    private int total = 0;
    private int[] counts;
    private static Regex _rgxSpace= new(@"\s+");
    private static Regex _rgxLine = new(".*:(.*)\\|(.*)");
    
    
    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
        counts = new int[_input.Length];
        
        for(var i = 0; i < _input.Length; i++)
            counts[i] = GetWonCount(_input[i]);
    }

    private static int GetWonCount(string inp)
    {
        inp = _rgxSpace.Replace(inp,  " ");
        var match = _rgxLine.Match(inp);
        var l1 = match.Groups[1].ToString().Trim().Split(" ").ToList();
        var l2 = match.Groups[2].ToString().Trim().Split(" ").ToList();
            
        return l1.Intersect(l2).Count();
    }
    
    

    public override ValueTask<string> Solve_1()
    {
        foreach (var c in counts.Where(x => x != 0))
                total += (int) Math.Floor(Math.Pow(2, c-1));    
        
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var cards = Enumerable.Repeat<int>(1, _input.Length).ToArray();

        for (int j = 0; j < _input.Length; j++)
        {
            var wonCount = counts[j];
            for (var i = 0; i < wonCount; i++)
            {
                cards[j + i + 1] += cards[j];
            }
        }
        
        return new ValueTask<string>(cards.Sum().ToString());
    } 
}
