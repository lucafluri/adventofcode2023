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

    private class Game
    {
        public int id { get; set; }
        
       
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

    private void ParseGames(string[] lines)
    {
        int id = 0;
        foreach (var line in lines)
        {
            id++;
            string l = line;
            Game g = new Game();
            g.minRed = 0;
            g.minGreen = 0;
            g.minBlue = 0;
            
            g.id = id;
             
            // Parse Line into sets
            //Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

            // Remove Game 1: with regex
            Regex rgx = new Regex(@"Game \d+:");
            l = rgx.Replace(l, "");
            
            // Remove all spaces
            l = l.Replace(" ", "");
            
            // Split into sets by ;
            string[] sets = l.Split(";");
            
            g.setCount = sets.Length;
            g.sets = sets;

            foreach (var set in sets)
            {
                var cubes = set.Split(",");
                foreach (var c in cubes)
                {
                    if (c.Contains("red"))
                    {
                        int redCount = int.Parse(c.Replace("red", ""));
                        if (redCount > 12)
                        {
                            g.isValid = false;
                        }
                        
                        if(redCount > g.minRed)
                            g.minRed = redCount;
                    }
                    if (c.Contains("green"))
                    {
                        int greenCount = int.Parse(c.Replace("green", ""));
                        if (greenCount > 13)
                        {
                            g.isValid = false;
                        }
                        if(greenCount > g.minGreen)
                            g.minGreen = greenCount;
                    }
                    if (c.Contains("blue"))
                    {
                        int blueCount = int.Parse(c.Replace("blue", ""));
                        if (blueCount > 14)
                        {
                            g.isValid = false;
                        }
                        if(blueCount > g.minBlue)
                            g.minBlue = blueCount;
                    }
                }
            }
            games.Add(g);
        }
    }


    public override ValueTask<string> Solve_1()
    {
        Console.WriteLine("");
        
        
        int total = 0;

        foreach (var game in games)
        {
            if (game.isValid)
            {
                total += game.id;
            }
        }
        
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int total = 0;

        foreach (var game in games) 
        {
            int power = game.minRed * game.minGreen * game.minBlue;
            total += power;
            
            // Console.WriteLine($"{game.id} - {game.minRed} - {game.minGreen} - {game.minBlue} - {power}");
        }
        
        return new ValueTask<string>(total.ToString());
        
    }
}
