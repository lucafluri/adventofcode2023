using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode;


//SOLVED IN PYTHON

public class Day01 : BaseDay
{
    private readonly string _input;
    private readonly string[] _inputLines;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
        _inputLines = File.ReadAllLines(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        Console.WriteLine("");
        Console.WriteLine(_inputLines.Length);

        var total = 0;

        foreach (var line in _inputLines)
        {
            //Regex remove anything that is not a number
            Regex rgx = new Regex(@"[^\d]");
            string l = rgx.Replace(line, "");

            total += int.Parse(l[0] + l[l.Length- 1].ToString());
        }
        
        return new ValueTask<string>(total.ToString()); // 54331
    }

    public override ValueTask<string> Solve_2()
    {
        var total = 0;
        
        foreach (var line in _inputLines)
        {
            string l = Regex.Replace(line, "one", "o1ne");
            l = Regex.Replace(l, "two", "t2wo");
            l = Regex.Replace(l, "three", "t3hree");
            l = Regex.Replace(l, "four", "f4our");
            l = Regex.Replace(l, "five", "f5ive");
            l = Regex.Replace(l, "six", "s6ix");
            l = Regex.Replace(l, "seven", "s7even");
            l = Regex.Replace(l, "eight", "e8ight");
            l = Regex.Replace(l, "nine", "n9ine");
            
            Regex rgx = new Regex(@"[^\d]");
            l = rgx.Replace(l, "");
        
            total += int.Parse(l[0] + l[l.Length- 1].ToString());
        }
        
        // LINQ version
        // var total = (from line in _inputLines 
        //     select Regex.Replace(line, "one", "o1ne") into l 
        //     select Regex.Replace(l, "two", "t2wo") into l 
        //     select Regex.Replace(l, "three", "t3hree") into l 
        //     select Regex.Replace(l, "four", "f4our") into l 
        //     select Regex.Replace(l, "five", "f5ive") into l 
        //     select Regex.Replace(l, "six", "s6ix") into l 
        //     select Regex.Replace(l, "seven", "s7even") into l 
        //     select Regex.Replace(l, "eight", "e8ight") into l 
        //     select Regex.Replace(l, "nine", "n9ine") into l 
        //     let rgx = new Regex(@"[^\d]") 
        //     select rgx.Replace(l, "") into l 
        //     select int.Parse(l[0] + l[l.Length - 1].ToString())).Sum();


        return new ValueTask<string>(total.ToString());
    }
}
