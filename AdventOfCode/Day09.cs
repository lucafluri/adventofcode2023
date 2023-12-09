using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day09 : BaseDay
{
    private List<string> _input;
    private List<int> _predictions = new();
    
    
    public Day09()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
    }

    private int GetPrediction(List<int> nums)
    {
        var diffs = new List<int>();
        for (var i = 0; i < nums.Count - 1; i++)
        {
            diffs.Add(nums[i+1] - nums[i]);
        }

        if (diffs.All(x => x == 0)) return diffs.Last();
        return  diffs.Last() + GetPrediction(diffs);
    }
    
    private int GetPredictionBefore(List<int> nums)
    {
        var diffs = new List<int>();
        for (var i = 0; i < nums.Count - 1; i++)
        {
            diffs.Add(nums[i+1] - nums[i]);
        }

        if (diffs.All(x => x == 0)) return diffs.First();
        return  diffs.First() - GetPredictionBefore(diffs);
    }
    
    

    public override ValueTask<string> Solve_1()
    {
        foreach (var line in _input)
        {
            // Console.WriteLine(GetPrediction(line.Split(" ").Select(int.Parse).ToList()));
            _predictions.Add(GetPrediction(line.Split(" ").Select(int.Parse).ToList()));
        }

        var sum = 0;
        for(var i = 0; i < _predictions.Count; i++)
        {
            sum += _predictions[i]+_input[i].Split(" ").Select(int.Parse).ToList().Last();
        }
        
        return new (sum.ToString()); // 1861775706
    }

    public override ValueTask<string> Solve_2()
    {
        _predictions.Clear();
        foreach (var line in _input)
        {
            // Console.WriteLine(GetPrediction(line.Split(" ").Select(int.Parse).ToList()));
            _predictions.Add(GetPredictionBefore(line.Split(" ").Select(int.Parse).ToList()));
        }


        var sum = 0;
        for(var i = 0; i < _predictions.Count; i++)
        {
            sum += _input[i].Split(" ").Select(int.Parse).ToList().First()-_predictions[i];
        } 
        
        return new (sum.ToString()); // 1082
    }  
}