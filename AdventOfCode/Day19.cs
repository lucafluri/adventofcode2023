using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day19 : BaseDay
{
    private readonly List<Part> _parts = [];
    private readonly Dictionary<string, Workflow> _workflows = new Dictionary<string, Workflow>();
    private readonly Dictionary<char, int> _categories = new Dictionary<char, int>()
    {
        { 'x', 0 }, { 'm', 1 }, { 'a', 2 }, { 's', 3 }
    };
    Dictionary<char, (long, long)> range = new Dictionary<char, (long, long)>()
    {
        {'x', (1, 4000)},
        {'m', (1, 4000)},
        {'a', (1, 4000)},
        {'s', (1, 4000)} 
    };
    
    private long _total = 0;
    
    
    private class Part(int x, int m, int a, int s)
    {
        public int X { get; set; } = x;
        public int M { get; set; } = m;
        public int A { get; set; } = a;
        public int S { get; set; } = s;
        public int[] Categories { get; set; } = new int[] { x, m, a, s };
        public int Sum { get; set; } = x + m + a + s;
    }
    
    private class Workflow(string id, List<(char cat, string op, int num, string d)> conditions, string dest)
    {
        public string Id { get; set; } = id;
        public List<(char cat, string op, int num, string d)> Conditions { get; set; } = conditions;
        public string Dest { get; set; } = dest;
    }
    
    public Day19()
    {
        var input = File.ReadAllText(InputFilePath).Split("\r\n\r\n");
        Console.WriteLine($"{input.Length}"); 
        var rules = input[0].Split("\r\n");
        var parts = input[1].Split("\r\n");

        foreach (var rule in rules)
        {
            var lr = rule.Split("{");
            var id = lr[0];
            var rs = lr[1].Split(",");
            var dest = rs[^1][..^1];
            
            var r = new List<(char cat, string op, int num, string d)>();
            foreach (var s in rs[..^1])
            {
                var p = s.Split(":");
                var cat = p[0][0];
                var lt = p[0][1].ToString();
                var num = int.Parse(p[0][2..]);
                var d = p[1];
                r.Add((cat, lt, num, d));
            }
            r.Add((' ',"", 0, dest));
            _workflows.Add(id, new Workflow(id, r, dest));
        }
        
        foreach (var part in parts)
        {
            var strs = part[1..^1].Split(",");
            var x = int.Parse(strs[0][2..]);
            var m = int.Parse(strs[1][2..]);
            var a = int.Parse(strs[2][2..]);
            var s = int.Parse(strs[3][2..]);
            _parts.Add(new Part(x, m, a, s));
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        foreach (var part in _parts)
        {
            var current = "in";
            while (!current.Equals("A") && !current.Equals("R"))
            {
                var wf = _workflows[current];
                foreach (var rule in wf.Conditions)
                {
                    var cat = rule.cat != ' ' ? part.Categories[_categories[rule.cat]] : ' ';
                    if (rule.op == "<")
                    {
                        if (cat >= rule.num) continue;  
                        current = rule.d;
                        break;
                    }
                    if (rule.op == ">")
                    {
                        if (cat <= rule.num) continue;
                        current = rule.d;
                        break;
                    }
                    current = rule.d;
                    break;
                }
            }
            if (current == "A") _total += part.Sum;
        } 
        return new (_total.ToString()); // 389114
    }
    
    private static (char cat, string op, int num, string d) Negate((char cat, string op, int num, string d) x)
    {
        return x.op switch
        {
            "<" => (x.cat, ">=", x.num, x.d),
            ">" => (x.cat, "<=", x.num, x.d),
            "<=" => (x.cat, ">", x.num, x.d),
            ">=" => (x.cat, "<", x.num, x.d),
            _ => (x.cat, x.op, x.num, x.d)
        };
    }
    
    public override ValueTask<string> Solve_2()
    {
        _total = 0;
        var queue = new Queue<(string, List<(char cat, string op, int num, string d)>)>();
        var accepted = new List<(string, List<(char cat, string op, int num, string d)>)>();
        
        queue.Enqueue(("in", []));
        
        while(queue.Count > 0)
        {
            var (path, conditions) = queue.Dequeue();
            var last = path.Split(" ").Last();
            
            switch (last)
            {
                case "A":
                    accepted.Add((path, conditions));
                    continue;
                case "R":
                    continue;
                default:
                {
                    foreach(var (cat, op, num, d) in _workflows[last].Conditions)
                    {
                        var newPath = $"{path} {d}";
                        var newConditions = new List<(char cat, string op, int num, string d)>(conditions);
                        if (cat != ' ')
                        {
                            newConditions.Add((cat, op, num, d));
                            conditions.Add(Negate((cat, op, num, d))); 
                        }
                        queue.Enqueue((newPath, newConditions));
                    }
                    break;
                }
            }
        }
        
        foreach (var (path, conditions) in accepted)
        {
            var ranges = new Dictionary<char, (long, long)>(range);
            foreach (var r in conditions)
            {
                ranges[r.cat] = r.op switch
                {
                    ">" => (Math.Max(r.num + 1, ranges[r.cat].Item1), ranges[r.cat].Item2),
                    ">=" => (Math.Max(r.num, ranges[r.cat].Item1), ranges[r.cat].Item2),
                    "<" => (ranges[r.cat].Item1, (Math.Min(r.num - 1, ranges[r.cat].Item2))),
                    "<=" => (ranges[r.cat].Item1, (Math.Min(r.num, ranges[r.cat].Item2))),
                    _ => ranges[r.cat]
                };
            }   
            
            _total += (ranges['x'].Item2 - ranges['x'].Item1 + 1) 
                   * (ranges['m'].Item2 - ranges['m'].Item1 + 1) 
                   * (ranges['a'].Item2 - ranges['a'].Item1 + 1) 
                   * (ranges['s'].Item2 - ranges['s'].Item1 + 1);
        }
        return new (_total.ToString()); // 125051049836302
    }
}