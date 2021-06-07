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
            this.FileOpener = new System.Windows.Forms.OpenFileDialog();
            this.FileSaver = new System.Windows.Forms.SaveFileDialog();
            this.picCaption = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripSplitButton();
            this.btnSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnInputAlways = new System.Windows.Forms.ToolStripMenuItem();
            this.btnInputCaption = new System.Windows.Forms.ToolStripMenuItem();
            this.btnInputClass = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNoFocus = new System.Windows.Forms.ToolStripMenuItem();
            this.minimap = new ZMapper.MinimapPanel();
            this.pnlMap = new ZMapper.BufferedPanel();
            this.itemPanel1 = new ZMapper.ItemPanel();
            this.picPOI = new System.Windows.Forms.PictureBox();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.MainToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaption)).BeginInit();
            this.menuStrip1.SuspendLayout();
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
            toolStripButton1});
            this.MainToolbar.Location = new System.Drawing.Point(0, 27);
            this.MainToolbar.Name = "MainToolbar";
            this.MainToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainToolbar.Size = new System.Drawing.Size(734, 25);
            this.MainToolbar.TabIndex = 7;
            this.MainToolbar.Text = "toolStrip1";
            this.MainToolbar.Visible = false;
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
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnHelp,
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.toolStripButton2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(734, 27);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnHelp
            // 
            this.btnHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.ForeColor = System.Drawing.Color.White;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 20);
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnNew
            // 
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(51, 20);
            this.btnNew.Text = "&New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(56, 20);
            this.btnOpen.Text = "&Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveAs});
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(63, 20);
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(123, 22);
            this.btnSaveAs.Text = "Save As...";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInputAlways,
            this.btnInputCaption,
            this.btnInputClass,
            this.toolStripSeparator1,
            this.btnAlwaysOnTop,
            this.btnNoFocus});
            this.toolStripButton2.ForeColor = System.Drawing.Color.White;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(29, 20);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(234, 6);
            // 
            // btnAlwaysOnTop
            // 
            this.btnAlwaysOnTop.Name = "btnAlwaysOnTop";
            this.btnAlwaysOnTop.Size = new System.Drawing.Size(237, 22);
            this.btnAlwaysOnTop.Text = "Always On Top";
            this.btnAlwaysOnTop.Click += new System.EventHandler(this.btnAlwaysOnTop_Click);
            // 
            // btnNoFocus
            // 
            this.btnNoFocus.Name = "btnNoFocus";
            this.btnNoFocus.Size = new System.Drawing.Size(237, 22);
            this.btnNoFocus.Text = "No Focus On Click";
            this.btnNoFocus.ToolTipText = "Keeps this program from stealing focus\r\nfrom other software when clicked.";
            this.btnNoFocus.Click += new System.EventHandler(this.btnNoFocus_Click);
            // 
            // minimap
            // 
            this.minimap.Location = new System.Drawing.Point(418, 58);
            this.minimap.Name = "minimap";
            this.minimap.Size = new System.Drawing.Size(304, 304);
            this.minimap.TabIndex = 6;
            this.minimap.ThumbClicked += new System.EventHandler<ZMapper.IndexEventArgs>(this.minimap_ThumbClicked);
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
            this.itemPanel1.Location = new System.Drawing.Point(35, 236);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(333, 180);
            this.itemPanel1.TabIndex = 1;
            // 
            // picPOI
            // 
            this.picPOI.Image = global::ZMapper.Properties.Resources.OWPOISelector;
            this.picPOI.Location = new System.Drawing.Point(0, 9);
            this.picPOI.Name = "picPOI";
            this.picPOI.Size = new System.Drawing.Size(334, 166);
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
            this.Controls.Add(this.MainToolbar);
            this.Controls.Add(this.picCaption);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.minimap);
            this.Controls.Add(this.pnlMap);
            this.Controls.Add(this.itemPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ZMapper";
            this.MainToolbar.ResumeLayout(false);
            this.MainToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaption)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.OpenFileDialog FileOpener;
        private System.Windows.Forms.SaveFileDialog FileSaver;
        private System.Windows.Forms.PictureBox picCaption;
        private System.Windows.Forms.PictureBox picPOI;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripSplitButton btnSave;
        private System.Windows.Forms.ToolStripMenuItem btnSaveAs;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem btnInputAlways;
        private System.Windows.Forms.ToolStripMenuItem btnInputCaption;
        private System.Windows.Forms.ToolStripMenuItem btnInputClass;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnAlwaysOnTop;
        private System.Windows.Forms.ToolStripMenuItem btnNoFocus;
    }
}

