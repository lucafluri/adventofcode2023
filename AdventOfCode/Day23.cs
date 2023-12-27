using System.Text.RegularExpressions;


namespace AdventOfCode;

public class Day23 : BaseDay
{
    private readonly Dictionary<(int, int), char> _grid;
    private bool[,] _visited; // assuming width and height are the dimensions of the grid

    private readonly (int, int) _start;
    private readonly (int, int) _end;
    private readonly int width;
    private readonly int height;
    private int _maxSteps = 0;
    
    private List<(int, int)> DefaultDirections = new(){ (0, 1), (-1, 0), (0, -1), (1, 0) };
    private static readonly Dictionary<char, (int, int)> GridDirections = new() {{ '>', (1, 0) }, { '<', (-1, 0) }, { '^', (0, -1) }, { 'v', (0, 1) }};
    
    private List<(int, int)> intersections;
    
    
    public Day23()
    {
        _grid = File.ReadAllLines(InputFilePath).SelectMany((line, y) => line.Select((c, x) => ((x, y), c))).ToDictionary(x => x.Item1, x => x.Item2);
        
        _start = _grid.First(x => x.Key.Item2 == 0 && x.Value == '.').Key;
        _end = _grid.First(x => x.Key.Item2 == _grid.Keys.Max(x => x.Item2) && x.Value == '.').Key;
        
        width = _grid.Keys.Max(x => x.Item1);
        height = _grid.Keys.Max(x => x.Item2);
        
        _visited = new bool[width+1, height+1]; // assuming width and height are the dimensions of the grid
        
        
        Console.WriteLine($"Start: {_start}");
        Console.WriteLine($"End: {_end}");
        

        intersections = _grid.Where(x => x.Value == '.' && 
                                             GridDirections.ContainsKey(_grid.GetValueOrDefault((x.Key.Item1, x.Key.Item2 - 1))) && GridDirections.ContainsKey(_grid.GetValueOrDefault((x.Key.Item1, x.Key.Item2 + 1))) ||
                                              GridDirections.ContainsKey(_grid.GetValueOrDefault((x.Key.Item1 - 1, x.Key.Item2))) && GridDirections.ContainsKey(_grid.GetValueOrDefault((x.Key.Item1 + 1, x.Key.Item2)))).Select(x => x.Key).Distinct().ToList();
        
        intersections.Add(_start);
        intersections.Add(_end);
        
        Console.WriteLine($"Intersections: {intersections.Count}");
        Console.WriteLine($"Intersections: {string.Join(", ", intersections)}");

        
        
    }

    private void DFS((int, int) pos, int steps, (int, int) end, bool part2 = false)
    {
        if (pos == end)
        {
            if(_maxSteps < steps) Console.WriteLine($"New Max steps: {steps}");
            _maxSteps = Math.Max(_maxSteps, steps);
        }
        
        var directions = part2 || !GridDirections.TryGetValue(_grid[pos], out var dir) ? DefaultDirections : new List<(int, int)> { dir };
        
        _visited[pos.Item1, pos.Item2] = true;
        
        foreach (var (dx, dy) in directions)
        { 
            var newPos = (pos.Item1 + dx, pos.Item2 + dy);
            if (_grid.TryGetValue(newPos, out char value) && value != '#' && !_visited[newPos.Item1, newPos.Item2])
            {
                DFS(newPos, steps + 1, end,part2);
            }
        } 
         
        _visited[pos.Item1, pos.Item2] = false;
    } 
    
    
    private List<((int, int), int)> FindAllConnectedIntersections((int, int) start)
    {
        var connectedIntersections = new List<((int, int), int)>();
        var visited = new HashSet<(int, int)>();
        var stack = new Stack<((int, int), int)>();
        stack.Push((start, 0));
        
        while (stack.Count > 0)
        {
            var (pos, steps) = stack.Pop();
            
            if (intersections.Contains(pos) && start != pos)
            {
                connectedIntersections.Add((pos, steps));
                continue;
            }
            
            visited.Add(pos);
            
            var directions = DefaultDirections;
            
            foreach (var (dx, dy) in directions)
            {
                var newPos = (pos.Item1 + dx, pos.Item2 + dy);
                if (_grid.TryGetValue(newPos, out char value) && value != '#' && !visited.Contains(newPos))
                {
                    stack.Push((newPos, steps + 1));
                }
            }
        }
        
        return connectedIntersections;
    }


    private void DFS_adj(Dictionary<(int, int), List<((int, int), int)>> adj, (int, int) start, (int, int) end)
    {
        Stack<((int, int), int, HashSet<(int, int)>)> stack = new Stack<((int, int), int, HashSet<(int, int)>)>();
        stack.Push((start, 0, new HashSet<(int, int)>() { start }));

        while (stack.Count > 0)
        {
            var (pos, steps, visited) = stack.Pop();

            if (pos == end)
            {
                if(_maxSteps < steps) Console.WriteLine($"New Max steps: {steps}");
                _maxSteps = Math.Max(_maxSteps, steps);
                continue;
            }

            foreach (var (newPos, dist) in adj[pos])
            {
                if (!visited.Contains(newPos))
                {
                    var newVisited = new HashSet<(int, int)>(visited) { newPos };
                    stack.Push((newPos, steps + dist, newVisited));
                }
            }
        }
    }
 



    public override ValueTask<string> Solve_1()
    {
        DFS(_start, 0,_end,false);
        Console.WriteLine($"Max steps Part 1: {_maxSteps}");
        return new (_maxSteps.ToString()); //2214
    }

    public override ValueTask<string> Solve_2()
    {  
        // Build direct Adjancency List from Intersections with first connected other intersection with distance between them
        var adjList = new Dictionary<(int, int), List<((int, int), int)>>();

        foreach (var intersection in intersections)
            adjList[intersection] = FindAllConnectedIntersections(intersection);
        
        _maxSteps = 0;     
        
        DFS_adj(adjList, _start, _end);
        Console.WriteLine($"Max steps Part 2: {_maxSteps}");
        return new (_maxSteps.ToString()); // 6594
    } 
}