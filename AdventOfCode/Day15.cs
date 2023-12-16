using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day15 : BaseDay
{
    private List<string> _input;
    private Dictionary<int, List<(string, int)>> _boxes = new();
    private int cv = 0;
    private Regex _rgx = new(@"(\w+)([=-](\d+)?)");
    
    public Day15()
    {
        _input = File.ReadAllLines(InputFilePath)[0].Split(',').ToList();
    }

    private void Hash(char c)
    {
        cv += (int)c;
        cv *= 17;
        cv %= 256;
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = 0;
        foreach (var str in _input)
        {
            foreach(var x in str)
            {
                Hash(x);
            }
            sum += cv;
            cv = 0; 
        }
        return new (sum.ToString()); //497373
    }

    public override ValueTask<string> Solve_2()
    {
        foreach (var instr in _input)
        {
            var matches = _rgx.Matches(instr);
            
            var label = matches[0].Groups[1].Value;
            var op = matches[0].Groups[2].Value[0];
            var val = 0;
            if(matches[0].Groups[3].Success)
                val = int.Parse(matches[0].Groups[3].Value);
            
            foreach (var c in label)
                Hash(c);
            
            var box = cv;
            cv = 0;

            switch (op)
            {
                case '=':
                    if (!_boxes.ContainsKey(box))
                        _boxes.Add(box, []);
                    if(_boxes[box].All(x => x.Item1 != label))
                        _boxes[box].Add((label, val));
                    else
                    {
                        var idx = _boxes[box].FindIndex(x => x.Item1 == label);
                        _boxes[box][idx] = (label, val);
                    }
                    break;
                case '-':
                    if (_boxes.ContainsKey(box))
                    {
                        var idx = _boxes[box].FindIndex(x => x.Item1 == label);
                        if (idx != -1)
                            _boxes[box].RemoveAt(idx);
                    }
                    break;
            }
        }
        
        var sum = 0;
        for (var i = 0; i < 256; i++)
        {
            if(!_boxes.ContainsKey(i)) continue;
            for (var j = 0; j < _boxes[i].Count; j++)
                sum += (i+1) * (j+1) * (_boxes[i][j].Item2); 
        }
        
        return new (sum.ToString()); // 259356
    } 
}