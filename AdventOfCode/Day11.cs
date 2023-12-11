using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~11ms
// Setup: ~4ms
// Part 1: ~4ms
// Part 2: ~3ms
public class Day11 : BaseDay
{
    private List<string> _input;
    private List<int> _indexX = new();
    private readonly List<int> _indexX2 = new();
    private List<int> _indexY = new();
    private readonly List<int> _indexY2 = new();
    private readonly List<(int, int)> _galaxies = new();
    private readonly List<int> _indexXEmpty = new();
    private readonly List<int> _indexYEmpty = new();
    
    public Day11()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        
        //Save Galaxies
        for(var i = 0; i<_input.Count; i++)
        {
            _indexY.Add(i);
            for(var j = 0; j<_input[i].Length; j++)
            {
                if(i==0) _indexX.Add(j);
                if(_input[i][j] == '#')
                    _galaxies.Add((j, i));
            }
        }
        
        // Save Empty rows and columns indices
        var xCopy = new List<int>(_indexX);
        var yCopy = new List<int>(_indexY);
        var galaxiesX = _galaxies.Select(x => x.Item1).ToList();
        var galaxiesY = _galaxies.Select(x => x.Item2).ToList();
        xCopy.RemoveAll(x => galaxiesX.Contains(x));
        yCopy.RemoveAll(x => galaxiesY.Contains(x));
        _indexXEmpty = xCopy.Distinct().ToList();
        _indexYEmpty = yCopy.Distinct().ToList();
        
        _indexX2 = new List<int>(_indexX);
        _indexY2 = new List<int>(_indexY);
    }
    
    private void ExpandEmptyLines(int add=1)
    {
        var shift = 0;
        for(var i = 0; i < _indexX.Count; i++)
        {
            if (_indexXEmpty.Contains(_indexX[i]))
                shift += add;
            
            _indexX[i] += shift;
        }
        
        shift = 0;
        for(var i = 0; i < _indexY.Count; i++)
        {
            if (_indexYEmpty.Contains(_indexY[i]))
                shift += add;
            
            _indexY[i] += shift;
        }
    }
    
    //Manhattan Distance
    private long CalcDistance((int, int) a, (int, int) b)
    {
        return Math.Abs(_indexX[a.Item1] - _indexX[b.Item1]) + Math.Abs(_indexY[a.Item2] - _indexY[b.Item2]);
    }
    
    private long GetSumOfDistances()
    {
        long sum = 0;
        for(var i = 0; i < _galaxies.Count; i++)
            for(var j = i+1; j < _galaxies.Count; j++)
                sum += CalcDistance(_galaxies[i], _galaxies[j]);
        
        return sum;
    }
    
    public override ValueTask<string> Solve_1()
    {
        ExpandEmptyLines();
        
        return new (GetSumOfDistances().ToString()); // 9445168
    }

    public override ValueTask<string> Solve_2()
    {
        _indexX = new List<int>(_indexX2);
        _indexY = new List<int>(_indexY2);
        ExpandEmptyLines(1000000-1);
        
        return new (GetSumOfDistances().ToString()); // 742305960572
    } 
}