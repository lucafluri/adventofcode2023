using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~39ms
// Setup: ~15ms
// Part 1: ~22ms
// Part 2: ~2ms
public class Day10 : BaseDay
{
    private List<string> _input;
    private List<List<Node>> _grid;
    private Node _start;

    private class Node(char type, int x, int y)
    {
        public char type { get; set; } = type;
        public int x { get; set; } = x;
        public int y { get; set; } = y;

        public Node[] neighbors = new Node[2]; // 0 = up, 1 = right, 2 = down, 3 = left
        public bool visited { get; set; } = false;
        public bool isLoop { get; set; } = false;
        public bool inLoop { get; set; } = false;

        public override bool Equals(object obj)
        {
            return obj is Node node &&
                   type == node.type &&
                   x == node.x &&
                   y == node.y;
        }
    }

    private void AddNeighbors(Node n)
    {
        switch (n.type)
        {
            case '|':
                if(n.y-1 >= 0) n.neighbors[0] = _grid[n.y - 1][n.x];
                if(n.y+1 < _grid.Count) n.neighbors[1] = _grid[n.y + 1][n.x];
                break;
            case '-':
                if(n.x+1 < _grid[0].Count) n.neighbors[0] = _grid[n.y][n.x + 1];
                if(n.x-1 >= 0) n.neighbors[1] = _grid[n.y][n.x - 1];
                break;
            case 'L':
                if(n.y-1 >= 0) n.neighbors[0] = _grid[n.y - 1][n.x];
                if(n.x+1 < _grid[0].Count) n.neighbors[1] = _grid[n.y][n.x + 1];
                break;
            case 'J':
                if(n.y-1 >= 0) n.neighbors[0] = _grid[n.y - 1][n.x];
                if(n.x-1 >= 0) n.neighbors[1] = _grid[n.y][n.x - 1];
                break;
            case '7':
                if(n.y+1 < _grid.Count) n.neighbors[0] = _grid[n.y + 1][n.x];
                if(n.x-1 >= 0) n.neighbors[1] = _grid[n.y][n.x - 1];
                break;
            case 'F':
                if(n.y+1 < _grid.Count) n.neighbors[0] = _grid[n.y + 1][n.x];
                if(n.x+1 < _grid[0].Count) n.neighbors[1] = _grid[n.y][n.x + 1];
                break;
        }
    }

    private void SetStartNeighbors(Node S)
    {
        //Check all Neighbors and add neighbors manually to S that have S as a neighbor
        var x = S.x;
        var y = S.y;
        
        var neighbors = new List<Node>();
        
        if(y-1 >= 0 && _grid[y-1][x].neighbors.Contains(S)) neighbors.Add(_grid[y-1][x]);
        if(y+1 < _grid.Count && _grid[y+1][x].neighbors.Contains(S)) neighbors.Add(_grid[y+1][x]);
        if(x-1 >= 0 && _grid[y][x-1].neighbors.Contains(S)) neighbors.Add(_grid[y][x-1]);
        if(x+1 < _grid[0].Count && _grid[y][x+1].neighbors.Contains(S)) neighbors.Add(_grid[y][x+1]);
        
        S.neighbors = neighbors.ToArray();
    }

    private void ReadInput()
    {
        var y = 0;
        foreach(var line in _input)
        {
            var chars = line.ToCharArray();
            var nodes = chars.Select((c, x) => new Node(c, x, y)).ToList();
            _grid ??= new List<List<Node>>();
            _grid.Add(nodes);
            
            if(nodes.Any(n => n.type == 'S'))
                _start = nodes.First(n => n.type == 'S');
            y++;
        }

        foreach (var node in _grid.SelectMany(line => line))
            AddNeighbors(node);
        
        SetStartNeighbors(_start);
    }
    
    private static int GetLoopLength(Node n, Node before, int distance)
    {
        var nn = n.neighbors.First(x => x != null && !x.Equals(before));
        distance++;
        
        n.visited = true;
        n.isLoop = true;

        if (nn.type != 'S') return GetLoopLength(nn, n, distance);
        nn.isLoop = true;
        return distance;
    }

    private int GetInnerArea()
    {
        var count = 0;
        foreach (var line in _grid)
        {
            var inside = false;
            var lastEdge = 'L';
            foreach (var t in line)
            {
                if (t.type == 'S') t.type = '|';
                if (t.isLoop && t.type == '|') inside = !inside;
                if (t.isLoop && (t.type == 'F' || t.type == 'L')) lastEdge = t.type;
                if (t.isLoop && (t.type == 'J' || t.type == '7'))
                {
                    var edge = t.type;
                    if(lastEdge == 'F' && edge =='J') inside = !inside;
                    if(lastEdge == 'L' && edge =='7') inside = !inside;
                    
                }

                if (t.isLoop) continue;
                t.inLoop = inside;
                if (inside)
                    count++;
            }
        }
        return count;
    }
    
    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        ReadInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var dist = GetLoopLength(_start.neighbors[0], _start, 0);
        dist = (int) Math.Ceiling(dist/2f);
       
        return new (dist.ToString()); // 6951
    }

    public override ValueTask<string> Solve_2()
    {
        return new (GetInnerArea().ToString()); //563
    } 
}