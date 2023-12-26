using System.Data;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day22 : BaseDay
{
    private List<Brick> _bricks;

    class Brick
    {
        public char id;
        public List<(int, int, int)> blocks = new();
        public List<Brick> supporting = new();
        public List<Brick> supportedBy = new();
        public List<Brick> wouldFall = new();
        public bool removable = false;
        
        public Brick(string line, char id)
        {
            this.id = id;
            var ends = line.Split("~");
            var start = ends[0].Split(","); 
            var end = ends[1].Split(",");
            
            // Switch start end so start is always lowest
            if (int.Parse(start[0]) > int.Parse(end[0]))
                (start, end) = (end, start);
            else if (int.Parse(start[0]) == int.Parse(end[0]))
                if (int.Parse(start[1]) > int.Parse(end[1]))
                    (start, end) = (end, start);
                else if (int.Parse(start[1]) == int.Parse(end[1]))
                    if (int.Parse(start[2]) > int.Parse(end[2]))
                        (start, end) = (end, start);
            
            for (var i = int.Parse(start[0]); i <= int.Parse(end[0]); i++)
                for (var j = int.Parse(start[1]); j <= int.Parse(end[1]); j++)
                    for (var k = int.Parse(start[2]); k <= int.Parse(end[2]); k++)
                        blocks.Add((i, j, k));
        }
        
        public override string ToString()
        {
            var own = string.Join("", blocks.Select(block => $"{block.Item1},{block.Item2},{block.Item3}\n"));
            return own + "Supporting:\n" + supporting.Count + "\nSupported by:\n" + supportedBy.Count + "\nRemovable:" + removable.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Brick brick &&
                   EqualityComparer<List<(int, int, int)>>.Default.Equals(blocks, brick.blocks);
        }
    }

    bool IsDirectlyBelow(Brick above, Brick below)
    {
        foreach (var bla in above.blocks)
            foreach (var blb in below.blocks)
                if (blb.Item1 == bla.Item1 && blb.Item2 == bla.Item2)
                    return blb.Item3 < bla.Item3;
        return false;
    }

    int DistanceTo(Brick brick, Brick lower)
    {
        var lowerMax = lower.blocks.Max(x => x.Item3);
        var aboveMin = brick.blocks.Min(x => x.Item3);
        return aboveMin - lowerMax;
    }
    
    bool IsSupporting(Brick below, Brick above)
    {
        // Check if brick is supporting supporting
        return below.blocks.Any(block => above.blocks.Contains((block.Item1, block.Item2, block.Item3 + 1)));
    }

    void MoveBrick(Brick b, int dist)
    {
        // foreach (var bl in b.blocks)
        for(var i = 0; i < b.blocks.Count; i++)
        {
            b.blocks[i] = (b.blocks[i].Item1, b.blocks[i].Item2, b.blocks[i].Item3 - dist);
        }
    }
    
    public Day22()
    {
        _bricks = File.ReadAllLines(InputFilePath).Select((line, idx) => new Brick(line, (char) Convert.ToChar(idx+65))).ToList();
        _bricks = _bricks.OrderBy(b => b.blocks.Min(block => block.Item3)).ToList();
        foreach (var b in _bricks)  
        {
            //move bricks as for down as possible
            // Get all blocks below
            var bricksBelow = _bricks.Where(bx => bx.blocks.Max(x => x.Item3) < b.blocks.Min(x => x.Item3)).ToList();
            bricksBelow = bricksBelow.Where(bx => IsDirectlyBelow(b, bx)).ToList();
            // Highest brick below
            if (bricksBelow.Count > 0)
            {
                var highestBelow = bricksBelow.OrderByDescending(bx => bx.blocks.Max(x => x.Item3)).First();
                // Distance to highest below
                var dist = DistanceTo(b, highestBelow);
                if (dist >= 1) MoveBrick(b, dist - 1);
                continue;
            }
            var toMove = b.blocks.Min(x => x.Item3)-1;
            MoveBrick(b, dist: toMove);
            
        }
        
        foreach (var b in _bricks){ 

            var bricksBelow = _bricks.Where(bx => bx.blocks.Max(x => x.Item3) < b.blocks.Min(x => x.Item3)).ToList();
            bricksBelow = bricksBelow.Where(bx => IsDirectlyBelow(b, bx)).ToList();
            foreach (var bb in bricksBelow.Where(bb => IsSupporting(bb, b)))
            {
                bb.supporting.Add(b);
                b.supportedBy.Add(bb);
            }
        } 
    }
    
    private HashSet<Brick> FindFallingBricks(Brick brick)
    {
        HashSet<Brick> fallingBricks = [];
        var directlySupported = brick.supporting.Where(supportedBrick => supportedBrick.supportedBy.Count == 1).ToList();
        
        foreach (var supportedBrick in directlySupported)
            DFS(supportedBrick);  

        return fallingBricks;

        void DFS(Brick current)
        {
            if (!fallingBricks.Add(current)) 
                return; 

            foreach (var supportedBrick in current.supporting)
                if (supportedBrick.supportedBy.All(b => fallingBricks.Contains(b)))
                    DFS(supportedBrick);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        List<Brick> removable = new();
        foreach (var b in _bricks)
        {
            if (b.supporting.Count == 0) 
            {
                b.removable = true;
                removable.Add(b);
                continue;
            }

            if(b.supporting.Any(ba => ba.supportedBy.Count <= 1))
                continue;
            
            b.removable = true;
            removable.Add(b);
        }
       
        return new (_bricks.Where(x=>x.removable).ToList().Count.ToString()); // 401
  
    }

    public override ValueTask<string> Solve_2()
    {
        var dict = new Dictionary<Brick, List<Brick>>();
        foreach(var b in _bricks.Where(x=>!x.removable).ToList())
        {
            var bricksThatFall = FindFallingBricks(b);
             dict.Add(b, bricksThatFall.ToList());
        }
        var sum = dict.Sum(x => x.Value.Count);
        
        return new(sum.ToString()); //63491
    } 
} 