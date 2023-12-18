using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~2.5s
// Setup: ~2s 
// Part 1: ~9ms
// Part 2: ~450ms
public class Day18 : BaseDay
{
    private List<string> _input;
    private List<(int, int)> _loop = [];
    private List<(int, int)> _loop2 = [];
    
    private static readonly Dictionary<string, (int dx, int dy)> DirectionMap = new()
    {
        {"U", (0, -1)}, {"D", (0, 1)}, {"L", (-1, 0)}, {"R", (1, 0)},
        {"0", (1, 0)}, {"1", (0, 1)}, {"2", (-1, 0)}, {"3", (0, -1)}
    };
    
    public Day18()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();

        var (x, y, x2, y2) = (0, 0, 0, 0);

        _loop.Capacity = _input.Sum(line => int.Parse(line.Split(' ')[1]));
        _loop2.Capacity = _input.Sum(line => (int)Convert.ToInt64(line.Split(' ')[2][2..7], 16));
        
        foreach (var line in _input)
        {
            var parts = line.Split(' ');
            var dir = parts[0];
            var steps = int.Parse(parts[1]);
            var color = parts[2][2..8]; 

            var steps2 = Convert.ToInt64(color[..5], 16);
            var dir2 = color[5].ToString();

            AddSteps(dir, steps, _loop);
            AddSteps(dir2, steps2, _loop2);
        }
    }

    private static void AddSteps(string dir, long steps, List<(int, int)> loop)
    {
        DirectionMap.TryGetValue(dir, out var direction);
        var (dx, dy) = direction;
        var (x, y) = loop.LastOrDefault();

        for (var i = 0; i < steps; i++)
        {
            x += dx;
            y += dy;
            loop.Add((x, y));
        }
    }
     
    private static long CalculateArea(IReadOnlyList<(int x, int y)> vertices)
    {
        double area = 0;
        var n = vertices.Count;
        for (var i = 0; i < n; i++)
        {
            var j = (i + 1) % n;
            area += (vertices[i].x * vertices[j].y) - (vertices[j].x * vertices[i].y);
        }
        return (long)(Math.Abs(area) / 2.0) + 1 + vertices.Count / 2;
    }
    
    private static long CalculateAreaParallel(IReadOnlyList<(int x, int y)> vertices)
    {
        double area = 0;
        var n = vertices.Count;

        Parallel.For(0, n, 
            () => 0.0, 
            (i, state, localArea) =>
            {
                var j = (i + 1) % n;
                localArea += (vertices[i].x * vertices[j].y) - (vertices[j].x * vertices[i].y);
                return localArea; 
            },
            localArea => 
            {
                lock (vertices) 
                    area += localArea;
            }
        );

        return (long)(Math.Abs(area) / 2.0) + 1 + vertices.Count / 2;
    }


    public override ValueTask<string> Solve_1()
    {        
        return new (CalculateAreaParallel(_loop).ToString()); // 46334
    } 

    public override ValueTask<string> Solve_2()
    {
        return new (CalculateAreaParallel(_loop2).ToString()); // 102000662718092
    } 
} 