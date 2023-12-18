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
    private readonly List<(int x, int y)> _loop2 = new();
    private readonly List<(int x, int y)> _loop = new();
    
    public Day18()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();

        var (x, y, x2, y2) = (0, 0, 0, 0);

        foreach (var match in _input.Select(line => _rgxline.Match(line)))
        {
            var dir = match.Groups[1].Value;
            var steps = int.Parse(match.Groups[2].Value);
            var color = match.Groups[3].Value;

            var steps2 = Convert.ToInt64(color.Substring(0, 5), 16);
            var dir2 = color.Substring(5, 1);

            AddSteps(ref x, ref y, dir, steps, _loop);
            AddSteps(ref x2, ref y2, dir2, steps2, _loop2);
        }
    }

    private static void AddSteps(ref int x, ref int y, string dir, long steps, List<(int, int)> loop)
    {
        for (var i = 0; i < steps; i++)
        {
            switch (dir)
            {
                case "U":
                case "3": y--; break;
                case "D":
                case "1": y++; break;
                case "L": 
                case "2": x--; break;
                case "R": 
                case "0": x++; break;
            }
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
        return (long)(Math.Abs(area) / 2.0);
    }

    private static long CalculateInteriorPoints(IReadOnlyList<(int x, int y)> vertices)
    {
        var area = CalculateArea(vertices);
        return (long)(area + 1 + vertices.Count / 2.0);
    }

    public override ValueTask<string> Solve_1()
    {        
        return new (CalculateInteriorPoints(_loop).ToString()); // 46334
    } 

    public override ValueTask<string> Solve_2()
    {
        return new (CalculateInteriorPoints(_loop2).ToString()); // 102000662718092
    } 
} 