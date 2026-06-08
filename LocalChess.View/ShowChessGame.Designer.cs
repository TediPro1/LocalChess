namespace LocalChess.View
{
    partial class ShowChessGame
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            boardPanel = new TableLayoutPanel();
            button1 = new Button();
            winner_label = new Label();
            win_condition_label = new Label();
            lobby_name_label = new Label();
            SuspendLayout();
            // 
            // boardPanel
            // 
            boardPanel.ColumnCount = 2;
            boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            boardPanel.Location = new Point(3, 101);
            boardPanel.Margin = new Padding(3, 4, 3, 4);
            boardPanel.Name = "boardPanel";
            boardPanel.RowCount = 2;
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.Size = new Size(229, 267);
            boardPanel.TabIndex = 0;
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderColor = Color.Black;
            button1.FlatAppearance.BorderSize = 2;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 255, 192);
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.Black;
            button1.Location = new Point(144, 375);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(86, 31);
            button1.TabIndex = 1;
            button1.Text = "Open";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // winner_label
            // 
            winner_label.AutoSize = true;
            winner_label.BorderStyle = BorderStyle.FixedSingle;
            winner_label.Location = new Point(3, 37);
            winner_label.Name = "winner_label";
            winner_label.Size = new Size(58, 22);
            winner_label.TabIndex = 2;
            winner_label.Text = "Winner";
            // 
            // win_condition_label
            // 
            win_condition_label.AutoSize = true;
            win_condition_label.BorderStyle = BorderStyle.FixedSingle;
            win_condition_label.Location = new Point(3, 64);
            win_condition_label.Name = "win_condition_label";
            win_condition_label.Size = new Size(106, 22);
            win_condition_label.TabIndex = 2;
            win_condition_label.Text = "Win Condition";
            // 
            // lobby_name_label
            // 
            lobby_name_label.AutoSize = true;
            lobby_name_label.BorderStyle = BorderStyle.FixedSingle;
            lobby_name_label.Location = new Point(3, 11);
            lobby_name_label.Name = "lobby_name_label";
            lobby_name_label.Size = new Size(51, 22);
            lobby_name_label.TabIndex = 2;
            lobby_name_label.Text = "Name";
            // 
            // ShowChessGame
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PapayaWhip;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(win_condition_label);
            Controls.Add(lobby_name_label);
            Controls.Add(winner_label);
            Controls.Add(button1);
            Controls.Add(boardPanel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ShowChessGame";
            Size = new Size(233, 409);
            Load += ShowChessGame_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel boardPanel;
        private Button button1;
        private Label winner_label;
        private Label win_condition_label;
        private Label lobby_name_label;
    }
}
