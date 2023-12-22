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
        
        Console.WriteLine($"Width: {_width}, Height: {_height}");
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
        foreach (var next in dirs.Select(dir => ((current.Item1 + dir.Item1) % _width, (current.Item2 + dir.Item2) % _height)))
        {
            if (!_grid.TryGetValue(next, out var value) || value != '.') continue;
            var subPaths = FindPaths(next, stepsLeft - 1);
            paths.UnionWith(subPaths);
        }

        _cache[(current, stepsLeft)] = paths;
        return paths;
    }

    private int GetReachableGardens(int steps, int maxRadius = 0)
    {
        var paths = FindPaths(_start, steps);
        var count = 0;
        count = paths.Count;
        if (maxRadius > 0)
        {
            // count = paths.Count(x => Math.Abs(x.Item1) <= maxRadius && Math.Abs(x.Item2) <= maxRadius);
            paths.RemoveWhere(x => Math.Abs(x.Item1-_start.Item1) > maxRadius || Math.Abs(x.Item2-_start.Item2) > maxRadius);
            count = paths.Count;
        }
        return count;
    }

    public override ValueTask<string> Solve_1() 
    {
        return new (GetReachableGardens(64, _width/2).ToString()); // 3699
    }

    public override ValueTask<string> Solve_2()
    {
        // var l = new List<long>();
        // for(var i = 1; i < _width; i++)
        // {
        // Console.WriteLine(GetReachableGardens(133, 65));
        // Console.WriteLine($"{l[^1]}");
        // }
        
        
        var steps = 26501365;
        var radius = _width / 2;
        var n = (steps - radius) / _width;
        
        Console.WriteLine($"Steps: {steps}, Radius: {radius}, N: {n}");
        // var innerArea = GetReachableGardens(_width, radius);
        var innerArea = 11286; 
         
        // for(var i = 0; i < 100; i++)
        // {
        //     Console.WriteLine($"{i}: {GetReachableGardens(_width+i, radius)}");
        // }
        // Console.WriteLine($"{GetReachableGardens(_width, radius)}");
        // Console.WriteLine($"{GetReachableGardens(_width+1, radius)}");
        // Console.WriteLine($"{GetReachableGardens(_width+2, radius)}");
        // 26501365 = 65 + (202300 * 131)
        // STEPS  = InnerRadius-1 + (Diamond Radius * width)
        // Num Paths: 11286 (odd steps - 131) => Calced via GetReachableGardens(131, 65)
        // Num Paths: 11365 (even steps - 132) => Calced via GetReachableGardens(132, 65)
        // Num Paths: 11397 (odd steps - 133) => Calced via GetReachableGardens(133, 65)
        
        // n = 202300 
        // (n+1)^2 + n^2 = Area
        // area - (n+1)/2 = area
        // area + n/2 = area

        // long n = 202300;
        // long n = ()
        
        
        long area = (n+1)*(n+1) + n*n;
        area -= (n+1)*2;
        area += (3*n)/2;
        area *= innerArea;
        Console.WriteLine($"Area: {area}");
        
        
        
        
        
        return new (area.ToString());
        // L-81_850_782_301
        // L-81_851_186_901
        // H-923_772_495_364_686
        // ?-2_782_053_454_950
        // ?-2_801_527_336_125  
    } 
}