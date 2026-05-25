namespace LocalChess.View
{
    partial class ChessBoardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessBoardForm));
            boardPanel = new TableLayoutPanel();
            moveHistoryListBox = new ListBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // boardPanel
            // 
            boardPanel.ColumnCount = 1;
            boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            boardPanel.Location = new Point(0, 0);
            boardPanel.Margin = new Padding(3, 2, 3, 2);
            boardPanel.Name = "boardPanel";
            boardPanel.RowCount = 1;
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.Size = new Size(684, 565);
            boardPanel.TabIndex = 0;
            // 
            // moveHistoryListBox
            // 
            moveHistoryListBox.FormattingEnabled = true;
            moveHistoryListBox.ItemHeight = 15;
            moveHistoryListBox.Location = new Point(683, 27);
            moveHistoryListBox.Name = "moveHistoryListBox";
            moveHistoryListBox.Size = new Size(148, 514);
            moveHistoryListBox.TabIndex = 1;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Symbol", 9F);
            button1.Location = new Point(684, 542);
            button1.Name = "button1";
            button1.Size = new Size(74, 23);
            button1.TabIndex = 2;
            button1.Text = "<";
            button1.UseVisualStyleBackColor = true;
            button1.Click += TurnBackOneMove;
            // 
            // button2
            // 
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Symbol", 9F);
            button2.Location = new Point(756, 542);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = ">";
            button2.UseVisualStyleBackColor = true;
            button2.Click += TurnForthOneMove;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(687, 9);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 3;
            label1.Text = "CurrentTurn";
            // 
            // ChessBoardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(831, 565);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(moveHistoryListBox);
            Controls.Add(boardPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "ChessBoardForm";
            Text = "Game";
            FormClosed += ChessBoardForm_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel boardPanel;
        private ListBox moveHistoryListBox;
        private Button button1;
        private Button button2;
        private Label label1;
    }
}