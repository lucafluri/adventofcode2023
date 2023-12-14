using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class uDay13 : BaseDay
{
    private List<List<string>> _input;
    private List<List<uint>> _grids_rows = new();
    private List<List<uint>> _grids_cols = new();
    
    
    public uDay13() 
    {
        _input = File.ReadAllText(InputFilePath).Split("\n\n").Select(x => x.Split("\n").ToList()).ToList();
        
        //Read-in as # = true, . = false bool list
        foreach (var grid in _input)
        {
            var height = grid.Count;
            var width = grid[0].Length;
            _grids_rows.Add(new List<uint>());
            foreach (var y in grid)
            {
                var row = new bool[width];
                for(var i = 0; i < width; i++) 
                    row[i] = y[i] == '#';
                _grids_rows[^1].Add(BoolArrayToInt(row));
            }
            
            _grids_cols.Add(new List<uint>());
            for (var x = 0; x < width; x++)
            {
                var col = new bool[height];
                for (var y = 0; y < height; y++)
                    col[y] = grid[y][x] == '#';
                _grids_cols[^1].Add(BoolArrayToInt(col));  
            }
        }
    }
    
    
    //Stolen from the Internet
    private uint BoolArrayToInt(bool[] bits){
        if(bits.Length > 32) throw new ArgumentException("Can only fit 32 bits in a uint");
 
        uint r = 0;
        for(int i = 0; i < bits.Length; i++) if(bits[i]) r |= (uint)(1 << (bits.Length- i - 1)); 
        return r;
    }

    private static int FindMirrorCenter(List<uint> list)
    {
        var center = 0;
        var left = new List<uint>(){};
        var right = new List<uint>(){};
        var start = 0;
        var length = 1; 
        
        for (var i = 0; i < list.Count-1; i++)
        {
            if(i < list.Count / 2)
            {
                length = i - start + 1;
            }
            else
            {
                length = list.Count - i - 1;
                start = i - length + 1;
            }
            
            left = list.Slice(start, length);
            right = list.Slice(start + length, length);
            right.Reverse();   
            
            if (left.SequenceEqual(right)) 
            {
                center = i+1;
                break;
            }
        }
        return center != 0 ? center  : 0;
    }
    
    private int FindMirrorAndCalcSum(List<List<uint>> rows, List<List<uint>> cols)
    {
        var rowIDSum = 0;
        var colIDSum = 0; 
        
        for(var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            var rowDist = FindMirrorCenter(row);
            rowIDSum += rowDist; 
        } 
        
        for(var i = 0; i < cols.Count; i++)
        {
            var col = cols[i];
            var colDist = FindMirrorCenter(col);
            colIDSum += colDist;
        }
       
        return colIDSum+100*rowIDSum; // 30535
    }

    private int GetNewGridSum(List<uint> originalRows, List<uint> originalCols)
    {
        int totalSum = 0;
    
        for (int y = 0; y < originalRows.Count; y++)
        {
            for (int x = 0; x < originalCols.Count; x++)
            {
                var tempRows = new List<uint>(originalRows);
                var tempCols = new List<uint>(originalCols);
                UpdateGridValues(tempRows, tempCols, x, y);
    
                int oldRowSum = FindMirrorCenter(originalRows);
                int oldColSum = FindMirrorCenter(originalCols);
                int newRowSum = FindMirrorCenter(tempRows);
                int newColSum = FindMirrorCenter(tempCols);
            
                // Check if new mirror center is different from the old one
                if ((newRowSum != oldRowSum && newRowSum > 0) || (newColSum != oldColSum && newColSum > 0))
                {
                    totalSum += newColSum + 100 * newRowSum;
                    return totalSum;
                }
            }
        }
        return totalSum;
    }
    
    private void UpdateGridValues(List<uint> rows, List<uint> cols, int x, int y)
    {
        uint powerForRow = (uint)Math.Pow(2, cols.Count - x - 1);
        uint powerForCol = (uint)Math.Pow(2, rows.Count - y - 1);

        var binary = Convert.ToString(rows[y], 2).PadLeft(cols.Count, '0').ToCharArray();

        switch (binary[x])
        {
            case '1':
                cols[x] -= powerForCol;
                rows[y] -= powerForRow;
                break;
            case '0':
                cols[x] += powerForCol;
                rows[y] += powerForRow;
                break;
        }
    }

  
    public override ValueTask<string> Solve_1()
    {
        return new (FindMirrorAndCalcSum(_grids_rows, _grids_cols).ToString()); // 30535
        // WRONG: 29823
    }

    public override ValueTask<string> Solve_2()
    {
        //Check all rows/colums nums if off by 2^x and replace change accordingly
        // int = 32 bits => ... 2^31, 2^30, 2^29, ... 2^2, 2^1, 2^0
        
        var _gr = _grids_rows.Select(x => x.ToList()).ToList();
        var _gc = _grids_cols.Select(x => x.ToList()).ToList();

        var newSum = 0;

        for (var i = 0; i < _gr.Count; i++)
        {
            var row = _gr[i];
            var col = _gc[i];
            newSum += GetNewGridSum(row, col);
        }

        return new (newSum.ToString()); // 30844
        // WRONG:  L-20091, H-33882, L-28475, H-35102, H-31763, L-29639, L-30036 L-30636
    } 
}