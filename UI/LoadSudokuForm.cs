using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace UI
{
    public partial class LoadSudokuForm : Form
    {
        public string SelectedCollection { get; private set; }
        public int SelectedSudokuIndex { get; private set; }
        public string SelectedSudoku { get; private set; }
        
        private Dictionary<string, string[]> _collections;
        
        public LoadSudokuForm()
        {
            InitializeComponent();

            LoadCollections();
        }

        private void LoadCollections()
        {
            _collections = new Dictionary<string, string[]>();
            
            foreach (string file in Directory.EnumerateFiles("Sudokus"))
            {
                string collectionName = Path.GetFileNameWithoutExtension(file);
                _collections[collectionName] = File.ReadAllLines(file);

                ListViewItem item = new ListViewItem(new[] {collectionName, _collections[collectionName][0]});
                collectionsList.Items.Add(item);
            }
        }

        private void OnCollectionsListSelectedIndexChanged(object sender, EventArgs e)
        {
            sudokusList.Items.Clear();

            string selectedCollection = collectionsList.SelectedItems[0].Text;

            for (int i = 1; i < _collections[selectedCollection].Length; i++)
            {
                ListViewItem item = new ListViewItem(new[] {i.ToString(), "N"});
                sudokusList.Items.Add(item);
            }
        }

        private void OnSudokusListMouseDoubleClick(object sender, MouseEventArgs e)
        {
            string selectedCollection = collectionsList.SelectedItems[0].Text;
            int selectedSudokuId = int.Parse(sudokusList.GetItemAt(e.X, e.Y).Text);

            SelectedCollection = selectedCollection;
            SelectedSudokuIndex = selectedSudokuId;
            SelectedSudoku = _collections[selectedCollection][selectedSudokuId];

            DialogResult = DialogResult.OK;
        }
    }
}
