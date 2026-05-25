namespace LocalChess.View
{
    partial class TestOnline
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
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(136, 195);
            button1.Name = "button1";
            button1.Size = new Size(176, 60);
            button1.TabIndex = 0;
            button1.Text = "Test Online White";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(489, 195);
            button2.Name = "button2";
            button2.Size = new Size(176, 60);
            button2.TabIndex = 0;
            button2.Text = "Test Online Black";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // TestOnline
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "TestOnline";
            Text = "TestOnline";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}