namespace LocalChess.View
{
    partial class Promote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Promote));
            queen_btn = new Button();
            rook_btn = new Button();
            bishop_btn = new Button();
            knight_btn = new Button();
            SuspendLayout();
            // 
            // queen_btn
            // 
            queen_btn.BackColor = Color.Transparent;
            queen_btn.BackgroundImageLayout = ImageLayout.Stretch;
            queen_btn.FlatAppearance.BorderColor = Color.Silver;
            queen_btn.FlatAppearance.BorderSize = 6;
            queen_btn.FlatStyle = FlatStyle.Flat;
            queen_btn.Location = new Point(390, 11);
            queen_btn.Name = "queen_btn";
            queen_btn.Size = new Size(100, 100);
            queen_btn.TabIndex = 0;
            queen_btn.UseVisualStyleBackColor = false;
            queen_btn.Click += queen_btn_Click;
            // 
            // rook_btn
            // 
            rook_btn.BackColor = Color.Transparent;
            rook_btn.BackgroundImageLayout = ImageLayout.Stretch;
            rook_btn.FlatAppearance.BorderColor = Color.Silver;
            rook_btn.FlatAppearance.BorderSize = 6;
            rook_btn.FlatStyle = FlatStyle.Flat;
            rook_btn.Location = new Point(264, 12);
            rook_btn.Name = "rook_btn";
            rook_btn.Size = new Size(100, 100);
            rook_btn.TabIndex = 1;
            rook_btn.UseVisualStyleBackColor = false;
            rook_btn.Click += rook_btn_Click;
            // 
            // bishop_btn
            // 
            bishop_btn.BackColor = Color.Transparent;
            bishop_btn.BackgroundImageLayout = ImageLayout.Stretch;
            bishop_btn.FlatAppearance.BorderColor = Color.Silver;
            bishop_btn.FlatAppearance.BorderSize = 6;
            bishop_btn.FlatStyle = FlatStyle.Flat;
            bishop_btn.Location = new Point(138, 12);
            bishop_btn.Name = "bishop_btn";
            bishop_btn.Size = new Size(100, 100);
            bishop_btn.TabIndex = 2;
            bishop_btn.UseVisualStyleBackColor = false;
            bishop_btn.Click += bishop_btn_Click;
            // 
            // knight_btn
            // 
            knight_btn.BackColor = Color.Transparent;
            knight_btn.BackgroundImageLayout = ImageLayout.Stretch;
            knight_btn.FlatAppearance.BorderColor = Color.Silver;
            knight_btn.FlatAppearance.BorderSize = 6;
            knight_btn.FlatStyle = FlatStyle.Flat;
            knight_btn.Location = new Point(12, 12);
            knight_btn.Name = "knight_btn";
            knight_btn.Size = new Size(100, 100);
            knight_btn.TabIndex = 3;
            knight_btn.UseVisualStyleBackColor = false;
            knight_btn.Click += knight_btn_Click;
            // 
            // Promote
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(502, 123);
            ControlBox = false;
            Controls.Add(knight_btn);
            Controls.Add(bishop_btn);
            Controls.Add(rook_btn);
            Controls.Add(queen_btn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Promote";
            Text = "Promotion";
            ResumeLayout(false);
        }

        #endregion

        private Button queen_btn;
        private Button rook_btn;
        private Button bishop_btn;
        private Button knight_btn;
    }
}