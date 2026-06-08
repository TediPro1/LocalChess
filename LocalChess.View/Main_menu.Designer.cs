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
            isOnlineCheckBox = new CheckBox();
            button2 = new Button();
            button1 = new Button();
            join_game_page = new TabPage();
            join_pass_hide_button = new Button();
            label1 = new Label();
            join_game_pass = new TextBox();
            button7 = new Button();
            button4 = new Button();
            button3 = new Button();
            listBox1 = new ListBox();
            new_game_page = new TabPage();
            hidePassButton = new Button();
            label3 = new Label();
            label2 = new Label();
            new_game_pass = new TextBox();
            new_game_name = new TextBox();
            button5 = new Button();
            button6 = new Button();
            game_history_page = new TabPage();
            button8 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            menu.SuspendLayout();
            main_page.SuspendLayout();
            join_game_page.SuspendLayout();
            new_game_page.SuspendLayout();
            game_history_page.SuspendLayout();
            SuspendLayout();
            // 
            // menu
            // 
            menu.Controls.Add(main_page);
            menu.Controls.Add(join_game_page);
            menu.Controls.Add(new_game_page);
            menu.Controls.Add(game_history_page);
            menu.Location = new Point(-5, -29);
            menu.Name = "menu";
            menu.SelectedIndex = 0;
            menu.Size = new Size(811, 485);
            menu.TabIndex = 0;
            // 
            // main_page
            // 
            main_page.BackgroundImage = View.Properties.Resources.ChessBG;
            main_page.BackgroundImageLayout = ImageLayout.Stretch;
            main_page.Controls.Add(isOnlineCheckBox);
            main_page.Controls.Add(button2);
            main_page.Controls.Add(button1);
            main_page.Location = new Point(4, 29);
            main_page.Name = "main_page";
            main_page.Padding = new Padding(3);
            main_page.Size = new Size(803, 452);
            main_page.TabIndex = 0;
            main_page.Text = "tabPage1";
            main_page.UseVisualStyleBackColor = true;
            // 
            // isOnlineCheckBox
            // 
            isOnlineCheckBox.AutoSize = true;
            isOnlineCheckBox.ForeColor = SystemColors.Control;
            isOnlineCheckBox.Location = new Point(367, 325);
            isOnlineCheckBox.Margin = new Padding(3, 4, 3, 4);
            isOnlineCheckBox.Name = "isOnlineCheckBox";
            isOnlineCheckBox.Size = new Size(74, 24);
            isOnlineCheckBox.TabIndex = 7;
            isOnlineCheckBox.Text = "Online";
            isOnlineCheckBox.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.FlatAppearance.BorderColor = Color.White;
            button2.FlatAppearance.BorderSize = 5;
            button2.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.White;
            button2.Location = new Point(451, 313);
            button2.Name = "button2";
            button2.Size = new Size(131, 45);
            button2.TabIndex = 0;
            button2.Text = "Join Game";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.FlatAppearance.BorderColor = Color.White;
            button1.FlatAppearance.BorderSize = 5;
            button1.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.White;
            button1.Location = new Point(219, 313);
            button1.Name = "button1";
            button1.Size = new Size(131, 45);
            button1.TabIndex = 0;
            button1.Text = "New Game";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // join_game_page
            // 
            join_game_page.BackgroundImage = View.Properties.Resources.ChessBG;
            join_game_page.BackgroundImageLayout = ImageLayout.Stretch;
            join_game_page.Controls.Add(join_pass_hide_button);
            join_game_page.Controls.Add(label1);
            join_game_page.Controls.Add(join_game_pass);
            join_game_page.Controls.Add(button7);
            join_game_page.Controls.Add(button4);
            join_game_page.Controls.Add(button3);
            join_game_page.Controls.Add(listBox1);
            join_game_page.Location = new Point(4, 29);
            join_game_page.Name = "join_game_page";
            join_game_page.Padding = new Padding(3);
            join_game_page.Size = new Size(803, 452);
            join_game_page.TabIndex = 1;
            join_game_page.Text = "tabPage2";
            join_game_page.UseVisualStyleBackColor = true;
            // 
            // join_pass_hide_button
            // 
            join_pass_hide_button.BackColor = Color.White;
            join_pass_hide_button.BackgroundImageLayout = ImageLayout.Stretch;
            join_pass_hide_button.FlatAppearance.BorderColor = Color.White;
            join_pass_hide_button.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            join_pass_hide_button.FlatStyle = FlatStyle.Flat;
            join_pass_hide_button.ForeColor = Color.White;
            join_pass_hide_button.Location = new Point(633, 209);
            join_pass_hide_button.Margin = new Padding(3, 4, 3, 4);
            join_pass_hide_button.Name = "join_pass_hide_button";
            join_pass_hide_button.Size = new Size(27, 27);
            join_pass_hide_button.TabIndex = 8;
            join_pass_hide_button.UseVisualStyleBackColor = false;
            join_pass_hide_button.Click += button7_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Transparent;
            label1.Location = new Point(395, 187);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 3;
            label1.Text = "Password";
            // 
            // join_game_pass
            // 
            join_game_pass.Location = new Point(395, 209);
            join_game_pass.Name = "join_game_pass";
            join_game_pass.Size = new Size(233, 27);
            join_game_pass.TabIndex = 2;
            join_game_pass.UseSystemPasswordChar = true;
            // 
            // button7
            // 
            button7.FlatAppearance.BorderColor = Color.White;
            button7.FlatAppearance.BorderSize = 5;
            button7.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            button7.FlatStyle = FlatStyle.Flat;
            button7.ForeColor = Color.White;
            button7.Location = new Point(679, 5);
            button7.Name = "button7";
            button7.Size = new Size(117, 41);
            button7.TabIndex = 1;
            button7.Text = "Game History";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click_2;
            // 
            // button4
            // 
            button4.FlatAppearance.BorderColor = Color.White;
            button4.FlatAppearance.BorderSize = 5;
            button4.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            button4.FlatStyle = FlatStyle.Flat;
            button4.ForeColor = Color.White;
            button4.Location = new Point(681, 408);
            button4.Name = "button4";
            button4.Size = new Size(117, 41);
            button4.TabIndex = 1;
            button4.Text = "Back";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.FlatAppearance.BorderColor = Color.White;
            button3.FlatAppearance.BorderSize = 5;
            button3.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            button3.FlatStyle = FlatStyle.Flat;
            button3.ForeColor = Color.White;
            button3.Location = new Point(395, 408);
            button3.Name = "button3";
            button3.Size = new Size(117, 41);
            button3.TabIndex = 1;
            button3.Text = "Join";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(389, 464);
            listBox1.TabIndex = 0;
            // 
            // new_game_page
            // 
            new_game_page.BackgroundImage = View.Properties.Resources.ChessBG;
            new_game_page.BackgroundImageLayout = ImageLayout.Stretch;
            new_game_page.Controls.Add(hidePassButton);
            new_game_page.Controls.Add(label3);
            new_game_page.Controls.Add(label2);
            new_game_page.Controls.Add(new_game_pass);
            new_game_page.Controls.Add(new_game_name);
            new_game_page.Controls.Add(button5);
            new_game_page.Controls.Add(button6);
            new_game_page.Location = new Point(4, 29);
            new_game_page.Name = "new_game_page";
            new_game_page.Padding = new Padding(3);
            new_game_page.Size = new Size(803, 452);
            new_game_page.TabIndex = 2;
            new_game_page.Text = "tabPage3";
            new_game_page.UseVisualStyleBackColor = true;
            // 
            // hidePassButton
            // 
            hidePassButton.BackColor = Color.White;
            hidePassButton.BackgroundImageLayout = ImageLayout.Stretch;
            hidePassButton.FlatAppearance.BorderColor = Color.White;
            hidePassButton.FlatAppearance.MouseOverBackColor = Color.Gray;
            hidePassButton.FlatStyle = FlatStyle.Flat;
            hidePassButton.Location = new Point(460, 272);
            hidePassButton.Margin = new Padding(3, 4, 3, 4);
            hidePassButton.Name = "hidePassButton";
            hidePassButton.Size = new Size(27, 27);
            hidePassButton.TabIndex = 7;
            hidePassButton.UseVisualStyleBackColor = false;
            hidePassButton.Click += button7_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.FlatStyle = FlatStyle.Flat;
            label3.ForeColor = Color.White;
            label3.Location = new Point(315, 247);
            label3.Name = "label3";
            label3.Size = new Size(142, 22);
            label3.TabIndex = 5;
            label3.Text = "Password (optional)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.FlatStyle = FlatStyle.Flat;
            label2.ForeColor = Color.White;
            label2.Location = new Point(315, 165);
            label2.Name = "label2";
            label2.Size = new Size(51, 22);
            label2.TabIndex = 5;
            label2.Text = "Name";
            // 
            // new_game_pass
            // 
            new_game_pass.Location = new Point(315, 272);
            new_game_pass.Name = "new_game_pass";
            new_game_pass.Size = new Size(142, 27);
            new_game_pass.TabIndex = 4;
            new_game_pass.UseSystemPasswordChar = true;
            // 
            // new_game_name
            // 
            new_game_name.Location = new Point(315, 189);
            new_game_name.Name = "new_game_name";
            new_game_name.Size = new Size(142, 27);
            new_game_name.TabIndex = 4;
            // 
            // button5
            // 
            button5.FlatAppearance.BorderColor = Color.White;
            button5.FlatAppearance.BorderSize = 5;
            button5.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            button5.FlatStyle = FlatStyle.Flat;
            button5.ForeColor = Color.White;
            button5.Location = new Point(680, 405);
            button5.Name = "button5";
            button5.Size = new Size(117, 41);
            button5.TabIndex = 2;
            button5.Text = "Back";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.FlatAppearance.BorderColor = Color.White;
            button6.FlatAppearance.BorderSize = 5;
            button6.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            button6.FlatStyle = FlatStyle.Flat;
            button6.ForeColor = Color.White;
            button6.Location = new Point(6, 405);
            button6.Name = "button6";
            button6.Size = new Size(117, 41);
            button6.TabIndex = 3;
            button6.Text = "Create";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // game_history_page
            // 
            game_history_page.Controls.Add(button8);
            game_history_page.Controls.Add(flowLayoutPanel1);
            game_history_page.Location = new Point(4, 29);
            game_history_page.Margin = new Padding(3, 4, 3, 4);
            game_history_page.Name = "game_history_page";
            game_history_page.Padding = new Padding(3, 4, 3, 4);
            game_history_page.Size = new Size(803, 452);
            game_history_page.TabIndex = 3;
            game_history_page.Text = "History";
            game_history_page.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.FlatAppearance.BorderColor = Color.Black;
            button8.FlatAppearance.BorderSize = 3;
            button8.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
            button8.FlatStyle = FlatStyle.Flat;
            button8.ForeColor = Color.Black;
            button8.Location = new Point(680, 407);
            button8.Name = "button8";
            button8.Size = new Size(117, 41);
            button8.TabIndex = 2;
            button8.Text = "Back";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 4);
            flowLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(797, 444);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // Main_menu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(800, 451);
            Controls.Add(menu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main_menu";
            Text = "Main menu";
            FormClosed += Main_menu_FormClosed;
            menu.ResumeLayout(false);
            main_page.ResumeLayout(false);
            main_page.PerformLayout();
            join_game_page.ResumeLayout(false);
            join_game_page.PerformLayout();
            new_game_page.ResumeLayout(false);
            new_game_page.PerformLayout();
            game_history_page.ResumeLayout(false);
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
        private Button hidePassButton;
        private Button join_pass_hide_button;
        private CheckBox isOnlineCheckBox;
        private TabPage game_history_page;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button8;
        private Button button7;
    }
}
