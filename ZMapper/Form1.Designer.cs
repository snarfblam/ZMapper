namespace ZMapper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripButton toolStripButton1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CursorTimer = new System.Windows.Forms.Timer(this.components);
            this.MainToolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnInputAlways = new System.Windows.Forms.ToolStripMenuItem();
            this.btnInputCaption = new System.Windows.Forms.ToolStripMenuItem();
            this.btnInputClass = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOpener = new System.Windows.Forms.OpenFileDialog();
            this.FileSaver = new System.Windows.Forms.SaveFileDialog();
            this.picCaption = new System.Windows.Forms.PictureBox();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripSplitButton();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.inputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimap = new ZMapper.MinimapPanel();
            this.pnlMap = new ZMapper.BufferedPanel();
            this.itemPanel1 = new ZMapper.ItemPanel();
            this.picPOI = new System.Windows.Forms.PictureBox();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.MainToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaption)).BeginInit();
            this.itemPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPOI)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Enabled = false;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(23, 22);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // CursorTimer
            // 
            this.CursorTimer.Interval = 300;
            this.CursorTimer.Tick += new System.EventHandler(this.CursorTimer_Tick);
            // 
            // MainToolbar
            // 
            this.MainToolbar.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.MainToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            toolStripButton1,
            this.toolStripDropDownButton1,
            this.toolStripButton2});
            this.MainToolbar.Location = new System.Drawing.Point(0, 0);
            this.MainToolbar.Name = "MainToolbar";
            this.MainToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainToolbar.Size = new System.Drawing.Size(734, 25);
            this.MainToolbar.TabIndex = 7;
            this.MainToolbar.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInputAlways,
            this.btnInputCaption,
            this.btnInputClass});
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(29, 22);
            this.toolStripButton2.Text = "Settings";
            // 
            // btnInputAlways
            // 
            this.btnInputAlways.Checked = true;
            this.btnInputAlways.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnInputAlways.Name = "btnInputAlways";
            this.btnInputAlways.Size = new System.Drawing.Size(237, 22);
            this.btnInputAlways.Text = "Input Always Enabled";
            this.btnInputAlways.Click += new System.EventHandler(this.btnInputAlways_Click);
            // 
            // btnInputCaption
            // 
            this.btnInputCaption.Name = "btnInputCaption";
            this.btnInputCaption.Size = new System.Drawing.Size(237, 22);
            this.btnInputCaption.Text = "Enabled For Window Caption...";
            this.btnInputCaption.Click += new System.EventHandler(this.btnInputCaption_Click);
            // 
            // btnInputClass
            // 
            this.btnInputClass.Name = "btnInputClass";
            this.btnInputClass.Size = new System.Drawing.Size(237, 22);
            this.btnInputClass.Text = "Enabled For Window Class...";
            this.btnInputClass.Click += new System.EventHandler(this.btnInputClass_Click);
            // 
            // FileOpener
            // 
            this.FileOpener.Filter = "JSON file (*.json)|*.json|All Files (*.*)|*.*";
            this.FileOpener.Title = "Open Tracker Data";
            // 
            // FileSaver
            // 
            this.FileSaver.Filter = "JSON file (*.json)|*.json|All Files (*.*)|*.*";
            this.FileSaver.Title = "Save Tracker Data";
            // 
            // picCaption
            // 
            this.picCaption.Location = new System.Drawing.Point(35, 31);
            this.picCaption.Name = "picCaption";
            this.picCaption.Size = new System.Drawing.Size(320, 24);
            this.picCaption.TabIndex = 8;
            this.picCaption.TabStop = false;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNew.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(51, 22);
            this.btnNew.Text = "&New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOpen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(56, 22);
            this.btnOpen.Text = "&Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem});
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(63, 22);
            this.btnSave.Text = "&Save";
            this.btnSave.ButtonClick += new System.EventHandler(this.btnSave_ButtonClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "Help";
            // 
            // inputToolStripMenuItem
            // 
            this.inputToolStripMenuItem.Name = "inputToolStripMenuItem";
            this.inputToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.inputToolStripMenuItem.Text = "Input";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // minimap
            // 
            this.minimap.Location = new System.Drawing.Point(418, 58);
            this.minimap.Name = "minimap";
            this.minimap.Size = new System.Drawing.Size(304, 304);
            this.minimap.TabIndex = 6;
            // 
            // pnlMap
            // 
            this.pnlMap.BackColor = System.Drawing.Color.Black;
            this.pnlMap.Location = new System.Drawing.Point(12, 58);
            this.pnlMap.Name = "pnlMap";
            this.pnlMap.Size = new System.Drawing.Size(384, 192);
            this.pnlMap.TabIndex = 0;
            this.pnlMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlMap_MouseDown);
            // 
            // itemPanel1
            // 
            this.itemPanel1.Controls.Add(this.picPOI);
            this.itemPanel1.Location = new System.Drawing.Point(35, 241);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(333, 180);
            this.itemPanel1.TabIndex = 1;
            // 
            // picPOI
            // 
            this.picPOI.Image = global::ZMapper.Properties.Resources.OWPOISelector;
            this.picPOI.Location = new System.Drawing.Point(0, 9);
            this.picPOI.Name = "picPOI";
            this.picPOI.Size = new System.Drawing.Size(333, 166);
            this.picPOI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picPOI.TabIndex = 9;
            this.picPOI.TabStop = false;
            this.picPOI.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(734, 409);
            this.Controls.Add(this.picCaption);
            this.Controls.Add(this.MainToolbar);
            this.Controls.Add(this.minimap);
            this.Controls.Add(this.pnlMap);
            this.Controls.Add(this.itemPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "ZMapper [Active]";
            this.MainToolbar.ResumeLayout(false);
            this.MainToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaption)).EndInit();
            this.itemPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPOI)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZMapper.BufferedPanel pnlMap;
        private System.Windows.Forms.Timer CursorTimer;
        private ItemPanel itemPanel1;
        private MinimapPanel minimap;
        private System.Windows.Forms.ToolStrip MainToolbar;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripSplitButton btnSave;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog FileOpener;
        private System.Windows.Forms.SaveFileDialog FileSaver;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem btnInputAlways;
        private System.Windows.Forms.ToolStripMenuItem btnInputCaption;
        private System.Windows.Forms.ToolStripMenuItem btnInputClass;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem inputToolStripMenuItem;
        private System.Windows.Forms.PictureBox picCaption;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.PictureBox picPOI;
    }
}

