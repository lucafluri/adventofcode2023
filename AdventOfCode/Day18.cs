using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day18 : BaseDay
{
    private List<string> _input;
    private static readonly Regex _rgxline = new(@"([UDLR])\s(\d+)\s\(#(.{6})\)");
    private Dictionary<(int x, int y), string> _loop = new();
    private List<(int, int)> _sorted = new();
    int minX, minY, maxX, maxY;
    
    public Day18()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        
        var x = 0;
        var y = 0;
        
        (minX, minY, maxX, maxY) = (x, y, x, y);
        foreach (var line in _input)
        {
            var match = _rgxline.Match(line);
            var dir = match.Groups[1].Value;
            var steps = int.Parse(match.Groups[2].Value);
            var color = match.Groups[3].Value;
            
            for (var i = 0; i < steps; i++)
            {
                switch (dir)
                {
                    case "U":
                        y--;
                        break;
                    case "D":
                        y++;
                        break;
                    case "L":
                        x--;
                        break;
                    case "R":
                        x++;
                        break;
                }
                
                if (x < minX)
                    minX = x;
                if (x > maxX)
                    maxX = x;
                if (y < minY)
                    minY = y;
                if (y > maxY)
                    maxY = y;

                if (!_loop.ContainsKey((x, y)))
                    _loop.Add((x, y), color);
            }
        }
         
        // Console.WriteLine(_loop.Count);
        // Console.WriteLine($"{string.Join(", ", _loop.Keys.Select(x => $"({x.x}, {x.y})"))}");
        // Console.WriteLine($"x: {minX} - {maxX}, y: {minY} - {maxY}");
        
        Shift();
        // Console.WriteLine($"{string.Join(", ", _loop.Keys.Select(x => $"({x.x}, {x.y})"))}");
        // Console.WriteLine($"x: {minX} - {maxX}, y: {minY} - {maxY}"); 
        
        _sorted = _loop.Keys.ToList();
        _sorted = _sorted.OrderBy(x => x.Item1).ThenBy(a => a.Item2).ToList();
        // Console.WriteLine($"{string.Join(", ", _sorted.Select(x => $"({x.Item1}, {x.Item2})"))}");
        
    }
     
    private void Shift()
    {
        var newLoop = new Dictionary<(int x, int y), string>();
        foreach (var (key, value) in _loop)
        {
            newLoop.Add((key.x - minX, key.y - minY), value);
        }

        _loop = newLoop;
        (minX, minY, maxX, maxY) = (0, 0, maxX - minX, maxY - minY);
    }
    
    // Calculate inner area of loop defined by 2d points in _sorted
    int CalculateArea()
    {
        var points = _sorted.Select(x => (x.Item1, x.Item2)).ToList();
        // Determine the dimensions of the grid

        bool[,] grid = new bool[maxX + 1, maxY + 1];
        foreach (var (x, y) in points)
        {
            grid[x, y] = true;
        }

        // Find a starting point for the flood fill
        (int, int) start = FindStartPoint(grid, maxX, maxY);
        if (start == (-1, -1))
        {
            return 0; // No starting point found, possibly an invalid loop
        }

        // Perform flood fill
        return FloodFill(grid, start.Item1, start.Item2, maxX, maxY);
    }

    static (int, int) FindStartPoint(bool[,] grid, int maxX, int maxY)
    {
        for (int x = 1; x < maxX; x++)
        {
            for (int y = 1; y < maxY; y++)
            {
                if (!grid[x, y])
                {
                    return (x, y);
                }
            }
        }
        return (-1, -1);
    }

    static int FloodFill(bool[,] grid, int startX, int startY, int maxX, int maxY)
    {
        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));

        int area = 0;

        while (queue.Count > 0)
        {
            (int x, int y) = queue.Dequeue();

            if (x < 0 || x > maxX || y < 0 || y > maxY || grid[x, y])
            {
                continue;
            }

            grid[x, y] = true;
            area++;

            queue.Enqueue((x + 1, y));
            queue.Enqueue((x - 1, y));
            queue.Enqueue((x, y + 1));
            queue.Enqueue((x, y - 1)); 
        } 

        return area;
    }
    
    
    

    public override ValueTask<string> Solve_1()
    {        
        var innerArea = CalculateArea();
        var area = _loop.Count + CalculateArea();
        Console.WriteLine($"{_loop.Count} + {innerArea} = {area}");
        return new (area.ToString());
        // L-23583
    }

    public override ValueTask<string> Solve_2()
    {
        
        return new (0.ToString());
    } 
} 