using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Logic;

namespace UI
{
    public partial class MainForm : Form
    {
        private Sudoku _sudoku;

        private int _squareSize;
        private int _numbersSize;

        private int _selectedRow;
        private int _selectedCol;

        private bool _highlightSelectedSquare;

        private HashSet<Square> _invalidSquares;
        private Dictionary<Keys, int> _keys;

        private Dictionary<Square, int[]> _hints;

        public MainForm()
        {
            InitializeComponent();

            _squareSize = grid.Width / Sudoku.Rows;
            _numbersSize = _squareSize - 30;

            _keys = new Dictionary<Keys, int>()
                        {
                            {Keys.NumPad1, 1},
                            {Keys.NumPad2, 2},
                            {Keys.NumPad3, 3},
                            {Keys.NumPad4, 4},
                            {Keys.NumPad5, 5},
                            {Keys.NumPad6, 6},
                            {Keys.NumPad7, 7},
                            {Keys.NumPad8, 8},
                            {Keys.NumPad9, 9},
                            {Keys.NumPad0, 0},
                        };
        }

        /// <summary>
        /// For each square of the sudoku, I get all the valid numbers that can be put inside, which means, those numbers who
        /// won't put the sudoku in an invalid state.
        /// </summary>
        private void UpdateHints()
        {
            for (int row = 0; row < Sudoku.Rows; row++)
            {
                for (int col = 0; col < Sudoku.Columns; col++)
                {
                    Square square = _sudoku[row, col];

                    if (square.Type == SquareType.Empty)
                    {
                        _hints[square] = _sudoku.GetValidNumbers(square);
                    }
                }
            }
        }

        private void OnGridPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            
            DrawGrid(e.Graphics);
            DrawSudoku(e.Graphics);
            DrawHighlightedSquares(e.Graphics);
            DrawHints(e.Graphics);
        }

        private void DrawHints(Graphics g)
        {
            foreach (var hint in _hints)
            {
                int x = hint.Key.Column*_squareSize;
                int y = hint.Key.Row*_squareSize;

                for (int i = 0; i < hint.Value.Length; i++)
                {
                    int validNumber = hint.Value[i];

                    g.DrawString(validNumber.ToString(), DefaultFont, Brushes.Black, x, y);
                    x += 11;

                    if (i == 3)
                    {
                        x = hint.Key.Column*_squareSize;
                        y += _squareSize - 13;
                    }
                }
            }
        }

        private void DrawHighlightedSquares(Graphics g)
        {
            if (_highlightSelectedSquare)
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(100, Color.YellowGreen)))
                {
                    g.FillRectangle(brush, _selectedCol*_squareSize, _selectedRow*_squareSize, _squareSize, _squareSize);
                }
            }

            using (Brush brush = new SolidBrush(Color.FromArgb(100, Color.Red)))
            {
                foreach (var invalidSquare in _invalidSquares)
                {
                    g.FillRectangle(brush, invalidSquare.Column*_squareSize, invalidSquare.Row*_squareSize, _squareSize, _squareSize);
                }
            }
        }   

        private void DrawSudoku(Graphics g)
        {
            using (Font font = new Font("Verdana", _numbersSize, FontStyle.Bold))
            {
                for (int row = 0; row < Sudoku.Rows; row++)
                {
                    for (int col = 0; col < Sudoku.Columns; col++)
                    {
                        string number = _sudoku[row, col].ToString();
                        switch (_sudoku[row, col].Type)
                        {
                            case SquareType.FixedNumber:
                                g.DrawString(number.ToString(), font, Brushes.DarkBlue, col*_squareSize + _squareSize/4, row*_squareSize + _squareSize/4);
                                break;
                            case SquareType.NormalNumber:
                                g.DrawString(number.ToString(), font, Brushes.DarkSeaGreen, col*_squareSize + _squareSize/4, row*_squareSize + _squareSize/4);
                                break;
                        }
                    }
                }
            }
        }

        private void DrawGrid(Graphics g)
        {
            using (Pen pen = new Pen(Brushes.Black))
            using (Pen pen2 = new Pen(Brushes.Black, 4))
            {
                for (int i = 0; i < 10; i++)
                {
                    Pen p = pen;

                    if (i % 3 == 0)
                    {
                        p = pen2;
                    }
                    
                    g.DrawLine(p, i*_squareSize, 0, i*_squareSize, grid.Height);
                    g.DrawLine(p, 0, i*_squareSize, grid.Width, i*_squareSize);
                }
            }
        }

        private void OnGridKeyUp(object sender, KeyEventArgs e)
        {
            if (_keys.ContainsKey(e.KeyCode))
            {
                DoMove(_keys[e.KeyCode]);
            }
        }

        /// <summary>
        /// Put a number in the sudoku. If the parameter number is 0, then I clear the square.
        /// </summary>
        /// <param name="number"></param>
        private void DoMove(int number)
        {
            if (_sudoku[_selectedRow, _selectedCol].Type != SquareType.FixedNumber)
            {
                if (number != 0)
                {
                    Square[] invalidSquares;

                    /* If the move isn't valid, I add the square to the list of invalid squares. I also
                     * add all the squares that got invalid because of the move */
                    if (!_sudoku.IsValidMove(_selectedRow, _selectedCol, number, out invalidSquares))
                    {
                        _invalidSquares.Add(_sudoku[_selectedRow, _selectedCol]);

                        foreach (Square invalidSquare in invalidSquares.Where(s => s.Type != SquareType.FixedNumber))
                        {
                            _invalidSquares.Add(invalidSquare);
                        }
                    }
                    else
                    {
                        _invalidSquares.Remove(_sudoku[_selectedRow, _selectedCol]);
                    }
                    _sudoku.Put(_selectedRow, _selectedCol, number);
                }
                else
                {
                    _sudoku.Clear(_selectedRow, _selectedCol);
                    _invalidSquares.Remove(_sudoku[_selectedRow, _selectedCol]);
                }

                /* After making a move, there could be some squares that were invalid, but now turn to be valid. I need to 
                 * scan the list of invalid squares and test each one to see which one got valid.
                 * Of course, the squares that can now turn to be valid, would be only those who are in the same row, column or box
                 * of the move just made. Nevertheless, I check all squares for simplicity */
                List<Square> nowValidSquares = _invalidSquares.Where(s => _sudoku.IsValidMove(s)).ToList();
                nowValidSquares.ForEach(s => _invalidSquares.Remove(s));

                /* After making a move, I need to clear the hints for the current square. Also, I need to 
                 * update the hints for the others squares.
                 * Same reasoning that before... the hints affected by the move would be those in the same row, column or box, but I
                 * check them all */
                _hints.Remove(_sudoku[_selectedRow, _selectedCol]);
                UpdateHints();

                grid.Invalidate();

                /*
                if (_sudoku.IsFull() && _invalidSquares.Count == 0)
                {
                    MessageBox.Show("COMPLETED!!!!");
                }*/
            }
        }

        private void OnGridMouseMove(object sender, MouseEventArgs e)
        {
            int newSelectedCol = e.X/_squareSize;
            int newSelectedRow = e.Y/_squareSize;

            if ((newSelectedCol != _selectedCol || newSelectedRow != _selectedRow))
            {
                _selectedCol = newSelectedCol;
                _selectedRow = newSelectedRow;

                _highlightSelectedSquare = _sudoku[newSelectedRow, newSelectedCol].Type != SquareType.FixedNumber;

                grid.Invalidate();
            }
        }

        private void OnOpenSudokuMenuClick(object sender, EventArgs e)
        {
            using (LoadSudokuForm loadSudokuForm = new LoadSudokuForm())
            {
                if (loadSudokuForm.ShowDialog() == DialogResult.OK)
                {
                    LoadSudoku(loadSudokuForm.SelectedSudoku);

                    statusLabel.Text = string.Format("Collection: {0}, Sudoku: {1}", loadSudokuForm.SelectedCollection, loadSudokuForm.SelectedSudokuIndex);

                    (grid as Control).KeyUp += OnGridKeyUp;
                    grid.MouseMove += OnGridMouseMove;
                    grid.Paint += OnGridPaint;
                }
            }
        }

        private void LoadSudoku(string sudoku)
        {
            _sudoku = new Sudoku(sudoku);
            _invalidSquares = new HashSet<Square>();

            _hints = new Dictionary<Square, int[]>();
            UpdateHints();

            grid.Select();
            grid.Invalidate();
        }

        private void OnSolveSudokuMenuClick(object sender, EventArgs e)
        {
            _invalidSquares.Clear();
            _hints.Clear();

            string previousStatusLabelText = statusLabel.Text;
            statusLabel.Text = "Solving...";
            statusBar.Refresh();

            _sudoku.Solve();

            statusLabel.Text = previousStatusLabelText;

            grid.Invalidate();
        }

        private void OnExitMenuClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
