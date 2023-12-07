using System.Text.RegularExpressions;
using Spectre.Console.Rendering;

namespace AdventOfCode;

// Runtime Total: ~19ms
// Setup: ~11ms
// Part 1: ~6ms
// Part 2: ~3ms
public class Day05 : BaseDay
{
    private readonly List<Map> _maps = new List<Map>();
    private readonly List<Int128> _seeds = new List<Int128>();
    private Queue<SeedRange> _ranges = new Queue<SeedRange>();

    private class Mapping
    {
        public Int128 Source { get; init; }
        public Int128 Dest { get; init; }
        public Int128 Range { get; init; }
    }

    private class Map
    {
        public readonly List<Mapping> Mappings = new List<Mapping>();
    }
    
    private class SeedRange()
    {
        public Int128 Start { get; init; }
        public Int128 End { get; init; }
    }
    
    private static List<Int128> GetNumbers(string line)
    {
        var rgxNonDigit = new Regex("[^0-9]");
        var s = rgxNonDigit.Replace(line, " ");
        s = Regex.Replace(s, @"\s+", " ");
        return (from seed in s.Trim().Split(" ") where seed != "" select Int128.Parse(seed)).ToList();
    }

    private static bool IsLetter(string str) => !String.IsNullOrEmpty(str) && Char.IsLetter(str[0]);
    
    public Day05()
    {
        var input = File.ReadAllLines(InputFilePath);

        for (var i = 0; i < input.Length; i++)
        {
            if (i == 0) _seeds = GetNumbers(input[i]);
            if (IsLetter(input[i])) _maps.Add(new Map());

            if (input[i] == "" || IsLetter(input[i])) continue;
            var nums = GetNumbers(input[i]);

            _maps[^1].Mappings.Add(new Mapping() { Dest = nums[0], Source = nums[1], Range = nums[2] });
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var seeds2 = new List<Int128>(_seeds);
        for(var i = 0; i < seeds2.Count; i++)
        {
            var seed = seeds2[i];
            foreach (var map in _maps)
            {
                foreach (var mapping in map.Mappings.Where(mapping => seed >= mapping.Source && seed < mapping.Source + mapping.Range))
                {
                    seed = mapping.Dest + (seed - mapping.Source);
                    break;
                }
            }
            seeds2[i] = seed;
        }
        return new ValueTask<string>(seeds2.Min().ToString());
    } 

    public override ValueTask<string> Solve_2()
    {
        for (var i = 0; i < _seeds.Count; i += 2)
        {
            var start = _seeds[i];
            var range = _seeds[i + 1];
            
            _ranges.Enqueue(new SeedRange(){Start = start, End = start + range});
        }
        
        foreach (var map in _maps)
        {
            var newRanges = new Queue<SeedRange>();
            while(_ranges.Count != 0)
            {
                var r = _ranges.Dequeue();
                var mapped = false;
                
                foreach (var m in map.Mappings)
                {
                    var overlapStart = Int128.Max(r.Start, m.Source);
                    var overlapEnd = Int128.Min(r.End, m.Source + m.Range);

                    if (overlapStart >= overlapEnd) continue;
                    newRanges.Enqueue(new SeedRange(){Start = overlapStart-m.Source+m.Dest, End = overlapEnd-m.Source+m.Dest});

                    if (r.Start < overlapStart)
                        _ranges.Enqueue(new SeedRange(){Start = r.Start, End = overlapStart});

                    if (overlapEnd < r.End)
                        _ranges.Enqueue(new SeedRange(){Start = overlapEnd, End = r.End});
                    mapped = true;
                    break;
                } 
                if(!mapped)
                    newRanges.Enqueue(r);
            }
            _ranges = newRanges;
        }
        
        return new ValueTask<string>(_ranges.MinBy(x => x.Start).Start.ToString()); //
    } 
}