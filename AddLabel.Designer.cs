namespace deskdecorator
{
    partial class AddLabel
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
            richTextBox1 = new RichTextBox();
            checkBox1 = new CheckBox();
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btn_SetPosition = new Button();
            fontDialog1 = new FontDialog();
            button1 = new Button();
            btn_FontSettings = new Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(12, 72);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(280, 92);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += checkTextInput;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(298, 67);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(73, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Dynamic";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 26);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(216, 23);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += checkTextInput;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 7);
            label1.Name = "label1";
            label1.Size = new Size(97, 15);
            label1.TabIndex = 3;
            label1.Text = "Label description";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 53);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 4;
            label2.Text = "Label text";
            // 
            // btn_SetPosition
            // 
            btn_SetPosition.BackColor = Color.DarkRed;
            btn_SetPosition.Enabled = false;
            btn_SetPosition.FlatAppearance.BorderSize = 0;
            btn_SetPosition.FlatStyle = FlatStyle.Flat;
            btn_SetPosition.ForeColor = SystemColors.ButtonFace;
            btn_SetPosition.Location = new Point(292, 167);
            btn_SetPosition.Name = "btn_SetPosition";
            btn_SetPosition.Size = new Size(79, 23);
            btn_SetPosition.TabIndex = 6;
            btn_SetPosition.Text = "Set position";
            btn_SetPosition.UseVisualStyleBackColor = false;
            btn_SetPosition.Click += OnBtn_SetPosition;
            // 
            // fontDialog1
            // 
            fontDialog1.Apply += fontDialog1_Apply;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(328, 4);
            button1.Name = "button1";
            button1.Size = new Size(43, 23);
            button1.TabIndex = 5;
            button1.Text = "Help";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btn_FontSettings
            // 
            btn_FontSettings.FlatStyle = FlatStyle.Flat;
            btn_FontSettings.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_FontSettings.Location = new Point(298, 93);
            btn_FontSettings.Name = "btn_FontSettings";
            btn_FontSettings.Size = new Size(73, 23);
            btn_FontSettings.TabIndex = 7;
            btn_FontSettings.Text = "Font";
            btn_FontSettings.UseVisualStyleBackColor = true;
            btn_FontSettings.Click += btn_FontSettings_Click;
            // 
            // AddLabel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(376, 202);
            Controls.Add(btn_FontSettings);
            Controls.Add(btn_SetPosition);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(richTextBox1);
            Controls.Add(checkBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "AddLabel";
            Text = "DeskDecorator - Create Label";
            KeyPress += AddLabel_KeyPress;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox richTextBox1;
        private CheckBox checkBox1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Button btn_SetPosition;
        private FontDialog fontDialog1;
        private Button button1;
        private Button btn_FontSettings;
    }
}