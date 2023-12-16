using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day16 : BaseDay
{
    private List<string> _input;
    private Dictionary<Complex, char> _map = new();
    private static readonly Complex UP = new(0, -1);
    private static readonly Complex DOWN = new(0, 1);
    private static readonly Complex LEFT = new(-1, 0);
    private static readonly Complex RIGHT = new(1, 0);

    private HashSet<Complex> endings = new();
    
    private void ReadDiagram()
    {
        _map.Clear();
        var y = 0;
        foreach (var line in _input)
        {
            var x = 0;
            foreach (var c in line)
            {
                _map[new Complex(x, y)] = c;
                x++;
            }
            y++;
        }
    }

    private static IEnumerable<Complex> GetNewDirs(char tile, Complex dir)
    {
        var x = (tile, dir);
        if (x == ('/', RIGHT) || x == ('\\', LEFT)) return [UP];
        if (x == ('/', LEFT) || x == ('\\', RIGHT)) return [DOWN];
        if (x == ('/', UP) || x == ('\\', DOWN)) return [RIGHT];
        if (x == ('/', DOWN) || x == ('\\', UP)) return [LEFT];
        if (x == ('-', UP) || x == ('-', DOWN)) return [LEFT, RIGHT];
        if (x == ('|', LEFT) || x == ('|', RIGHT)) return [UP, DOWN];
        return [dir];
    }

    private int CountTiles((Complex, Complex) start)
    {
        if(endings.Contains(start.Item1)) return 0;
        var queue = new Queue<(Complex, Complex)>();    
        queue.Enqueue(start);
        var visited = new HashSet<(Complex, Complex)>();

        while (queue.Count != 0)
        {
            var b = queue.Dequeue();
            visited.Add(b); 
            var newDirs = GetNewDirs(_map[b.Item1], b.Item2);
            foreach (var dir in newDirs)
            {
                var newPos = b.Item1 + dir;
                if (_map.ContainsKey(newPos) && !visited.Contains((newPos, dir)))
                {
                    queue.Enqueue((newPos, dir));
                }
            }
            if(queue.Count == 0) endings.Add(b.Item1);
        }
        return visited.Select(x => x.Item1).Distinct().Count();
    }

    // Get all start beams around the edge of the map
    private (Complex, Complex)[] GetStarts()
    {
        var beams = new List<(Complex, Complex)>();
        var maxX = (int) _map.Keys.Max(x => x.Real);
        var maxY = (int) _map.Keys.Max(x => x.Imaginary);
        
        for (var x = 0; x <= maxX; x++)
        {
            beams.Add((new Complex(x, 0), DOWN));
            beams.Add((new Complex(x, maxY), DOWN));
        }
        for (var y = 0; y <= maxY; y++)
        {
            beams.Add((new Complex(0, y), RIGHT));
            beams.Add((new Complex(maxX, y),LEFT));
        }
        return beams.ToArray();
    }
    
    
    public Day16()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        ReadDiagram();
    } 
    
    

    public override ValueTask<string> Solve_1()
    {
        var start =(new Complex(0, 0), RIGHT); 
        return new (CountTiles(start).ToString()); // 7307
    }
  
    public override ValueTask<string> Solve_2()
    {
        endings.Clear();
        var starts = GetStarts();
        int max = 0;
        object lockObj = new object();

        Parallel.ForEach(starts, start =>
        {
            var count = CountTiles(start);
            lock (lockObj)
            {
                if (count > max) max = count;
            }
        });
        
        return new (max.ToString()); // 7635
    } 
}