using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day24 : BaseDay
{
    private List<string> _input;
    private List<HS> _hs = new();
    

    struct HS
    {
        public (long, long, long) pos;
        public (int, int, int) vel;
    }
    
    public Day24()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        foreach (var line in _input)
        {
            var parts = line.Replace(" ", "").Split('@');
            var pos = parts[0].Split(",").Select(long.Parse).ToArray();
            var vel = parts[1].Split(",").Select(int.Parse).ToArray();
            
            _hs.Add(new HS()
            {
                pos = (pos[0], pos[1], pos[2]),
                vel = (vel[0], vel[1], vel[2])
            });
        }
    }
    
    static (BigInteger x, BigInteger y, BigInteger t, BigInteger t2)? IntersectPointInt(HS a, HS b)
    {
        var p1 = (a.pos.Item1, a.pos.Item2);
        var p2 = (a.pos.Item1 + a.vel.Item1, a.pos.Item2 + a.vel.Item2);
        var p3 = (b.pos.Item1, b.pos.Item2);
        var p4 = (b.pos.Item1 + b.vel.Item1, b.pos.Item2 + b.vel.Item2);
        
        double denominator = (p4.Item2 - p3.Item2) * (p2.Item1 - p1.Item1) - (p4.Item1 - p3.Item1) * (p2.Item2 - p1.Item2);
        double numeratora = (p4.Item1 - p3.Item1) * (p1.Item2 - p3.Item2) - (p4.Item2 - p3.Item2) * (p1.Item1 - p3.Item1);
        double numeratorb = (p2.Item1 - p1.Item1) * (p1.Item2 - p3.Item2) - (p2.Item2 - p1.Item2) * (p1.Item1 - p3.Item1);
        
        if (denominator == 0) // Parallel
            return null;
        
        var ua = numeratora / denominator;
        var ub = numeratorb / denominator;
        
        if (ua < 0 || ub < 0 )
            return null;
        
        var r = (p1.Item1 + ua * (p2.Item1 - p1.Item1), p1.Item2 + ua * (p2.Item2 - p1.Item2));

        return (new BigInteger((long) Math.Round(r.Item1)), new BigInteger((long) Math.Round(r.Item2)), new BigInteger((long) Math.Round(ua)), new BigInteger((long) Math.Round(ub)));
    }

    public override ValueTask<string> Solve_1()
    {
        var count = 0; 
     
        for(var i = 0; i < _hs.Count; i++)
        for(var j = i + 1; j < _hs.Count; j++)
        {
            var p = IntersectPointInt(_hs[i], _hs[j]);
            if (p == null || p.Value.t < 0) continue; 
            if (p.Value.Item1 >= 200000000000000 && p.Value.Item1 <= 400000000000000 && p.Value.Item2 >= 200000000000000 && p.Value.Item2 <= 400000000000000)
            // if (p.Value.Item1 >= 7 && p.Value.Item1 <= 27 && p.Value.Item2 >= 7 && p.Value.Item2 <= 27 && p.Value.t > 0)
                count++;  
        }
       
        return new (count.ToString()); // 15889
    }

    public override ValueTask<string> Solve_2()
    {
        var range = 250;
        
        var (h0, h1, h2) = (_hs[0] , _hs[1], _hs[2]);
        
        // Find plane where all 3 hails intersect
        for(var y = -range; y < range; y++)
        for(var x = -range; x < range; x++)
        {
            var h0t = h0 with { vel = (h0.vel.Item1 + x, h0.vel.Item2 + y, h0.vel.Item3) };
            var h1t = h1 with { vel = (h1.vel.Item1 + x, h1.vel.Item2 + y, h1.vel.Item3) };
            var h2t = h2 with { vel = (h2.vel.Item1 + x, h2.vel.Item2 + y, h2.vel.Item3) };
            
            var i1 = IntersectPointInt(h0t, h1t);
            var i2 = IntersectPointInt(h0t, h2t);
            
            if (i1 == null || i2 == null ) continue;
            
            if (i1.Value.Item1 != i2.Value.Item1) continue;
            if (i1.Value.Item2 != i2.Value.Item2) continue;
        
            for (var z = -range; z < range; z++)
            {
                // Interpolate z
                var z1 = (h1.pos.Item3 + i1.Value.t2 * (h1t.vel.Item3+z));
                var z2 = (h2.pos.Item3 + i2.Value.t2 * (h2t.vel.Item3+z));
                
                if (z1 != z2) continue;
                return new ((i1.Value.Item1 + i1.Value.Item2 + z1).ToString());
            }
        }
        return new ("NA".ToString()); // 801386475216902
    } 
}