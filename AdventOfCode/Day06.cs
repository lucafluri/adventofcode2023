using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string[] _input;
    private readonly List<int> _time;
    private readonly List<int> _record;
    
    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath);
        _time = GetNumbers(_input[0]);
        _record = GetNumbers(_input[1]);
    }
    
    private static List<int> GetNumbers(string line)
    {
        var rgxNonDigit = new Regex("[^0-9]");
        var s = rgxNonDigit.Replace(line, " ");
        s = Regex.Replace(s, @"\s+", " ");
        return (from x in s.Trim().Split(" ") where x != "" select int.Parse(x)).ToList();
    }

    private long solve(long t, long r)
    {
        // (7-x)x = 9
        // (t-x)x = d
        // x^2 - tx + d = 0
        double underroot = (t * t) - (4 * (r+1));
        var max = (long) ((-1)*t + Math.Sqrt(underroot)) / (2);
        var min = (long) ((-1)*t - Math.Sqrt(underroot)) / (2);

        return max - min;
    }

    public override ValueTask<string> Solve_1()
    {
        long prod = 1;
        for(var i = 0; i < _time.Count; i++)
            prod *= solve(_time[i], _record[i]);
       
        return new ValueTask<string>(prod.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var s = _time.Aggregate("", (current, n) => current + n);
        var t = long.Parse(s);
        s = _record.Aggregate("", (current, n) => current + n);
        var r = long.Parse(s);
        
        return new ValueTask<string>(solve(t, r).ToString());
    } 
}