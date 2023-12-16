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
    
    struct Beam
    {
        public Complex pos { get; set; }
        public Complex dir { get; set; }
        
        // public override bool Equals(object obj)
        // {
        //     return obj is Beam other && Equals(other);
        // }
    }
    
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

    private int CountTiles(Beam start)
    {
        if(endings.Contains(start.pos)) return 0;
        var queue = new Queue<Beam>();
        queue.Enqueue(start);
        var visited = new HashSet<Beam>();

        while (queue.Count != 0)
        {
            var b = queue.Dequeue();
            // Console.WriteLine($"{b.pos} {b.dir} on {_map[b.pos]}");
            visited.Add(b); 
            var newDirs = GetNewDirs(_map[b.pos], b.dir);
            foreach (var dir in newDirs)
            {
                var newPos = b.pos + dir;
                if (_map.ContainsKey(newPos) && !visited.Contains(new Beam(){pos = newPos, dir = dir}))
                {
                    queue.Enqueue(new Beam(){pos = newPos, dir = dir});
                }
            }
            if(queue.Count == 0) endings.Add(b.pos);
        }
        return visited.Select(x => x.pos).Distinct().Count();
    }

    // Get all start beams around the edge of the map
    private Beam[] GetStarts()
    {
        var beams = new List<Beam>();
        var maxX = (int) _map.Keys.Max(x => x.Real);
        var maxY = (int) _map.Keys.Max(x => x.Imaginary);
        
        for (var x = 0; x <= maxX; x++)
        {
            beams.Add(new Beam(){pos = new Complex(x, 0), dir = DOWN});
            beams.Add(new Beam(){pos = new Complex(x, maxY), dir = UP});
        }
        for (var y = 0; y <= maxY; y++)
        {
            beams.Add(new Beam(){pos = new Complex(0, y), dir = RIGHT});
            beams.Add(new Beam(){pos = new Complex(maxX, y), dir = LEFT});
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
        var start = new Beam(){pos = new Complex(0, 0), dir = RIGHT};
       
        return new (CountTiles(start).ToString()); // 7307
    }
  
    public override ValueTask<string> Solve_2()
    {
        endings.Clear();
        var max = 0;
        foreach (var start in GetStarts())
        {
            var count = CountTiles(start);
            if (count > max) max = count;
        }
        
        return new (max.ToString()); // 7635
    } 
}