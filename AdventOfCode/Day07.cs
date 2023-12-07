using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~76ms
// Setup: ~34ms
// Part 1: ~9ms
// Part 2: ~33ms
public class Day07 : BaseDay
{
    private List<string> _input;
    private List<(string, int)> _hands = new List<(string, int)>();
    private Dictionary<string, int> strengths = new Dictionary<string, int>();
    private Dictionary<char, int> cards = new Dictionary<char, int>();
    private bool part2 = false;
    
    public Day07()
    {
        cards.Add('2', 2);
        cards.Add('3', 3);
        cards.Add('4', 4);
        cards.Add('5', 5);
        cards.Add('6', 6);
        cards.Add('7', 7);
        cards.Add('8', 8);
        cards.Add('9', 9);
        cards.Add('T', 10);
        cards.Add('J', 11);
        cards.Add('Q', 12);
        cards.Add('K', 13);
        cards.Add('A', 14);
        
        _input = File.ReadAllLines(InputFilePath).ToList();
        SetHands();
        
    }

    private void SetHands()
    {
        _hands.Clear();
        strengths.Clear();
        foreach (var hand in _input)
        {
            var matchLine = new Regex(@"(.*)\s(\d+)");
            var g = matchLine.Match(hand);
            var h = g.Groups[1].Value;
            var b = int.Parse(g.Groups[2].Value);
            _hands.Add((h, b));
            strengths.Add(h, GetStrength(h));
        }
    }
    
    // Strength of hand: 1-7
    private int GetStrength(string hand)
    {
        var c = hand.ToCharArray().ToList();

        if (part2 && hand != "JJJJJ")
        {
            var maxChar = c.Where(x => x != 'J').GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
            var h2 = hand.Replace("J", maxChar.ToString());
            c = h2.ToCharArray().ToList();
        }
        
        var counts = c.GroupBy(x => x).Select(x => x.Count()).ToList();

        return counts.Max() switch
        {
            5 => 7, // Five of a kind
            4 => 6, // Four of a kind
            3 when counts.Count == 2 => 5, //Full House
            3 when counts.Count == 3 => 4, //Three of a kind
            2 when counts.Count == 3 => 3, //Two pairs
            2 when counts.Count == 4 => 2, //One pair
            _ => 1
        };
    }

    private int CompareByCard(string s1, string s2)
    {
        var c1 = s1.ToCharArray().ToList();
        var c2 = s2.ToCharArray().ToList();
        
        for(var i = 0; i < c1.Count; i++)
        {
            var v1 = cards[c1[i]];
            var v2 = cards[c2[i]];
            if (v1 > v2)
                return 1;
            if (v1 < v2)
                return -1;
        }
        return 0;
    }

    private int CompareLines((string, int) s1,(string, int) s2)
    {
        var s1Strength = strengths[s1.Item1];
        var s2Strength = strengths[s2.Item1];
        
        if(s1Strength > s2Strength)
            return 1;
        if(s1Strength < s2Strength)
            return -1;
        return CompareByCard(s1.Item1, s2.Item1);
    }

    private long SortAndGetCount()
    {
        long total = 0;
        _hands.Sort(CompareLines);
        
        for(var i = 0; i < _hands.Count; i++)
        {
            total += _hands[i].Item2*(i+1);
        }  
        return total;
    }
    
    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>(SortAndGetCount().ToString()); // 249748283
    }

    public override ValueTask<string> Solve_2()
    {
        part2 = true;
        cards['J'] = 1;
        SetHands();
        
        return new ValueTask<string>(SortAndGetCount().ToString()); // 248029057
    } 
}