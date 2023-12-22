using System.Security.Cryptography.X509Certificates;
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

    public int FindPaths(int step, (int, int) start)
    {
        var (x, y) = start;
        var visited = new Dictionary<(int, int), int> { { (x, y), 0 } };
        var distance = 0;
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));
        var result = 0;

        while (distance < step)
        {
            distance++;
            var nextQueue = new Queue<(int, int)>();
            while (queue.Count > 0)
            {
                (x, y) = queue.Dequeue();
                foreach (var (dx, dy) in dirs)
                {
                    var (nx, ny) = (dx + x, dy + y);
                    var (tx, ty) = ((nx % _width + _width) % _width, (ny % _height + _height) % _height);
                    if (_grid.ContainsKey((tx, ty)) &&
                        (_grid[(tx, ty)] == '#' || visited.ContainsKey((nx, ny)))) continue;
                    visited[(nx, ny)] = distance;
                    nextQueue.Enqueue((nx, ny));
                }
            }
            queue = nextQueue;

            if (distance != step) continue;
            result += visited.Values.Count(v => v % 2 == step % 2);
        }
        return result;
    }

    public override ValueTask<string> Solve_1() 
    {
        return new ( FindPaths(64, _start).ToString()); // 3699
    }

    public override ValueTask<string> Solve_2()
    {
        // Grows quadratically => x = steps
        // y = ax^2 + bx + c
        // y(0) with x = width/2 until edge
        // y(1) with x = width/2 + width
        // y(2) with x = width/2 + 2*width
        
        long y0 = FindPaths(65, _start);
        long y1 = FindPaths(65 + _width, _start);
        long y2 = FindPaths(65 + 2*_width, _start);
        
        Console.WriteLine($"y0: {y0}, y1: {y1}, y2: {y2}");

        Int128 n = 26501365;
        n = (n-_width/2)/_width;
        Console.WriteLine($"N: {n}");
        
        //  (1) y0 = c, (2) y1 = a + b + c, (3) y2 = 4a + b + c
        var c = y0;
        var b = (4 * y1 - y2 - 3 * y0) / 2;
        var a = y1 - y0 - b;
        
        var res = a * n * n + b * n + c;
        
        return new (res.ToString()); // 613391294577878
    } 
}