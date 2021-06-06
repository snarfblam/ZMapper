namespace ZMapper
{
    partial class InputModeEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.radPattern = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radRegex = new System.Windows.Forms.RadioButton();
            this.txtMatchString = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radPattern
            // 
            this.radPattern.AutoSize = true;
            this.radPattern.Checked = true;
            this.radPattern.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPattern.Location = new System.Drawing.Point(16, 13);
            this.radPattern.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radPattern.Name = "radPattern";
            this.radPattern.Size = new System.Drawing.Size(120, 20);
            this.radPattern.TabIndex = 3;
            this.radPattern.TabStop = true;
            this.radPattern.Text = "Pattern Match";
            this.radPattern.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(41, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(277, 57);
            this.label1.TabIndex = 4;
            this.label1.Text = "An asterisk ( * ) will match any sequence of (zero or more) characters, and a que" +
                "stion mark ( ? ) will match any one character.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(41, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(277, 57);
            this.label2.TabIndex = 6;
            this.label2.Text = "Use .NET regular expression syntax. Match any sequence of zero or more characters" +
                " with \".*\" or any single character with \".\"";
            // 
            // radRegex
            // 
            this.radRegex.AutoSize = true;
            this.radRegex.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radRegex.Location = new System.Drawing.Point(16, 98);
            this.radRegex.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radRegex.Name = "radRegex";
            this.radRegex.Size = new System.Drawing.Size(149, 20);
            this.radRegex.TabIndex = 5;
            this.radRegex.Text = "Regular Expression";
            this.radRegex.UseVisualStyleBackColor = true;
            // 
            // txtMatchString
            // 
            this.txtMatchString.Location = new System.Drawing.Point(12, 191);
            this.txtMatchString.Name = "txtMatchString";
            this.txtMatchString.Size = new System.Drawing.Size(317, 23);
            this.txtMatchString.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(254, 220);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(173, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // InputModeEditor
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(341, 251);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtMatchString);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radRegex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radPattern);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputModeEditor";
            this.ShowIcon = false;
            this.Text = "Input Mode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radPattern;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radRegex;
        private System.Windows.Forms.TextBox txtMatchString;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}