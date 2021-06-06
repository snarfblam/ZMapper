namespace ZMapper
{
    partial class HTMLForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HTMLForm));
            this.contentPanel = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.AutoScroll = true;
            this.contentPanel.AutoScrollMinSize = new System.Drawing.Size(534, 20);
            this.contentPanel.BackColor = System.Drawing.SystemColors.Window;
            this.contentPanel.BaseStylesheet = null;
            this.contentPanel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(534, 545);
            this.contentPanel.TabIndex = 0;
            this.contentPanel.Text = "htmlPanel1";
            this.contentPanel.LinkClicked += new System.EventHandler<TheArtOfDev.HtmlRenderer.Core.Entities.HtmlLinkClickedEventArgs>(this.contentPanel_LinkClicked);
            this.contentPanel.ImageLoad += new System.EventHandler<TheArtOfDev.HtmlRenderer.Core.Entities.HtmlImageLoadEventArgs>(this.contentPanel_ImageLoad);
            // 
            // HTMLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 545);
            this.Controls.Add(this.contentPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HTMLForm";
            this.Text = "ZMapper Help";
            this.ResumeLayout(false);

        }

        #endregion

        private TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel contentPanel;
    }
}