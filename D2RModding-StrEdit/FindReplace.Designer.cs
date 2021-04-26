
namespace D2RModding_StrEdit
{
    partial class FindReplace
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.findButton = new System.Windows.Forms.Button();
            this.find_InText = new System.Windows.Forms.RadioButton();
            this.find_InKeys = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(293, 119);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.findButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.find_InText, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.find_InKeys, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.18182F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.81818F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(287, 116);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // findButton
            // 
            this.findButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.findButton.Location = new System.Drawing.Point(209, 91);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(75, 22);
            this.findButton.TabIndex = 0;
            this.findButton.Text = "Find";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.onFindClicked);
            // 
            // find_InText
            // 
            this.find_InText.AutoSize = true;
            this.find_InText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.find_InText.Location = new System.Drawing.Point(15, 55);
            this.find_InText.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.find_InText.Name = "find_InText";
            this.find_InText.Size = new System.Drawing.Size(257, 30);
            this.find_InText.TabIndex = 1;
            this.find_InText.Text = "Find in Text";
            this.find_InText.UseVisualStyleBackColor = true;
            this.find_InText.CheckedChanged += new System.EventHandler(this.onRadioSelection);
            // 
            // find_InKeys
            // 
            this.find_InKeys.AutoSize = true;
            this.find_InKeys.Checked = true;
            this.find_InKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.find_InKeys.Location = new System.Drawing.Point(15, 23);
            this.find_InKeys.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.find_InKeys.Name = "find_InKeys";
            this.find_InKeys.Size = new System.Drawing.Size(257, 26);
            this.find_InKeys.TabIndex = 2;
            this.find_InKeys.TabStop = true;
            this.find_InKeys.Text = "Find in Keys";
            this.find_InKeys.UseVisualStyleBackColor = true;
            this.find_InKeys.CheckedChanged += new System.EventHandler(this.onRadioSelection);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(287, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.onTextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.onKeyPressed);
            // 
            // FindReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(293, 119);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindReplace";
            this.Text = "Find/Replace";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.RadioButton find_InText;
        private System.Windows.Forms.RadioButton find_InKeys;
        private System.Windows.Forms.TextBox textBox1;
    }
}