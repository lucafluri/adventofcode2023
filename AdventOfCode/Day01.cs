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
    private int _total = 0;
    private readonly Regex _rgx = new(@"[^\d]"); // Matches everything except nums
    private readonly Dictionary<string, string> nums = new Dictionary<string, string>
    {
        {"one", "on1e"},
        {"two", "t2wo"},
        {"three", "t3hree"},
        {"four", "f4our"},
        {"five", "f5ive"},
        {"six", "s6ix"},
        {"seven", "s7even"},
        {"eight", "e8ight"},
        {"nine", "n9ine"}
    };

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
            var l = line;
            foreach(var entry in nums)
                l = l.Replace(entry.Key, entry.Value);
            
            l = _rgx.Replace(l, "");
            _total += int.Parse(l[0] + l[^1].ToString());
        }
        return new(_total.ToString()); // 54518
    }
}
