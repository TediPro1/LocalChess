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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessBoardForm));
            boardPanel = new TableLayoutPanel();
            moveHistoryListBox = new ListBox();
            button1 = new Button();
            button2 = new Button();
            curr_turn_label = new Label();
            white_time = new System.Windows.Forms.Timer(components);
            black_time = new System.Windows.Forms.Timer(components);
            black_timer_label = new Label();
            white_timer_label = new Label();
            white_taken_piece_panel = new FlowLayoutPanel();
            black_taken_piece_panel = new FlowLayoutPanel();
            white_material_label = new Label();
            black_material_label = new Label();
            SuspendLayout();
            // 
            // boardPanel
            // 
            boardPanel.ColumnCount = 1;
            boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            boardPanel.Location = new Point(0, 0);
            boardPanel.Name = "boardPanel";
            boardPanel.RowCount = 1;
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            boardPanel.Size = new Size(782, 753);
            boardPanel.TabIndex = 0;
            // 
            // moveHistoryListBox
            // 
            moveHistoryListBox.FormattingEnabled = true;
            moveHistoryListBox.Location = new Point(781, 150);
            moveHistoryListBox.Margin = new Padding(3, 4, 3, 4);
            moveHistoryListBox.Name = "moveHistoryListBox";
            moveHistoryListBox.Size = new Size(169, 464);
            moveHistoryListBox.TabIndex = 1;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Symbol", 9F);
            button1.Location = new Point(782, 613);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(85, 31);
            button1.TabIndex = 2;
            button1.Text = "<";
            button1.UseVisualStyleBackColor = true;
            button1.Click += TurnBackOneMove;
            // 
            // button2
            // 
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Symbol", 9F);
            button2.Location = new Point(864, 613);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(86, 31);
            button2.TabIndex = 2;
            button2.Text = ">";
            button2.UseVisualStyleBackColor = true;
            button2.Click += TurnForthOneMove;
            // 
            // curr_turn_label
            // 
            curr_turn_label.AutoSize = true;
            curr_turn_label.Location = new Point(785, 12);
            curr_turn_label.Name = "curr_turn_label";
            curr_turn_label.Size = new Size(132, 20);
            curr_turn_label.TabIndex = 3;
            curr_turn_label.Text = "CurrentTurn: White";
            // 
            // black_timer_label
            // 
            black_timer_label.AutoSize = true;
            black_timer_label.Font = new Font("Segoe UI", 18F);
            black_timer_label.Location = new Point(849, 39);
            black_timer_label.Name = "black_timer_label";
            black_timer_label.Size = new Size(89, 41);
            black_timer_label.TabIndex = 5;
            black_timer_label.Text = "00:00";
            // 
            // white_timer_label
            // 
            white_timer_label.AutoSize = true;
            white_timer_label.Font = new Font("Segoe UI", 18F);
            white_timer_label.Location = new Point(849, 703);
            white_timer_label.Name = "white_timer_label";
            white_timer_label.Size = new Size(89, 41);
            white_timer_label.TabIndex = 5;
            white_timer_label.Text = "00:00";
            // 
            // white_taken_piece_panel
            // 
            white_taken_piece_panel.AutoScroll = true;
            white_taken_piece_panel.Location = new Point(782, 88);
            white_taken_piece_panel.Name = "white_taken_piece_panel";
            white_taken_piece_panel.Size = new Size(168, 55);
            white_taken_piece_panel.TabIndex = 6;
            white_taken_piece_panel.WrapContents = false;
            // 
            // black_taken_piece_panel
            // 
            black_taken_piece_panel.AutoScroll = true;
            black_taken_piece_panel.Location = new Point(782, 644);
            black_taken_piece_panel.Name = "black_taken_piece_panel";
            black_taken_piece_panel.Size = new Size(168, 55);
            black_taken_piece_panel.TabIndex = 6;
            black_taken_piece_panel.WrapContents = false;
            // 
            // white_material_label
            // 
            white_material_label.AutoSize = true;
            white_material_label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            white_material_label.Location = new Point(785, 54);
            white_material_label.Name = "white_material_label";
            white_material_label.Size = new Size(0, 23);
            white_material_label.TabIndex = 7;
            // 
            // black_material_label
            // 
            black_material_label.AutoSize = true;
            black_material_label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            black_material_label.Location = new Point(788, 718);
            black_material_label.Name = "black_material_label";
            black_material_label.Size = new Size(0, 23);
            black_material_label.TabIndex = 7;
            // 
            // ChessBoardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(950, 753);
            Controls.Add(black_material_label);
            Controls.Add(white_material_label);
            Controls.Add(black_taken_piece_panel);
            Controls.Add(white_taken_piece_panel);
            Controls.Add(white_timer_label);
            Controls.Add(black_timer_label);
            Controls.Add(curr_turn_label);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(moveHistoryListBox);
            Controls.Add(boardPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ChessBoardForm";
            Text = "Game";
            FormClosed += ChessBoardForm_FormClosed;
            Load += ChessBoardForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel boardPanel;
        private ListBox moveHistoryListBox;
        private Button button1;
        private Button button2;
        private Label curr_turn_label;
        private System.Windows.Forms.Timer white_time;
        private System.Windows.Forms.Timer black_time;
        private Label black_timer_label;
        private Label white_timer_label;
        private FlowLayoutPanel white_taken_piece_panel;
        private FlowLayoutPanel black_taken_piece_panel;
        private Label white_material_label;
        private Label black_material_label;
    }
}
