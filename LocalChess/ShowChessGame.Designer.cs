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
            boardPanel.Location = new Point(3, 76);
            boardPanel.Name = "boardPanel";
            boardPanel.RowCount = 2;
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.Size = new Size(200, 200);
            boardPanel.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(128, 282);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Open";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // winner_label
            // 
            winner_label.AutoSize = true;
            winner_label.Location = new Point(3, 28);
            winner_label.Name = "winner_label";
            winner_label.Size = new Size(45, 15);
            winner_label.TabIndex = 2;
            winner_label.Text = "Winner";
            // 
            // win_condition_label
            // 
            win_condition_label.AutoSize = true;
            win_condition_label.Location = new Point(3, 48);
            win_condition_label.Name = "win_condition_label";
            win_condition_label.Size = new Size(84, 15);
            win_condition_label.TabIndex = 2;
            win_condition_label.Text = "Win Condition";
            // 
            // lobby_name_label
            // 
            lobby_name_label.AutoSize = true;
            lobby_name_label.Location = new Point(3, 8);
            lobby_name_label.Name = "lobby_name_label";
            lobby_name_label.Size = new Size(39, 15);
            lobby_name_label.TabIndex = 2;
            lobby_name_label.Text = "Name";
            // 
            // ShowChessGame
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PapayaWhip;
            Controls.Add(win_condition_label);
            Controls.Add(lobby_name_label);
            Controls.Add(winner_label);
            Controls.Add(button1);
            Controls.Add(boardPanel);
            Name = "ShowChessGame";
            Size = new Size(206, 308);
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
