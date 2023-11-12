namespace Workshop1._1
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            info_label = new Label();
            username_label = new Label();
            password_label = new Label();
            username_box = new TextBox();
            password_box = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // info_label
            // 
            info_label.AutoSize = true;
            info_label.Location = new Point(78, 37);
            info_label.Name = "info_label";
            info_label.Size = new Size(92, 15);
            info_label.TabIndex = 3;
            info_label.Text = "Prijava u sistem!";
            // 
            // username_label
            // 
            username_label.AutoSize = true;
            username_label.Location = new Point(18, 80);
            username_label.Name = "username_label";
            username_label.Size = new Size(88, 15);
            username_label.TabIndex = 4;
            username_label.Text = "Korisničko ime:";
            // 
            // password_label
            // 
            password_label.AutoSize = true;
            password_label.Location = new Point(18, 112);
            password_label.Name = "password_label";
            password_label.Size = new Size(50, 15);
            password_label.TabIndex = 5;
            password_label.Text = "Lozinka:";
            // 
            // username_box
            // 
            username_box.Location = new Point(115, 80);
            username_box.Name = "username_box";
            username_box.Size = new Size(100, 23);
            username_box.TabIndex = 6;
            username_box.Text = "rana";
            // 
            // password_box
            // 
            password_box.Location = new Point(115, 112);
            password_box.Name = "password_box";
            password_box.Size = new Size(100, 23);
            password_box.TabIndex = 7;
            password_box.Text = "password";
            password_box.UseSystemPasswordChar = true;
            // 
            // button1
            // 
            button1.Location = new Point(45, 154);
            button1.Name = "button1";
            button1.Size = new Size(160, 23);
            button1.TabIndex = 8;
            button1.Text = "Prijava";
            button1.UseVisualStyleBackColor = true;
            button1.Click += login_button_Click;
            // 
            // button2
            // 
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.Location = new Point(190, 213);
            button2.Name = "button2";
            button2.Size = new Size(25, 25);
            button2.TabIndex = 9;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Center;
            button3.Location = new Point(221, 213);
            button3.Name = "button3";
            button3.Size = new Size(25, 25);
            button3.TabIndex = 10;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(258, 250);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(password_box);
            Controls.Add(username_box);
            Controls.Add(password_label);
            Controls.Add(username_label);
            Controls.Add(info_label);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Workshop1.1 - Prijava";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label info_label;
        private Label username_label;
        private Label password_label;
        private TextBox username_box;
        private TextBox password_box;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}