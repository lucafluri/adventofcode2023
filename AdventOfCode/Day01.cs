using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~15ms
// Setup: ~4ms
// Part 1: ~5ms
// Part 2: ~6ms
public partial class Day01 : BaseDay
{
    private List<string> _inputLines;
    private readonly Regex _rgx = new(@"[^\d]"); // Matches everything except nums
    private readonly Regex _rgx1 = new(@"one"); 
    private readonly Regex _rgx2 = new(@"two"); 
    private readonly Regex _rgx3 = new(@"three"); 
    private readonly Regex _rgx4 = new(@"four"); 
    private readonly Regex _rgx5 = new(@"five"); 
    private readonly Regex _rgx6 = new(@"six"); 
    private readonly Regex _rgx7 = new(@"seven"); 
    private readonly Regex _rgx8 = new(@"eight"); 
    private readonly Regex _rgx9 = new(@"nine"); 
        
    private int _total = 0;

    public Day01()
    {
        _inputLines = File.ReadAllLines(InputFilePath).ToList();
    }

    public override ValueTask<string> Solve_1()
    {   
        foreach (var l in _inputLines.Select(line => _rgx.Replace(line, "")))
            _total += int.Parse(l[0] + l[^1].ToString());
        
        return new(_total.ToString()); // 54331
    }

    public override ValueTask<string> Solve_2()
    {
        _total = 0;
        
        foreach (var line in _inputLines)
        {
            var l = _rgx1.Replace(line, "on1e");
            l = _rgx2.Replace(l, "t2wo");
            l = _rgx3.Replace(l,  "t3hree");
            l = _rgx4.Replace(l,  "f4our");
            l = _rgx5.Replace(l,  "f5ive");
            l = _rgx6.Replace(l,  "s6ix");
            l = _rgx7.Replace(l,  "s7even");
            l = _rgx8.Replace(l,  "e8ight");
            l = _rgx9.Replace(l,  "n9ine");
            
            l = _rgx.Replace(l, "");
            _total += int.Parse(l[0] + l[^1].ToString());
        }
        return new(_total.ToString());
    }
}
