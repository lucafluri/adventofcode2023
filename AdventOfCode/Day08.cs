using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~880ms
// Setup: ~27ms
// Part 1: ~127ms
// Part 2: ~726ms
public class Day08 : BaseDay
{
    private List<string> _input;
    private List<Node> _nodes = new();
    private readonly char[] instr;
    private readonly Regex _rgxLine = new(@"(\w+) = \((\w+), (\w+)\)");

    private class Node
    {
        public string Name { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        
        public char lastChar => Name[^1];

        public override bool Equals(object obj)
        {
            return Name == ((Node)obj)?.Name;
        }
    }
    
    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        instr = _input[0].ToCharArray();
        SetNodes();
    }
    
    private void SetNodes()
    {
        _nodes.Clear();
        for(var i = 2; i<_input.Count; i++)
        {
            var line = _input[i];
            var g = _rgxLine.Match(line);
            var n = g.Groups[1].Value;
            var l = g.Groups[2].Value;
            var r = g.Groups[3].Value;

            var node = new Node() { Name = n };
            
            if(l != n)
                node.Left = _nodes.Where(x => x.Name==l).ToList().Count == 0 ? new Node { Name = l} : _nodes.Where(x => x.Name==l).ToList()[0];
            else node.Left = node;
                
            if(r!=n) node.Right = _nodes.Where(x => x.Name==r).ToList().Count == 0 ? new Node { Name = r } : _nodes.Where(x => x.Name==r).ToList()[0];
            else node.Right = node;
            
            _nodes.Add(node);
            
        }
    }

    private long GetLoopLength(Node n)
    {
        var current = n;
        var count = 0;
        var q = new Queue<char>(instr);
        while(q.Count > 0)
        {
            var c = q.Dequeue();
            if (q.Count == 0) foreach (var ch in instr) q.Enqueue(ch);
            
            if (c.Equals('L'))
            {
                current = _nodes.Find(x => x.Name == current.Left.Name);
                if (current.lastChar == 'Z')
                {
                    count++;
                    break;
                }
            }
        
            if (c.Equals('R'))
            {
                current = _nodes.Find(x => x.Name == current.Right.Name);
                if (current.lastChar == 'Z')
                {
                    count++;
                    break;
                }
            } 
            count++;
        }

        return count;
    }

    // Stolen from the internet
    private static long lcm_of_array_elements(int[] element_array)
    {
        long lcm_of_array_elements = 1;
        int divisor = 2;
         
        while (true) {
             
            int counter = 0;
            bool divisible = false;
            for (int i = 0; i < element_array.Length; i++) {
 
                // lcm_of_array_elements (n1, n2, ... 0) = 0.
                // For negative number we convert into
                // positive and calculate lcm_of_array_elements.
                if (element_array[i] == 0) {
                    return 0;
                }
                else if (element_array[i] < 0) {
                    element_array[i] = element_array[i] * (-1);
                }
                if (element_array[i] == 1) {
                    counter++;
                }
 
                // Divide element_array by devisor if complete
                // division i.e. without remainder then replace
                // number with quotient; used for find next factor
                if (element_array[i] % divisor == 0) {
                    divisible = true;
                    element_array[i] = element_array[i] / divisor;
                }
            }
 
            // If divisor able to completely divide any number
            // from array multiply with lcm_of_array_elements
            // and store into lcm_of_array_elements and continue
            // to same divisor for next factor finding.
            // else increment divisor
            if (divisible) {
                lcm_of_array_elements = lcm_of_array_elements * divisor;
            }
            else {
                divisor++;
            }
 
            // Check if all element_array is 1 indicate 
            // we found all factors and terminate while loop.
            if (counter == element_array.Length) { 
                return lcm_of_array_elements;
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var current = _nodes.Find(x => x.Name == "AAA");
        return new (GetLoopLength(current).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var nodes = _nodes.FindAll(x => x.lastChar == 'A');
        var loopLengths = new int[nodes.Count];
        
        for (var i = 0; i < nodes.Count; i++) loopLengths[i] = ((int) GetLoopLength(nodes[i]));
        return new (lcm_of_array_elements(loopLengths).ToString());
    } 
}