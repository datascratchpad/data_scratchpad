namespace Data_Scratchpad
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MF_StatusStrip = new System.Windows.Forms.StatusStrip();
            this.MF_StripStatus_Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.MF_MenuStrip = new System.Windows.Forms.MenuStrip();
            this.MF_MenuStrip_File = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_File_New = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_File_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MF_MenuStrip_File_CloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MF_MenuStrip_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert_Commands = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert_Commands_Source = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert_Commands_AnalysisWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert_Commands_Inspect = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert_Commands_CodePointInspect = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert_Commands_ByteInspect = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Help_Documentation = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Help_QuickStart = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_MenuStrip_Help_About = new System.Windows.Forms.ToolStripMenuItem();
            this.MF_TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.MF_Toolbar = new System.Windows.Forms.ToolStrip();
            this.MF_Toolbar_Run = new System.Windows.Forms.ToolStripButton();
            this.MF_Toolbar_Stop = new System.Windows.Forms.ToolStripButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.MF_StatusStrip.SuspendLayout();
            this.MF_MenuStrip.SuspendLayout();
            this.MF_TabControl.SuspendLayout();
            this.MF_Toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MF_StatusStrip
            // 
            this.MF_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MF_StripStatus_Label});
            this.MF_StatusStrip.Location = new System.Drawing.Point(0, 688);
            this.MF_StatusStrip.Name = "MF_StatusStrip";
            this.MF_StatusStrip.Size = new System.Drawing.Size(1082, 22);
            this.MF_StatusStrip.TabIndex = 0;
            // 
            // MF_StripStatus_Label
            // 
            this.MF_StripStatus_Label.Name = "MF_StripStatus_Label";
            this.MF_StripStatus_Label.Size = new System.Drawing.Size(0, 17);
            // 
            // MF_MenuStrip
            // 
            this.MF_MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MF_MenuStrip_File,
            this.MF_MenuStrip_Insert,
            this.MF_MenuStrip_Help});
            this.MF_MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MF_MenuStrip.Name = "MF_MenuStrip";
            this.MF_MenuStrip.Size = new System.Drawing.Size(1082, 24);
            this.MF_MenuStrip.Stretch = false;
            this.MF_MenuStrip.TabIndex = 1;
            // 
            // MF_MenuStrip_File
            // 
            this.MF_MenuStrip_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MF_MenuStrip_File_New,
            this.MF_MenuStrip_File_Open,
            this.MF_MenuStrip_File_Save,
            this.MF_MenuStrip_File_SaveAs,
            this.toolStripSeparator1,
            this.MF_MenuStrip_File_CloseTab,
            this.toolStripSeparator2,
            this.MF_MenuStrip_File_Exit});
            this.MF_MenuStrip_File.Name = "MF_MenuStrip_File";
            this.MF_MenuStrip_File.Size = new System.Drawing.Size(37, 20);
            this.MF_MenuStrip_File.Text = "&File";
            // 
            // MF_MenuStrip_File_New
            // 
            this.MF_MenuStrip_File_New.Name = "MF_MenuStrip_File_New";
            this.MF_MenuStrip_File_New.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.MF_MenuStrip_File_New.Size = new System.Drawing.Size(203, 22);
            this.MF_MenuStrip_File_New.Text = "&New Scratchpad";
            this.MF_MenuStrip_File_New.Click += new System.EventHandler(this.MF_MenuStrip_File_New_Click);
            // 
            // MF_MenuStrip_File_Open
            // 
            this.MF_MenuStrip_File_Open.Name = "MF_MenuStrip_File_Open";
            this.MF_MenuStrip_File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MF_MenuStrip_File_Open.Size = new System.Drawing.Size(203, 22);
            this.MF_MenuStrip_File_Open.Text = "&Open";
            this.MF_MenuStrip_File_Open.Click += new System.EventHandler(this.MF_MenuStrip_File_Open_Click);
            // 
            // MF_MenuStrip_File_Save
            // 
            this.MF_MenuStrip_File_Save.Name = "MF_MenuStrip_File_Save";
            this.MF_MenuStrip_File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MF_MenuStrip_File_Save.Size = new System.Drawing.Size(203, 22);
            this.MF_MenuStrip_File_Save.Text = "&Save";
            this.MF_MenuStrip_File_Save.Click += new System.EventHandler(this.MF_MenuStrip_File_Save_Click);
            // 
            // MF_MenuStrip_File_SaveAs
            // 
            this.MF_MenuStrip_File_SaveAs.Name = "MF_MenuStrip_File_SaveAs";
            this.MF_MenuStrip_File_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.MF_MenuStrip_File_SaveAs.Size = new System.Drawing.Size(203, 22);
            this.MF_MenuStrip_File_SaveAs.Text = "Save &As...";
            this.MF_MenuStrip_File_SaveAs.Click += new System.EventHandler(this.MF_MenuStrip_File_SaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
            // 
            // MF_MenuStrip_File_CloseTab
            // 
            this.MF_MenuStrip_File_CloseTab.Name = "MF_MenuStrip_File_CloseTab";
            this.MF_MenuStrip_File_CloseTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.MF_MenuStrip_File_CloseTab.Size = new System.Drawing.Size(203, 22);
            this.MF_MenuStrip_File_CloseTab.Text = "&Close tab";
            this.MF_MenuStrip_File_CloseTab.Click += new System.EventHandler(this.MF_MenuStrip_File_CloseTab_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(200, 6);
            // 
            // MF_MenuStrip_File_Exit
            // 
            this.MF_MenuStrip_File_Exit.Name = "MF_MenuStrip_File_Exit";
            this.MF_MenuStrip_File_Exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.MF_MenuStrip_File_Exit.Size = new System.Drawing.Size(203, 22);
            this.MF_MenuStrip_File_Exit.Text = "E&xit";
            this.MF_MenuStrip_File_Exit.Click += new System.EventHandler(this.MF_MenuStrip_File_Exit_Click);
            // 
            // MF_MenuStrip_Insert
            // 
            this.MF_MenuStrip_Insert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MF_MenuStrip_Insert_Commands});
            this.MF_MenuStrip_Insert.Name = "MF_MenuStrip_Insert";
            this.MF_MenuStrip_Insert.Size = new System.Drawing.Size(48, 20);
            this.MF_MenuStrip_Insert.Text = "&Insert";
            // 
            // MF_MenuStrip_Insert_Commands
            // 
            this.MF_MenuStrip_Insert_Commands.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MF_MenuStrip_Insert_Commands_Source,
            this.MF_MenuStrip_Insert_Commands_AnalysisWindow,
            this.MF_MenuStrip_Insert_Commands_Inspect,
            this.MF_MenuStrip_Insert_Commands_CodePointInspect,
            this.MF_MenuStrip_Insert_Commands_ByteInspect,
            this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat});
            this.MF_MenuStrip_Insert_Commands.Name = "MF_MenuStrip_Insert_Commands";
            this.MF_MenuStrip_Insert_Commands.Size = new System.Drawing.Size(136, 22);
            this.MF_MenuStrip_Insert_Commands.Text = "&Commands";
            // 
            // MF_MenuStrip_Insert_Commands_Source
            // 
            this.MF_MenuStrip_Insert_Commands_Source.Name = "MF_MenuStrip_Insert_Commands_Source";
            this.MF_MenuStrip_Insert_Commands_Source.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.MF_MenuStrip_Insert_Commands_Source.Size = new System.Drawing.Size(251, 22);
            this.MF_MenuStrip_Insert_Commands_Source.Text = "&Source";
            this.MF_MenuStrip_Insert_Commands_Source.Click += new System.EventHandler(this.MF_MenuStrip_Insert_Commands_Source_Click);
            // 
            // MF_MenuStrip_Insert_Commands_AnalysisWindow
            // 
            this.MF_MenuStrip_Insert_Commands_AnalysisWindow.Name = "MF_MenuStrip_Insert_Commands_AnalysisWindow";
            this.MF_MenuStrip_Insert_Commands_AnalysisWindow.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
            this.MF_MenuStrip_Insert_Commands_AnalysisWindow.Size = new System.Drawing.Size(251, 22);
            this.MF_MenuStrip_Insert_Commands_AnalysisWindow.Text = "Analysis &Window";
            this.MF_MenuStrip_Insert_Commands_AnalysisWindow.Click += new System.EventHandler(this.MF_MenuStrip_Insert_Commands_AnalysisWindow_Click);
            // 
            // MF_MenuStrip_Insert_Commands_Inspect
            // 
            this.MF_MenuStrip_Insert_Commands_Inspect.Name = "MF_MenuStrip_Insert_Commands_Inspect";
            this.MF_MenuStrip_Insert_Commands_Inspect.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.I)));
            this.MF_MenuStrip_Insert_Commands_Inspect.Size = new System.Drawing.Size(251, 22);
            this.MF_MenuStrip_Insert_Commands_Inspect.Text = "&Inspect";
            this.MF_MenuStrip_Insert_Commands_Inspect.Click += new System.EventHandler(this.MF_MenuStrip_Insert_Commands_Inspect_Click);
            // 
            // MF_MenuStrip_Insert_Commands_CodePointInspect
            // 
            this.MF_MenuStrip_Insert_Commands_CodePointInspect.Name = "MF_MenuStrip_Insert_Commands_CodePointInspect";
            this.MF_MenuStrip_Insert_Commands_CodePointInspect.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.MF_MenuStrip_Insert_Commands_CodePointInspect.Size = new System.Drawing.Size(251, 22);
            this.MF_MenuStrip_Insert_Commands_CodePointInspect.Text = "&Code Point Inspect";
            this.MF_MenuStrip_Insert_Commands_CodePointInspect.Click += new System.EventHandler(this.MF_MenuStrip_Insert_Commands_CodePointInspect_Click);
            // 
            // MF_MenuStrip_Insert_Commands_ByteInspect
            // 
            this.MF_MenuStrip_Insert_Commands_ByteInspect.Name = "MF_MenuStrip_Insert_Commands_ByteInspect";
            this.MF_MenuStrip_Insert_Commands_ByteInspect.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.MF_MenuStrip_Insert_Commands_ByteInspect.Size = new System.Drawing.Size(251, 22);
            this.MF_MenuStrip_Insert_Commands_ByteInspect.Text = "&Byte Inspect";
            this.MF_MenuStrip_Insert_Commands_ByteInspect.Click += new System.EventHandler(this.MF_MenuStrip_Insert_Commands_ByteInspect_Click);
            // 
            // MF_MenuStrip_Insert_Commands_AnalyseFileFormat
            // 
            this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat.Name = "MF_MenuStrip_Insert_Commands_AnalyseFileFormat";
            this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat.Size = new System.Drawing.Size(251, 22);
            this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat.Text = "&Analyse File Format";
            this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat.Click += new System.EventHandler(this.MF_MenuStrip_Insert_Commands_AnalyseFileFormat_Click);
            // 
            // MF_MenuStrip_Help
            // 
            this.MF_MenuStrip_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MF_MenuStrip_Help_Documentation,
            this.MF_MenuStrip_Help_QuickStart,
            this.MF_MenuStrip_Help_About});
            this.MF_MenuStrip_Help.Name = "MF_MenuStrip_Help";
            this.MF_MenuStrip_Help.Size = new System.Drawing.Size(44, 20);
            this.MF_MenuStrip_Help.Text = "&Help";
            // 
            // MF_MenuStrip_Help_Documentation
            // 
            this.MF_MenuStrip_Help_Documentation.Name = "MF_MenuStrip_Help_Documentation";
            this.MF_MenuStrip_Help_Documentation.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.MF_MenuStrip_Help_Documentation.Size = new System.Drawing.Size(183, 22);
            this.MF_MenuStrip_Help_Documentation.Text = "&Documentation";
            this.MF_MenuStrip_Help_Documentation.Click += new System.EventHandler(this.MF_MenuStrip_Help_Documentation_Click);
            // 
            // MF_MenuStrip_Help_QuickStart
            // 
            this.MF_MenuStrip_Help_QuickStart.Name = "MF_MenuStrip_Help_QuickStart";
            this.MF_MenuStrip_Help_QuickStart.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.MF_MenuStrip_Help_QuickStart.Size = new System.Drawing.Size(183, 22);
            this.MF_MenuStrip_Help_QuickStart.Text = "&Quick start guide";
            this.MF_MenuStrip_Help_QuickStart.Click += new System.EventHandler(this.MF_MenuStrip_Help_QuickStart_Click);
            // 
            // MF_MenuStrip_Help_About
            // 
            this.MF_MenuStrip_Help_About.Name = "MF_MenuStrip_Help_About";
            this.MF_MenuStrip_Help_About.Size = new System.Drawing.Size(183, 22);
            this.MF_MenuStrip_Help_About.Text = "&About";
            this.MF_MenuStrip_Help_About.Click += new System.EventHandler(this.MF_MenuStrip_Help_About_Click);
            // 
            // MF_TabControl
            // 
            this.MF_TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MF_TabControl.Controls.Add(this.tabPage1);
            this.MF_TabControl.Location = new System.Drawing.Point(12, 52);
            this.MF_TabControl.Name = "MF_TabControl";
            this.MF_TabControl.SelectedIndex = 0;
            this.MF_TabControl.Size = new System.Drawing.Size(1058, 633);
            this.MF_TabControl.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1050, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MF_Toolbar
            // 
            this.MF_Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MF_Toolbar_Run,
            this.MF_Toolbar_Stop});
            this.MF_Toolbar.Location = new System.Drawing.Point(0, 24);
            this.MF_Toolbar.Name = "MF_Toolbar";
            this.MF_Toolbar.Size = new System.Drawing.Size(1082, 25);
            this.MF_Toolbar.TabIndex = 3;
            this.MF_Toolbar.Text = "toolStrip1";
            // 
            // MF_Toolbar_Run
            // 
            this.MF_Toolbar_Run.Image = ((System.Drawing.Image)(resources.GetObject("MF_Toolbar_Run.Image")));
            this.MF_Toolbar_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MF_Toolbar_Run.Name = "MF_Toolbar_Run";
            this.MF_Toolbar_Run.Size = new System.Drawing.Size(71, 22);
            this.MF_Toolbar_Run.Text = "Run (F5)";
            this.MF_Toolbar_Run.Click += new System.EventHandler(this.MF_Toolbar_Run_Click);
            // 
            // MF_Toolbar_Stop
            // 
            this.MF_Toolbar_Stop.Enabled = false;
            this.MF_Toolbar_Stop.Image = ((System.Drawing.Image)(resources.GetObject("MF_Toolbar_Stop.Image")));
            this.MF_Toolbar_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MF_Toolbar_Stop.Name = "MF_Toolbar_Stop";
            this.MF_Toolbar_Stop.Size = new System.Drawing.Size(51, 22);
            this.MF_Toolbar_Stop.Text = "Stop";
            this.MF_Toolbar_Stop.Click += new System.EventHandler(this.MF_Toolbar_Stop_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1082, 710);
            this.Controls.Add(this.MF_Toolbar);
            this.Controls.Add(this.MF_TabControl);
            this.Controls.Add(this.MF_StatusStrip);
            this.Controls.Add(this.MF_MenuStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MF_MenuStrip;
            this.Name = "MainForm";
            this.Text = "Data Scratchpad";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.MF_StatusStrip.ResumeLayout(false);
            this.MF_StatusStrip.PerformLayout();
            this.MF_MenuStrip.ResumeLayout(false);
            this.MF_MenuStrip.PerformLayout();
            this.MF_TabControl.ResumeLayout(false);
            this.MF_Toolbar.ResumeLayout(false);
            this.MF_Toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip MF_StatusStrip;
        private ToolStripStatusLabel MF_StripStatus_Label;
        private MenuStrip MF_MenuStrip;
        private ToolStripMenuItem MF_MenuStrip_File;
        private ToolStripMenuItem MF_MenuStrip_File_New;
        private ToolStripMenuItem MF_MenuStrip_File_Exit;
        private TabControl MF_TabControl;
        private TabPage tabPage1;
        private ToolStrip MF_Toolbar;
        private ToolStripButton MF_Toolbar_Run;
        private ToolStripButton MF_Toolbar_Stop;
        private ToolStripMenuItem MF_MenuStrip_File_Save;
        private ToolStripMenuItem MF_MenuStrip_File_SaveAs;
        private ToolStripSeparator toolStripSeparator1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem MF_MenuStrip_File_CloseTab;
        private ToolStripMenuItem MF_MenuStrip_File_Open;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem MF_MenuStrip_Help;
        private ToolStripMenuItem MF_MenuStrip_Help_Documentation;
        private ToolStripMenuItem MF_MenuStrip_Help_QuickStart;
        private ToolStripMenuItem MF_MenuStrip_Help_About;
        private ToolStripMenuItem MF_MenuStrip_Insert;
        private ToolStripMenuItem MF_MenuStrip_Insert_Commands;
        private ToolStripMenuItem MF_MenuStrip_Insert_Commands_Source;
        private ToolStripMenuItem MF_MenuStrip_Insert_Commands_AnalysisWindow;
        private ToolStripMenuItem MF_MenuStrip_Insert_Commands_Inspect;
        private ToolStripMenuItem MF_MenuStrip_Insert_Commands_CodePointInspect;
        private ToolStripMenuItem MF_MenuStrip_Insert_Commands_ByteInspect;
        private ToolStripMenuItem MF_MenuStrip_Insert_Commands_AnalyseFileFormat;
    }
}