using System.Text.RegularExpressions;
using Spectre.Console.Rendering;

namespace AdventOfCode;

// Runtime Total: ~11ms
// Setup: ~5ms
// Part 1: ~3ms
// Part 2: ~3ms
public class Day05 : BaseDay
{
    private readonly List<Map> _maps = new();
    private readonly List<long> _seeds = new();
    private Queue<SeedRange> _ranges = new();
    private static Regex _rgxNonDigit = new("[^0-9]");
    private static Regex _rgxSpace= new(@"\s+");

    private class Mapping
    {
        public long Source { get; init; }
        public long Dest { get; init; }
        public long Range { get; init; }
    }

    private class Map
    {
        public readonly List<Mapping> Mappings = new();
    }
    
    private struct SeedRange()
    {
        public long Start { get; init; }
        public long End { get; init; }
    }
    
    private static List<long> GetNumbers(string line)
    {
        var s = _rgxNonDigit.Replace(line, " ");
        s = _rgxSpace.Replace(s,  " ");
        return (from seed in s.Trim().Split(" ") where seed != "" select long.Parse(seed)).ToList();
    }

    private static bool IsLetter(string str) => !string.IsNullOrEmpty(str) && char.IsLetter(str[0]);
    
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
        var seeds2 = new List<long>(_seeds);
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
                    var overlapStart = long.Max(r.Start, m.Source);
                    var overlapEnd = long.Min(r.End, m.Source + m.Range);

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