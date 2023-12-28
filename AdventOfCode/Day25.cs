using System.Text.RegularExpressions;
using PostSharp.Reflection;

namespace AdventOfCode;

public class Day25 : BaseDay
{
    private Dictionary<string, List<string>> _graph = new();
    private readonly Dictionary<string, List<string>> _graphT = new();
    
    void AddEdge(string from, string to)
    {
        if (!_graph.ContainsKey(from))
        {
            _graph[from] = new();
            _graphT[from] = new();
        }
        _graph[from].Add(to);
        _graphT[from].Add(to);
    }
    
    public Day25()
    {
        foreach (var line in  File.ReadAllLines(InputFilePath).ToList())
        {
            var parts = line.Split(": ");
            var key = parts[0];
            var nodes = parts[1].Split(' ');
            foreach (var node in nodes)
            {
                AddEdge(key, node);
                AddEdge(node, key);
            }
        }
    }

    // Karger's algorithm for finding the minimum cut in a graph
    private (int numCuts, int s1, int s2) Karger()
    {
        _graph = _graphT.ToDictionary(k => k.Key, v => new List<string>(v.Value));
        var components = _graph.Keys.ToDictionary(k => k, v => 1);
        
        var random = new Random();
        while (_graph.Count > 2) // split into 2 components
        {
            var edgeIndex = random.Next(_graph.Count);
            var from = _graph.Keys.ElementAt(edgeIndex);
            var to = _graph[from][random.Next(_graph[from].Count)];
            
            var merged = $"{from}-{to}";
            _graph[merged] = new(_graph[from].Where(n => n != to));
            _graph[merged].AddRange(_graph[to].Where(n => n != from));
            
            foreach (var node in _graph[from])
                while (_graph[node].Remove(from))
                    _graph[node].Add(merged);
            
            foreach (var node in _graph[to])
                while (_graph[node].Remove(to))
                    _graph[node].Add(merged);
            
            components[merged] = components[from] + components[to];
            
            _graph.Remove(from);
            _graph.Remove(to);
        }
        
        var s1 = _graph.Keys.First();
        var s2 = _graph.Keys.Last();
        return (_graph[s1].Count, components[s1], components[s2]);
    }
    
    public override ValueTask<string> Solve_1()
    {
        var (cuts, s1, s2) = Karger();
        while (cuts != 3)
            (cuts, s1, s2) = Karger();
            
        return new ((s1*s2).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new (0.ToString());
    } 
}