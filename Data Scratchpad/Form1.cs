using ds_common;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;
using System.Reflection;

namespace Data_Scratchpad
{


    public partial class MainForm : Form
    {




        const string TabName_Output_Text = "output_text";
        const string TabName_Output_Grid = "output_grid";
        const string TabName_Output_Graph = "output_graph";
        const string TabName_Info = "info";
        const string dtfs_ToSecondsPrecision = "yyyy-MM-dd HH:mm:ss";
        const string dtfs_ToMillisecondsPrecision = "yyyy-MM-dd HH:mm:ss.fff";
        const string NewScratchpadNamePrefix = "New Scratchpad ";
        const string ScratchpadFileExtension = "pad";

        List<OOpenFilesList> LOpenFilesList = new List<OOpenFilesList>();
        string LastUsedSaveDirectory = Environment.CurrentDirectory;
        bool FileIsOpening = false;
        const long MaxScratchpadFileSize = 1000000;
        const long MaxTextFileViewerFileSize = 1000000;

        static List<string> CurrentCommandSet;

        static TabPage CurrentlyExecuting_Scratchpad;
        static RichTextBox CurrentlyExecuting_rtb;
        static TabPage CurrentlyExecuting_TabPage_Info;
        static TabPage CurrentlyExecuting_TabPage_Output_Text;
        static TabPage CurrentlyExecuting_TabPage_Output_Grid;
        static TabPage CurrentlyExecuting_TabPage_Output_Graph;

        const string TabNamePrefixForTextFiles = "TextFileViewer_";
        string HelpFilePath_Documentation = Path.Combine(System.AppContext.BaseDirectory, "Documentation\\Data Scratchpad Documentation.txt");
        string HelpFilePath_QuickStartGuid = Path.Combine(System.AppContext.BaseDirectory, "Documentation\\Data Scratchpad Quick Start Guide.txt");




        const string TemplateCmds_Source = ":source\r\n{\r\nsource_type = file\r\nfile_path = \r\nfile_format = csv\r\nfile_encoding = utf8\r\nheader_rows_count = 1\r\nskip_empty_rows = Y\r\nskip_final_empty_row = N\r\n}\r\n\r\n";
        const string TemplateCmds_AnalysisWindow = ":analysis_window\r\n{\r\nrows = \r\nfields = \r\nfield_conditions = \r\n}\r\n\r\n";
        const string TemplateCmds_Inspect = ":inspect {}\r\n\r\n";
        const string TemplateCmds_CodePointInspect = ":code_point_inspect\r\n{\r\nstart_character = 1\r\n}\r\n\r\n";
        const string TemplateCmds_ByteInspect = ":byte_inspect\r\n{\r\nstart_byte = 1\r\n}\r\n\r\n";
        const string TemplateCmds_AnalyseFileFormat = ":analyse_file_format {}\r\n\r\n";





        public MainForm()
        {
            InitializeComponent();

            SetupBackgroundWorker();

            bool kr = CheckCommandLineArguments();

            if (kr == false)
            {
                CreateNewScratchpad(true);
            }


            MF_TabControl.TabPages.RemoveAt(0);

            DataProcessing.ProcessingInitialisations();

            this.Show();

            SetFocus_CurrentScratchpad_PrimaryInputTextBox();

        }










        private bool CheckCommandLineArguments()
        {
            bool ret = false;

            if (Environment.GetCommandLineArgs() == null) return ret;
            if (Environment.GetCommandLineArgs().Length < 2) return ret;

            string t = Environment.GetCommandLineArgs()[1].Trim();

            if (string.IsNullOrEmpty(t)) return ret;

            bool kr = LoadScratchpad(t);

            return kr;
        }









        private void SetupBackgroundWorker()
        {
            this.backgroundWorker1 = new BackgroundWorker();

            this.backgroundWorker1.DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.WorkerSupportsCancellation = true;
        }






        public string CreateNewScratchpad(bool NoAsterisk)
        {
            // Returns the name of the newly created scratchpad, which is actually the name of the tab, the text of the tab and the filename of the entry in LOpenFilesList
            string spname;
            int n = 1;
            int tn = 0;
            int u;
            string t;
            string Asterisk = "*";

            if (NoAsterisk) Asterisk = string.Empty;


            // Loop over the tab pages
            for (int i = 0; i < MF_TabControl.TabPages.Count; i++)
            {
                // Check if the tab page text includes an asterisk
                u = MF_TabControl.TabPages[i].Text.IndexOf('*');
                if (u > -1)
                {
                    // Check if the tab page name includes the standard prefix
                    if (MF_TabControl.TabPages[i].Text.Contains(NewScratchpadNamePrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        // Try to get the number at the end of the tab page text
                        t = MF_TabControl.TabPages[i].Text.Replace(NewScratchpadNamePrefix, string.Empty).Replace("*", string.Empty).Trim();
                        if (string.IsNullOrEmpty(t) == false)
                        {
                            if (int.TryParse(t, out tn))
                            {
                                if (tn + 1 > n)
                                {
                                    n = tn + 1;
                                }
                            }
                        }
                    }
                }
            }


            spname = Asterisk + NewScratchpadNamePrefix + n.ToString();

            OOpenFilesList oofi = new OOpenFilesList();
            oofi.Path = string.Empty;
            oofi.Filename = spname;

            LOpenFilesList.Add(oofi);


            TabPage tab = new TabPage();
            tab.Text = spname;
            tab.Name = spname;


            MF_TabControl.TabPages.Add(tab);

            MF_TabControl.SelectedTab = tab;
            tab.Focus();


            //[************* Add a SplitContainer
            SplitContainer splitContainer = new SplitContainer();
            SplitterPanel SPanel1 = new SplitterPanel(splitContainer);
            SplitterPanel SPanel2 = new SplitterPanel(splitContainer);

            int w = (int)(splitContainer.Width / 3.0d);

            splitContainer.Location = new Point(3, 3);
            splitContainer.Margin = new Padding(3, 3, 3, 3);
            splitContainer.BackColor = Color.White;
            splitContainer.Visible = true;
            splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Vertical;
            splitContainer.IsSplitterFixed = false;
            splitContainer.FixedPanel = FixedPanel.None;
            splitContainer.SplitterDistance = w;

            tab.Controls.Add(splitContainer);
            //*************]



            //[************* Add a RichTextBox in the left panel
            RichTextBox rtb = CreateRichTextBox(Color.White, Color.Black, false);
            rtb.TextChanged += new EventHandler(PrimaryRTBs_TextChangedEvent);
            splitContainer.Panel1.Controls.Add(rtb);
            //*************]



            //[************* Add a TabControl in the right panel
            TabControl tc = new TabControl();
            TabPage otp;

            tc.Location = new Point(3, 3);
            tc.Margin = new Padding(3, 3, 3, 3);
            tc.Dock = DockStyle.Fill;
            tc.Visible = true;

            //[***** Add the text output tab page
            otp = new TabPage();
            otp.Name = "output_text";
            otp.Text = "Text output";
            otp.BackColor = Color.LightGray;
            // Add a RichTextBox in the output tab page
            rtb = CreateRichTextBox(Color.LightGray, Color.Black, true);
            otp.Controls.Add(rtb);
            // Add the tab page to the tab control
            tc.TabPages.Add(otp);
            //*****]

            //[***** Add the grid output tab page
            otp = new TabPage();
            otp.Name = "output_grid";
            otp.Text = "Grid output";
            otp.BackColor = Color.LightGray;
            // Add a DataGridView in the output tab page
            DataGridView dg = CreateDataGridView();
            otp.Controls.Add(dg);
            // Add the tab page to the tab control
            tc.TabPages.Add(otp);
            //*****]

            //[***** Add the graph output tab page
            otp = new TabPage();
            otp.Name = "output_graph";
            otp.Text = "Graph output";
            otp.BackColor = Color.LightGray;
            // Add a drawing canvas in the output tab page
            //...
            //otp.Controls.Add(dg);
            // Add the tab page to the tab control
            tc.TabPages.Add(otp);
            //*****]


            //[***** Add the processing info output tab page
            otp = new TabPage();
            otp.Name = "info";
            otp.Text = "Debug";
            otp.BackColor = Color.DarkGray;
            // Add a RichTextBox in the output tab page
            rtb = CreateRichTextBox(Color.DarkGray, Color.White, true);
            otp.Controls.Add(rtb);
            // Add the tab page to the tab control
            tc.TabPages.Add(otp);
            //*****]


            splitContainer.Panel2.Controls.Add(tc);
            //*************]


            return spname;
        }

















        public void CreateNewTextFileTab(string TextFileFullPath, bool WrapText)
        {
            if (string.IsNullOrEmpty(TextFileFullPath)) return;

            try
            {
                Path.GetFullPath(TextFileFullPath);
            }
            catch (Exception)
            {
                string msgStr = "Invalid path to the text file.";
                MessageBox.Show(msgStr, "Error - couldn't open text file", MessageBoxButtons.OK);
                return;
            }


            if (File.Exists(TextFileFullPath) == false)
            {
                string msgStr = "Invalid path to the text file.";
                MessageBox.Show(msgStr, "Error - couldn't open text file", MessageBoxButtons.OK);
                return;
            }


            FileInfo fi = new FileInfo(TextFileFullPath);

            if (fi.Length > MaxTextFileViewerFileSize)
            {
                string msgStr = "The text file is too large to open (max 1MB).";
                MessageBox.Show(msgStr, "Error - couldn't open file", MessageBoxButtons.OK);
                return;
            }


            string tname = Path.GetFileName(TextFileFullPath);


            TabPage tab = new TabPage();
            tab.Text = tname;
            tab.Name = TabNamePrefixForTextFiles + tname;



            MF_TabControl.TabPages.Add(tab);

            MF_TabControl.SelectedTab = tab;
            tab.Focus();




            //[************* Add a RichTextBox in the tab
            RichTextBox rtb = CreateRichTextBox(Color.White, Color.Black, false);
            rtb.ReadOnly = true;
            rtb.WordWrap = WrapText;

            // Read the contents of the file
            string tfl = File.ReadAllText(TextFileFullPath);
            if (string.IsNullOrEmpty(tfl) == false)
            {
                rtb.Text = tfl;
            }

            tab.Controls.Add(rtb);
            //*************]






        }















        private void PrimaryRTBs_TextChangedEvent(object sender, EventArgs e)
        {
            if (FileIsOpening) return;

            RichTextBox rtb = (RichTextBox)sender;
            TabPage tp = GetControlObj_CurrentScratchpad_TabPage();


            if (tp.Text.Contains('*') == false)
            {
                string t = tp.Text;
                int u = LOpenFilesList.FindIndex(x => string.Equals(x.Filename, t, StringComparison.OrdinalIgnoreCase));

                t = "*" + t;
                tp.Text = t;
                tp.Name = t;


                if (u > -1)
                {
                    LOpenFilesList[u].Filename = t;
                }
                else
                {
                    OOpenFilesList oofi = new OOpenFilesList();
                    oofi.Filename = t;
                    oofi.Path = string.Empty;

                    LOpenFilesList.Add(oofi);
                }

            }

        }










        private DataGridView CreateDataGridView()
        {
            DataGridView dg = new DataGridView();

            dg.Dock = DockStyle.Fill;
            dg.Location = new Point(3, 3);
            dg.Margin = new Padding(3, 3, 3, 3);
            dg.ScrollBars = ScrollBars.Both;
            dg.BackColor = Color.LightGray;
            dg.ReadOnly = true;
            dg.Visible = true;
            dg.AllowUserToAddRows = false;
            dg.AllowUserToResizeColumns = true;
            dg.AllowUserToOrderColumns = false;

            return dg;
        }









        private RichTextBox CreateRichTextBox(Color BackgroundColour, Color TextForeColour, bool ReadOnly)
        {
            RichTextBox rtb = new RichTextBox();

            rtb.Dock = DockStyle.Fill;
            rtb.Location = new Point(3, 3);
            rtb.Margin = new Padding(3, 3, 3, 3);
            rtb.ScrollBars = RichTextBoxScrollBars.Both;
            rtb.BackColor = BackgroundColour;
            rtb.ReadOnly = ReadOnly;
            rtb.Multiline = true;
            rtb.WordWrap = false;
            rtb.Visible = true;
            //[************************** The RichTextBox formatting properties
            Font rFont = new Font(FontFamily.GenericMonospace, 10);
            rtb.ForeColor = TextForeColour;
            rtb.Font = rFont;
            //**************************]

            return rtb;
        }






        private void MF_MenuStrip_File_New_Click(object sender, EventArgs e)
        {
            CreateNewScratchpad(true);
        }






        private void MF_Toolbar_Run_Click(object sender, EventArgs e)
        {
            DoRun_Wrapper();
        }







        private void DoRun_Wrapper()
        {
            if (this.backgroundWorker1.IsBusy) return;

            // Don't run anything if the tab is a text file viewer (primarily for internal documentation)
            if (MF_TabControl.SelectedTab.Name.StartsWith(TabNamePrefixForTextFiles)) return;


            MF_Toolbar_Run.Enabled = false;
            MF_Toolbar_Stop.Enabled = true;

            CurrentlyExecuting_Scratchpad = GetControlObj_CurrentScratchpad_TabPage();
            CurrentlyExecuting_rtb = GetControlObj_CurrentScratchpad_PrimaryInputTextBox();
            CurrentlyExecuting_TabPage_Info = GetControlObj_CurrentScratchpad_OutputTabPage_Info();
            CurrentlyExecuting_TabPage_Output_Text = GetControlObj_CurrentScratchpad_OutputTabPage_OutputText();
            CurrentlyExecuting_TabPage_Output_Grid = GetControlObj_CurrentScratchpad_OutputTabPage_OutputGrid();
            CurrentlyExecuting_TabPage_Output_Graph = GetControlObj_CurrentScratchpad_OutputTabPage_OutputGraph();

            CurrentlyExecuting_rtb.Enabled = false;
            CurrentlyExecuting_rtb.Cursor = Cursors.WaitCursor;
            CurrentlyExecuting_rtb.UseWaitCursor = true;





            bool kr = DoRun();


            if (kr == false)
            {
                MF_Toolbar_Run.Enabled = true;
                MF_Toolbar_Stop.Enabled = false;
                CurrentlyExecuting_rtb.Enabled = true;
                CurrentlyExecuting_rtb.Cursor = Cursors.IBeam;
                CurrentlyExecuting_rtb.UseWaitCursor = false;
            }
        }










        private OBWResult RunDataProcessing(List<string> LS)
        {
            // LS should be rtb.Lines.ToList() of the RichTextBox currently being executed.

            OActivityState AState = new OActivityState();
            ofr sr = new ofr();

            // Run the processing
            sr = DataProcessing.RunFullProcess(ref LS, ref AState);


            OBWResult ret = new OBWResult();
            ret.sr = sr;
            ret.AState = AState;


            // Clear the Activity State object
            AState = new OActivityState();

            return ret;
        }












        private bool PostDataProcessing(ref OBWResult bwr)
        {
            bool ret = true;

            //RichTextBox rtb = GetControlObj_CurrentScratchpad_PrimaryInputTextBox();
            //TabPage tp = GetControlObj_CurrentScratchpad_OutputTabPage_Info();
            //List<string> LS = rtb.Lines.ToList();
            //OActivityState AState = bwr.AState;
            //ofr sr = bwr.sr;


            string t = string.Empty;
            RichTextBox rtb_info = (RichTextBox)(CurrentlyExecuting_TabPage_Info.Controls[0]);


            if (bwr.sr.PrimaryReturnValue != RefReturnValues.Success)
            {


                if (bwr.sr.ProcessingPoint == "Execute command")
                {
                    if (string.IsNullOrEmpty(bwr.AState.ActivityName))
                    {
                        t += Environment.NewLine;
                        t += Environment.NewLine + "----------";
                        t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                        t += " | Error: No command specified.";
                    }
                    else
                    {
                        t += Environment.NewLine;
                        t += Environment.NewLine + "----------";
                        t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                        t += " | Error: There was a problem while executing the command. Further details:";
                        t += Environment.NewLine;
                        t += Environment.NewLine + bwr.sr.ChildReturnObject.AdditionalInfo;
                        //t += Environment.NewLine + "- Primary return value: " + sr.ChildReturnObject.PrimaryReturnValue.ToString();
                        //t += Environment.NewLine + "- In method: " + sr.ChildReturnObject.FunctionName;
                        //t += Environment.NewLine + "- Information: " + sr.ChildReturnObject.AdditionalInfo;
                        if (bwr.sr.ChildReturnObject.ErrorMessage != null)
                        {
                            if (string.IsNullOrEmpty(bwr.sr.ChildReturnObject.ErrorMessage) == false)
                            {
                                t += Environment.NewLine + "- Error message: " + bwr.sr.ChildReturnObject.ErrorMessage;
                                if (bwr.sr.ChildReturnObject.ErrorDetails != null)
                                {
                                    if (string.IsNullOrEmpty(bwr.sr.ChildReturnObject.ErrorDetails) == false)
                                    {
                                        t += Environment.NewLine + "- Error details: " + bwr.sr.ChildReturnObject.ErrorDetails;
                                    }
                                }
                            }
                        }
                        //t += Environment.NewLine;
                        //t += Environment.NewLine;
                        //t += PrepareActivityStateOutputInfo_PostRun(ref AState);
                        t += Environment.NewLine;
                    }
                }
                else
                {

                    t += Environment.NewLine;
                    t += Environment.NewLine + "----------";
                    t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                    t += " | " + bwr.sr.AdditionalInfo;
                    t += Environment.NewLine;
                }

                rtb_info.Text += t;

                // Scroll to the end of the text box
                rtb_info.SelectionStart = rtb_info.Text.Length;
                rtb_info.ScrollToCaret();

                SetActiveOutputTabPage_CurrentlyExecutingScratchpad_Info();

                CurrentlyExecuting_rtb.Focus();

                return ret;
            }
            else
            {


                //if (AState.Completed == false)
                //{
                //    if (string.IsNullOrEmpty(AState.ActivityName))
                //    {
                //        t += Environment.NewLine;
                //        t += Environment.NewLine + "----------";
                //        t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                //        t += " | Error: No command specified.";
                //    }
                //    else
                //    {
                //        t += Environment.NewLine;
                //        t += Environment.NewLine + "----------";
                //        t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                //        t += " | Error: There was a problem while executing the command. Further details:";
                //        t += Environment.NewLine;
                //        t += Environment.NewLine;
                //        t += PrepareActivityStateOutputInfo_PostRun(ref AState);
                //    }

                //    rtb_info.Text += t;

                //    // Scroll to the end of the text box
                //    rtb_info.SelectionStart = rtb_info.Text.Length;
                //    rtb_info.ScrollToCaret();

                //    SetActiveOutputTabPage_CurrentScratchpad_Info();

                //    rtb.Focus();

                //    return;
                //}


                if (bwr.AState.ofr != null)
                {
                    if (bwr.AState.ofr.PrimaryReturnValue != RefReturnValues.Success || bwr.AState.Completed == false)
                    {
                        t += Environment.NewLine;
                        t += Environment.NewLine + "----------";
                        t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                        t += " | Error: There was a problem while executing the command. Further details:";
                        t += Environment.NewLine;
                        t += Environment.NewLine + "- Processing response: " + bwr.AState.ofr.PrimaryReturnValue.ToString();
                        t += Environment.NewLine + "- In method: " + bwr.AState.ofr.FunctionName;
                        if (string.IsNullOrEmpty(bwr.AState.ofr.AdditionalInfo) == false)
                        {
                            t += Environment.NewLine + "- Information: " + bwr.AState.ofr.AdditionalInfo;
                        }
                        if (bwr.AState.OutputMessagesToUser != null)
                        {
                            if (bwr.AState.OutputMessagesToUser.Count > 0)
                            {
                                t += Environment.NewLine;
                                for (int i = 0; i < bwr.AState.OutputMessagesToUser.Count; i++)
                                {
                                    t += Environment.NewLine + bwr.AState.OutputMessagesToUser[i];
                                }
                            }
                            t += Environment.NewLine;
                        }

                        rtb_info.Text += t;

                        // Scroll to the end of the text box
                        rtb_info.SelectionStart = rtb_info.Text.Length;
                        rtb_info.ScrollToCaret();

                        SetActiveOutputTabPage_CurrentlyExecutingScratchpad_Info();

                        CurrentlyExecuting_rtb.Focus();

                        return ret;
                    }
                }
                else
                {
                    if (bwr.AState.Completed == false)
                    {
                        if (string.IsNullOrEmpty(bwr.AState.ActivityName))
                        {
                            t += Environment.NewLine;
                            t += Environment.NewLine + "----------";
                            t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                            t += " | Error: No command specified.";
                        }
                        else
                        {
                            t += Environment.NewLine;
                            t += Environment.NewLine + "----------";
                            t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                            t += " | Error: There was a problem while executing the command. Further details:";
                            t += Environment.NewLine;
                            t += Environment.NewLine;
                            t += PrepareActivityStateOutputInfo_PostRun(ref bwr.AState);
                        }

                        rtb_info.Text += t;

                        // Scroll to the end of the text box
                        rtb_info.SelectionStart = rtb_info.Text.Length;
                        rtb_info.ScrollToCaret();

                        SetActiveOutputTabPage_CurrentlyExecutingScratchpad_Info();

                        CurrentlyExecuting_rtb.Focus();

                        return ret;
                    }

                }


                //[*********** Output the processing details
                t = Environment.NewLine;
                t += Environment.NewLine + "----------";
                t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                t += " | Command completed successfully. Processing details:";
                t += Environment.NewLine;
                t += Environment.NewLine;
                t += PrepareActivityStateOutputInfo_PostRun(ref bwr.AState);

                rtb_info.Text += t;

                // Scroll to the end of the text box
                rtb_info.SelectionStart = rtb_info.Text.Length;
                rtb_info.ScrollToCaret();
                //***********]



                //[************************* Output the results
                string VMethod = ds_common.Utilities.VisualisationType_Text;
                bool kr = true;


                // Check if a visualisation method has been specified
                if (bwr.AState.OutputObj.Information != null)
                {
                    if (bwr.AState.OutputObj.Information.Count > 0)
                    {
                        switch (bwr.AState.OutputObj.Information[0])
                        {
                            case ds_common.Utilities.VisualisationType_Text:
                                VMethod = ds_common.Utilities.VisualisationType_Text;
                                break;

                            case ds_common.Utilities.VisualisationType_Grid:
                                VMethod = ds_common.Utilities.VisualisationType_Grid;
                                break;

                            case ds_common.Utilities.VisualisationType_Graph:
                                VMethod = ds_common.Utilities.VisualisationType_Graph;
                                break;
                        }
                    }
                }



                // Proceed with the rendering of the output, based on the visualisation method
                switch (VMethod)
                {
                    case ds_common.Utilities.VisualisationType_Text:
                        kr = OutputResults_Text(ref bwr.AState);
                        break;

                    case ds_common.Utilities.VisualisationType_Grid:
                        kr = OutputResults_Grid(ref bwr.AState);
                        break;

                    case ds_common.Utilities.VisualisationType_Graph:
                        break;
                }



                if (kr == false)
                {
                    t = Environment.NewLine;
                    t += Environment.NewLine + "*** While the command processing completed successfully, there was a problem rendering the output ***";
                    t += Environment.NewLine + "This is most likely due to a bug in the code for the command where the output data are collated.";
                    t += Environment.NewLine + "Please report this to the developer.";

                    rtb_info.Text += t;

                    // Scroll to the end of the text box
                    rtb_info.SelectionStart = rtb_info.Text.Length;
                    rtb_info.ScrollToCaret();

                    SetActiveOutputTabPage_CurrentlyExecutingScratchpad_Info();

                    CurrentlyExecuting_rtb.Focus();
                    return ret;
                }



                SetFocus_CurrentlyExecutingScratchpad_PrimaryInputTextBox();
                //*************************]
            }


            return ret;
        }







        private void BackgroundWorker1_DoWork(object? sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;


            // Start the time-consuming operation.
            e.Result = RunDataProcessing(CurrentCommandSet);

        }







        private void BackgroundWorker1_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            CurrentlyExecuting_rtb.Enabled = true;
            CurrentlyExecuting_rtb.UseWaitCursor = false;
            CurrentlyExecuting_rtb.Cursor = Cursors.IBeam;

            MF_Toolbar_Stop.Enabled = false;
            MF_Toolbar_Run.Enabled = true;

            if (e.Result != null)
            {
                OBWResult bwr = (OBWResult)e.Result;


                // Check if the process was cancelled by the user
                if (bwr.sr.PrimaryReturnValue == RefReturnValues.CancelledByUser)
                {
                    // The user cancelled the operation.

                    //[******** Report the cancellation on the info tab
                    //TabPage tp = GetControlObj_CurrentScratchpad_OutputTabPage_Info();
                    RichTextBox rtb_info = (RichTextBox)(CurrentlyExecuting_TabPage_Info.Controls[0]);
                    string t;

                    t = Environment.NewLine;
                    t += Environment.NewLine + "----------";
                    t += Environment.NewLine + DateTime.Now.ToString(dtfs_ToSecondsPrecision);
                    t += " | Command execution was cancelled by the user.";
                    t += Environment.NewLine;
                    t += bwr.AState.ProcessedRowsCount.ToString() + " rows processed during the execution period.";
                    t += Environment.NewLine;


                    rtb_info.Text += t;

                    // Scroll to the end of the text box
                    rtb_info.SelectionStart = rtb_info.Text.Length;
                    rtb_info.ScrollToCaret();

                    SetActiveOutputTabPage_CurrentlyExecutingScratchpad_Info();
                    //********]


                }
                else
                {
                    // The operation completed normally.
                    PostDataProcessing(ref bwr);
                }

                // Clear the memory object
                bwr = new OBWResult();
            }
        }









        private bool DoRun()
        {
            RichTextBox rtb = GetControlObj_CurrentScratchpad_PrimaryInputTextBox();

            //[****** Do basic empty content checks (more advanced ones will be run in the Data Processing module)
            if (string.IsNullOrEmpty(rtb.Text)) return false;
            if (rtb.TextLength == 0) return false;
            //******]

            CurrentCommandSet = rtb.Lines.ToList();

            int rtbCaretPos = rtb.SelectionStart;

            if (rtb.Text.Contains("\v"))
            {
                rtb.Text = rtb.Text.Replace("\v", Environment.NewLine);
                rtb.SelectionStart = rtbCaretPos;
            }

            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.RunWorkerAsync();

            return true;
        }












        private bool OutputResults_Grid(ref OActivityState AState)
        {
            bool ret = false;
            if (AState == null) return ret;
            if (AState.OutputObj == null) return ret;
            if (AState.OutputObj.PrimaryOutput == null) return ret;
            if (AState.OutputObj.PrimaryOutput.Count == 0) return ret;


            TabPage tb_Output = GetControlObj_CurrentScratchpad_OutputTabPage_OutputGrid();
            DataGridView dg = (DataGridView)(tb_Output.Controls[0]);
            List<object> LFields = new List<object>();

            const int OutputRowsLimit = 10000;

            dg.Columns.Clear();
            dg.Rows.Clear();
            dg.Refresh();

            DataGridViewColumn dgCol;
            DataGridViewRow dgRow;
            DataGridViewCell dgCell = new DataGridViewTextBoxCell();


            //[**** Set the default data cell style
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            CellStyle.NullValue = "NULL";
            CellStyle.BackColor = Color.White;
            CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            //****]

            //[**** Set the default row header cell style
            DataGridViewCellStyle RowHeaderCellStyle = new DataGridViewCellStyle();
            RowHeaderCellStyle.NullValue = string.Empty;
            RowHeaderCellStyle.BackColor = Color.LightGray;
            RowHeaderCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            //****]

            //[**** Set the default column header cell style
            DataGridViewCellStyle ColumnHeaderCellStyle = new DataGridViewCellStyle();
            ColumnHeaderCellStyle.NullValue = string.Empty;
            ColumnHeaderCellStyle.BackColor = Color.LightGray;
            ColumnHeaderCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****]



            // Use a try here, primarily to catch the scenario where the PrimaryOutput objects can't be cast into lists of objects.
            try
            {
                // Loop over the Primary Output objects
                for (int i = 0; i < AState.OutputObj.PrimaryOutput.Count; i++)
                {
                    LFields = (List<object>)AState.OutputObj.PrimaryOutput[i];

                    dgRow = new DataGridViewRow();
                    dgRow.Resizable = DataGridViewTriState.False;
                    dgRow.HeaderCell.Value = (i + 1).ToString();
                    dgRow.HeaderCell.Style = RowHeaderCellStyle;


                    if (LFields.Count > 0)
                    {
                        // Loop over the fields in the row
                        for (int j = 0; j < LFields.Count; j++)
                        {
                            dgCell = new DataGridViewTextBoxCell();
                            dgCell.Style = CellStyle;

                            // Set the cell value
                            dgCell.Value = LFields[j];


                            // If new columns need to be added, then add them
                            if (dg.Columns.Count < j + 1)
                            {
                                dgCol = new DataGridViewColumn(dgCell);
                                dgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                dgCol.Width = 100;
                                dgCol.HeaderCell.Style = ColumnHeaderCellStyle;
                                dgCol.SortMode = DataGridViewColumnSortMode.NotSortable;

                                if (AState.SourceOriginalFieldHeaders != null)
                                {
                                    if (AState.SourceOriginalFieldHeaders.Count >= j)
                                    {
                                        dgCol.HeaderText = AState.SourceOriginalFieldHeaders[j];
                                    }
                                    else
                                    {
                                        dgCol.HeaderText = "Column " + (j + 1).ToString();
                                    }
                                }
                                else
                                {
                                    dgCol.HeaderText = "Column " + (j + 1).ToString();
                                }

                                // Add the column
                                dg.Columns.Add(dgCol);
                            }

                            // Add the cell to the row
                            dgRow.Cells.Add(dgCell);
                        }


                        // Add the row of data
                        dg.Rows.Add(dgRow);
                    }
                    else
                    {
                        //[******** If there are no records in the row

                        dgCell = new DataGridViewTextBoxCell();
                        dgCell.Style = CellStyle;

                        // Set the cell value
                        dgCell.Value = string.Empty;

                        // Create a column if one doesn't already exist
                        if (dg.Columns.Count == 0)
                        {
                            // Define the column
                            dgCol = new DataGridViewColumn(dgCell);
                            dgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgCol.Width = 100;
                            dgCol.HeaderCell.Style = ColumnHeaderCellStyle;
                            dgCol.SortMode = DataGridViewColumnSortMode.NotSortable;

                            if (AState.SourceOriginalFieldHeaders != null)
                            {
                                if (AState.SourceOriginalFieldHeaders.Count > 0)
                                {
                                    dgCol.HeaderText = AState.SourceOriginalFieldHeaders[0];
                                }
                                else
                                {
                                    dgCol.HeaderText = "Column 1";
                                }
                            }
                            else
                            {
                                dgCol.HeaderText = "Column 1";
                            }

                            // Add the column
                            dg.Columns.Add(dgCol);
                        }

                        // Add the cell to the row
                        dgRow.Cells.Add(dgCell);

                        // Add the row of data
                        dg.Rows.Add(dgRow);
                        //********]
                    }

                    if (i >= OutputRowsLimit) break;
                }
            }
            catch (Exception)
            {
                return ret;
            }



            SetActiveOutputTabPage_CurrentlyExecutingScratchpad_OutputGrid();

            dg.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            dg.ColumnHeadersHeight = 50;

            ret = true;
            return ret;

        }











        private bool OutputResults_Text(ref OActivityState AState)
        {
            bool ret = false;
            if (AState == null) return ret;
            if (AState.OutputObj == null) return ret;
            if (AState.OutputObj.PrimaryOutput == null) return ret;
            if (AState.OutputObj.PrimaryOutput.Count == 0) return ret;



            TabPage tb_Output = GetControlObj_CurrentScratchpad_OutputTabPage_OutputText();
            RichTextBox rtb_output_text = (RichTextBox)(tb_Output.Controls[0]);
            StringBuilder sb = new StringBuilder();
            StringBuilder sbPO = new StringBuilder();


            if (string.IsNullOrEmpty(rtb_output_text.Text) == false)
            {
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine + "----------");
                sb.Append(Environment.NewLine);
            }
            sb.Append(DateTime.Now.ToString(dtfs_ToSecondsPrecision));
            sb.Append(" | Output of command \"" + AState.ActivityName + "\":");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);


            // Use a try here, primarily to catch the scenario where the PrimaryOutput objects can't be cast into strings.
            try
            {
                // Loop over the Primary Output objects
                for (int i = 0; i < AState.OutputObj.PrimaryOutput.Count; i++)
                {
                    sbPO = new StringBuilder((string)AState.OutputObj.PrimaryOutput[i]);

                    if (string.Equals(AState.ActivityName, DataProcessing.CommandName_INSPECT, StringComparison.OrdinalIgnoreCase) | string.Equals(AState.ActivityName, DataProcessing.CommandName_CODE_POINT_INSPECT, StringComparison.OrdinalIgnoreCase) | string.Equals(AState.ActivityName, DataProcessing.CommandName_BYTE_INSPECT, StringComparison.OrdinalIgnoreCase))
                    {
                        sbPO.Replace("\n", @"[\n]");
                        sbPO.Replace("\r", @"[\r]");
                        sbPO.Replace("\v", @"[\v]");
                        sbPO.Replace("\0", @"[NUL]");
                        sbPO.Replace("\u0080", @"[PAD]");

                        if (string.Equals(AState.ActivityName, DataProcessing.CommandName_CODE_POINT_INSPECT, StringComparison.OrdinalIgnoreCase))
                        {
                            sbPO.Replace("\t", @"[\t]");
                        }
                    }

                    sb.Append(Environment.NewLine);
                    sb.Append(sbPO.ToString());
                }
                sb.Append(Environment.NewLine);
            }
            catch (Exception)
            {
                return ret;
            }


            rtb_output_text.Text += sb.ToString();

            // Scroll to the end of the text box
            rtb_output_text.SelectionStart = rtb_output_text.Text.Length;
            rtb_output_text.ScrollToCaret();


            SetActiveOutputTabPage_CurrentlyExecutingScratchpad_OutputText();


            ret = true;
            return ret;
        }











        private string PrepareActivityStateOutputInfo_PostRun(ref OActivityState AState)
        {
            string ret = string.Empty;

            ret += "- Command: " + AState.ActivityName;

            if (AState.Started)
            {
                ret += Environment.NewLine + "- Execution start: " + AState.StartedDT.ToString(dtfs_ToMillisecondsPrecision);
            }
            else
            {
                ret += Environment.NewLine + "- Execution did not start";
                return ret;
            }

            if (AState.HasInitialisationCompleted)
            {
                ret += Environment.NewLine + "- Initialisation process complete";
            }
            else
            {
                ret += Environment.NewLine + "- Initialisation process did not complete";
                return ret;
            }

            if (AState.DataExtractComplete)
            {
                ret += Environment.NewLine + "- Data extraction complete: " + AState.DataExtractCompleteDT.ToString(dtfs_ToMillisecondsPrecision);
            }
            else
            {
                ret += Environment.NewLine + "- Data extraction did not complete";
                return ret;
            }

            ret += Environment.NewLine + "- Number of rows processed: " + AState.ProcessedRowsCount.ToString("#,0");

            if (AState.Completed)
            {
                ret += Environment.NewLine + "- Execution complete: " + AState.CompletedDT.ToString(dtfs_ToMillisecondsPrecision);
            }
            else
            {
                ret += Environment.NewLine + "- Execution did not complete";
                return ret;
            }

            ret += Environment.NewLine;

            return ret;
        }











        private void DoFormatInputText()
        {

            RichTextBox rtb = GetControlObj_CurrentScratchpad_PrimaryInputTextBox();

            List<string> LS = rtb.Lines.ToList();

            if (LS == null) return;
            if (LS.Count == 0) return;

            int p;
            string ltr1;
            string ltr2;

            for (int i = 0; i < LS.Count; i++)
            {
                ltr1 = LS[i].Trim();
                if (string.IsNullOrEmpty(ltr1) == false)
                {


                    //[********** Check for command names
                    for (int j = 0; j < DataProcessing.LCommandNames_Broad.Count; j++)
                    {
                        if (string.Equals(ltr1, ":" + DataProcessing.LCommandNames_Broad[j], StringComparison.OrdinalIgnoreCase) || string.Equals(ltr1.Replace(" ", String.Empty), ":" + DataProcessing.LCommandNames_Broad[j] + "{}", StringComparison.OrdinalIgnoreCase))
                        {
                            RTB_ChangeWholeLineColour(ref rtb, LS[i], i, Color.Navy);
                        }
                    }
                    //**********]



                    //[************* Check for brace pairs
                    if (string.Equals(ltr1, "{"))
                    {
                        p = -1;
                        for (int j = i + 1; j < LS.Count; j++)
                        {
                            ltr2 = LS[j].Trim();
                            if (ltr2.Trim().StartsWith(":")) break;
                            if (string.Equals(ltr2.Trim(), "}"))
                            {
                                p = j;
                                break;
                            }
                        }
                        if (p > i)
                        {
                            RTB_ChangeWholeLineColour(ref rtb, LS[i], i, Color.Navy);

                            RTB_ChangeWholeLineColour(ref rtb, LS[p], p, Color.Navy);
                        }
                    }
                    //*************]



                    //[************* Check for parameters (generic)
                    if (ltr1.Contains("="))
                    {
                        p = LS[i].IndexOf("=");
                        if (p > 0)
                        {
                            RTB_ChangePartialLineColour(ref rtb, i, 0, p, Color.DarkOliveGreen);
                            if (p + 1 < LS[i].Length)
                            {
                                RTB_ChangePartialLineColour(ref rtb, i, p, LS[i].Length - 1, Color.Black);
                            }
                        }
                    }
                    //*************]

                }
            }


        }








        private void RTB_ChangePartialLineColour(ref RichTextBox rtb, int LineID, int StartPos, int EndPos, Color clr)
        {
            // The LineID must be the zero-based ID of the line of text in the RichTextBox.Lines array.
            // StartPos and EndPos must be character positions relative to the start of the line of text

            //int u = rtb.Find(WholeLineStr, RichTextBoxFinds.NoHighlight);

            int u = rtb.GetFirstCharIndexFromLine(LineID);
            if (u > -1)
            {
                // Save the original caret position
                int p = rtb.SelectionStart;

                // Set the selection and change the colour
                rtb.Select(u + StartPos, EndPos - StartPos);
                rtb.SelectionColor = clr;
                rtb.SelectionLength = 0;

                // Set the caret position to the original
                rtb.SelectionStart = p;
            }
        }







        private void RTB_ChangeWholeLineColour(ref RichTextBox rtb, string WholeLineStr, int LineID, Color clr)
        {
            // The WholeLineStr must be the exact, full line of text taken from the RichTextBox (using, e.g., RichTextBox.Lines).
            // The LineID must be the zero-based ID of the line of text in the RichTextBox.Lines array.

            int u = rtb.GetFirstCharIndexFromLine(LineID);
            //int u = rtb.Find(WholeLineStr, RichTextBoxFinds.None);
            if (u > -1)
            {
                // Save the original caret position
                int p = rtb.SelectionStart;

                // Set the selection and change the colour
                rtb.Select(u, WholeLineStr.Length);
                rtb.SelectionColor = clr;
                rtb.SelectionLength = 0;

                // Set the caret position to the original
                rtb.SelectionStart = p;
            }
        }










        private TabPage GetControlObj_CurrentScratchpad_TabPage()
        {
            return MF_TabControl.SelectedTab;
        }


        private RichTextBox GetControlObj_CurrentScratchpad_PrimaryInputTextBox()
        {
            return (RichTextBox)((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel1.Controls[0];
        }


        private TabPage GetControlObj_CurrentScratchpad_OutputTabPage_OutputText()
        {
            return ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).TabPages[TabName_Output_Text];
        }

        private TabPage GetControlObj_CurrentScratchpad_OutputTabPage_OutputGrid()
        {
            return ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).TabPages[TabName_Output_Grid];
        }

        private TabPage GetControlObj_CurrentScratchpad_OutputTabPage_OutputGraph()
        {
            return ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).TabPages[TabName_Output_Graph];
        }

        private TabPage GetControlObj_CurrentScratchpad_OutputTabPage_Info()
        {
            return ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).TabPages[TabName_Info];
        }



        private void SetFocus_CurrentScratchpad_PrimaryInputTextBox()
        {
            GetControlObj_CurrentScratchpad_PrimaryInputTextBox().Focus();
        }


        private void SetFocus_CurrentlyExecutingScratchpad_PrimaryInputTextBox()
        {
            CurrentlyExecuting_rtb.Focus();
        }



        private void SetActiveOutputTabPage_CurrentScratchpad_OutputText()
        {
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = GetControlObj_CurrentScratchpad_OutputTabPage_OutputText();
        }

        private void SetActiveOutputTabPage_CurrentScratchpad_OutputGrid()
        {
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = GetControlObj_CurrentScratchpad_OutputTabPage_OutputGrid();
        }

        private void SetActiveOutputTabPage_CurrentScratchpad_OutputGraph()
        {
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = GetControlObj_CurrentScratchpad_OutputTabPage_OutputGraph();
        }

        private void SetActiveOutputTabPage_CurrentScratchpad_Info()
        {
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = GetControlObj_CurrentScratchpad_OutputTabPage_Info();
        }



        private void SetActiveOutputTabPage_CurrentlyExecutingScratchpad_Info()
        {
            MF_TabControl.SelectedTab = CurrentlyExecuting_Scratchpad;
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = CurrentlyExecuting_TabPage_Info;
        }

        private void SetActiveOutputTabPage_CurrentlyExecutingScratchpad_OutputText()
        {
            MF_TabControl.SelectedTab = CurrentlyExecuting_Scratchpad;
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = CurrentlyExecuting_TabPage_Output_Text;
        }

        private void SetActiveOutputTabPage_CurrentlyExecutingScratchpad_OutputGrid()
        {
            MF_TabControl.SelectedTab = CurrentlyExecuting_Scratchpad;
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = CurrentlyExecuting_TabPage_Output_Grid;
        }

        private void SetActiveOutputTabPage_CurrentlyExecutingScratchpad_OutputGraph()
        {
            MF_TabControl.SelectedTab = CurrentlyExecuting_Scratchpad;
            ((TabControl)(((SplitContainer)MF_TabControl.SelectedTab.Controls[0]).Panel2.Controls[0])).SelectedTab = CurrentlyExecuting_TabPage_Output_Graph;
        }






        private void DoStopExecution()
        {
            if (this.backgroundWorker1.IsBusy)
            {
                MF_Toolbar_Stop.Enabled = false;

                SharedVars.StopProcessingRequestedByUser = true;
                this.backgroundWorker1.CancelAsync();
            }
        }








        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                if (GetControlObj_CurrentScratchpad_PrimaryInputTextBox().Focused)
                {
                    DoRun_Wrapper();
                    return;
                }
            }



            if (e.KeyCode == Keys.C & e.Modifiers == Keys.Control)
            {
                if (MF_Toolbar_Stop.Enabled == true)
                {
                    DoStopExecution();
                    return;
                }
            }



            if (e.KeyCode == Keys.Enter || (e.KeyCode == Keys.V & e.Modifiers == Keys.Control) || (e.KeyCode == Keys.Insert & e.Modifiers == Keys.Shift))
            {
                if (GetControlObj_CurrentScratchpad_TabPage().Name.StartsWith(TabNamePrefixForTextFiles) == false)
                {
                    if (GetControlObj_CurrentScratchpad_PrimaryInputTextBox().Focused)
                    {
                        DoFormatInputText();
                        return;
                    }
                }
            }


            //if (e.KeyCode == Keys.O & e.Modifiers == Keys.Control)
            //{
            //    DoOpenScratchpad();
            //}

            //if (e.KeyCode == Keys.N & e.Modifiers == Keys.Control)
            //{
            //    CreateNewScratchpad();
            //}

            //if (e.KeyCode == Keys.S & e.Modifiers == Keys.Control)
            //{
            //    DoSaveTab();
            //}

        }










        private void MF_Toolbar_Stop_Click(object sender, EventArgs e)
        {
            DoStopExecution();
        }



        private class OBWResult
        {
            public ofr sr;
            public OActivityState AState;
        }


        private class OOpenFilesList
        {
            public string Path;
            public string Filename;
        }










        private void MF_MenuStrip_File_CloseTab_Click(object sender, EventArgs e)
        {
            DoCloseTab();
        }











        private void DoCloseTab()
        {
            TabPage tp = GetControlObj_CurrentScratchpad_TabPage();

            bool ContClose = true;


            // Check if file has been edited
            if (tp.Text.Contains('*'))
            {
                DialogResult res = MessageBox.Show("Do you want to save this Scratchpad before closing it? (Yes will open the Save As... dialog)", "Save Scratchpad?", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Cancel)
                {
                    ContClose = false;
                }
                else
                {
                    if (res == DialogResult.Yes)
                    {
                        bool kr = DoSaveAsTab();
                        if (kr == false) ContClose = false;
                    }
                }
            }

            if (ContClose == false) return;


            // If there an entry for the tab page text in the list of open files, then remove that
            int u = LOpenFilesList.FindIndex(x => string.Equals(x.Filename, tp.Text, StringComparison.OrdinalIgnoreCase));
            if (u > -1)
            {
                LOpenFilesList.RemoveAt(u);
            }


            // Remove the tab page
            MF_TabControl.TabPages.Remove(tp);


            // Create a new Scratchpad if all other tabs have been closed
            if (MF_TabControl.TabPages.Count == 0)
            {
                CreateNewScratchpad(true);
            }
        }








        private void MF_MenuStrip_File_Save_Click(object sender, EventArgs e)
        {
            DoSaveTab();
        }










        private void DoSaveTab()
        {
            TabPage tp = GetControlObj_CurrentScratchpad_TabPage();
            string t = tp.Text;

            if (t.Contains('*') == false) return;

            //if (t.Contains(NewScratchpadNamePrefix, StringComparison.OrdinalIgnoreCase))
            //{
            //    DoSaveAsTab();
            //    return;
            //}


            int u = LOpenFilesList.FindIndex(x => string.Equals(x.Filename, t, StringComparison.OrdinalIgnoreCase));

            if (u > -1)
            {
                if (string.IsNullOrEmpty(LOpenFilesList[u].Path))
                {
                    DoSaveAsTab();
                    return;
                }
            }
            else
            {
                OOpenFilesList oofi = new OOpenFilesList();
                oofi.Filename = t;
                oofi.Path = String.Empty;
                LOpenFilesList.Add(oofi);

                DoSaveAsTab();
                return;
            }


            t = t.Replace("*", string.Empty);
            string p = LOpenFilesList[u].Path;
            string FullPath = Path.Combine(p, t);
            RichTextBox rtb = GetControlObj_CurrentScratchpad_PrimaryInputTextBox();


            if (Directory.Exists(p) == false)
            {
                DoSaveAsTab();
                return;
            }


            // Try to write the file
            try
            {
                File.WriteAllLines(FullPath, rtb.Lines);
            }
            catch (Exception e)
            {
                string msgStr = "There was a problem saving the Scratchpad commands to the existing file. Error message below.";
                msgStr += Environment.NewLine + Environment.NewLine;
                msgStr += e.Message;
                MessageBox.Show(msgStr, "Error - couldn't save file", MessageBoxButtons.OK);
                return;
            }

            // Update the tab page and list with the modified (no asterisk) file name
            tp.Text = t;
            tp.Name = t;
            LOpenFilesList[u].Filename = t;

            LastUsedSaveDirectory = p;

        }













        private bool DoSaveAsTab()
        {
            // Return true if the file was ultimately saved

            TabPage tp = GetControlObj_CurrentScratchpad_TabPage();
            string t = tp.Text;
            string p = string.Empty;
            int u = LOpenFilesList.FindIndex(x => string.Equals(x.Filename, t, StringComparison.OrdinalIgnoreCase));
            if (u > -1)
            {
                p = LOpenFilesList[u].Path;
            }

            //[********** Create and configure the save dialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Scratchpad files (*." + ScratchpadFileExtension + ")|*." + ScratchpadFileExtension + "|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FileName = t.Replace("*", string.Empty);
            sfd.AddExtension = true;
            sfd.DefaultExt = ScratchpadFileExtension;
            sfd.CheckPathExists = true;
            sfd.FilterIndex = 0;
            if (string.IsNullOrEmpty(p) == false)
            {
                sfd.InitialDirectory = p;
            }
            else
            {
                sfd.InitialDirectory = LastUsedSaveDirectory;
            }
            sfd.OverwritePrompt = true;
            sfd.SupportMultiDottedExtensions = true;
            sfd.Title = "Save Scratchpad file";
            sfd.ValidateNames = true;
            //**********]

            DialogResult res = sfd.ShowDialog();

            if (res == DialogResult.Cancel) return false;

            RichTextBox rtb = GetControlObj_CurrentScratchpad_PrimaryInputTextBox();
            string sfd_filepath = sfd.FileName;


            // Try to write the file
            try
            {
                File.WriteAllLines(sfd_filepath, rtb.Lines);
            }
            catch (Exception e)
            {
                string msgStr = "There was a problem saving the Scratchpad commands to the existing file. Error message below.";
                msgStr += Environment.NewLine + Environment.NewLine;
                msgStr += e.Message;
                MessageBox.Show(msgStr, "Error - couldn't save file", MessageBoxButtons.OK);
                return false;
            }

            t = Path.GetFileName(sfd_filepath);
            p = Path.GetDirectoryName(sfd_filepath);

            if (p == null) p = string.Empty;

            tp.Text = t;
            tp.Name = t;
            LOpenFilesList[u].Filename = t;
            LOpenFilesList[u].Path = p;

            if (string.IsNullOrEmpty(p)) p = Environment.CurrentDirectory;

            LastUsedSaveDirectory = p;

            return true;
        }








        private void MF_MenuStrip_File_SaveAs_Click(object sender, EventArgs e)
        {
            DoSaveAsTab();
        }








        private void MF_MenuStrip_File_Open_Click(object sender, EventArgs e)
        {
            DoOpenScratchpad();
        }








        private void DoOpenScratchpad()
        {

            //[********* Configure the open file dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Scratchpad files (*." + ScratchpadFileExtension + ")|*." + ScratchpadFileExtension + "|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DefaultExt = ScratchpadFileExtension;
            ofd.InitialDirectory = LastUsedSaveDirectory;
            ofd.Multiselect = false;
            ofd.ShowReadOnly = false;
            ofd.SupportMultiDottedExtensions = true;
            ofd.Title = "Open Scratchpad file";
            ofd.ValidateNames = true;
            //*********]

            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.Cancel) return;

            string FullPath = ofd.FileName;

            if (string.IsNullOrEmpty(FullPath)) return;


            // Try to load the Scratchpad file
            LoadScratchpad(FullPath);
        }








        private bool LoadScratchpad(string FullPath)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(FullPath)) return ret;

            try
            {
                Path.GetFullPath(FullPath);
            }
            catch (Exception)
            {
                string msgStr = "Invalid path to the Scratchpad file.";
                MessageBox.Show(msgStr, "Error - couldn't open file", MessageBoxButtons.OK);
                return ret;
            }

            if (File.Exists(FullPath) == false)
            {
                string msgStr = "The selected Scratchpad file doesn't exist.";
                MessageBox.Show(msgStr, "Error - couldn't open file", MessageBoxButtons.OK);
                return ret;
            }

            string f = Path.GetFileName(FullPath);
            string p = Path.GetDirectoryName(FullPath);
            if (p == null) p = string.Empty;
            string t = CreateNewScratchpad(false);
            string sptext = string.Empty;

            FileInfo fi = new FileInfo(FullPath);

            if (fi.Length > MaxScratchpadFileSize)
            {
                string msgStr = "The selected Scratchpad file is too large to open (max 1MB).";
                MessageBox.Show(msgStr, "Error - couldn't open file", MessageBoxButtons.OK);
                return ret;
            }


            try
            {
                sptext = File.ReadAllText(FullPath);
            }
            catch (Exception e)
            {
                string msgStr = "There was a problem opening the Scratchpad file. Error message below.";
                msgStr += Environment.NewLine + Environment.NewLine;
                msgStr += e.Message;
                MessageBox.Show(msgStr, "Error - couldn't open file", MessageBoxButtons.OK);
                return ret;
            }


            TabPage tp = MF_TabControl.TabPages[t];
            int u = LOpenFilesList.FindIndex(x => string.Equals(x.Filename, t, StringComparison.OrdinalIgnoreCase));

            // Update the tab
            tp.Text = f;
            tp.Name = f;

            // Update the list entry
            if (u > -1)
            {
                LOpenFilesList[u].Filename = f;
                LOpenFilesList[u].Path = p;
            }

            RichTextBox rtb = (RichTextBox)((SplitContainer)tp.Controls[0]).Panel1.Controls[0];

            // Set the contents of the text box
            FileIsOpening = true;  // Modify the flag to indicate that the TextChanged event shouldn't react to this
            rtb.Text = sptext;

            // Select the tab and set the focus on the RichTextBox
            MF_TabControl.SelectedTab = tp;
            rtb.Focus();

            // Format the content of the text box
            DoFormatInputText();
            FileIsOpening = false;  // Set the flag back


            ret = true;
            return ret;
        }





        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool Cont = true;

            for (int i = 0; i < MF_TabControl.TabCount; i++)
            {
                if (MF_TabControl.TabPages[i].Text.Contains('*'))
                {
                    Cont = false;
                    break;
                }
            }


            if (Cont == false)
            {
                DialogResult res = MessageBox.Show("There are some unsaved Scratchpads. Close the application anyway?", "Closing check", MessageBoxButtons.YesNo);
                if (res == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }















        private void MF_MenuStrip_Help_Documentation_Click(object sender, EventArgs e)
        {
            CreateNewTextFileTab(HelpFilePath_Documentation, true);
        }






        private void MF_MenuStrip_Help_QuickStart_Click(object sender, EventArgs e)
        {
            CreateNewTextFileTab(HelpFilePath_QuickStartGuid, true);
        }




        private void MF_MenuStrip_Help_About_Click(object sender, EventArgs e)
        {
            string t = "The Data Scratchpad" + Environment.NewLine + "Version 0.1" + Environment.NewLine + "Written by David Charles" + Environment.NewLine + "Copyright 2023";
            MessageBox.Show(t, "About", MessageBoxButtons.OK);
        }







        private void AppendCommandTemplate(BuiltInCommands WhichCommand)
        {
            RichTextBox rtb = GetControlObj_CurrentScratchpad_PrimaryInputTextBox();

            string t = "";

            switch (WhichCommand)
            {
                case BuiltInCommands.Source:
                    t = TemplateCmds_Source;
                    break;

                case BuiltInCommands.AnalysisWindow:
                    t = TemplateCmds_AnalysisWindow;
                    break;

                case BuiltInCommands.Inspect:
                    t = TemplateCmds_Inspect;
                    break;

                case BuiltInCommands.CodePointInspect:
                    t = TemplateCmds_CodePointInspect;
                    break;

                case BuiltInCommands.ByteInspect:
                    t = TemplateCmds_ByteInspect;
                    break;

                case BuiltInCommands.AnalyseFileFormat:
                    t = TemplateCmds_AnalyseFileFormat;
                    break;
            }


            // Add the template command text to the text box
            if (string.IsNullOrEmpty(t)) return;

            // Determine the spacing
            string tspaces = string.Empty;
            if (rtb.Text.Length != 0) tspaces = "\r\n\r\n";


            // Append the template command text
            rtb.Text = rtb.Text + tspaces + t;


            // Scroll to the end of the text box
            rtb.SelectionStart = rtb.Text.Length;
            rtb.ScrollToCaret();


            // Format the content of the text box
            DoFormatInputText();

        }





        private void MF_MenuStrip_Insert_Commands_Source_Click(object sender, EventArgs e)
        {
            AppendCommandTemplate(BuiltInCommands.Source);
        }


        private void MF_MenuStrip_Insert_Commands_AnalysisWindow_Click(object sender, EventArgs e)
        {
            AppendCommandTemplate(BuiltInCommands.AnalysisWindow);
        }


        private void MF_MenuStrip_Insert_Commands_Inspect_Click(object sender, EventArgs e)
        {
            AppendCommandTemplate(BuiltInCommands.Inspect);
        }


        private void MF_MenuStrip_Insert_Commands_CodePointInspect_Click(object sender, EventArgs e)
        {
            AppendCommandTemplate(BuiltInCommands.CodePointInspect);
        }


        private void MF_MenuStrip_Insert_Commands_ByteInspect_Click(object sender, EventArgs e)
        {
            AppendCommandTemplate(BuiltInCommands.ByteInspect);
        }


        private void MF_MenuStrip_Insert_Commands_AnalyseFileFormat_Click(object sender, EventArgs e)
        {
            AppendCommandTemplate(BuiltInCommands.AnalyseFileFormat);
        }






        private enum BuiltInCommands
        {
            Source = 0,
            AnalysisWindow = 1,
            Inspect = 2,
            CodePointInspect = 3,
            ByteInspect = 4,
            AnalyseFileFormat = 5
        }





        private void MF_MenuStrip_File_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }





    }












    public class SharedVars
    {

        public static bool StopProcessingRequestedByUser = false;

    }


}