using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~9ms
// Setup: ~6ms
// Part 1: ~0.25ms
// Part 2: ~2ms
public class Day04 : BaseDay
{
    private readonly string[] _input;
    private int total = 0;
    private int[] counts;
    
    
    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
        counts = new int[_input.Length];
        
        for(var i = 0; i < _input.Length; i++)
        {
            counts[i] = getWonCount(_input[i]);
        }
    }

    private int getWonCount(string inp)
    {
        // string inp = line;
        inp = Regex.Replace(inp, @"\s+", " ");
        Regex rgx = new Regex(".*:(.*)\\|(.*)");
        var match = rgx.Match(inp);
        var l1 = match.Groups[1].ToString().Trim().Split(" ").ToList();
        var l2 = match.Groups[2].ToString().Trim().Split(" ").ToList();
            
        return l1.Intersect(l2).Count();
    }
    
    

    public override ValueTask<string> Solve_1()
    {
        foreach (var c in counts)
        {
            if(c != 0)
                total += (int) Math.Floor(Math.Pow(2, c-1));    
        }
       
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        // SLOW SOLUTION
        // total = 0;
        
        // var idx = 0;
        //var cards = new int[_input.Length];
      
        // var q = new Queue<int>();
        // var m = new Dictionary<int, List<int>>();
        //
        // //Save into map
        // foreach (var line in _input)
        // {
        //     idx++;
        //
        //     var count = getWonCount(line);
        //     var wins = new List<int>();
        //     for(var i = idx+1; i <= idx + count; i++)
        //     {
        //         wins.Add(i);
        //     }
        //     
        //     m.Add(idx, wins);
        //     q.Enqueue(idx);
        // }
        //
        //
        // int current;
        // while (q.Any())
        // {
        //     current = q.Dequeue(); 
        //     cards[current]++;
        //     
        //     //Iterate over all won cards 
        //     foreach(var i in m[current])
        //     {
        //         q.Enqueue(i);
        //     }
        // }
        //
        // total = cards.Sum();
        
        
        // Nice and fast Solution
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
