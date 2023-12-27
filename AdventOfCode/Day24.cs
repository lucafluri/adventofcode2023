using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day24 : BaseDay
{
    private List<string> _input;
    private List<HS> _hs = new();


    struct HS
    {
        public (int x, int y, int z) pos;
        public (int x, int y, int z) vel;
    }
    
    
    public Day24()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        foreach (var line in _input)
        {
            var parts = line.Replace(" ", "").Split('@');
            var pos = parts[0].Split(",").Select(int.Parse).ToArray();
            var vel = parts[1].Split(",").Select(int.Parse).ToArray();
            
            _hs.Add(new HS()
            {
                pos = (pos[0], pos[1], pos[2]),
                vel = (vel[0], vel[1], vel[2])
            });
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