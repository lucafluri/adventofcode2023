using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day21 : BaseDay
{
    Dictionary<(int, int), char> _grid = new();
    private readonly (int, int) _start = (0, 0);
    private List<(int, int)> dirs = new() { (0, -1), (1, 0), (0, 1), (-1, 0) };
    private Dictionary<((int, int), int), HashSet<(int, int)>> _cache = new();
    private int _width = 0;
    private int _height = 0;
    
    public Day21()
    {
        var input = File.ReadAllLines(InputFilePath).ToList();

        var y = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var c in line.ToCharArray())
            {
                _grid[(x, y)] = c;
                if (c == 'S')
                {
                    _start = (x, y);
                    _grid[(x, y)] = '.';
                }
                x++;
            }
            y++;
        }
        _width = input[0].Length;
        _height = input.Count;
    }
    
    private HashSet<(int, int)> FindPaths((int, int) current, int stepsLeft)
    {
        if (stepsLeft == 0)
        {
            return new HashSet<(int, int)> { current }; 
        }

        if (_cache.TryGetValue((current, stepsLeft), out var memoizedResult))
        {
            return memoizedResult;
        }
 
        var paths = new HashSet<(int, int)>();
        foreach (var next in dirs.Select(dir => (current.Item1 + dir.Item1, current.Item2 + dir.Item2)))
        {
            if (!_grid.TryGetValue(next, out var value) || value != '.') continue;
            var subPaths = FindPaths(next, stepsLeft - 1);
            paths.UnionWith(subPaths);
        }

        _cache[(current, stepsLeft)] = paths;
        return paths;
    }

    private int GetReachableGardens(int steps)
    {
        var paths = FindPaths(_start, steps);
        return paths.Count;
    }

    public override ValueTask<string> Solve_1() 
    {
       
        return new (GetReachableGardens(64).ToString()); // 3699
    }

    public override ValueTask<string> Solve_2()
    {
        
        return new (0.ToString());
    } 
}