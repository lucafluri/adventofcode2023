using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day17 : BaseDay
{
    private List<string> _input;
    private readonly int[,] _grid;

    private int FindPath(int min, int max)
    {
        var queue = new PriorityQueue<(int x, int y, int dx, int dy, int heatloss), int>(); // Sort by heatloss
        var visited = new HashSet<(int x, int y, int dx, int dy)>();
        queue.Enqueue((0, 0, 0, 0, 0), 0);

        while (queue.Count > 0)
        {
            var (x, y, dx, dy, heatloss) = queue.Dequeue();
            if (x == _grid.GetLength(0) - 1 && y == _grid.GetLength(1) - 1)
                return heatloss;
    
            if (!visited.Add((x, y, dx, dy))) continue;
            
            var newDirs = new List<(int dx, int dy)>();
            if (dx == 0 && dy != 0) // Moving vertically, so add horizontal directions
                newDirs.AddRange(new[] { (1, 0), (-1, 0) });
            if (dy == 0 && dx != 0) // Moving horizontally, so add vertical directions
                newDirs.AddRange(new[] { (0, 1), (0, -1) }); 
            if (dx == 0 && dy == 0) 
                newDirs.AddRange(new[] { (1, 0), (0, 1) }); // Start, so add both directions

            foreach (var (nx, ny) in newDirs)  
            {
                var (tx, ty, nh) = (x, y, heatloss);
                for (var i = 1; i <= max; i++)
                {
                    tx += nx;
                    ty += ny;  
                    if (tx < 0 || tx >= _grid.GetLength(0) || ty < 0 || ty >= _grid.GetLength(1)) break;
                    nh += _grid[tx, ty];
                    if (i >= min) queue.Enqueue((tx, ty, nx, ny, nh), nh);
                }
            }    
        }
        return -1;
    }

    public Day17()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        _grid = new int[_input[0].Length, _input.Count];
        
        for (int y = 0; y < _input.Count; y++)
        {
            var nums = _input[y].ToCharArray();
            for (int x = 0; x < nums.Length; x++)
            {
                _grid[x, y] = int.Parse(nums[x].ToString());
            }
        }
    }
    
    

    public override ValueTask<string> Solve_1()
    {
        return new (FindPath(1, 3).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        
        return new (FindPath(4, 10).ToString());
    } 
}