namespace LocalChess
{
    partial class Main_menu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_menu));
            menu = new TabControl();
            main_page = new TabPage();
            button2 = new Button();
            button1 = new Button();
            join_game_page = new TabPage();
            label1 = new Label();
            join_game_pass = new TextBox();
            button4 = new Button();
            button3 = new Button();
            listBox1 = new ListBox();
            new_game_page = new TabPage();
            label3 = new Label();
            label2 = new Label();
            new_game_pass = new TextBox();
            new_game_name = new TextBox();
            button5 = new Button();
            button6 = new Button();
            menu.SuspendLayout();
            main_page.SuspendLayout();
            join_game_page.SuspendLayout();
            new_game_page.SuspendLayout();
            SuspendLayout();
            // 
            // menu
            // 
            menu.Controls.Add(main_page);
            menu.Controls.Add(join_game_page);
            menu.Controls.Add(new_game_page);
            menu.Location = new Point(-4, -22);
            menu.Margin = new Padding(3, 2, 3, 2);
            menu.Name = "menu";
            menu.SelectedIndex = 0;
            menu.Size = new Size(710, 364);
            menu.TabIndex = 0;
            // 
            // main_page
            // 
            main_page.BackgroundImage = View.Properties.Resources.ChessBG;
            main_page.BackgroundImageLayout = ImageLayout.Stretch;
            main_page.Controls.Add(button2);
            main_page.Controls.Add(button1);
            main_page.Location = new Point(4, 24);
            main_page.Margin = new Padding(3, 2, 3, 2);
            main_page.Name = "main_page";
            main_page.Padding = new Padding(3, 2, 3, 2);
            main_page.Size = new Size(702, 336);
            main_page.TabIndex = 0;
            main_page.Text = "tabPage1";
            main_page.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(395, 235);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(115, 34);
            button2.TabIndex = 0;
            button2.Text = "Join Game";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(192, 235);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(115, 34);
            button1.TabIndex = 0;
            button1.Text = "New Game";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // join_game_page
            // 
            join_game_page.BackgroundImage = View.Properties.Resources.ChessBG;
            join_game_page.BackgroundImageLayout = ImageLayout.Stretch;
            join_game_page.Controls.Add(label1);
            join_game_page.Controls.Add(join_game_pass);
            join_game_page.Controls.Add(button4);
            join_game_page.Controls.Add(button3);
            join_game_page.Controls.Add(listBox1);
            join_game_page.Location = new Point(4, 24);
            join_game_page.Margin = new Padding(3, 2, 3, 2);
            join_game_page.Name = "join_game_page";
            join_game_page.Padding = new Padding(3, 2, 3, 2);
            join_game_page.Size = new Size(702, 336);
            join_game_page.TabIndex = 1;
            join_game_page.Text = "tabPage2";
            join_game_page.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Transparent;
            label1.Location = new Point(346, 140);
            label1.Name = "label1";
            label1.Size = new Size(57, 15);
            label1.TabIndex = 3;
            label1.Text = "Password";
            // 
            // join_game_pass
            // 
            join_game_pass.Location = new Point(346, 157);
            join_game_pass.Margin = new Padding(3, 2, 3, 2);
            join_game_pass.Name = "join_game_pass";
            join_game_pass.Size = new Size(204, 23);
            join_game_pass.TabIndex = 2;
            // 
            // button4
            // 
            button4.Location = new Point(596, 306);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(102, 31);
            button4.TabIndex = 1;
            button4.Text = "Back";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Location = new Point(346, 306);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(102, 31);
            button3.TabIndex = 1;
            button3.Text = "Join";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(0, 0);
            listBox1.Margin = new Padding(3, 2, 3, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(341, 349);
            listBox1.TabIndex = 0;
            // 
            // new_game_page
            // 
            new_game_page.BackgroundImage = View.Properties.Resources.ChessBG;
            new_game_page.BackgroundImageLayout = ImageLayout.Stretch;
            new_game_page.Controls.Add(label3);
            new_game_page.Controls.Add(label2);
            new_game_page.Controls.Add(new_game_pass);
            new_game_page.Controls.Add(new_game_name);
            new_game_page.Controls.Add(button5);
            new_game_page.Controls.Add(button6);
            new_game_page.Location = new Point(4, 24);
            new_game_page.Margin = new Padding(3, 2, 3, 2);
            new_game_page.Name = "new_game_page";
            new_game_page.Padding = new Padding(3, 2, 3, 2);
            new_game_page.Size = new Size(702, 336);
            new_game_page.TabIndex = 2;
            new_game_page.Text = "tabPage3";
            new_game_page.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(326, 180);
            label3.Name = "label3";
            label3.Size = new Size(112, 15);
            label3.TabIndex = 5;
            label3.Text = "Password (optional)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(326, 112);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 5;
            label2.Text = "Name";
            // 
            // new_game_pass
            // 
            new_game_pass.Location = new Point(326, 204);
            new_game_pass.Margin = new Padding(3, 2, 3, 2);
            new_game_pass.Name = "new_game_pass";
            new_game_pass.Size = new Size(110, 23);
            new_game_pass.TabIndex = 4;
            new_game_pass.UseSystemPasswordChar = true;
            // 
            // new_game_name
            // 
            new_game_name.Location = new Point(326, 130);
            new_game_name.Margin = new Padding(3, 2, 3, 2);
            new_game_name.Name = "new_game_name";
            new_game_name.Size = new Size(110, 23);
            new_game_name.TabIndex = 4;
            // 
            // button5
            // 
            button5.Location = new Point(596, 306);
            button5.Margin = new Padding(3, 2, 3, 2);
            button5.Name = "button5";
            button5.Size = new Size(102, 31);
            button5.TabIndex = 2;
            button5.Text = "Back";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(3, 304);
            button6.Margin = new Padding(3, 2, 3, 2);
            button6.Name = "button6";
            button6.Size = new Size(102, 31);
            button6.TabIndex = 3;
            button6.Text = "Create";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Main_menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(700, 338);
            Controls.Add(menu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Main_menu";
            Text = "Main menu";
            menu.ResumeLayout(false);
            main_page.ResumeLayout(false);
            join_game_page.ResumeLayout(false);
            join_game_page.PerformLayout();
            new_game_page.ResumeLayout(false);
            new_game_page.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl menu;
        private TabPage main_page;
        private Button button2;
        private Button button1;
        private TabPage join_game_page;
        private ListBox listBox1;
        private Button button3;
        private Label label1;
        private TextBox join_game_pass;
        private Button button4;
        private TabPage new_game_page;
        private Button button5;
        private Button button6;
        private Label label3;
        private Label label2;
        private TextBox new_game_pass;
        private TextBox new_game_name;
    }
}
