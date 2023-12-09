using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~9ms
// Setup: ~2ms
// Part 1: ~5ms
// Part 2: ~3ms
public class Day09 : BaseDay
{
    private readonly List<List<int>> _nums = new();
    private List<int> _predictions = new();
    
    public Day09()
    {
        foreach (var line in File.ReadAllLines(InputFilePath).ToList())
        {
            _nums.Add(line.Split(" ").Select(int.Parse).ToList());
        }
    }

    private static int GetPrediction(IReadOnlyList<int> nums, bool part2 = false)
    {
        var diffs = new List<int>();
        for (var i = 0; i < nums.Count - 1; i++)
            diffs.Add(nums[i+1] - nums[i]);

        if (diffs.All(x => x == 0)) return !part2 ? diffs.Last() : diffs.First();
        return !part2 ? diffs.Last() + GetPrediction(diffs) : diffs.First() - GetPrediction(diffs, true);
    }

    private void FillPrediction(bool part2 = false)
    {
        _predictions.Clear();
        foreach (var line in _nums)
        {
            _predictions.Add(GetPrediction(line, part2));
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        FillPrediction(false);
        var sum = _predictions.Select((t, i) => t + _nums[i].Last()).Sum();
        return new (sum.ToString()); // 1861775706
    }

    public override ValueTask<string> Solve_2()
    {
        FillPrediction(true);
        var sum = _predictions.Select((t, i) => _nums[i].First() - t).Sum();
        return new (sum.ToString()); // 1082
    }  
}