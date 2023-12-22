using System.Data;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day22 : BaseDay
{
    private List<Brick> _bricks;

    class Brick
    {
        public List<(int, int, int)> blocks;
        
        public Brick(string line)
        {
            var ends = line.Split("~");
            var start = ends[0].Split(",");
            var end = ends[1].Split(",");
            
            blocks = new List<(int, int, int)>();
            for (int i = int.Parse(start[0]); i <= int.Parse(end[0]); i++)
            {
                for (int j = int.Parse(start[1]); j <= int.Parse(end[1]); j++)
                {
                    for (int k = int.Parse(start[2]); k <= int.Parse(end[2]); k++)
                    {
                        blocks.Add((i, j, k));
                    }
                }
            }
        }
        
        public override string ToString()
        {
            // Print all blocks in format: x,y,z
            return string.Join("", blocks.Select(block => $"{block.Item1},{block.Item2},{block.Item3}\n"));
        }
    }
    
    public Day22()
    {
       _bricks = File.ReadAllLines(InputFilePath).Select(line => new Brick(line)).ToList();

       
       
       // Sort bricks by z lowest value
        _bricks = _bricks.OrderBy(b => b.blocks.Min(block => block.Item3)).ToList();
        
        foreach (var b in _bricks)
        {
           Console.WriteLine(b); 
           // Console.WriteLine();
        }
    }

    public override ValueTask<string> Solve_1()
    {
       
        return new (0.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        
        return new (0.ToString());
    } 
}