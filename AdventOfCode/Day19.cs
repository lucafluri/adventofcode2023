using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day19 : BaseDay
{
    private readonly List<Part> _parts = [];
    private readonly Dictionary<string, Rule> _rules = new Dictionary<string, Rule>();
    private readonly Dictionary<char, int> _props = new Dictionary<char, int>()
    {
        { 'x', 0 }, { 'm', 1 }, { 'a', 2 }, { 's', 3 }
    };
    
    private long _total = 0;
    
    
    private class Part(int x, int m, int a, int s)
    {
        public int X { get; set; } = x;
        public int M { get; set; } = m;
        public int A { get; set; } = a;
        public int S { get; set; } = s;
        public int[] Props { get; set; } = new int[] { x, m, a, s };

        public int Sum { get; set; } = x + m + a + s;
    }
    
    private class Rule(string id, List<(char prop, string op, int num, string d)> rules, string dest)
    {
        public string Id { get; set; } = id;
        public List<(char prop, string op, int num, string d)> rules { get; set; } = rules;
        public string Dest { get; set; } = dest;
    }
    
    public Day19()
    {
        var input = File.ReadAllText(InputFilePath).Split("\n\n");
        var rules = input[0].Split("\n");
        var parts = input[1].Split("\n");

        foreach (var rule in rules)
        {
            var lr = rule.Split("{");
            var id = lr[0];
            var rs = lr[1].Split(",");
            var dest = rs[^1][..^1];
            
            var r = new List<(char prop, string op, int num, string d)>();
            foreach (var s in rs[..^1])
            {
                var p = s.Split(":");
                var prop = p[0][0];
                var lt = p[0][1].ToString();
                var num = int.Parse(p[0][2..]);
                var d = p[1];
                r.Add((prop, lt, num, d));
            }
            _rules.Add(id, new Rule(id, r, dest));
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
            var workflow = "in";
            while (!workflow.Equals("A") && !workflow.Equals("R"))
            {
                var wf = _rules[workflow];
                var foundNew = false;
                foreach (var rule in wf.rules)
                {
                    var prop = part.Props[_props[rule.prop]];
                    if (rule.op == "<")
                    {
                        if (prop >= rule.num) continue;  
                        workflow = rule.d;
                        foundNew = true;
                        break;
                    }

                    if (prop <= rule.num) continue;
                    workflow = rule.d;
                    foundNew = true;
                    break;
                }
                if(!foundNew) workflow = wf.Dest;
            }
            if (workflow == "A") _total += part.Sum;
        } 
       
        return new (_total.ToString()); // 389114
    }
    
    public override ValueTask<string> Solve_2()
    {
        // xmas in range of 1-4000 => 4^4000 possible combinations... nope

        // 1. find all possible paths to A
        // Queue is all possible paths, if path is A, add to accepted
        var stack = new Stack<(string, List<(char prop, string op, int num, string d)>)>();
        var accepted = new List<(string, List<(char prop, string op, int num, string d)>)>();
        
        stack.Push(("in", []));
        
        while(stack.Count > 0)
        {
            var (path, rules) = stack.Pop();
            // Console.WriteLine($"{path} - {rules.Count}");
            var last = path.Split(" ").Last(); 
            switch (last)
            {
                case "A":
                    accepted.Add((path, rules));
                    continue;
                case "R":
                    continue;
            }
            
            foreach(var r in _rules[last].rules)
            {
                var nextRules = rules.Select(x => x).ToList();
                var nextPath = path + " " + r.d;
                nextRules.Add(r);
                rules.Add(Negate(r));
                stack.Push((nextPath, nextRules));
            }
        }
        
        long sum = 0;
        var range = new Dictionary<char, (long, long)>()
        {
            {'x', (1, 4000)},
            {'m', (1, 4000)},
            {'a', (1, 4000)},
            {'s', (1, 4000)}
        };
        
        foreach (var (_, rules) in accepted)
        {
            foreach (var r in rules)
            {
                range[r.prop] = r.op switch
                {
                    ">" => (r.num + 1, range[r.prop].Item2),
                    ">=" => (r.num, range[r.prop].Item2),
                    "<" => (range[r.prop].Item1, r.num - 1),
                    "<=" => (range[r.prop].Item1, r.num),
                    _ => range[r.prop]
                };
            }   
            sum += (range['x'].Item2 - range['x'].Item1 + 1) 
                   * (range['m'].Item2 - range['m'].Item1 + 1) 
                   * (range['a'].Item2 - range['a'].Item1 + 1) 
                   * (range['s'].Item2 - range['s'].Item1 + 1);
        }
        
        return new (sum.ToString());
    }

    private (char prop, string op, int num, string d) Negate((char prop, string op, int num, string d) x)
    {
        return x.op switch
        {
            "<" => (x.prop, ">=", x.num, x.d),
            ">" => (x.prop, "<=", x.num, x.d),
            "<=" => (x.prop, ">", x.num, x.d),
            _ => (x.prop, "<", x.num, x.d)
        };
    }
}