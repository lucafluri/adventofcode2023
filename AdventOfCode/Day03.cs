using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~9ms
// Setup: ~4ms
// Part 1: ~3ms
// Part 2: ~2ms
public class Day03 : BaseDay
{
    private readonly string[] _input;
    private List<Num>[] nums;
    private List<Symbol>[] symbols;
    
    private int total = 0;
    
    private class Num
    {
        public int value { get; set; }
        public int x { get; set; }
        public int length { get; set; }
    }
    
    private class Symbol
    {
        public char value { get; set; }
        public int x { get; set; }
    }
    
    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath);
        nums = new List<Num>[_input.Length];
        symbols = new List<Symbol>[_input.Length];
        
        var idx = 0;

        foreach (var line in _input)
        {
            // Regex match all numbers into num objects
            Regex rgx = new Regex(@"\d+");
            var matches = rgx.Matches(line);
            nums[idx] = new List<Num>();
            foreach (Match match in matches)
            {
                Num n = new Num();
                n.value = int.Parse(match.Value);
                n.x = match.Index;
                n.length = match.Length;
                nums[idx].Add(n);
            }
            
            // Regex match all symbols into symbol objects except dots
            rgx = new Regex(@"[^\.0-9]");
            matches = rgx.Matches(line);
            symbols[idx] = new List<Symbol>();
            foreach (Match match in matches)
            {
                Symbol s = new Symbol();
                s.value = match.Value[0];
                s.x = match.Index;
                symbols[idx].Add(s);
            }
            idx++;
        }
    }
    
    

    public override ValueTask<string> Solve_1()
    {
        for (var i = 0; i < nums.Length; i++)
        {
            foreach (var num in nums[i])
            {
                // Check if symbol is adjacent to num in same line
                foreach (var symbol in symbols[i]
                             .Where(symbol => symbol.x >= num.x - 1 && symbol.x <= num.x + num.length))
                    total += num.value;

                // Check if symbol is adjacent to num in line above
                if (i == 0) continue;
                foreach (var symbol in symbols[i - 1]
                             .Where(symbol => symbol.x >= num.x - 1 && symbol.x <= num.x + num.length))
                    total += num.value;

                // Check if symbol is adjacent to num in line below
                if (i >= _input.Length - 1) continue;
                foreach (var symbol in symbols[i + 1]
                             .Where(symbol => symbol.x >= num.x - 1 && symbol.x <= num.x + num.length))
                    total += num.value;
            }
        }
        
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        total = 0;
        //Iterate over all symbols per line and check whether symbol is adjacent to exactly two nums in same line and above and below
        //If so multiply nums together and add to total2
        for (var i = 0; i < symbols.Length; i++)
        {
            foreach (var symbol in symbols[i])
            {
                var numCount = 0;
                var num1 = 0;
                var num2 = 0;
                // Check if symbol is adjacent to num in same line
                foreach (var num in nums[i]
                             .Where(num => symbol.x >= num.x - 1 && symbol.x <= num.x + num.length))
                {
                    numCount++;
                    if (numCount == 1)
                        num1 = num.value;
                    else
                        num2 = num.value;
                    
                }

                // Check if symbol is adjacent to num in line above
                if (i > 0)
                {
                    foreach (var num in nums[i - 1]
                                 .Where(num => symbol.x >= num.x - 1 && symbol.x <= num.x + num.length))
                    {
                        numCount++;
                        if (numCount == 1)
                            num1 = num.value;
                        else if (numCount == 2)
                            num2 = num.value;
                        else break;
                    }
                }

                // Check if symbol is adjacent to num in line below
                if (i < _input.Length - 1)
                {
                    foreach (var num in nums[i + 1]
                                 .Where(num => symbol.x >= num.x - 1 && symbol.x <= num.x + num.length))
                    {
                        numCount++;
                        if (numCount == 1)
                            num1 = num.value;
                        else if (numCount == 2)
                            num2 = num.value;
                        else break;
                    }
                }

                if (numCount == 2)
                    total += num1 * num2;
            }
        }
        
        return new ValueTask<string>(total.ToString());
    } 
}
