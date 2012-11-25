using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class Sudoku
    {
        public static int Rows = 9;
        public static int Columns = 9;

        private readonly Square[,] _grid;
        private List<int> _fixedNumbersPosition;
        private static readonly Tuple<int, int>[] Boxes;
        
        static Sudoku()
        {
            Boxes = new Tuple<int, int>[9];
            int boxIndex = 0;

            /*  I save for each of the possible boxes (numbered from 0 to 8) the row and column 
             *  where it starts. For instance, for the box 5, the row and column will be: [3,6] */
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Boxes[boxIndex] = new Tuple<int, int>(row * 3, col * 3);
                    boxIndex++;
                }
            }
        }

        public Sudoku(string sudoku)
        {
            _grid = new Square[Rows, Columns];
            PopulateSudokuGrid(sudoku);
        }

        public void Put(int row, int col, int value)
        {
            _grid[row, col].Type = SquareType.NormalNumber;
            _grid[row, col].Value = value;
        }

        public void Clear(int row, int col)
        {
            _grid[row, col].Value = 0;
            _grid[row, col].Type = SquareType.Empty;
        }

        public int[] GetValidNumbers(Square square)
        {
            return GetValidNumbers(square.Row, square.Column);
        }

        public int[] GetValidNumbers(int row, int col)
        {
            HashSet<int> validNumbers = new HashSet<int>(Enumerable.Range(1, 9));

            for (int i = 0; i < Rows; i++)
            {
                validNumbers.Remove(_grid[row, i].Value);
                validNumbers.Remove(_grid[i, col].Value);
            }

            int boxIndex = GetBoxIndex(row, col);
            int boxStartingRow = Boxes[boxIndex].Item1;
            int boxStartingColumn = Boxes[boxIndex].Item2;

            for (int i = boxStartingRow; i < boxStartingRow + 3; i++)
            {
                for (int j = boxStartingColumn; j < boxStartingColumn + 3; j++)
                {
                    validNumbers.Remove(_grid[i, j].Value);
                }
            }

            return validNumbers.ToArray();
        }

        public bool IsValidMove(Square square)
        {
            Square[] invalidSquares;
            return IsValidMove(square.Row, square.Column, square.Value, out invalidSquares);
        }

        public bool IsValidMove(int row, int col, int number, out Square[] invalidSquares)
        {
            HashSet<Square> temp = new HashSet<Square>();

            /* I set temporary the square value to 0, so I won't have a match when the ifs below test the square
             * against itself. I'll restore the value before I return from the method */
            _grid[row, col].Value = 0;

            // I check if in the given row and column I have the given number
            for (int i = 0; i < Columns; i++)
            {
                if (_grid[row, i].Value == number)
                {
                    temp.Add(_grid[row, i]);
                }
                else if (_grid[i, col].Value == number)
                {
                    temp.Add(_grid[i, col]);
                }
            }
            
            // Since I have previously saved the row and column where every box starts, it's easy to check the 9 squares of the box
            int boxIndex = GetBoxIndex(row, col);
            int boxStartingRow = Boxes[boxIndex].Item1;
            int boxStartingColumn = Boxes[boxIndex].Item2;

            for (int i = boxStartingRow; i < boxStartingRow + 3; i++)
            {
                for (int j = boxStartingColumn; j < boxStartingColumn + 3; j++)
                {
                    if (_grid[i, j].Value == number)
                    {
                        temp.Add(_grid[i, j]);
                    }
                }
            }

            _grid[row, col].Value = number;

            invalidSquares = temp.ToArray();
            return invalidSquares.Length == 0;
        }
     
        public bool IsFull()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (_grid[row, col].Type == SquareType.Empty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool Solve()
        {
            ClearSudoku();
            return Solve(0);
        }

        public void ClearSudoku()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (_grid[row, col].Type != SquareType.FixedNumber)
                    {
                        _grid[row, col].Value = 0;
                        _grid[row, col].Type = SquareType.Empty;
                    }
                }
            }
        }

        public Square this[int row, int column]
        {
            get { return _grid[row, column]; }
        }

        private void PopulateSudokuGrid(string sudoku)
        {
            _fixedNumbersPosition = new List<int>();

            int i = 0;
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    _grid[row, col] = new Square()
                                          {
                                              Type = SquareType.Empty,
                                              Row = row,
                                              Column = col,
                                              Value = sudoku[i] == '.' ? 0 : int.Parse(sudoku[i].ToString())
                                          };

                    if (_grid[row, col].Value != 0)
                    {
                        _grid[row, col].Type = SquareType.FixedNumber;
                        _fixedNumbersPosition.Add(row * Columns + col);
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// Given a row and a column, I calculate and return the box index that they belong to.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static int GetBoxIndex(int row, int col)
        {
            return (row/3*3) + col/3;
        }

        private bool Solve(int squareIndex)
        {
            /* If at any point I move on to square 81 (beyond the last valid square 80),
             * it means that I filled the sudoku, and if I filled the sudoku it means that all squares filled
             * are valid, so the sudoku is solved */
            if (squareIndex == Rows * Columns)
            {
                return true;
            }

            bool solved = false;

            int row = squareIndex / 9;
            int col = squareIndex % 9;

            /* If the current square is a starting number, I skip it over */
            if (_grid[row, col].Type == SquareType.FixedNumber)
            {
                solved = Solve(squareIndex + 1);
            }
            else
            {
                int i = 1;

                /* Simple backtracking here: for every number, I made a recursive call
                 * to see if the number solved the sudoku. If it's not, I try the next possible number. Otherwise, the sudoku is solved */
                while (i < 10 && !solved)
                {
                    if (IsValidMove(row, col, i))
                    {
                        _grid[row, col].Value = i;
                        _grid[row, col].Type = SquareType.NormalNumber;
                        solved = Solve(squareIndex + 1);
                    }
                    i++;
                }

                if (!solved)
                {
                    _grid[row, col].Value = 0;
                    _grid[row, col].Type = SquareType.Empty;
                }
            }
            return solved;
        }

        /// <summary>
        /// Overload that performs faster, and it's intended to be used in the Solve method.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool IsValidMove(int row, int col, int number)
        {
            // I check if in the given row and column I have the given number
            for (int i = 0; i < Columns; i++)
            {
                if (_grid[row, i].Value == number || _grid[i, col].Value == number)
                {
                    return false;
                }
            }

            // Since I have previously saved the row and column where every box starts, it's easy to check the 9 squares of the box
            int boxIndex = GetBoxIndex(row, col);
            int boxStartingRow = Boxes[boxIndex].Item1;
            int boxStartingColumn = Boxes[boxIndex].Item2;

            for (int i = boxStartingRow; i < boxStartingRow + 3; i++)
            {
                for (int j = boxStartingColumn; j < boxStartingColumn + 3; j++)
                {
                    if (_grid[i, j].Value == number)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}
