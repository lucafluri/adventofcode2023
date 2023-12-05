using System.Text.RegularExpressions;
using Spectre.Console.Rendering;

namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _input;
    
    private int total = 0;
    List<Map> maps = new List<Map>();
    List<Int128> seeds = new List<Int128>();
    Queue<SeedRange> ranges = new Queue<SeedRange>();

    public class Mapping
    {
        public Int128 source { get; set; }
        public Int128 dest { get; set; }
        public Int128 range { get; set; }
    }

    public class Map
    {
        public List<Mapping> mappings = new List<Mapping>();
    }
    
    private List<Int128> GetNumbers(string line)
    {
        Regex rgxNonDigit = new Regex("[^0-9]");
        var numbers = new List<Int128>();
        var s = rgxNonDigit.Replace(line, " ");
        s = Regex.Replace(s, @"\s+", " ");
        foreach (var seed in s.Trim().Split(" "))
        {
            if(seed != "")
                numbers.Add(Int128.Parse(seed));
        }
        return numbers;
    }

    bool isLetter(string str) => !String.IsNullOrEmpty(str) && Char.IsLetter(str[0]);
    
    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);

        for (var i = 0; i < _input.Length; i++)
        {
            if(_input[i].Length == 0)
                continue;
            if (i == 0)
                seeds = GetNumbers(_input[i]);

            if (isLetter(_input[i]))
                maps.Add(new Map());

            if (_input[i] == "" || isLetter(_input[i])) continue;
            var nums = GetNumbers(_input[i]);
            Mapping m = new Mapping();
            m.dest = nums[0];
            m.source = nums[1];
            m.range = nums[2];
            maps[^1].mappings.Add(m);
        }
    }

    public class SeedRange()
    {
        public Int128 start { get; set; }
        public Int128 end { get; set; }
    }

    public override ValueTask<string> Solve_1()
    {
        List<Int128> seeds2 = new List<Int128>(seeds);
        for(var i = 0; i < seeds2.Count; i++)
        {
            var seed = seeds2[i];
            foreach (var map in maps)
            {
                foreach (var mapping in map.mappings)
                {
                    if (seed >= mapping.source && seed < mapping.source + mapping.range)
                    {
                        seed = mapping.dest + (seed - mapping.source);
                        break;
                    }
                }
            }
            seeds2[i] = seed;
        }
        return new ValueTask<string>(seeds2.Min().ToString());
    } 

    public override ValueTask<string> Solve_2()
    {
        for (var i = 0; i < seeds.Count; i += 2)
        {
            Int128 start = seeds[i];
            Int128 range = seeds[i + 1];
            
            ranges.Enqueue(new SeedRange(){start = start, end = start + range});
        }
        
        // printRanges(ranges, "Initial");
        foreach (var map in maps)
        {
            var newRanges = new Queue<SeedRange>();
            while(ranges.Any())
            {
                var r = ranges.Dequeue();
                var mapped = false;
                
                foreach (var m in map.mappings)
                {
                    var olstart = Int128.Max(r.start, m.source);
                    var olend = Int128.Min(r.end, m.source + m.range);
                    
                    
                    if (olstart < olend)
                    {
                        newRanges.Enqueue(new SeedRange(){start = olstart-m.source+m.dest, end = olend-m.source+m.dest});

                        if (r.start < olstart)
                            ranges.Enqueue(new SeedRange(){start = r.start, end = olstart});

                        if (olend < r.end)
                            ranges.Enqueue(new SeedRange(){start = olend, end = r.end});
                        mapped = true;
                        break;
                    }
                } 
                if(!mapped)
                    newRanges.Enqueue(r);
            }
            ranges = newRanges;
        }
        
        return new ValueTask<string>(ranges.MinBy(x => x.start).start.ToString()); //
    } 
}