using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~24ms
// Setup: ~2ms
// Part 1: ~15ms
// Part 2: ~7ms
public class Day07 : BaseDay
{
    private readonly List<string> _input;
    private List<(string hand, int bet, int strength)> _hands = new(); 
    private Dictionary<char, int> _cards = new();
    private bool _part2 = false;
    private readonly Regex _rgxLine = new(@"(.*)\s(\d+)");
    
    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();

        var i = 2;
        "23456789TJQKA".ToCharArray().ToList().ForEach(x => _cards.Add(x, i++));
    }

    private void SetHands()
    {
        _hands.Clear();
        foreach (var hand in _input)
        {
            var g = _rgxLine.Match(hand);
            var h = g.Groups[1].Value;
            var b = int.Parse(g.Groups[2].Value);
            _hands.Add((h, b, GetStrength(h)));
        }
    }
    
    // Strength of hand: 1-7 
    private int GetStrength(string hand)
    {
        var c = hand.ToCharArray().ToList();

        if (_part2 && hand != "JJJJJ")
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
            var v1 = _cards[c1[i]];
            var v2 = _cards[c2[i]];
            if (v1 > v2) return 1;
            if (v1 < v2) return -1;
        }
        return 0;
    }

    private int CompareLines((string, int, int) s1,(string, int, int) s2)
    {
        var s1Strength = s1.Item3;
        var s2Strength = s2.Item3;
        
        if(s1Strength > s2Strength) return 1;
        if(s1Strength < s2Strength) return -1;
        
        return CompareByCard(s1.Item1, s2.Item1);
    }

    private long SortAndGetCount()
    {
        long total = 0;
        _hands.Sort(CompareLines);
        
        for(var i = 0; i < _hands.Count; i++) total += _hands[i].Item2*(i+1);
        
        return total;
    }

    public override ValueTask<string> Solve_1()
    {
        SetHands();
        return new ValueTask<string>(SortAndGetCount().ToString()); // 249748283
    }


    public override ValueTask<string> Solve_2()
    {
        _part2 = true;
        _cards['J'] = 1;
        SetHands();
        
        return new ValueTask<string>(SortAndGetCount().ToString()); // 248029057
    } 
}