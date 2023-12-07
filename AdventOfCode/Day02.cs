using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~3ms
// Setup: ~3ms
// Part 1: ~0.4ms
// Part 2: ~0.2ms
public class Day02 : BaseDay
{
    private readonly string _input;
    private readonly string[] _inputLines;
    private List<Game> games = new List<Game>();
    private Regex rgx = new(@"Game \d+:");

    private class Game
    {
        public int id { get; init; }
        
       
        public int minRed { get; set; }
        public int minGreen { get; set; }
        public int minBlue { get; set; }
        public int setCount { get; set; }
        
        public string[] sets { get; set; }
        
        public bool isValid = true;
        
    }

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
        _inputLines = File.ReadAllLines(InputFilePath);
        ParseGames(_inputLines);
    }

    private void ParseGames(IEnumerable<string> lines)
    {
        var id = 0;
        foreach (var line in lines)
        {
            id++;
            var l = line;
            var g = new Game
            {
                minRed = 0,
                minGreen = 0,
                minBlue = 0,
                id = id
            };

            // Parse Line into sets
            //Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

            // Remove Game 1: with regex
            l = rgx.Replace(l, "");
            l = l.Replace(" ", "");
            
            // Split into sets by ;
            var sets = l.Split(";");
            
            g.setCount = sets.Length;
            g.sets = sets;

            foreach (var set in sets)
            {
                var cubes = set.Split(",");
                foreach (var c in cubes)
                {
                    if (c.Contains("red"))
                    {
                        var redCount = int.Parse(c.Replace("red", ""));
                        if (redCount > 12)
                        {
                            g.isValid = false;
                        }
                        
                        if(redCount > g.minRed)
                            g.minRed = redCount;
                    }
                    if (c.Contains("green"))
                    {
                        var greenCount = int.Parse(c.Replace("green", ""));
                        if (greenCount > 13)
                        {
                            g.isValid = false;
                        }
                        if(greenCount > g.minGreen)
                            g.minGreen = greenCount;
                    }

                    if (!c.Contains("blue")) continue;
                    var blueCount = int.Parse(c.Replace("blue", ""));
                    if (blueCount > 14)
                    {
                        g.isValid = false;
                    }
                    if(blueCount > g.minBlue)
                        g.minBlue = blueCount;
                }
            }
            games.Add(g);
        }
    }


    public override ValueTask<string> Solve_1()
    {
        var total = games.Where(game => game.isValid).Sum(game => game.id);

        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var total = games.Sum(game => game.minRed * game.minGreen * game.minBlue);

        return new ValueTask<string>(total.ToString());
        
    }
}
