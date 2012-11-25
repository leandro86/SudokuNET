namespace UI
{
    partial class LoadSudokuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.collectionsList = new System.Windows.Forms.ListView();
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sudokusList = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.solved = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.collectionsList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 301);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Collections";
            // 
            // collectionsList
            // 
            this.collectionsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.description});
            this.collectionsList.FullRowSelect = true;
            this.collectionsList.Location = new System.Drawing.Point(7, 22);
            this.collectionsList.Name = "collectionsList";
            this.collectionsList.Size = new System.Drawing.Size(520, 273);
            this.collectionsList.TabIndex = 0;
            this.collectionsList.UseCompatibleStateImageBehavior = false;
            this.collectionsList.View = System.Windows.Forms.View.Details;
            this.collectionsList.SelectedIndexChanged += new System.EventHandler(this.OnCollectionsListSelectedIndexChanged);
            // 
            // name
            // 
            this.name.Text = "Name";
            this.name.Width = 129;
            // 
            // description
            // 
            this.description.Text = "Description";
            this.description.Width = 384;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sudokusList);
            this.groupBox2.Location = new System.Drawing.Point(553, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 301);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sudokus";
            // 
            // sudokusList
            // 
            this.sudokusList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.solved});
            this.sudokusList.FullRowSelect = true;
            this.sudokusList.Location = new System.Drawing.Point(7, 22);
            this.sudokusList.Name = "sudokusList";
            this.sudokusList.Size = new System.Drawing.Size(140, 273);
            this.sudokusList.TabIndex = 1;
            this.sudokusList.UseCompatibleStateImageBehavior = false;
            this.sudokusList.View = System.Windows.Forms.View.Details;
            this.sudokusList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnSudokusListMouseDoubleClick);
            // 
            // id
            // 
            this.id.Text = "Id";
            this.id.Width = 51;
            // 
            // solved
            // 
            this.solved.Text = "Solved";
            this.solved.Width = 81;
            // 
            // LoadSudokuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 319);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "LoadSudokuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LoadSudokuForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView collectionsList;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader description;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView sudokusList;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader solved;
    }
}