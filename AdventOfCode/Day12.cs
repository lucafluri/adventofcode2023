using System.Text.RegularExpressions;
using System.Xml.Linq;
using PostSharp.Patterns.Caching;
using PostSharp.Patterns.Caching.Backends;

namespace AdventOfCode;

// Runtime Total: ~3.9s
// Setup: ~4ms
// Part 1: ~97ms
// Part 2: ~8.835s
public class Day12 : BaseDay
{
    private List<string> _inputLines;
    private readonly List<(string, int[])> _in = new();
    private readonly List<(string, int[])> _in2 = new();
    private readonly Dictionary<string, long> _cache = new();
    
    
    public Day12()
    {
        _inputLines = File.ReadAllLines(InputFilePath).ToList();
        foreach (var x in _inputLines)
        {
            var line = x.Split();
            var sizes = line[1].Split(",").Select(int.Parse).ToArray();
            _in.Add((line[0] + '.', sizes));
            
            var newline = line[0] + '?' + line[0] + '?' + line[0] + '?' + line[0] + '?' + line[0] + '.';
            var newSizes = new List<int>();
            for(var i = 0; i<5;i++) newSizes.AddRange(sizes);
            _in2.Add((newline, newSizes.ToArray()));
        }
    } 
    
    private static string HashKey(string s, IEnumerable<int> sizes, int groupProgress)
    {
        return s + " - " + string.Join(",", string.Join(",", sizes) + " - " + groupProgress);
    }

    [Cache]
    private long GetSolutionCount(string s, int[] sizes, int groupProgress)
    {
        if (_cache.TryGetValue(HashKey(s, sizes, groupProgress), out var count))
            return count;
        
        if (s.Length == 0)
            return sizes.Length == 0 && groupProgress == 0 ? 1 : 0;

        
        long sc = 0;
        var next = s[0] == '?' ? new []{'#', '.'} : new[] { s[0] };
        foreach (var c in next)
        {
            if (c == '#')
                sc += GetSolutionCount(s[1..], sizes, groupProgress + 1);
            else
            {
                if (groupProgress > 0)
                {
                    if (sizes.Length > 0 && sizes[0] == groupProgress)
                        sc += GetSolutionCount(s[1..], sizes[1..], 0);
                        
                }
                else
                    sc += GetSolutionCount(s[1..], sizes, 0);
            }
        }
        _cache[HashKey(s, sizes, groupProgress)] = sc;
        return sc;
    }
    
    

    public override ValueTask<string> Solve_1()
    {
        long sum = _in.Sum(l => GetSolutionCount(l.Item1, l.Item2, 0));

        return new (sum.ToString()); // 7694
    } 

    public override ValueTask<string> Solve_2()
    {
        long sum = _in2.Sum(l => GetSolutionCount(l.Item1, l.Item2, 0));

        return new (sum.ToString()); // 5071883216318
    } 
}