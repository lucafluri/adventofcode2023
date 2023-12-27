using System.Text.RegularExpressions;


namespace AdventOfCode;

public class Day23 : BaseDay
{
    private static Random rng = new Random();  
    private readonly Dictionary<(int, int), char> _grid;
    // private Dictionary<(int, int), bool> _visited = new();
    // private bool[,] _visited; // assuming width and height are the dimensions of the grid

    private Dictionary<(int, int), int> cache = new(); 
    private readonly (int, int) _start;
    private readonly (int, int) _end;
    private readonly int width;
    private readonly int height;
    private int _maxSteps = 0;
    
    private List<(int, int)> DefaultDirections = new(){ (0, 1), (-1, 0), (0, -1), (1, 0) };
    private static readonly Dictionary<char, (int, int)> GridDirections = new() {{ '>', (1, 0) }, { '<', (-1, 0) }, { '^', (0, -1) }, { 'v', (0, 1) }};
    
    void PrintGrid()
    {
        for (var y = 0; y <= _grid.Keys.Max(x => x.Item2); y++)
        {
            for (var x = 0; x <= _grid.Keys.Max(x => x.Item1); x++)
            {
                Console.Write(_grid[(x, y)]);
                
            }
            Console.WriteLine();
        }
    }
    
    public Day23()
    {
        _grid = File.ReadAllLines(InputFilePath).SelectMany((line, y) => line.Select((c, x) => ((x, y), c))).ToDictionary(x => x.Item1, x => x.Item2);
        
        _start = _grid.First(x => x.Key.Item2 == 0 && x.Value == '.').Key;
        _end = _grid.First(x => x.Key.Item2 == _grid.Keys.Max(x => x.Item2) && x.Value == '.').Key;
        
        width = _grid.Keys.Max(x => x.Item1);
        height = _grid.Keys.Max(x => x.Item2);
        
        // _visited = new bool[width+1, height+1]; // assuming width and height are the dimensions of the grid
        

        
        Console.WriteLine($"Start: {_start}");
        Console.WriteLine($"End: {_end}");
    }

    // private void DFS((int, int) pos, int steps, bool part2 = false)
    // {
    //     if (pos == _end)
    //     {
    //         if(_maxSteps < steps) Console.WriteLine($"New Max steps: {steps}");
    //         _maxSteps = Math.Max(_maxSteps, steps);
    //     }
    //     
    //     // DefaultDirections = DefaultDirections.OrderBy(x => rng.Next()).ToList();
    //     var directions = part2 || !GridDirections.TryGetValue(_grid[pos], out var dir) ? DefaultDirections : new List<(int, int)> { dir };
    //     
    //     _visited[pos.Item1, pos.Item2] = true;
    //     
    //     foreach (var (dx, dy) in directions)
    //     { 
    //         var newPos = (pos.Item1 + dx, pos.Item2 + dy);
    //         if (_grid.TryGetValue(newPos, out char value) && value != '#' && !_visited[newPos.Item1, newPos.Item2])
    //         {
    //             DFS(newPos, steps + 1, part2);
    //         }
    //     } 
    //      
    //     _visited[pos.Item1, pos.Item2] = false;
    // } 
     
    // private int IterativeDFS((int, int) start, bool part2 = false)
    // {
    //     Stack<((int, int), int)> stack = new Stack<((int, int), int)>();
    //     stack.Push((start, 0));
    //
    //     while (stack.Count > 0)
    //     {
    //         var (pos, steps) = stack.Pop();
    //
    //         if (pos == _end)
    //         {
    //             _maxSteps = Math.Max(_maxSteps, steps);
    //             continue; // Continue to check other paths even after finding the end
    //         }
    //
    //         _visited[pos.Item1, pos.Item2] = true;
    //
    //         var directions = part2 || !GridDirections.TryGetValue(_grid[pos], out var dir) ? DefaultDirections : new List<(int, int)> { dir };
    //
    //         foreach (var (dx, dy) in directions)
    //         {
    //             var newPos = (pos.Item1 + dx, pos.Item2 + dy);
    //             if (_grid.TryGetValue(newPos, out char value) && value != '#' && !_visited[newPos.Item1, newPos.Item2])
    //             {
    //                 stack.Push((newPos, steps + 1));
    //             }
    //         }
    //
    //         _visited[pos.Item1, pos.Item2] = false; // Unmark visited after processing to allow other paths
    //     }
    //
    //     return _maxSteps;
    // }
    //

    private HashSet<(int, int)> _visited = new HashSet<(int, int)>();
    private Dictionary<(int, int), List<(int, int)>> _validMovesCache = new Dictionary<(int, int), List<(int, int)>>();

    private void DFS((int, int) pos, int steps, bool part2 = false)
    {
        if (pos == _end)  
        {
            if (_maxSteps < steps) Console.WriteLine($"New Max steps: {steps}");
            _maxSteps = Math.Max(_maxSteps, steps);
            return;
        }

        _visited.Add(pos);

        List<(int, int)> directions;
        if (!_validMovesCache.TryGetValue(pos, out directions))
        {
            // Console.WriteLine($"Cache miss for {pos}");
            directions = part2 || !GridDirections.TryGetValue(_grid[pos], out var dir) ? DefaultDirections : new List<(int, int)> { dir };
            _validMovesCache[pos] = directions;
        }

        foreach (var (dx, dy) in directions)
        {
            var newPos = (pos.Item1 + dx, pos.Item2 + dy);
            if (_grid.TryGetValue(newPos, out char value) && value != '#' && !_visited.Contains(newPos))
            {
                DFS(newPos, steps + 1, part2);
            }
        }

        _visited.Remove(pos);
    }






    public override ValueTask<string> Solve_1()
    {
        // IterativeDFS(_start);
        DFS(_start, 0,false);
        Console.WriteLine($"Max steps Part 1: {_maxSteps}");
        return new (_maxSteps.ToString()); //2214
    }

    public override ValueTask<string> Solve_2()
    {  
        // _visited = new bool[width+1, height+1]; // assuming width and height are the dimensions of the grid
        _visited = new HashSet<(int, int)>();
        _validMovesCache = new Dictionary<(int, int), List<(int, int)>>();
        cache = new();
        // _memo = new Dictionary<(int, int), int>();
        _maxSteps = 0;     
        DFS(_start, 0, true);
        // IterativeDFS(_start, true);
        return new (_maxSteps.ToString());
        //4646, 6552 LOW
         
    } 
}