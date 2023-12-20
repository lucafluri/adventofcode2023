using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day20 : BaseDay
{
    Dictionary<string , (string, string[])> modules = new Dictionary<string, (string, string[])>();
    Dictionary<string, bool> flips = new Dictionary<string, bool>();
    Dictionary<string, Dictionary<string, bool>> conjs = new Dictionary<string, Dictionary<string, bool>>();
    Dictionary<string, int> rx_bits = new Dictionary<string, int>();
    long minPart2 = 0;
    
    public Day20()
    {
        var input = File.ReadAllLines(InputFilePath).ToList();
        foreach (var line in input)
        {
            var parts = line.Replace(",", "").Split();
            var module = parts[0];
            var dests = parts[2..].ToArray();
            var type = "";

            if (module.StartsWith('%') || module.StartsWith('&'))
            {
                type = module[..1];
                module = module[1..];
            }

            modules[module] = (type, dests);

            foreach (var d in dests)
            {
                conjs.TryAdd(d, new Dictionary<string, bool>());
                conjs[d][module] = false;
            }
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        int[] counts = [0, 0];
        var i = -1;
        while(minPart2 == 0)
        {
            i++;
            
            // Check for part 1 and 2 conditions
            if (i == 1000) Console.WriteLine($"Part 1: {counts[0]} - {counts[1]} - {counts[0] * counts[1]}");
            if (rx_bits.All(x => x.Value > 0))
            {
                var product = rx_bits.Aggregate(1L, (acc, x) => acc * x.Value);
                if (product != 1)
                {
                    minPart2 = product;
                    Console.WriteLine($"Part 2: {minPart2}");
                }
            }
                
            var queue = new Queue<(string from, string to, bool pulse)>();
            queue.Enqueue(("", "broadcaster", false));
            
            while (queue.Count > 0)
            {
                var (from, mod, pulse) = queue.Dequeue();
               
                if(i<1000)counts[pulse ? 1 : 0]++;
                if (!modules.ContainsKey(mod)) continue;
                
                var (type, dests) = modules[mod];
                bool pulseOut;
                
                switch (type)
                {
                    case "": // broadcaster
                        pulseOut = pulse;
                        break;
                    case "%" when !pulse: // flip-flop
                        flips.TryAdd(mod, false);
                        flips[mod] = !flips[mod];
                        pulseOut = flips[mod];
                        break;
                    case "&": // AND Gate aka Conjunction
                        conjs[mod][from] = pulse;
                        pulseOut = !conjs[mod].All(x => x.Value);
                        if (dests.Contains("rx")) 
                        {
                            foreach (var (k, v) in conjs[mod])
                            {
                                rx_bits.TryAdd(k, 0);
                                if(v) rx_bits[k] = i+1;
                            }
                        }
                        break;
                    default:
                        continue;
                }
                    
                foreach (var d in dests)
                    queue.Enqueue((mod, d, pulseOut));
            }
        }
        var prod = counts[0] * counts[1];
        return new (prod.ToString()); // 832957356
    }

    public override ValueTask<string> Solve_2()
    {
        return new (minPart2.ToString()); // 240162699605221
    } 
}