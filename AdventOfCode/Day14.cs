using System.Text.RegularExpressions;

namespace AdventOfCode;

// Runtime Total: ~Xms
// Setup: ~Xms
// Part 1: ~Xms
// Part 2: ~Xms
public class Day14 : BaseDay
{
    private List<string> _input;
    private char[,] _grid;
    private char[,] _grid2;
    private int rows, cols;
    
    public Day14() 
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        rows = _input.Count;
        cols = _input[0].Length;
        _grid = new char[cols, rows];

        for (var y = 0; y < rows; y++)
        {
            for(var x = 0; x < cols; x++)
                _grid[x, y] = _input[y][x];
        }
        
        _grid2 = _grid.Clone() as char[,];
        
        printGrid();
    }
    
    private void printGrid()
    {
        for (var y = 0; y < rows; y++)
        {
            for(var x = 0; x < cols; x++)
                Console.Write(_grid[x, y]);
            Console.WriteLine("");
        }
        Console.WriteLine("");
    }
    
    private int CalculateTotalLoad()
    {
        int totalLoad = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (_grid[col, row] == 'O')
                {
                    totalLoad += rows - row;
                }
            }
        } 
        return totalLoad;
    }
    
    void MoveRocksNorth()
    {
        for (int col = 0; col < cols; col++)
        {
            int firstEmptyRow = 0;
            for (int row = 0; row < rows; row++)
            {
                if (_grid[col, row] == 'O')
                {
                    if (row != firstEmptyRow)
                    {
                        _grid[col, firstEmptyRow] = 'O';
                        _grid[col, row] = '.';
                    }
                    firstEmptyRow++;
                }
                else if (_grid[col, row] == '#')
                {
                    firstEmptyRow = row + 1;
                }
            }
        }
    }
    
    void MoveRocksSouth()
    {
        for (int col = 0; col < cols; col++)
        {
            int firstEmptyRow = rows - 1;
            for (int row = rows - 1; row >= 0; row--)
            {
                if (_grid[col, row] == 'O')
                {
                    if (row != firstEmptyRow)
                    {
                        _grid[col, firstEmptyRow] = 'O';
                        _grid[col, row] = '.';
                    }
                    firstEmptyRow--;
                }
                else if (_grid[col, row] == '#')
                {
                    firstEmptyRow = row - 1;
                }
            }
        }
    }
    
    void MoveRocksEast()
    {
        for (int row = 0; row < rows; row++)
        {
            int firstEmptyCol = cols - 1;
            for (int col = cols - 1; col >= 0; col--)
            {
                if (_grid[col, row] == 'O')
                {
                    if (col != firstEmptyCol)
                    {
                        _grid[firstEmptyCol, row] = 'O';
                        _grid[col, row] = '.';
                    }
                    firstEmptyCol--;
                }
                else if (_grid[col, row] == '#')
                {
                    firstEmptyCol = col - 1;
                }
            }
        }
    }
    
    void MoveRocksWest()
    {
        for (int row = 0; row < rows; row++)
        {
            int firstEmptyCol = 0;
            for (int col = 0; col < cols; col++)
            {
                if (_grid[col, row] == 'O')
                {
                    if (col != firstEmptyCol)
                    {
                        _grid[firstEmptyCol, row] = 'O';
                        _grid[col, row] = '.';
                    }
                    firstEmptyCol++;
                }
                else if (_grid[col, row] == '#')
                {
                    firstEmptyCol = col + 1;
                }
            }
        }
    }
    
    private void MoveRocks()
    {
        MoveRocksNorth();
        MoveRocksWest();
        MoveRocksSouth();
        MoveRocksEast();
    }
    

    public override ValueTask<string> Solve_1()
    {
        MoveRocksNorth();
        printGrid();
       
        return new (CalculateTotalLoad().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        _grid = _grid2.Clone() as char[,];

        for (int i = 1; i <= 1000; i++) // Same results every 1000 iterations
        {
            MoveRocks();
            // if(i%1000 == 0) Console.WriteLine($"{i}: {CalculateTotalLoad()}");
        }
        
        return new (CalculateTotalLoad().ToString());
    } 
}