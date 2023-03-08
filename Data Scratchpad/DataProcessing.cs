using ds_common;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace Data_Scratchpad
{
    public static class DataProcessing
    {




        public const string str_sglquote = "\'";
        public const string str_dblquote = "\"";
        public const string str_slashed_sglquote = "\\'";
        public const string str_escaped_sglquote = "''";
        public const string str_slashed_dblquote = "\\\"";
        public const string str_escaped_dblquote = "\"\"";

        static Random RSampler = new Random();
        static int StaticSeed;

        public const string CommandName_BYTE_INSPECT = "byte_inspect";
        public const string CommandName_CODE_POINT_INSPECT = "code_point_inspect";
        public const string CommandName_INSPECT = "inspect";
        public const string CommandName_ANALYSE_FILE_FORMAT = "analyse_file_format";
        public const string CommandStr_Source = "source";
        public const string CommandStr_AnalysisWindow = "analysis_window";

        public static List<string> LCommandNames = new List<string>();
        public static List<string> LCommandNames_Broad = new List<string>();




        static List<Type?> LDLLs;







        public static void ProcessingInitialisations()
        {

            // This must be run at application launch
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // This must be run at application launch
            CreateStaticRandomisationSeed();

            // This must be run at application launch
            LoadDLLs();

            // This must be run at application launch
            CreateCommandsList();

        }











        static void CreateCommandsList()
        {
            LCommandNames = new List<string>();

            LCommandNames.Add(CommandName_BYTE_INSPECT);
            LCommandNames.Add(CommandName_CODE_POINT_INSPECT);
            LCommandNames.Add(CommandName_INSPECT);
            LCommandNames.Add(CommandName_ANALYSE_FILE_FORMAT);

            // Get the method names from all DLLs
            for (int i = 0; i < LDLLs.Count; i++)
            {
                foreach (MethodInfo m in LDLLs[i].GetMethods())
                {
                    if (string.IsNullOrEmpty(m.Name) == false)
                    {
                        if (m.Name != "ToString" & m.Name != "Equals" & m.Name != "GetHashCode" & m.Name != "GetType") LCommandNames.Add(m.Name);
                    }
                }
            }

            // Populate the broad list which includes the configuration commands, Source and Analysis Window
            LCommandNames_Broad = new List<string>();
            LCommandNames_Broad.AddRange(LCommandNames);
            LCommandNames_Broad.Add(CommandStr_Source);
            LCommandNames_Broad.Add(CommandStr_AnalysisWindow);
        }








        static void LoadDLLs()
        {
            string DLLPath;


#if DEBUG
            DLLPath = "C:\\Users\\David.000\\Source\\repos\\ds_core\\ds_core\\bin\\x64\\Debug\\ds_core.dll";
#else
            DLLPath = Path.Combine(System.AppContext.BaseDirectory, "lib\\ds_core.dll");
#endif



            //string DLLPath = "C:\\Users\\David.000\\Source\\repos\\ds_core\\ds_core\\bin\\Debug\\ds_core.dll";
            //string DLLPath = Path.Combine(System.AppContext.BaseDirectory, "lib\\ds_core.dll");

            Assembly DLL = Assembly.LoadFrom(DLLPath);

            LDLLs = new List<Type?>();

            LDLLs.Add(DLL.GetType("ds_core.Commands"));

        }






        static void CreateStaticRandomisationSeed()
        {
            string dts = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            if (dts.Length < 14)
            {
                dts += new string('0', 14 - dts.Length);
            }
            StaticSeed = int.Parse(dts.Substring(4, 9));

            RSampler = new Random(StaticSeed);
        }












        static void CheckStuff()
        {

            List<string> LS = new List<string>();
            OSource s = new OSource();
            OAnalysisWindow AW = new OAnalysisWindow();
            OActivityState AState = new OActivityState();
            bool sr;
            ofr kr;





            //[************************************ Set up some demo configuration to process ------- DEMO


            //[****************** Create the source object from some demo configuration text
            //LS.Add("");
            //LS.Add("source_type = file");
            //LS.Add("source_name = Test 1: UTF8");
            //LS.Add("file_path= E:\\Projects\\Data Scratchpad\\JV benchmark data.txt");
            //LS.Add("file_format = genericdelimited");
            //LS.Add("file_encoding = utf8");
            ////LS.Add("header_rows_count = 0");
            ////LS.Add("fixed_field_count = false");
            //LS.Add("skip_empty_rows = N");
            //LS.Add("skip_final_empty_row = N");
            //LS.Add("line_terminator_codes=13,10");
            ////LS.Add("line_terminator_string=qwerty");
            //LS.Add("field_delimiter_codes=9");
            ////LS.Add("field_delimiter_string=asdf");
            //LS.Add("");

            LS.Add("");
            LS.Add("source_type = file");
            LS.Add("source_name = Test 1: UTF8");
            LS.Add("file_path= E:\\Projects\\Data Scratchpad\\Table2.1_22062017_3.csv");
            LS.Add("file_format = genericdelimited");
            LS.Add("file_encoding = utf8");
            LS.Add("header_rows_count = 1");
            //LS.Add("fixed_field_count = false");
            LS.Add("skip_empty_rows = N");
            LS.Add("skip_final_empty_row = Y");
            LS.Add("line_terminator_codes=13,10");
            //LS.Add("line_terminator_string=qwerty");
            //LS.Add("field_delimiter_codes = 9");
            LS.Add("field_delimiter_string=,");
            //LS.Add("field_container_string=\"");
            LS.Add("");


            //LS.Add("");
            //LS.Add("source_type = file");
            //LS.Add("source_name = Test 1: UTF8");
            //LS.Add("file_path= E:\\Projects\\Geokex\\Datasets\\House sales\\pp-monthly-update-new-version.csv");
            //LS.Add("file_format = genericdelimited");
            //LS.Add("file_encoding = ANSI");
            //LS.Add("header_rows_count = 0");
            ////LS.Add("fixed_field_count = false");
            //LS.Add("skip_empty_rows = N");
            //LS.Add("skip_final_empty_row = Y");
            //LS.Add("line_terminator_codes=10");
            ////LS.Add("line_terminator_string=qwerty");
            ////LS.Add("field_delimiter_codes = 9");
            //LS.Add("field_delimiter_string=,");
            //LS.Add("field_container_string=\"");
            //LS.Add("");




            // Parse the configuration text to create the source object
            kr = ParseSourceDefinition(LS, ref s);
            //******************]



            //[**************************** Create the Analysis Window
            AW = CreateDefaultAnalysisWindow();

            LS = new List<string>();
            LS.Add("rows = 1 to 10");
            //LS.Add("fields = 8, 14");
            //LS.Add("field_conditions = 8 does_not_end_with 10");
            //LS.Add("");
            //LS.Add("apply_fields_conditions_first = false");
            //LS.Add("");

            sr = ParseAnalysisWindowDefinition(LS, ref AW);
            //****************************]



            //[********** Create a demo command configuration object
            //OCommandConfig_CODEPOINTINSPECTION CConfig_CodePointInspection = new OCommandConfig_CODEPOINTINSPECTION();
            //CConfig_CodePointInspection.StartByte = 0;
            //CConfig_CodePointInspection.EndByte = 1000;
            //**********]

            //[********** Create a demo command configuration object
            //OCommandConfig_INSPECT CConfig_Inspect = new OCommandConfig_INSPECT();
            //CConfig_Inspect.ExpandLineBreaks = false;
            //**********]

            ////[********** Create a demo command configuration object
            //OCommandConfig_ANALYSE_Arithmetic_Median CConfig_Median = new OCommandConfig_ANALYSE_Arithmetic_Median();
            //CConfig_Median.FieldID = 2;
            //CConfig_Median.InterpolationMethod = MedianInterpolationMethod.NearestNeighbourAverage;
            ////**********]

            ////[********** Create a demo command configuration object
            //OCommandConfig_ANALYSE_Arithmetic_ArithmeticMean CConfig_ArithmeticMean = new OCommandConfig_ANALYSE_Arithmetic_ArithmeticMean();
            //CConfig_ArithmeticMean.FieldID = 2;
            ////**********]




            // Set up the demo activity state object
            AState = CreateDefaultActivityState();




            //AState.ActivityName = CommandName_INSPECT;

            // For Mean calculations
            AState.ActivityName = "Median";
            AState.ConfigObj = new List<OStrKeyValuePair>();
            OStrKeyValuePair kvp = new OStrKeyValuePair();
            kvp.KeyName = "field id";
            kvp.ValueStr = "8";
            AState.ConfigObj.Add(kvp);

            kvp = new OStrKeyValuePair();
            kvp.KeyName = "interpolation method";
            kvp.ValueStr = "Nearest Neighbour Average";
            AState.ConfigObj.Add(kvp);

            //************************************]




            //[******************************************************** This is the heart of the Data Scratchpad processing routine


            // Run the Activity Function Caller to perform any necessary command initialisation
            ActivityFunctionCaller(ref s, ref AW, ref AState, null);

            // Run the file processor
            kr = Read_File_Text_Tabulated(ref s, ref AW, ref AState);

            // Check the response of the function
            if (kr.PrimaryReturnValue != RefReturnValues.Success)
            {

                // Report back to the user that there was a processing error
                // ...

                // Force dispose (hand over to GC) the Activity State object, as it may contain a large amount of data accummulated during processing
                AState = new OActivityState();
                return;
            }

            // Run the Activity Function Caller again to finalise the command processing and trigger the output rendering process
            ActivityFunctionCaller(ref s, ref AW, ref AState, null);

            //********************************************************]




        }


        //List<string> LD = new List<string>();
        //Stopwatch sw = new Stopwatch();


        //for (int i = 0; i < 100; i++)
        //{
        //    sw.Reset();
        //    sw.Start();

        //    Read_File_Text_Tabulated(s, AW);

        //    sw.Stop();

        //    LD = new List<string>();
        //    LD.Add(sw.ElapsedMilliseconds.ToString());
        //    File.AppendAllLines("E:\\Projects\\Data Scratchpad\\performance.txt", LD);
        //}









        ////[******************************** Template OFR function
        //static ofr TemplateOFRFunction()
        //{
        //    // Prepare the return object
        //    ofr ret = new ofr();
        //    ret.DT = DateTime.Now;
        //    ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
        //    ret.FunctionName = "TemplateOFRFunction";
        //    // Set the Processing Point description
        //    string ProcessingPoint = "Initialising function";
        //    string ainfo = string.Empty;


        //    // Set the primary return value to success
        //    ret.PrimaryReturnValue = RefReturnValues.Success;
        //    return ret;
        //}
        ////********************************]











        public static ofr RunFullProcess(ref List<string> LS, ref OActivityState AState)
        {
            // This function executes the full processing flow, based on a given set of strings which are the command inputs.

            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "RunFullProcess";
            // Set the Processing Point description
            ret.ProcessingPoint = "Validate inputs";
            ret.AdditionalInfo = string.Empty;


            //[***** Do empty content checks
            if (LS == null)
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidInputParameters;
                return ret;
            }

            if (LS.Count == 0)
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidInputParameters;
                return ret;
            }

            int n = 0;
            foreach (string k in LS)
            {
                if (string.IsNullOrEmpty(k) == false) n += k.Length;
            }
            if (n == 0)
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidInputParameters;
                return ret;
            }
            //*****]


            OSource s = new OSource();
            OAnalysisWindow AW = CreateDefaultAnalysisWindow();
            AState = CreateDefaultActivityState();
            ofr sr;
            bool flag;
            bool AnalysisWindowUsed = false;


            // Set the Processing Point description
            ret.ProcessingPoint = "Extract Source input";
            ret.AdditionalInfo = string.Empty;


            //[****************************** Try to get the Source configuration
            List<string> InputStrs_Source = new List<string>();
            sr = GetCommandContentFromInput(ref LS, CommandStr_Source, ref InputStrs_Source);
            if (sr.PrimaryReturnValue != RefReturnValues.Success)
            {
                if (sr.PrimaryReturnValue == RefReturnValues.Indeterminate)
                {
                    ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                    ret.AdditionalInfo = "Error: No valid Source configuration was found";
                    return ret;
                }
                else
                {
                    ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                    ret.AdditionalInfo = sr.AdditionalInfo;
                    return ret;
                }
            }


            // Parse the strings
            sr = ParseSourceDefinition(InputStrs_Source, ref s);
            if (sr.PrimaryReturnValue != RefReturnValues.Success)
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                ret.AdditionalInfo = "Error: Couldn't parse the Source definition. Further details:";
                ret.AdditionalInfo += Environment.NewLine + "- " + sr.AdditionalInfo;
                return ret;
            }
            //******************************]




            //[****************************** See if there's an Analysis Window - if so, try to parse it
            List<string> InputStrs_AnalysisWindow = new List<string>();
            sr = GetCommandContentFromInput(ref LS, CommandStr_AnalysisWindow, ref InputStrs_AnalysisWindow);
            if (sr.PrimaryReturnValue != RefReturnValues.Success & sr.PrimaryReturnValue != RefReturnValues.Indeterminate)
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                ret.AdditionalInfo = "Error: It looks like an Analysis Window was supposed to be defined, but its definition was incomplete. Further details:";
                ret.AdditionalInfo += Environment.NewLine + "- " + sr.AdditionalInfo;
                return ret;
            }

            if (InputStrs_AnalysisWindow.Count > 0)
            {
                // Parse the strings
                flag = ParseAnalysisWindowDefinition(InputStrs_AnalysisWindow, ref AW);
                if (flag == false)
                {
                    ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                    ret.AdditionalInfo = "Couldn't parse the Analysis Window definition.";
                    return ret;
                }
                AnalysisWindowUsed = true;
            }
            //******************************]



            List<string> LCS = new List<string>();
            List<OStrKeyValuePair> CConfig;
            string tt;

            for (int i = 0; i < LS.Count; i++)
            {
                if (string.IsNullOrEmpty(LS[i]) == false)
                {
                    if (string.Equals(LS[i].Trim().Substring(0, 1), ":"))
                    {
                        if (LS[i].Trim().Length > 1)
                        {
                            for (int j = 0; j < LCommandNames.Count; j++)
                            {

                                // Check if the row contains the command
                                if (LS[i].Contains(LCommandNames[j], StringComparison.InvariantCultureIgnoreCase))
                                {

                                    tt = LS[i].Replace(" ", string.Empty).Replace(":", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);
                                    if (string.Equals(tt, LCommandNames[j], StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (!(LS[i].StartsWith(':') & LS[i].Contains('{') & LS[i].Contains('}')))
                                        {
                                            sr = GetCommandContentFromInput(ref LS, LCommandNames[j], ref LCS);
                                            if (sr.PrimaryReturnValue != RefReturnValues.Success)
                                            {
                                                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                                                ret.AdditionalInfo = "Error: It looks like the command \"" + LCommandNames[j] + "\" was supposed to be defined, but its definition was incomplete. Further details:";
                                                ret.AdditionalInfo += Environment.NewLine + "- " + sr.AdditionalInfo;
                                                return ret;
                                            }
                                        }

                                        CConfig = new List<OStrKeyValuePair>();

                                        if (LCS.Count > 0)
                                        {
                                            // Parse the strings
                                            flag = ParseCommandDefinition(LCS, ref CConfig);
                                            if (flag == false)
                                            {
                                                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                                                ret.AdditionalInfo = "Couldn't parse the definition for command \"" + LCommandNames[j] + "\"";
                                                return ret;
                                            }
                                        }




                                        //[**************************** Run command flow
                                        AState = CreateDefaultActivityState();
                                        AState.ActivityName = LCommandNames[j];
                                        AState.ConfigObj = CConfig;
                                        AState.AnalysisWindowUsed = AnalysisWindowUsed;

                                        // Run the Activity Function Caller to perform any necessary command initialisation
                                        ActivityFunctionCaller(ref s, ref AW, ref AState, null);

                                        // Run the file processor
                                        sr = Read_File_Text_Tabulated(ref s, ref AW, ref AState);

                                        // Check the response of the function
                                        if (sr.PrimaryReturnValue != RefReturnValues.Success)
                                        {
                                            // Force dispose (hand over to GC) some parts of the Activity State object, as it may contain a large amount of data accummulated during processing
                                            AState.IntermediateObj = new List<object>();
                                            AState.OutputObj = new OCommandOutput();

                                            // Report back to the user that there was a processing error
                                            ret.PrimaryReturnValue = sr.PrimaryReturnValue;
                                            ret.ProcessingPoint = "Execute command";
                                            ret.AdditionalInfo = "Error: There was a problem while executing the command. Further details:";
                                            ret.ChildReturnObject = sr;
                                            return ret;
                                        }

                                        // Run the Activity Function Caller again to finalise the command processing
                                        ActivityFunctionCaller(ref s, ref AW, ref AState, null);

                                        ret.PrimaryReturnValue = RefReturnValues.Success;
                                        //****************************]





                                    }
                                }
                            }
                        }
                    }
                }
            }



            // Set the primary return value to success
            ret.PrimaryReturnValue = RefReturnValues.Success;
            return ret;
        }




















        static ofr GetCommandContentFromInput(ref List<string> LS, string CommandStr, ref List<string> LOutput)
        {
            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "GetCommandContentFromInput";
            // Set the Processing Point description
            ret.ProcessingPoint = "Validate inputs";
            ret.AdditionalInfo = string.Empty;


            LOutput = new List<string>();
            if (LS == null) return ret;
            if (LS.Count == 0) return ret;
            if (string.IsNullOrEmpty(CommandStr)) return ret;

            const string Str_LeftBrace = "{";
            const string Str_RightBrace = "}";
            bool FoundCompleteCommand = false;
            string tt;


            // Loop over the rows
            for (int i = 0; i < LS.Count; i++)
            {
                if (string.IsNullOrEmpty(LS[i].Trim()) == false)
                {
                    if (LS[i].Length > 1)
                    {
                        if (string.Equals(LS[i].Substring(0, 1), ":"))
                        {
                            // Check if the row contains the command
                            if (LS[i].Contains(CommandStr, StringComparison.InvariantCultureIgnoreCase))
                            {

                                tt = LS[i].Replace(" ", string.Empty).Replace(":", string.Empty).Replace(Str_LeftBrace, string.Empty).Replace(Str_RightBrace, string.Empty);
                                if (string.Equals(tt, CommandStr, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (LS[i].StartsWith(':') & LS[i].Contains(Str_LeftBrace) & LS[i].Contains(Str_RightBrace))
                                    {
                                        FoundCompleteCommand = true;
                                        break;
                                    }

                                    ret.PrimaryReturnValue = RefReturnValues.ConditionalSuccess;
                                    ret.AdditionalInfo = "Found command keyword.";

                                    // Only proceed if there are at least two more rows in the list (as there must at least be the two braces on separate rows for the command to be valid)
                                    if (i < LS.Count - 2)
                                    {

                                        if (string.Equals(LS[i + 1].Trim(), Str_LeftBrace, StringComparison.OrdinalIgnoreCase))
                                        {
                                            ret.AdditionalInfo += " Found opening brace.";

                                            // Only proceed if there is at least one more row in the list
                                            if (i + 1 < LS.Count - 1)
                                            {
                                                // Loop over the remaining rows
                                                for (int j = i + 2; j < LS.Count; j++)
                                                {
                                                    if (string.Equals(LS[j].Trim(), Str_RightBrace, StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        ret.AdditionalInfo += " Found closing brace.";
                                                        FoundCompleteCommand = true;
                                                        break;
                                                    }

                                                    LOutput.Add(LS[j]);
                                                }
                                            }
                                        }

                                        if (FoundCompleteCommand) break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (FoundCompleteCommand) break;
            }


            if (FoundCompleteCommand)
            {
                ret.PrimaryReturnValue = RefReturnValues.Success;
            }
            else
            {
                if (ret.AdditionalInfo.Contains("Found opening brace"))
                {
                    ret.AdditionalInfo += " *** Did not find closing brace ***";
                }
                else
                {
                    if (ret.AdditionalInfo.Contains("Found command keyword"))
                    {
                        ret.AdditionalInfo += " *** Did not find opening brace ***";
                    }
                    else
                    {
                        ret.AdditionalInfo += "*** Did not find command keyword ***";
                    }
                }
            }

            return ret;
        }

















        static OAnalysisWindow CreateDefaultAnalysisWindow()
        {
            OAnalysisWindow ret = new OAnalysisWindow();

            ret.FieldSamplingConditions = new List<OFieldAnalysisContext>();
            ret.UseFields = new List<OFieldIDPair>();
            ret.RowsSamplingType = SamplingType.All;
            ret.RowsSamplingTypeQualifier = SamplingTypeQualifier.NoQualifier;
            ret.RowsList = new List<long>();
            ret.RowsPercentage = 100.0d;
            ret.ApplyRowSamplingAfterFieldSampling = true;

            return ret;
        }






        static OActivityState CreateDefaultActivityState()
        {
            OActivityState ret = new OActivityState();

            ret = new OActivityState();
            ret.EarlyTermination = false;
            ret.ProcessedRowsCount = 0;
            ret.OutputObj = new OCommandOutput();
            ret.HasInitialisationCompleted = false;
            ret.Started = false;
            ret.Completed = false;
            ret.OutputMessagesToUser = new List<string>();

            return ret;
        }





        static void ActivityFunctionCaller(ref OSource s, ref OAnalysisWindow AW, ref OActivityState AState, OIncrementalDataObject? IDO = null)
        {
            if (s == null) return;
            if (AW == null) return;
            if (AState == null) return;


            if (AState.HasInitialisationCompleted == false)
            {
                if (AW.RowsSamplingType == SamplingType.Percentage)
                {
                    if (AW.RowsSamplingTypeQualifier == SamplingTypeQualifier.RandomStatic)
                    {
                        RSampler = new Random(StaticSeed);
                    }
                    else
                    {
                        if (AW.RowsSamplingTypeQualifier == SamplingTypeQualifier.RandomDynamic)
                        {
                            string dts = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                            int n = int.Parse(dts.Substring(4, 9));
                            RSampler = new Random(n);
                        }
                    }
                }
            }




            switch (AState.ActivityName.ToLowerInvariant())

            {
                case CommandName_BYTE_INSPECT:

                    Commands.RunCommand_BYTE_INSPECT(ref AState, IDO);
                    break;

                case CommandName_CODE_POINT_INSPECT:

                    Commands.RunCommand_CODE_POINT_INSPECT(ref AState, IDO);
                    break;

                case CommandName_INSPECT:

                    Commands.RunCommand_INSPECT(ref AState, IDO);
                    break;

                case CommandName_ANALYSE_FILE_FORMAT:

                    Commands.RunCommand_ANALYSE_FILE_FORMAT(ref AState, IDO);
                    break;




                default:
                    Type? DLLType = LDLLs[0];
                    object? ro = null;
                    ofr sr;

                    if (DLLType != null)
                    {
                        MethodInfo? m = DLLType.GetMethod(AState.ActivityName);

                        if (m != null)
                        {
                            object[] Parameters = new object[] { AState, IDO };
                            ro = m.Invoke(null, Parameters);

                            if (ro != null)
                            {
                                sr = (ofr)ro;

                                if (sr.PrimaryReturnValue != RefReturnValues.Success)
                                {
                                    AState.EarlyTermination = true;
                                    AState.ofr.PrimaryReturnValue = RefReturnValues.InvalidInputParameters;
                                    AState.ofr.AdditionalInfo = sr.AdditionalInfo;
                                }
                            }
                        }
                    }

                    break;
            }


        }




























































        static ofr Read_File_Text_Tabulated(ref OSource s, ref OAnalysisWindow AW, ref OActivityState AState)
        {
            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "Read_File_Text_Tabulated";
            // Set the Processing Point description
            string ProcessingPoint = "Initialising function";
            string ainfo = string.Empty;



            if (s == null) return ret;
            if (AW == null) return ret;
            if (AState == null) return ret;

            //***** Check here that the Analysis Window object is valid



            //[***************************************************** Check Source object
            // Set the Processing Point description
            ProcessingPoint = "Check Source object validity";

            if (s.SourceType != SourceTypes.File)
            {
                ainfo = "Source object is not set to File, while trying to use the read file function";
                CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInCodeOutsideOfThisFunction, ProcessingPoint, ainfo);
                return ret;
            }
            if (s.SourceParameters == null)
            {
                ainfo = "Source object is missing the source parameters object";
                CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInCodeOutsideOfThisFunction, ProcessingPoint, ainfo);
                return ret;
            }

            OSourceParameters_File spo;

            try
            {
                spo = (OSourceParameters_File)s.SourceParameters;
            }
            catch (Exception e)
            {
                ainfo = "Source parameters object is not an OSourceParametersFile object";
                CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInCodeOutsideOfThisFunction, ProcessingPoint, ainfo, null, e);
                return ret;
            }

            if (BasicValidateObject_SourceParametersFile(spo) == false)
            {
                ainfo = "Source parameters object contains at least one invalid configuration";
                CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInCodeOutsideOfThisFunction, ProcessingPoint, ainfo);
                return ret;
            }


            if (File.Exists(spo.FullPathAndFilename) == false)
            {
                ainfo = "The specified source file does not exist";
                CreateOFRErrorResponse(ref ret, RefReturnValues.InvalidInputParameters, ProcessingPoint, ainfo);
                return ret;
            }
            //*****************************************************]




            // Set the Processing Point description
            ProcessingPoint = "Determine the file reading method";



            if (BasicValidateObject_OSPF_Text_Tabulated(spo.Params_Text_Tabulated) == false)
            {
                ainfo = "Params_Text_Tabulated object in Source parameters object contains at least one invalid configuration";
                CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInCodeOutsideOfThisFunction, ProcessingPoint, ainfo);
                return ret;
            }





            //[********************** Common variables & constants
            char[] UseLineTerminators = new char[0];
            char[] UseDelimiters = new char[0];
            bool CheckStringVersion = false;
            int n_ult;
            int n_delim;
            int n_fc = spo.Params_Text_Tabulated.FieldContainers.Length;
            bool IsQuoteFieldContainers = false;
            bool SupportSlashEscapedQuotes = false;
            List<string> LFields = new List<string>();
            long RawRowCount = 0;
            long FilteredRowCount = 0;
            long ProcessedRowCount = 0;
            const string RefStr_LineTerminators_r = @"\r";  // Carriage Return, ASCII 13
            const string RefStr_LineTerminators_n = @"\n";  // New Line, ASCII 10
            const string RefStr_LineTerminators_rn = @"\r\n";  // Carriage Return & New Line, ASCII 13 & 10
            bool UseCustomReadingMethod = false;
            OTextProcessParams tpp;
            bool IsFinalRow = false;

            // For the file stream
            Encoding enc;
            FileStreamOptions fso = new FileStreamOptions();
            fso.Access = FileAccess.Read;
            fso.Mode = FileMode.Open;
            fso.Share = FileShare.Read;
            fso.Options = FileOptions.SequentialScan;
            //**********************]













            switch (spo.Encoding)
            {
                case FileEncodings.InvalidMatch:
                    ainfo = "No valid file encoding was specified";
                    CreateOFRErrorResponse(ref ret, RefReturnValues.InvalidInputParameters, ProcessingPoint, ainfo);
                    return ret;

                case FileEncodings.ASCII:
                    enc = Encoding.ASCII;
                    break;
                case FileEncodings.UTF7:
                    enc = Encoding.UTF7;
                    break;
                case FileEncodings.UTF8:
                    enc = Encoding.UTF8;
                    break;
                case FileEncodings.UTF16:
                    enc = Encoding.GetEncoding(1200);
                    break;
                case FileEncodings.UTF16BE:
                    enc = Encoding.GetEncoding(1201);
                    break;
                case FileEncodings.UTF32:
                    enc = Encoding.UTF32;
                    break;
                case FileEncodings.UTF32BE:
                    enc = Encoding.GetEncoding(12001);
                    break;
                case FileEncodings.ANSI:
                    enc = Encoding.GetEncoding(1252);
                    break;
                default:
                    enc = Encoding.UTF8;
                    break;
            }

            // Check for the default case
            if (enc == Encoding.UTF8 & spo.Encoding != FileEncodings.UTF8)
            {
                ainfo = "No valid file encoding was specified";
                CreateOFRErrorResponse(ref ret, RefReturnValues.InvalidInputParameters, ProcessingPoint, ainfo);
                return ret;
            }







            //[********************* Determine the usable line terminators
            if (spo.Params_Text_Tabulated.LineTerminatorCodes != null)
            {
                if (spo.Params_Text_Tabulated.LineTerminatorCodes.Count > 0)
                {

                    //[********** Determine the usable line terminators
                    UseLineTerminators = new char[spo.Params_Text_Tabulated.LineTerminatorCodes.Count];
                    for (int i = 0; i < spo.Params_Text_Tabulated.LineTerminatorCodes.Count; i++)
                    {
                        UseLineTerminators[i] = (char)spo.Params_Text_Tabulated.LineTerminatorCodes[i];
                    }
                    //**********]


                    // Check for line terminators that the StreamReader can handle (both CR LF, only CR, and only LF)
                    switch (spo.Params_Text_Tabulated.LineTerminatorCodes.Count)
                    {
                        case 1:
                            if (spo.Params_Text_Tabulated.LineTerminatorCodes[0] == 13 || spo.Params_Text_Tabulated.LineTerminatorCodes[0] == 10)
                            {
                                UseCustomReadingMethod = false;
                            }
                            else
                            {
                                UseCustomReadingMethod = true;
                            }
                            break;
                        case 2:
                            if (spo.Params_Text_Tabulated.LineTerminatorCodes[0] == 13 & spo.Params_Text_Tabulated.LineTerminatorCodes[1] == 10)
                            {
                                UseCustomReadingMethod = false;
                            }
                            else
                            {
                                UseCustomReadingMethod = true;
                            }
                            break;
                        default:
                            UseCustomReadingMethod = true;
                            break;
                    }
                }
                else
                {
                    CheckStringVersion = true;
                }
            }
            else
            {
                CheckStringVersion = true;
            }

            // Check the LineTerminators string (rather than the codes array, the scenarios for which were handled above)
            if (CheckStringVersion)
            {
                if (string.IsNullOrWhiteSpace(spo.Params_Text_Tabulated.LineTerminators) == false)
                {
                    if (string.Equals(spo.Params_Text_Tabulated.LineTerminators, RefStr_LineTerminators_r, StringComparison.OrdinalIgnoreCase) || string.Equals(spo.Params_Text_Tabulated.LineTerminators, RefStr_LineTerminators_n, StringComparison.OrdinalIgnoreCase) || string.Equals(spo.Params_Text_Tabulated.LineTerminators, RefStr_LineTerminators_rn, StringComparison.OrdinalIgnoreCase))
                    {

                        //[********** Determine the usable line terminators
                        if (string.Equals(spo.Params_Text_Tabulated.LineTerminators, RefStr_LineTerminators_r, StringComparison.OrdinalIgnoreCase))
                        {
                            UseLineTerminators = new char[1];
                            UseLineTerminators[0] = '\r';
                        }
                        if (string.Equals(spo.Params_Text_Tabulated.LineTerminators, RefStr_LineTerminators_n, StringComparison.OrdinalIgnoreCase))
                        {
                            UseLineTerminators = new char[1];
                            UseLineTerminators[0] = '\n';
                        }
                        if (string.Equals(spo.Params_Text_Tabulated.LineTerminators, RefStr_LineTerminators_rn, StringComparison.OrdinalIgnoreCase))
                        {
                            UseLineTerminators = new char[2];
                            UseLineTerminators[0] = '\r';
                            UseLineTerminators[1] = '\n';
                        }
                        //**********]

                        UseCustomReadingMethod = false;
                    }
                    else
                    {
                        //[********** Determine the usable line terminators
                        UseLineTerminators = new char[spo.Params_Text_Tabulated.LineTerminators.Length];
                        for (int i = 0; i < spo.Params_Text_Tabulated.LineTerminators.Length; i++)
                        {
                            UseLineTerminators[i] = spo.Params_Text_Tabulated.LineTerminators[i];
                        }
                        //**********]

                        UseCustomReadingMethod = true;
                    }
                }
                else
                {
                    ainfo = "No valid line terminators were specified";
                    List<RefReturnValues> trr = new List<RefReturnValues>();
                    trr.Add(RefReturnValues.ErrorInCodeOutsideOfThisFunction);
                    CreateOFRErrorResponse(ref ret, RefReturnValues.InvalidInputParameters, ProcessingPoint, ainfo, trr);
                    return ret;
                }
            }
            //*********************]










            //[********************* Determine the usable field delimiters
            CheckStringVersion = false;

            // Set the Processing Point description
            ProcessingPoint = "Process a GenericDelimited file - check the field delimiters";

            if (spo.Params_Text_Tabulated.FieldDelimiterCodes != null)
            {
                if (spo.Params_Text_Tabulated.FieldDelimiterCodes.Count > 0)
                {
                    UseDelimiters = new char[spo.Params_Text_Tabulated.FieldDelimiterCodes.Count];
                    for (int i = 0; i < spo.Params_Text_Tabulated.FieldDelimiterCodes.Count; i++)
                    {
                        UseDelimiters[i] = (char)spo.Params_Text_Tabulated.FieldDelimiterCodes[i];
                    }
                }
                else
                {
                    CheckStringVersion = true;
                }
            }
            else
            {
                CheckStringVersion = true;
            }

            if (CheckStringVersion)
            {
                UseDelimiters = new char[spo.Params_Text_Tabulated.FieldDelimiters.Length];
                for (int i = 0; i < spo.Params_Text_Tabulated.FieldDelimiters.Length; i++)
                {
                    UseDelimiters[i] = spo.Params_Text_Tabulated.FieldDelimiters[i];
                }
            }
            //*********************]







            // Some checks on the line terminators, plus a key assignment
            n_ult = UseLineTerminators.Length;

            if (n_ult == 0)
            {
                ainfo = "No valid line terminators were specified";
                List<RefReturnValues> trr = new List<RefReturnValues>();
                trr.Add(RefReturnValues.ErrorInCodeOutsideOfThisFunction);
                CreateOFRErrorResponse(ref ret, RefReturnValues.InvalidInputParameters, ProcessingPoint, ainfo, trr);
                return ret;
            }



            // Some checks on the delimiters, plus a key assignment
            n_delim = UseDelimiters.Length;

            if (n_delim == 0)
            {
                ainfo = "No valid field delimiters were specified";
                List<RefReturnValues> trr = new List<RefReturnValues>();
                trr.Add(RefReturnValues.ErrorInCodeOutsideOfThisFunction);
                CreateOFRErrorResponse(ref ret, RefReturnValues.InvalidInputParameters, ProcessingPoint, ainfo, trr);
                return ret;
            }

            if (n_delim > 1) UseCustomReadingMethod = true;





            // Check the field containers
            if (n_fc > 0)
            {
                UseCustomReadingMethod = true;

                if (string.Equals(spo.Params_Text_Tabulated.FieldContainers, str_dblquote, StringComparison.OrdinalIgnoreCase) || string.Equals(spo.Params_Text_Tabulated.FieldContainers, str_sglquote, StringComparison.OrdinalIgnoreCase))
                {
                    IsQuoteFieldContainers = true;
                }
            }




            // If there's no header, then we need to make an adjustment to the conditional field sampling IDs here.
            // This is because the user is required to enter the IDs as 1-based, and the PostProcessRow_TEXT() will adjust them to zero-based while processing the header,
            // however when there is no header, then the adjustment won't be made in PostProcessRow_TEXT(), so it needs to be done somewhere else, before processing starts.
            if (spo.Params_Text_Tabulated.NumHeaderRows == 0)
            {
                if (AW.FieldSamplingConditions.Count > 0)
                {
                    for (int i = 0; i < AW.FieldSamplingConditions.Count; i++)
                    {
                        // If there's a field ID specified...
                        if (AW.FieldSamplingConditions[i].FieldIDPair.ID > 0)
                        {
                            // Adjust the field ID to make it zero-based (as the user was required to enter 1-based values, and this won't be done anywhere else in the scenario where there are no headers specified)
                            AW.FieldSamplingConditions[i].FieldIDPair.ID--;
                        }
                    }
                }
            }







            //[****** Populate the object for common text processing parameters
            tpp = new OTextProcessParams();
            tpp.UseLineTerminators = UseLineTerminators;
            tpp.n_ult = n_ult;
            tpp.UseDelimiters = UseDelimiters;
            tpp.n_delim = n_delim;
            tpp.FieldContainers = spo.Params_Text_Tabulated.FieldContainers;
            tpp.n_fc = n_fc;
            tpp.IsQuoteFieldContainers = IsQuoteFieldContainers;
            tpp.SupportSlashEscapedQuotes = SupportSlashEscapedQuotes;
            //******]










            if (string.Equals(AState.ActivityName, CommandName_ANALYSE_FILE_FORMAT, StringComparison.OrdinalIgnoreCase))
            {

                //[************************************************************************** Analyse File Format

                //[********************** Variables used for the Analyse File Format method
                const int CPIBlockSize = 2048;
                fso.BufferSize = CPIBlockSize;
                List<object> LObj = new List<object>();
                OIncrementalDataObject IDO;
                string Likely_Encoding = "Possibly UTF8: Couldn't match any common Byte Order Marks (BOM), however, note that the BOM for UTF8 is optional, so it's possible that the encoding is actually UTF8.";
                string Likely_LineTerminators = "Couldn't identify any common line terminator characters within the first " + CPIBlockSize.ToString() + " bytes. This file may not be a \"row\"-based structured data file.";
                string Likely_FieldDelimiters = "Couldn't identify any common field delimiters.";
                string Likely_FieldContainers = "Couldn't identify any common field containers.";
                Encoding LEnc = Encoding.UTF8;
                string cbStr;
                //**********************]


                AState.ProcessedRowsCount = 0;



                try
                {
                    ProcessingPoint = "Open and analyse source file - Analyse File Format method";


                    byte[] ByteBuffer = new byte[CPIBlockSize];
                    int BytesRead;


                    using (FileStream fs = new FileStream(spo.FullPathAndFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, CPIBlockSize, false))
                    {

                        BytesRead = fs.Read(ByteBuffer, 0, CPIBlockSize);
                        if (BytesRead == 0)
                        {
                            ainfo = "No bytes were read from the file";
                            CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInExternalFile, ProcessingPoint, ainfo, null, null);
                            return ret;
                        }

                        if (BytesRead < CPIBlockSize)
                        {
                            Array.Resize(ref ByteBuffer, BytesRead);
                        }



                        // Test for ASCII preamble (BOM)
                        if (DoesPreambleMatch(Encoding.ASCII, ref ByteBuffer))
                        {
                            Likely_Encoding = "ASCII";
                            LEnc = Encoding.ASCII;
                        }

                        // Test for ANSI preamble (BOM)
                        if (DoesPreambleMatch(Encoding.GetEncoding(1252), ref ByteBuffer))
                        {
                            Likely_Encoding = "ANSI (code page 1252)";
                            LEnc = Encoding.GetEncoding(1252);
                        }

                        // Test for UTF7 preamble (BOM)
                        if (DoesPreambleMatch(Encoding.UTF7, ref ByteBuffer))
                        {
                            Likely_Encoding = "UTF7";
                            LEnc = Encoding.UTF7;
                        }

                        // Test for UTF8 preamble (BOM)
                        if (DoesPreambleMatch(Encoding.UTF8, ref ByteBuffer))
                        {
                            Likely_Encoding = "UTF8";
                            LEnc = Encoding.UTF8;
                        }

                        // Test for UTF16-LE preamble (BOM)
                        if (DoesPreambleMatch(Encoding.GetEncoding(1200), ref ByteBuffer))
                        {
                            Likely_Encoding = "UTF16 - Little Endian";
                            LEnc = Encoding.GetEncoding(1200);
                        }

                        // Test for UTF16-BE preamble (BOM)
                        if (DoesPreambleMatch(Encoding.GetEncoding(1201), ref ByteBuffer))
                        {
                            Likely_Encoding = "UTF16 - Big Endian";
                            LEnc = Encoding.GetEncoding(1201);
                        }

                        // Test for UTF32-LE preamble (BOM)
                        if (DoesPreambleMatch(Encoding.UTF32, ref ByteBuffer))
                        {
                            Likely_Encoding = "UTF32 - Little Endian";
                            LEnc = Encoding.UTF32;
                        }

                        // Test for UTF32-BE preamble (BOM)
                        if (DoesPreambleMatch(Encoding.GetEncoding(12001), ref ByteBuffer))
                        {
                            Likely_Encoding = "UTF32 - Big Endian";
                            LEnc = Encoding.GetEncoding(12001);
                        }




                        cbStr = LEnc.GetString(ByteBuffer);
                        if (string.IsNullOrEmpty(cbStr) == false)
                        {



                            if (CheckForAtLeastNInstances("\r\n", ref cbStr, 2))
                            {
                                Likely_LineTerminators = "CR LF | control characters 13 (0x0d) and 10 (0x0a)";
                            }
                            else
                            {
                                if (CheckForAtLeastNInstances("\r", ref cbStr, 2))
                                {
                                    Likely_LineTerminators = "CR | control character 13 (0x0d)";
                                }
                                else
                                {
                                    if (CheckForAtLeastNInstances("\n", ref cbStr, 2))
                                    {
                                        Likely_LineTerminators = "LF | control character 10 (0x0a)";
                                    }
                                    else
                                    {
                                        if (CheckForAtLeastNInstances("\r\n", ref cbStr, 1))
                                        {
                                            Likely_LineTerminators = "CR LF | control characters 13 (0x0d) and 10 (0x0a)";
                                        }
                                        else
                                        {
                                            if (CheckForAtLeastNInstances("\r", ref cbStr, 1))
                                            {
                                                Likely_LineTerminators = "CR | control character 13 (0x0d)";
                                            }
                                            else
                                            {
                                                if (CheckForAtLeastNInstances("\n", ref cbStr, 1))
                                                {
                                                    Likely_LineTerminators = "LF | control character 10 (0x0a)";
                                                }
                                            }
                                        }
                                    }
                                }
                            }




                            //[******************************* Try to identify any common field delimitors
                            if (CheckForAtLeastNInstances(",", ref cbStr, 3))
                            {
                                Likely_FieldDelimiters = "Comma | ',' | character code 44 (0x2c)";
                            }
                            else
                            {
                                if (CheckForAtLeastNInstances("\t", ref cbStr, 3))
                                {
                                    Likely_FieldDelimiters = "Tab | character code 9 (0x09)";
                                }
                                else
                                {
                                    if (CheckForAtLeastNInstances("|", ref cbStr, 3))
                                    {
                                        Likely_FieldDelimiters = "Pipe | character code 124 (0x7c)";
                                    }
                                    else
                                    {
                                        if (CheckForAtLeastNInstances(";", ref cbStr, 3))
                                        {
                                            Likely_FieldDelimiters = "Semicolon | character code 59 (0x3b)";
                                        }
                                    }
                                }
                            }
                            //*******************************]


                            //[******************************* Try to identify any common field delimitors
                            if (CheckForAtLeastNInstances("\"", ref cbStr, 4))
                            {
                                Likely_FieldContainers = "Quotation mark | \" | character code 34 (0x22)";
                            }
                            else
                            {
                                if (CheckForAtLeastNInstances("\'", ref cbStr, 4))
                                {
                                    Likely_FieldContainers = "Apostrophe | \' | character code 39 (0x27)";
                                }
                            }
                            //*******************************]

                        }



                        LObj.Add((object)Likely_Encoding);
                        LObj.Add((object)Likely_LineTerminators);
                        LObj.Add((object)Likely_FieldDelimiters);
                        LObj.Add((object)Likely_FieldContainers);



                        IDO = new OIncrementalDataObject();
                        IDO.IDO = LObj;
                        IDO.ProcessedRowID = 0;
                        IDO.SourceIsText = true;


                        ActivityFunctionCaller(ref s, ref AW, ref AState, IDO);


                        ret.PrimaryReturnValue = RefReturnValues.Success;
                    }
                }
                catch (IOException e)  // This catch should trigger if there is a problem opening and reading from the file
                {
                    ainfo = "Cannot read from the specified source file";
                    CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInExternalFile, ProcessingPoint, ainfo, null, e);
                    return ret;
                }
                //**************************************************************************] END: Code Point Inspection



                return ret;
            }




















            if (string.Equals(AState.ActivityName, CommandName_BYTE_INSPECT, StringComparison.OrdinalIgnoreCase))
            {

                //[************************************************************************** Byte Inspection

                //[********************** Variables used for the Byte Inspection method
                const int CPIBlockSize = 2048;
                fso.BufferSize = CPIBlockSize;
                List<object> LObj = new List<object>();
                OIncrementalDataObject IDO;
                long ByteCounter = 0;
                long CharacterCounter = 0;
                string ct;
                long StartByte = 1;
                long EndByte = 100;
                long MaxByteLength = 1000;
                //**********************]


                AState.ProcessedRowsCount = 0;

                // Get the start byte position for the inspection
                ct = ds_common.Utilities.GetConfigParameter("start_byte", ref AState);
                if (string.IsNullOrEmpty(ct) == false)
                {
                    if (long.TryParse(ct, out StartByte))
                    {
                        if (StartByte <= 0) StartByte = 1;
                    }
                }


                // Get the end byte position (the limit) for the inspection
                ct = ds_common.Utilities.GetConfigParameter("end_byte", ref AState);
                if (string.IsNullOrEmpty(ct) == false)
                {
                    if (long.TryParse(ct, out EndByte))
                    {
                        if (EndByte <= StartByte) EndByte = StartByte + 100;
                    }
                }
                else
                {
                    if (StartByte > 0) EndByte = StartByte + 100;
                }


                // Limit the maximum range
                if (EndByte - StartByte > MaxByteLength)
                {
                    EndByte = StartByte + MaxByteLength;
                }


                // Adjust the start and end byte variables for the 1-basing imposed on the user
                StartByte--;
                EndByte--;



                try
                {
                    ProcessingPoint = "Open and read from source file - Byte Inspection method";


                    byte[] ByteBuffer = new byte[CPIBlockSize];
                    int BytesRead;
                    StringBuilder sb;
                    string tba;
                    byte[] SampleBytes = new byte[4];
                    int SampleBytesLength = 4;
                    int blen = CPIBlockSize;


                    using (FileStream fs = new FileStream(spo.FullPathAndFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, CPIBlockSize, false))
                    {

                        if (StartByte > fs.Length)
                        {
                            ainfo = "start_byte position was larger than the size of the file";
                            CreateOFRErrorResponse(ref ret, RefReturnValues.InvalidConfigurationSpecification, ProcessingPoint, ainfo, null, null);
                            return ret;
                        }

                        if (fs.Length - StartByte < CPIBlockSize)
                        {
                            blen = (int)(fs.Length - StartByte);
                        }

                        fs.Seek(StartByte, SeekOrigin.Begin);

                        ByteCounter = StartByte;

                        BytesRead = fs.Read(ByteBuffer, 0, blen);
                        if (BytesRead == 0)
                        {
                            ainfo = "No bytes were read from the file";
                            CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInExternalFile, ProcessingPoint, ainfo, null, null);
                            return ret;
                        }

                        if (BytesRead < CPIBlockSize)
                        {
                            Array.Resize(ref ByteBuffer, BytesRead);
                        }


                        // Loop over the characters
                        // This loop incorporates the counting of bytes, which can vary per character, based on the encoding.
                        for (int i = 0; i < ByteBuffer.Length; i++)
                        {

                            if (i > (EndByte - StartByte)) break;

                            LObj = new List<object>();

                            sb = new StringBuilder();

                            if (ByteBuffer.Length - i < 4)
                            {
                                SampleBytesLength = ByteBuffer.Length - i;
                                SampleBytes = new byte[SampleBytesLength];
                            }

                            Array.Copy(ByteBuffer, i, SampleBytes, 0, SampleBytesLength);


                            for (int j = 7; j >= 0; j--)
                            {
                                if (GetBitState(ByteBuffer[i], j))
                                {
                                    sb.Append("1");
                                }
                                else
                                {
                                    sb.Append("0");
                                }
                            }
                            tba = sb.ToString();


                            LObj.Add((object)(ByteCounter + 1).ToString());
                            LObj.Add((object)(ByteBuffer[i]).ToString());
                            LObj.Add((object)(ByteBuffer[i]).ToString("x2"));
                            LObj.Add((object)tba);


                            ////[************ Change control characters to the printable glyph versions between 0x2400 and 0x243F
                            //c = Encoding.ASCII.GetChars(SampleBytes)[0];
                            //if (char.IsControl(c)) c = Encoding.Unicode.GetChars(new byte[] { BitConverter.GetBytes((int)c + 0x2400)[0] })[0];
                            //LObj.Add((object)(c.ToString()));

                            //c = Encoding.GetEncoding(1252).GetChars(SampleBytes)[0];
                            //if (char.IsControl(c)) c = Encoding.Unicode.GetChars(new byte[] { BitConverter.GetBytes((int)c + 0x2400)[0] })[0];
                            //LObj.Add((object)(c.ToString()));

                            //c = Encoding.UTF7.GetChars(SampleBytes)[0];
                            //if (char.IsControl(c)) c = Encoding.Unicode.GetChars(new byte[] { BitConverter.GetBytes((int)c + 0x2400)[0] })[0];
                            //LObj.Add((object)(c.ToString()));

                            //c = Encoding.UTF8.GetChars(SampleBytes)[0];
                            //if (char.IsControl(c)) c = Encoding.Unicode.GetChars(new byte[] { BitConverter.GetBytes((int)c + 0x2400)[0] })[0];
                            //LObj.Add((object)(c.ToString()));
                            ////************]


                            LObj.Add((object)(Encoding.ASCII.GetChars(SampleBytes)[0].ToString()));
                            LObj.Add((object)(Encoding.GetEncoding(1252).GetChars(SampleBytes)[0].ToString()));
                            LObj.Add((object)(Encoding.UTF7.GetChars(SampleBytes)[0].ToString()));
                            LObj.Add((object)(Encoding.UTF8.GetChars(SampleBytes)[0].ToString()));


                            if (SampleBytesLength >= 2)
                            {
                                LObj.Add((object)(Encoding.GetEncoding(1200).GetChars(SampleBytes)[0].ToString()));
                                LObj.Add((object)(Encoding.GetEncoding(1201).GetChars(SampleBytes)[0].ToString()));
                            }
                            else
                            {
                                LObj.Add((object)string.Empty);
                                LObj.Add((object)string.Empty);
                            }

                            if (SampleBytesLength == 4)
                            {
                                LObj.Add((object)(Encoding.UTF32.GetChars(SampleBytes)[0].ToString()));
                                LObj.Add((object)(Encoding.GetEncoding(12001).GetChars(SampleBytes)[0].ToString()));
                            }
                            else
                            {
                                LObj.Add((object)string.Empty);
                                LObj.Add((object)string.Empty);
                            }


                            IDO = new OIncrementalDataObject();
                            IDO.IDO = LObj;
                            IDO.ProcessedRowID = CharacterCounter;
                            IDO.SourceIsText = true;


                            AState.ProcessedRowsCount++;

                            ActivityFunctionCaller(ref s, ref AW, ref AState, IDO);



                            ByteCounter++;
                        }


                        // Set the primary return value to success
                        ret.PrimaryReturnValue = RefReturnValues.Success;
                    }





                }
                catch (IOException e)  // This catch should trigger if there is a problem opening and reading from the file
                {
                    ainfo = "Cannot read from the specified source file";
                    CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInExternalFile, ProcessingPoint, ainfo, null, e);
                    return ret;
                }
                //**************************************************************************] END: Code Point Inspection


                return ret;
            }


















            if (string.Equals(AState.ActivityName, CommandName_CODE_POINT_INSPECT, StringComparison.OrdinalIgnoreCase))
            {

                //[************************************************************************** Code Point Inspection

                //[********************** Variables used for the Code Point Inspection method
                const int CPIBlockSize = 2048;
                fso.BufferSize = CPIBlockSize;
                Span<char> CharBuffer = new Span<char>(new char[CPIBlockSize]);
                int CharactersRead = 0;
                List<object> LObj = new List<object>();
                OIncrementalDataObject IDO;
                //long ByteCounter = 0;
                long CharacterCounter = 0;
                string ct;
                long StartCharacter = 1;
                long EndCharacter = 100;
                long MaxCharacterLength = 1000;
                int CWidth;
                //**********************]


                AState.ProcessedRowsCount = 0;

                // Get the start byte position for the inspection
                ct = ds_common.Utilities.GetConfigParameter("start_character", ref AState);
                if (string.IsNullOrEmpty(ct) == false)
                {
                    if (long.TryParse(ct, out StartCharacter))
                    {
                        if (StartCharacter <= 0) StartCharacter = 1;
                    }
                }


                // Get the end byte position (the limit) for the inspection
                ct = ds_common.Utilities.GetConfigParameter("end_character", ref AState);
                if (string.IsNullOrEmpty(ct) == false)
                {
                    if (long.TryParse(ct, out EndCharacter))
                    {
                        if (EndCharacter <= StartCharacter) EndCharacter = StartCharacter + 100;
                    }
                }
                else
                {
                    if (StartCharacter > 0) EndCharacter = StartCharacter + 100;
                }


                // Limit the maximum range
                if (EndCharacter - StartCharacter > MaxCharacterLength)
                {
                    EndCharacter = StartCharacter + MaxCharacterLength;
                }


                // Adjust the start and end character variables for the 1-basing imposed on the user
                StartCharacter--;
                EndCharacter--;


                try
                {
                    ProcessingPoint = "Open and read from source file - Code Point Inspection method";

                    bool DoSurrogatePairs;
                    int cbint1 = 0;
                    int cbint2 = 0;
                    int SPCodePoint;
                    string SurrogatePair;
                    char UseCB;

                    using (StreamReader read_sw = new StreamReader(spo.FullPathAndFilename, enc, true, fso))
                    {

                        read_sw.BaseStream.Position = 0;


                        while (!read_sw.EndOfStream)
                        {
                            // Reset the buffer
                            CharBuffer = new Span<char>(new char[CPIBlockSize]);

                            // Read a block of data into a buffer
                            CharactersRead = read_sw.Read(CharBuffer);


                            // Determine if this is the last chunk of data read from the file
                            if (CharactersRead < CPIBlockSize)
                            {
                                CharBuffer = CharBuffer.Slice(0, CharactersRead);
                            }



                            // Loop over the characters
                            // This loop incorporates the counting of bytes, which can vary per character, based on the encoding.
                            for (int i = 0; i < CharBuffer.Length; i++)
                            {
                                DoSurrogatePairs = false;
                                SurrogatePair = string.Empty;
                                SPCodePoint = 0;
                                UseCB = CharBuffer[i];
                                cbint1 = (int)UseCB;

                                // Check if we need to handle surrogate pairs
                                if (cbint1 >= 55296 & cbint1 <= 56319)
                                {
                                    if (i + 1 < CharBuffer.Length)
                                    {
                                        cbint2 = (int)CharBuffer[i + 1];
                                        if (cbint2 >= 56320 & cbint2 <= 57343) DoSurrogatePairs = true;

                                    }
                                }


                                if (DoSurrogatePairs)
                                {
                                    SPCodePoint = 65536 + ((cbint1 - 55296) * 1024) + (cbint2 - 56320);
                                    SurrogatePair = char.ConvertFromUtf32(SPCodePoint);
                                    CWidth = enc.GetByteCount(SurrogatePair);
                                    // Move the loop forward past the next character
                                    i++;
                                }
                                else
                                {
                                    CWidth = enc.GetByteCount(CharBuffer.Slice(i, 1));
                                }


                                // If in the requested byte range, process the data
                                if (CharacterCounter >= StartCharacter & CharacterCounter <= EndCharacter)
                                {
                                    LObj = new List<object>();

                                    LObj.Add((object)((CharacterCounter + 1).ToString()));
                                    LObj.Add((object)((CWidth).ToString()));

                                    if (DoSurrogatePairs)
                                    {
                                        LObj.Add((object)(SurrogatePair));
                                        LObj.Add((object)(SPCodePoint.ToString()));
                                        LObj.Add((object)(SPCodePoint.ToString("x8")));
                                    }
                                    else
                                    {
                                        LObj.Add((object)((UseCB).ToString()));
                                        LObj.Add((object)(((int)UseCB).ToString()));
                                        LObj.Add((object)(((int)UseCB).ToString("x8")));
                                    }

                                    IDO = new OIncrementalDataObject();
                                    IDO.IDO = LObj;
                                    IDO.ProcessedRowID = CharacterCounter;
                                    IDO.SourceIsText = true;

                                    AState.ProcessedRowsCount++;

                                    ActivityFunctionCaller(ref s, ref AW, ref AState, IDO);
                                }

                                //ByteCounter += CWidth;
                                CharacterCounter++;



                                if (CharacterCounter > EndCharacter) break;
                            }





                            if (CharacterCounter > EndCharacter) break;

                        } // End While


                        read_sw.Close();
                        read_sw.Dispose();

                        // Set the primary return value to success
                        ret.PrimaryReturnValue = RefReturnValues.Success;
                    }









                }
                catch (IOException e)  // This catch should trigger if there is a problem opening and reading from the file
                {
                    ainfo = "Cannot read from the specified source file";
                    CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInExternalFile, ProcessingPoint, ainfo, null, e);
                    return ret;
                }
                //**************************************************************************] END: Code Point Inspection


                return ret;
            }











            if (UseCustomReadingMethod == false)
            {


                //[************************************************************************** The standard file reading method
                // This is used when no field containers are specified, standard line terminators are specified and single-character delimiters are specified.
                // See the custom file reading method (below) for the alternative.
                // The custom file reading method is slow. The standard file reading method should be much faster.

                //[********************** Variables used for the standard read method
                string l;
                string[] sa;
                //**********************]


                try
                {
                    ProcessingPoint = "Open and read from source file - standard reading method";

                    using (StreamReader read_sw = new StreamReader(spo.FullPathAndFilename, enc, false, fso))
                    {


                        while (!read_sw.EndOfStream)
                        {
                            l = read_sw.ReadLine();

                            LFields = new List<string>();
                            IsFinalRow = false;
                            if (read_sw.EndOfStream) IsFinalRow = true;


                            // Split the row by the delimiter
                            sa = l.Split(UseDelimiters);

                            RawRowCount++;

                            LFields = sa.ToList();

                            PostProcessRow_TEXT(ref s, ref AW, ref AState, ref spo, ref LFields, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref IsFinalRow);

                            // Check if the early termination flag has been set. This only gets set in specific scenarios where the requested row sampling has been satisfied and there's no further need to read the rest of the file.
                            if (AState.EarlyTermination) break;

                            //// Handle the scenario where the delimiters haven't been correctly assigned by the user.
                            //// We don't want the accumulation buffer (in sb) to get too large. So cause a truncation of the process by setting LastRead = true.
                            //if (AState.ActivityName == ActivityName.Inspect)
                            //{
                            //    if (RawRowCount == 0 & CConfig_Inspect.ByteLimit > 0)
                            //    {
                            //        if (sb.Length > CConfig_Inspect.ByteLimit) LastRead = true;
                            //    }
                            //}



                            // Check if the user has requested to terminate the processing
                            if (Data_Scratchpad.SharedVars.StopProcessingRequestedByUser) break;

                        } // End While




                    }
                }
                catch (IOException e)  // This catch should trigger if there is a problem opening and reading from the file
                {
                    ainfo = "Cannot read from the specified source file";
                    CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInExternalFile, ProcessingPoint, ainfo, null, e);
                    return ret;
                }
                //**************************************************************************] END: The standard file reading method



            }
            else
            {



                //[************************************************************************** The custom file reading method
                // This is used when any of these things are true:
                // - field containers are specified
                // - custom line terminators are specified (not ASCII 13 & 10 )
                // - multi-character delimiters are specified

                //[********************** Variables used for the custom read method
                const int BlockSize = 2048;
                fso.BufferSize = BlockSize;
                Span<char> CharBuffer = new Span<char>(new char[BlockSize]);
                Span<char> BTCSpan;
                int BytesRead = 0;
                const int MaxAccumulatedBufferSize = 10000000;
                StringBuilder sb = new StringBuilder();
                bool DoFinalCheck = true;
                bool LastRead = false;
                string ct;
                long ByteLimit = 10000;
                int MaxRowLimit = 1000;
                //**********************]


                // Get the byte limit for the inspection
                ct = ds_common.Utilities.GetConfigParameter("Byte_limit", ref AState);
                if (string.IsNullOrEmpty(ct) == false)
                {
                    if (long.TryParse(ct, out ByteLimit))
                    {
                        if (ByteLimit <= 0) ByteLimit = 10000;
                        if (ByteLimit > 100000) ByteLimit = 100000;
                    }
                }



                try
                {
                    ProcessingPoint = "Open and read from source file - custom reading method";

                    using (StreamReader read_sw = new StreamReader(spo.FullPathAndFilename, enc, false, fso))
                    {

                        while (!read_sw.EndOfStream)
                        {
                            // Reset the buffer
                            CharBuffer = new Span<char>(new char[BlockSize]);

                            // Read a block of data into a buffer
                            BytesRead = read_sw.Read(CharBuffer);


                            // Determine if this is the last chunk of data read from the file
                            if (BytesRead < BlockSize)
                            {
                                LastRead = true;
                                CharBuffer = CharBuffer.Slice(0, BytesRead);
                            }



                            sb.Append(CharBuffer);

                            // Handle the scenario where the delimiters haven't been correctly assigned by the user.
                            // We don't want the accumulation buffer (in sb) to get too large. So cause a truncation of the process by setting LastRead = true.
                            if (string.Equals(AState.ActivityName, CommandName_INSPECT, StringComparison.OrdinalIgnoreCase))
                            {
                                if (ProcessedRowCount == 0 & ByteLimit > 0)
                                {
                                    if (sb.Length > ByteLimit)
                                    {
                                        if (AState.OutputObj.Information != null)
                                        {
                                            if (AState.OutputObj.Information.Count > 0)
                                            {
                                                AState.OutputObj.Information[0] = ds_common.Utilities.VisualisationType_Text;
                                            }
                                            else
                                            {
                                                AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Text);
                                            }
                                        }
                                        else
                                        {
                                            AState.OutputObj.Information = new List<string>();
                                            AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Text);
                                        }
                                        AState.OutputObj.PrimaryOutput.Add(sb.ToString());
                                        DoFinalCheck = false;
                                        break;
                                    }
                                }

                                if (ProcessedRowCount >= MaxRowLimit)
                                {
                                    DoFinalCheck = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (sb.Length > MaxAccumulatedBufferSize)
                                {
                                    DoFinalCheck = false;
                                    break;
                                }
                            }



                            DoMainLineChecks(ref s, ref AW, ref AState, ref LFields, ref sb, ref LastRead, ref tpp, ref spo, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref DoFinalCheck, ref IsFinalRow);


                            // Check if the early termination flag has been set. This only gets set in specific scenarios where the requested row sampling has been satisfied and there's no further need to read the rest of the file.
                            if (AState.EarlyTermination) break;


                            // Check if the user has requested to terminate the processing
                            if (Data_Scratchpad.SharedVars.StopProcessingRequestedByUser) break;

                        } // End While


                        // Catch the final edge case
                        if (BytesRead == BlockSize & DoFinalCheck & AState.EarlyTermination == false)
                        {
                            IsFinalRow = true;

                            if (spo.Params_Text_Tabulated.SkipEmptyRows == false & spo.Params_Text_Tabulated.SkipFinalEmptyRow == false)
                            {
                                if (string.Equals(sb.ToString(), string.Empty) == false)
                                {
                                    RawRowCount++;
                                    IsFinalRow = true;

                                    BTCSpan = sb.ToString().ToArray();
                                    // Do something with the accumulated string which now contains the final valid set of fields
                                    LFields = GetFieldsFromPreProcessedString(ref BTCSpan, ref tpp);
                                    // Do the relevant processing here ********************************************************************************************

                                    PostProcessRow_TEXT(ref s, ref AW, ref AState, ref spo, ref LFields, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref IsFinalRow);
                                }
                            }
                        }


                    }
                }
                catch (IOException e)  // This catch should trigger if there is a problem opening and reading from the file
                {
                    ainfo = "Cannot read from the specified source file";
                    CreateOFRErrorResponse(ref ret, RefReturnValues.ErrorInExternalFile, ProcessingPoint, ainfo, null, e);
                    return ret;
                }
                //**************************************************************************] END: The custom file reading method


            }



            if (Data_Scratchpad.SharedVars.StopProcessingRequestedByUser)
            {
                Data_Scratchpad.SharedVars.StopProcessingRequestedByUser = false;
                // Set the primary return value to cancelled by user
                ret.PrimaryReturnValue = RefReturnValues.CancelledByUser;
            }
            else
            {
                // Set the primary return value to success
                ret.PrimaryReturnValue = RefReturnValues.Success;
            }


            return ret;
        }








        static bool CheckForAtLeastNInstances(string CheckFor, ref string InString, int CountInstances)
        {
            //if (InString.Select(x => x.Equals(CheckFor)).ToList().Count >= CountInstances) return true;

            int n = 0;

            for (int i = 0; i < (InString.Length - CheckFor.Length); i++)
            {
                if (string.Equals(InString.Substring(i, CheckFor.Length), CheckFor)) n++;

            }


            if (n >= CountInstances) return true;

            return false;
        }







        static bool DoesPreambleMatch(Encoding enc, ref byte[] FileEarlyByteArray)
        {
            // FileEarlyByteArray is an array of bytes obtained from the start of the file. It must be at least four bytes long - the length of the largest unicode preamble (BOM)
            bool ret = false;

            byte[] EPremable = enc.GetPreamble();
            if (EPremable.Length == 0) return ret;
            if (FileEarlyByteArray.Length >= EPremable.Length)
            {
                byte[] Testb = new byte[EPremable.Length];
                Array.Copy(FileEarlyByteArray, 0, Testb, 0, EPremable.Length);
                if (Testb.SequenceEqual(EPremable)) ret = true;
            }

            return ret;
        }










        static bool GetBitState(byte b, int BitNumber)
        {
            return (b & (1 << BitNumber)) != 0;
        }















        static void DoMainLineChecks(ref OSource s, ref OAnalysisWindow AW, ref OActivityState AState, ref List<string> LFields, ref StringBuilder sb, ref bool LastRead, ref OTextProcessParams tpp, ref OSourceParameters_File spo, ref long RawRowCount, ref long FilteredRowCount, ref long ProcessedRowCount, ref bool DoFinalCheck, ref bool IsFinalRow)
        {
            bool HasValidLineTerminator = false;
            string PreLineTerminator = string.Empty;
            string Excess = string.Empty;
            Span<char> BTCSpan;


            BTCSpan = sb.ToString().ToArray();

            HasValidLineTerminator = NewLineCheck(ref BTCSpan, ref tpp, ref PreLineTerminator, ref Excess);


            if (HasValidLineTerminator)
            {
                if (spo.Params_Text_Tabulated.SkipEmptyRows)
                {
                    if (string.Equals(PreLineTerminator, string.Empty) == false)
                    {
                        RawRowCount++;
                        IsFinalRow = false;

                        // Do something with the string that contains the valid set of fields
                        BTCSpan = PreLineTerminator.ToArray();
                        LFields = GetFieldsFromPreProcessedString(ref BTCSpan, ref tpp);
                        // Do the relevant processing here ********************************************************************************************

                        PostProcessRow_TEXT(ref s, ref AW, ref AState, ref spo, ref LFields, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref IsFinalRow);
                    }
                }
                else
                {
                    RawRowCount++;
                    IsFinalRow = false;

                    // Do something with the string that contains the valid set of fields
                    BTCSpan = PreLineTerminator.ToArray();
                    LFields = GetFieldsFromPreProcessedString(ref BTCSpan, ref tpp);
                    // Do the relevant processing here ********************************************************************************************

                    PostProcessRow_TEXT(ref s, ref AW, ref AState, ref spo, ref LFields, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref IsFinalRow);
                }



                // Reset the string builder object with any excess after the line terminators
                sb.Clear();
                //sb = new StringBuilder();

                if (Excess.Length > 0)
                {
                    sb.Append(Excess);

                    //if (LastRead)
                    //{

                    BTCSpan = sb.ToString().ToArray();

                    // Run the process recursively not to check for any final line terminator and zero-length string combinations
                    DoMainLineChecks(ref s, ref AW, ref AState, ref LFields, ref sb, ref LastRead, ref tpp, ref spo, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref DoFinalCheck, ref IsFinalRow);

                    //}
                }
                else
                {
                    if (spo.Params_Text_Tabulated.SkipEmptyRows == false & spo.Params_Text_Tabulated.SkipFinalEmptyRow == false)
                    {
                        RawRowCount++;
                        IsFinalRow = true;

                        // Handle the scenario where the final excess after a new line terminator is a zero-length string
                        LFields = new List<string>();
                        // Do the relevant processing here ********************************************************************************************

                        PostProcessRow_TEXT(ref s, ref AW, ref AState, ref spo, ref LFields, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref IsFinalRow);
                    }

                    DoFinalCheck = false;
                }
            }
            else
            {
                if (LastRead)
                {
                    if (BTCSpan.Length == 0)
                    {
                        if (spo.Params_Text_Tabulated.SkipEmptyRows == false & spo.Params_Text_Tabulated.SkipFinalEmptyRow == false)
                        {
                            RawRowCount++;
                            IsFinalRow = true;

                            // Do something with the accumulated string which now contains the final valid set of fields
                            LFields = GetFieldsFromPreProcessedString(ref BTCSpan, ref tpp);
                            // Do the relevant processing here ********************************************************************************************
                            PostProcessRow_TEXT(ref s, ref AW, ref AState, ref spo, ref LFields, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref IsFinalRow);
                        }

                        DoFinalCheck = false;
                    }
                    else
                    {
                        RawRowCount++;
                        IsFinalRow = false;

                        // Do something with the accumulated string which now contains the final valid set of fields
                        LFields = GetFieldsFromPreProcessedString(ref BTCSpan, ref tpp);
                        // Do the relevant processing here ********************************************************************************************
                        PostProcessRow_TEXT(ref s, ref AW, ref AState, ref spo, ref LFields, ref RawRowCount, ref FilteredRowCount, ref ProcessedRowCount, ref IsFinalRow);

                        DoFinalCheck = false;
                    }
                }

            }

        }












        static bool PostProcessRow_TEXT(ref OSource s, ref OAnalysisWindow AW, ref OActivityState AState, ref OSourceParameters_File spo, ref List<string> LFields, ref long RawRowCount, ref long FilteredRowCount, ref long ProcessedRowCount, ref bool IsFinalRow)
        {
            // This function returns true when the List<string> LFields may proceed to be used by the processing code, based on the post-processing rules.
            // If it returns false, then it means that the contents of LFields has not met the conditions specified by the post-processing rules.
            // Note that the contents of LFields may have been modified by the post-processing rules.

            bool ret = false;


            // If appropriate, get the field names and IDs
            if (spo.Params_Text_Tabulated.NumHeaderRows > 0)
            {
                if (RawRowCount <= spo.Params_Text_Tabulated.NumHeaderRows)
                {
                    if (RawRowCount == spo.Params_Text_Tabulated.HeaderRowWithFieldNames)
                    {
                        // Get the original list of field header names
                        AState.SourceOriginalFieldHeaders = new List<string>();
                        AState.SourceOriginalFieldHeaders.AddRange(LFields);

                        //[************ If the Analysis Window specifies only particular fields to be included, populate the appropriate look-up variables with IDs and names
                        if (AW.UseFields.Count > 0)
                        {
                            for (int i = 0; i < AW.UseFields.Count; i++)
                            {
                                // If there's a field ID specified... Note that field numbers must be 1-based (a stipulation imposed on the user)
                                if (AW.UseFields[i].ID > 0)
                                {
                                    // If there's not already a corresponding field name, then populate it
                                    if (string.IsNullOrEmpty(AW.UseFields[i].FieldName) == true)
                                    {
                                        if (AW.UseFields[i].ID <= AState.SourceOriginalFieldHeaders.Count)
                                        {
                                            AW.UseFields[i].FieldName = AState.SourceOriginalFieldHeaders[AW.UseFields[i].ID - 1];
                                        }
                                    }
                                }
                                else
                                {
                                    // If there's no field ID specified, but there is a field name...
                                    if (string.IsNullOrEmpty(AW.UseFields[i].FieldName) == false)
                                    {
                                        // Get the ID by matching the field name with the header fields just obtained (above)
                                        int u;
                                        string t = AW.UseFields[i].FieldName;

                                        u = AState.SourceOriginalFieldHeaders.FindIndex(x => string.Equals(x, t, StringComparison.OrdinalIgnoreCase));
                                        if (u > -1)
                                        {
                                            AW.UseFields[i].ID = u + 1;  // Field numbers must be 1-based (a stipulation also imposed on the user)
                                        }
                                    }
                                }
                            }
                        }
                        //************]


                        //[************ If the Analysis Window specifies sampling conditions based on fields, then populate the appropriate look-up variables with IDs and names
                        //  *** IMPORTANT *** these look-up variables (for sampling conditions) are mapped relative to the source dataset, not the user-requested fields.
                        //  *** Hence, these IDs are ___zero-based___!!!!! ***
                        if (AW.FieldSamplingConditions.Count > 0)
                        {
                            for (int i = 0; i < AW.FieldSamplingConditions.Count; i++)
                            {
                                // If there's a field ID specified...
                                if (AW.FieldSamplingConditions[i].FieldIDPair.ID > 0)
                                {
                                    // Adjust the field ID to make it zero-based (as the user was required to enter 1-based values
                                    AW.FieldSamplingConditions[i].FieldIDPair.ID--;

                                    // If there's not already a corresponding field name, then populate it
                                    if (string.IsNullOrEmpty(AW.FieldSamplingConditions[i].FieldIDPair.FieldName) == true)
                                    {
                                        if (AW.FieldSamplingConditions[i].FieldIDPair.ID <= AState.SourceOriginalFieldHeaders.Count - 1)
                                        {
                                            AW.FieldSamplingConditions[i].FieldIDPair.FieldName = AState.SourceOriginalFieldHeaders[AW.FieldSamplingConditions[i].FieldIDPair.ID];
                                        }
                                    }
                                }
                                else
                                {
                                    // If there's no field ID specified, but there is a field name...
                                    if (string.IsNullOrEmpty(AW.FieldSamplingConditions[i].FieldIDPair.FieldName) == false)
                                    {
                                        // Get the ID by matching the field name with the header fields just obtained (above)
                                        int u;
                                        string t = AW.FieldSamplingConditions[i].FieldIDPair.FieldName;

                                        u = AState.SourceOriginalFieldHeaders.FindIndex(x => string.Equals(x, t, StringComparison.OrdinalIgnoreCase));
                                        if (u > -1)
                                        {
                                            AW.FieldSamplingConditions[i].FieldIDPair.ID = u;
                                        }
                                    }
                                }
                            }
                        }
                        //************]
                    }

                    return ret;
                }
            }






            // Skip empty rows, if requested
            if (LFields.Count == 0)
            {
                if (spo.Params_Text_Tabulated.SkipEmptyRows)
                {
                    if (IsFinalRow)
                    {
                        if (spo.Params_Text_Tabulated.SkipFinalEmptyRow)
                        {
                            return ret;
                        }
                    }
                    else
                    {
                        return ret;
                    }
                }

                if (IsFinalRow & spo.Params_Text_Tabulated.SkipFinalEmptyRow) return ret;
            }


            // Skip rows with a different number of fields than specified, if requested
            if (spo.Params_Text_Tabulated.SkipRowsWithDifferentNumFields != null & spo.Params_Text_Tabulated.FixedNumFields)
            {
                if ((bool)spo.Params_Text_Tabulated.SkipRowsWithDifferentNumFields & spo.Params_Text_Tabulated.NumFields != null)
                {
                    if (LFields.Count != (int)spo.Params_Text_Tabulated.NumFields) return ret;
                }
            }


            // Trim white space, if requested
            if (spo.Params_Text_Tabulated.TrimFieldWhiteSpace)
            {
                for (int i = 0; i < LFields.Count; i++)
                {
                    LFields[i] = LFields[i].Trim();
                }
            }





            OIncrementalDataObject IDO = new OIncrementalDataObject();








            //[************************************************** Do field-based sampling
            // If there are some constraints based on the Analysis Window, incorporate those in the field and row sampling
            if (AW.FieldSamplingConditions.Count > 0)
            {
                if (LFields.Count == 0) return ret;

                double TestVal;
                for (int i = 0; i < AW.FieldSamplingConditions.Count; i++)
                {
                    if (AW.FieldSamplingConditions[i].IsNumber)
                    {
                        if (double.TryParse(LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID], out TestVal))
                        {
                            switch (AW.FieldSamplingConditions[i].Condition)
                            {
                                case SamplingConditionType.AbsoluteEquals:
                                    if (TestVal != AW.FieldSamplingConditions[i].NumericConditionValue1) return ret;
                                    break;

                                case SamplingConditionType.AbsoluteNotEquals:
                                    if (TestVal == AW.FieldSamplingConditions[i].NumericConditionValue1) return ret;
                                    break;

                                case SamplingConditionType.AbsoluteGreaterThan:
                                    if (TestVal <= AW.FieldSamplingConditions[i].NumericConditionValue1) return ret;
                                    break;

                                case SamplingConditionType.AbsoluteGreaterThanOrEqual:
                                    if (TestVal < AW.FieldSamplingConditions[i].NumericConditionValue1) return ret;
                                    break;

                                case SamplingConditionType.AbsoluteLessThan:
                                    if (TestVal >= AW.FieldSamplingConditions[i].NumericConditionValue1) return ret;
                                    break;

                                case SamplingConditionType.AbsoluteLessThanOrEqual:
                                    if (TestVal > AW.FieldSamplingConditions[i].NumericConditionValue1) return ret;
                                    break;

                                case SamplingConditionType.AbsoluteBetween:
                                    if (TestVal < AW.FieldSamplingConditions[i].NumericConditionValue1 || TestVal > AW.FieldSamplingConditions[i].NumericConditionValue2) return ret;
                                    break;
                            }

                        }
                        else
                        {
                            return ret;
                        }
                    }
                    else
                    {
                        switch (AW.FieldSamplingConditions[i].Condition)
                        {
                            case SamplingConditionType.AbsoluteEquals:
                                if (string.Equals(LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID], AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == false) return ret;
                                break;

                            case SamplingConditionType.AbsoluteNotEquals:
                                if (string.Equals(LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID], AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == true) return ret;
                                break;

                            case SamplingConditionType.AbsoluteContains:
                                if (LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID].Contains(AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == false) return ret;
                                break;

                            case SamplingConditionType.AbsoluteNotContains:
                                if (LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID].Contains(AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == true) return ret;
                                break;

                            case SamplingConditionType.AbsoluteStartsWith:
                                if (LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID].StartsWith(AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == false) return ret;
                                break;

                            case SamplingConditionType.AbsoluteNotStartsWith:
                                if (LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID].StartsWith(AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == true) return ret;
                                break;

                            case SamplingConditionType.AbsoluteEndsWith:
                                if (LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID].EndsWith(AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == false) return ret;
                                break;

                            case SamplingConditionType.AbsoluteNotEndsWith:
                                if (LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID].EndsWith(AW.FieldSamplingConditions[i].StrConditionValue, StringComparison.OrdinalIgnoreCase) == true) return ret;
                                break;

                            case SamplingConditionType.AbsoluteIsEmpty:
                                if (string.IsNullOrEmpty(LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID]) == false) return ret;
                                break;

                            case SamplingConditionType.AbsoluteIsNotEmpty:
                                if (string.IsNullOrEmpty(LFields[AW.FieldSamplingConditions[i].FieldIDPair.ID]) == true) return ret;
                                break;
                        }

                    }
                }
            }
            //**************************************************]




            // Increment the counter that indicates the potential row count based on field filtering
            FilteredRowCount++;


            // Create the effective "data row count" variable, to accommodate the presence of header rows
            long UsableRowCount = RawRowCount;
            if (AW.ApplyRowSamplingAfterFieldSampling)
            {
                UsableRowCount = FilteredRowCount;
            }
            else
            {
                if (spo.Params_Text_Tabulated.NumHeaderRows > 0)
                {
                    if (RawRowCount > spo.Params_Text_Tabulated.NumHeaderRows)
                    {
                        UsableRowCount = RawRowCount - (int)spo.Params_Text_Tabulated.NumHeaderRows;
                    }
                }
            }



            //[********************************************************************* Do row count-based sampling
            if (AW.RowsSamplingType != SamplingType.All)
            {
                switch (AW.RowsSamplingType)
                {

                    case SamplingType.NumbersList:
                        if (AW.RowsList != null)
                        {
                            if (AW.RowsList.Count > 0)
                            {
                                switch (AW.RowsSamplingTypeQualifier)
                                {
                                    // An explicit list of desired rows
                                    case SamplingTypeQualifier.NoQualifier:
                                        if (AW.RowsList.Contains(UsableRowCount) == false)
                                        {
                                            if (AState.ProcessedRowsCount == AW.RowsList.Count)
                                            {
                                                // Set the early termination flag, so that the file reading process doesn't have to keep reading the rest of the file unnecessarily
                                                AState.EarlyTermination = true;
                                            }
                                            return ret;
                                        }
                                        break;

                                    // The first X-many rows
                                    case SamplingTypeQualifier.First:
                                        if (AW.RowsList[0] > 0)
                                        {
                                            if (UsableRowCount > AW.RowsList[0])
                                            {
                                                // Set the early termination flag, so that the file reading process doesn't have to keep reading the rest of the file unnecessarily
                                                AState.EarlyTermination = true;
                                                return ret;
                                            }
                                        }
                                        break;

                                    case SamplingTypeQualifier.Last:
                                        break;
                                }
                            }
                        }
                        break;

                    case SamplingType.NumbersRange:
                        if (AW.RowsList != null)
                        {
                            if (AW.RowsList.Count >= 2)
                            {
                                if (AW.RowsList[0] <= AW.RowsList[1])
                                {
                                    if (UsableRowCount < AW.RowsList[0]) return ret;
                                    if (UsableRowCount > AW.RowsList[1])
                                    {
                                        // Set the early termination flag, so that the file reading process doesn't have to keep reading the rest of the file unnecessarily
                                        AState.EarlyTermination = true;
                                        return ret;
                                    }
                                }
                            }
                        }
                        break;

                    case SamplingType.Percentage:
                        if (AW.RowsPercentage >= 0.0d & AW.RowsPercentage <= 1.0d)
                        {
                            switch (AW.RowsSamplingTypeQualifier)
                            {
                                // The first X% of rows
                                case SamplingTypeQualifier.First:
                                    // Currently unsupported - requires pre-processing the file to determine the total number of raw rows
                                    break;

                                // The last X% of rows
                                case SamplingTypeQualifier.Last:
                                    // Currently unsupported - requires pre-processing the file to determine the total number of raw rows
                                    break;

                                // Random sampling using a seed fixed at the point of application start, so that all samples commanded within the same instance of the program will produce the same outputs, for e.g. comparison purposes
                                case SamplingTypeQualifier.RandomStatic:
                                    if (RSampler.NextDouble() > AW.RowsPercentage) return ret;
                                    break;

                                // Random sampling using a dynamic seed, so that samples will be randomised on every command execution
                                case SamplingTypeQualifier.RandomDynamic:
                                    //double k = RSampler.NextDouble();
                                    if (RSampler.NextDouble() > AW.RowsPercentage) return ret;
                                    break;
                            }
                        }
                        break;

                }
            }
            //*********************************************************************]




            // If all of the sampling conditions, row exclusions, etc have been satisfied, then proceed to add the row to the process-able dataset
            ret = true;
            List<object> UseLFields = new List<object>();


            //[*************** Only include the requested fields, if necessary
            if (AW.UseFields.Count > 0)
            {
                for (int i = 0; i < AW.UseFields.Count; i++)
                {
                    // Field numbers used for sampling must be 1-based (a stipulation imposed on the user, and supported in the above header processing code)
                    if (AW.UseFields[i].ID > 0 & AW.UseFields[i].ID <= LFields.Count)
                    {
                        UseLFields.Add(LFields[AW.UseFields[i].ID - 1]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < LFields.Count; i++)
                {
                    UseLFields.Add((object)LFields[i]);
                }
                //UseLFields = LFields;
            }
            //***************]




            ProcessedRowCount++;

            IDO.IDO = UseLFields;
            IDO.ProcessedRowID = ProcessedRowCount;

            // Set this without conditional checks, because this is within the function PostProcessRow_TEXT(). Must change if used in a non-text-based source.
            IDO.SourceIsText = true;

            // Update the state object's processed rows counter
            AState.ProcessedRowsCount = ProcessedRowCount;


            //[************************* If all good to process the row, then call the Activity Function Caller
            ActivityFunctionCaller(ref s, ref AW, ref AState, IDO);
            //*************************]



            return ret;
        }










        static List<string> GetFieldsFromPreProcessedString(ref Span<char> BufferToCheck, ref OTextProcessParams tpp)
        {
            // This takes as input a string which is a single set of valid fields, with no valid line terminators (i.e. it's already been "split" by the valid line terminator)
            List<string> ret = new List<string>();
            int LastPos = 0;
            string t;
            Span<char> tSpan;

            if (BufferToCheck.Length == 0) return ret;

            // If field containers are not being used, then simply split by the field delimiter
            if (tpp.n_fc == 0)
            {

                for (int i = 0; i < BufferToCheck.Length; i++)
                {
                    if (i + tpp.n_delim <= BufferToCheck.Length)
                    {
                        if (BufferToCheck.Slice(i, tpp.n_delim).SequenceEqual(tpp.UseDelimiters))
                        {
                            t = BufferToCheck.Slice(LastPos, i - LastPos).ToString();
                            ret.Add(t);

                            LastPos = i + tpp.n_delim;
                            i += tpp.n_delim - 1;

                            // Handle the scenario where the final element after a delimiter is a zero-length string
                            if (LastPos >= BufferToCheck.Length) ret.Add(string.Empty);
                        }
                        else
                        {
                            if (BufferToCheck.Slice(LastPos).IndexOf(tpp.UseDelimiters) == -1)
                            {
                                t = BufferToCheck.Slice(LastPos).ToString();
                                ret.Add(t);
                                return ret;
                            }
                        }
                    }
                    else
                    {
                        t = BufferToCheck.Slice(LastPos).ToString();
                        ret.Add(t);
                        return ret;
                    }
                }

                return ret;
            }



            // From here there are field containers, so need to handle them


            int p1 = -1;
            int p2 = -1;

            // Loop over the characters to try to find the first occurrence of a valid line terminator
            for (int i = 0; i < BufferToCheck.Length; i++)
            {

                // If the counter is not inside a pair of field containers, check if the field delimiter exists
                if (p1 == -1)
                {
                    if (i + tpp.n_delim <= BufferToCheck.Length)
                    {
                        if (BufferToCheck.Slice(i, tpp.n_delim).SequenceEqual(tpp.UseDelimiters))
                        {
                            t = BufferToCheck.Slice(LastPos, i - LastPos).ToString();

                            // Remove the field containers, if they exist
                            tSpan = t.ToArray();
                            //if (t.Substring(0, tpp.n_fc) == tpp.FieldContainers & t.Substring(t.Length - tpp.n_fc, tpp.n_fc) == tpp.FieldContainers)
                            if (tSpan.Slice(0, tpp.n_fc).SequenceEqual(tpp.FieldContainers) & tSpan.Slice(t.Length - tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                            {
                                t = tSpan.Slice(tpp.n_fc, tSpan.Length - (tpp.n_fc * 2)).ToString();
                            }

                            // Convert the escaped and slashed quotes to just the quotes, as necessary
                            if (tpp.IsQuoteFieldContainers)
                            {
                                if (tpp.SupportSlashEscapedQuotes)
                                {
                                    if (string.Equals(tpp.FieldContainers, str_sglquote))
                                    {
                                        if (t.IndexOf(str_slashed_sglquote) > -1)
                                        {
                                            t = t.Replace(str_slashed_sglquote, str_sglquote);
                                        }
                                    }
                                    else
                                    {
                                        if (string.Equals(tpp.FieldContainers, str_dblquote))
                                        {
                                            if (t.IndexOf(str_slashed_dblquote) > -1)
                                            {
                                                t = t.Replace(str_slashed_dblquote, str_dblquote);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (string.Equals(tpp.FieldContainers, str_sglquote))
                                    {
                                        if (t.IndexOf(str_escaped_sglquote) > -1)
                                        {
                                            t = t.Replace(str_escaped_sglquote, str_sglquote);
                                        }
                                    }
                                    else
                                    {
                                        if (string.Equals(tpp.FieldContainers, str_dblquote))
                                        {
                                            if (t.IndexOf(str_escaped_dblquote) > -1)
                                            {
                                                t = t.Replace(str_escaped_dblquote, str_dblquote);
                                            }
                                        }
                                    }
                                }
                            }

                            ret.Add(t);

                            LastPos = i + tpp.n_delim;
                            i += tpp.n_delim;
                        }
                    }
                    else
                    {
                        t = BufferToCheck.Slice(LastPos).ToString();
                        ret.Add(t);
                        return ret;
                    }
                }




                if (tpp.IsQuoteFieldContainers)
                {

                    if (i == LastPos)
                    {
                        if (i + tpp.n_fc <= BufferToCheck.Length)
                        {
                            if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                            {
                                if (p1 == -1)
                                {
                                    p1 = i;
                                }
                                else
                                {
                                    p2 = i;
                                }
                                i += tpp.n_fc - 1;
                            }
                        }
                    }
                    else
                    {
                        if (i + tpp.n_fc + 1 <= BufferToCheck.Length)
                        {
                            if (tpp.SupportSlashEscapedQuotes)
                            {
                                if (BufferToCheck.Slice(i, tpp.n_fc + 1).SequenceEqual('\\' + tpp.FieldContainers))
                                {
                                    i++;
                                }
                                else
                                {
                                    if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                                    {
                                        if (p1 == -1)
                                        {
                                            p1 = i;
                                        }
                                        else
                                        {
                                            p2 = i;
                                        }
                                        i += tpp.n_fc - 1;
                                    }
                                }
                            }
                            else
                            {
                                if (BufferToCheck.Slice(i, tpp.n_fc + 1).SequenceEqual(str_escaped_dblquote) || BufferToCheck.Slice(i, tpp.n_fc + 1).SequenceEqual(str_escaped_sglquote))
                                {
                                    i++;
                                }
                                else
                                {
                                    if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                                    {
                                        if (p1 == -1)
                                        {
                                            p1 = i;
                                        }
                                        else
                                        {
                                            p2 = i;
                                        }
                                        i += tpp.n_fc - 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (i + tpp.n_fc <= BufferToCheck.Length)
                            {
                                if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                                {
                                    if (p1 == -1)
                                    {
                                        p1 = i;
                                    }
                                    else
                                    {
                                        p2 = i;
                                    }
                                    i += tpp.n_fc - 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (i + tpp.n_fc <= BufferToCheck.Length)
                    {
                        if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                        {
                            if (p1 == -1)
                            {
                                p1 = i;
                            }
                            else
                            {
                                p2 = i;
                            }
                            i += tpp.n_fc - 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (p1 > -1 & p2 > -1)
                {
                    p1 = -1;
                    p2 = -1;
                }
            }  // End For


            // Do a check for any excess characters after the last delimiter
            if (LastPos < BufferToCheck.Length)
            {
                if (p1 == -1)  // The last field was correctly contained 
                {

                    t = BufferToCheck.Slice(LastPos).ToString();

                    // Remove the field containers, if they exist
                    tSpan = t.ToArray();
                    //if (t.Substring(0, tpp.n_fc) == tpp.FieldContainers & t.Substring(t.Length - tpp.n_fc, tpp.n_fc) == tpp.FieldContainers)
                    if (tSpan.Slice(0, tpp.n_fc).SequenceEqual(tpp.FieldContainers) & tSpan.Slice(t.Length - tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                    {
                        t = tSpan.Slice(tpp.n_fc, tSpan.Length - (tpp.n_fc * 2)).ToString();
                    }

                    // Convert the escaped and slashed quotes to just the quotes, as necessary
                    if (tpp.IsQuoteFieldContainers)
                    {
                        if (tpp.SupportSlashEscapedQuotes)
                        {
                            if (string.Equals(tpp.FieldContainers, str_sglquote))
                            {
                                if (t.IndexOf(str_slashed_sglquote) > -1)
                                {
                                    t = t.Replace(str_slashed_sglquote, str_sglquote);
                                }
                            }
                            else
                            {
                                if (string.Equals(tpp.FieldContainers, str_dblquote))
                                {
                                    if (t.IndexOf(str_slashed_dblquote) > -1)
                                    {
                                        t = t.Replace(str_slashed_dblquote, str_dblquote);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (string.Equals(tpp.FieldContainers, str_sglquote))
                            {
                                if (t.IndexOf(str_escaped_sglquote) > -1)
                                {
                                    t = t.Replace(str_escaped_sglquote, str_sglquote);
                                }
                            }
                            else
                            {
                                if (string.Equals(tpp.FieldContainers, str_dblquote))
                                {
                                    if (t.IndexOf(str_escaped_dblquote) > -1)
                                    {
                                        t = t.Replace(str_escaped_dblquote, str_dblquote);
                                    }
                                }
                            }
                        }
                    }

                    ret.Add(t);

                }
                else
                {  // The last field was not correctly contained - treat as a literal string
                    t = BufferToCheck.Slice(LastPos).ToString();
                    ret.Add(t);
                    return ret;
                }
            }





            return ret;
        }




















        static bool NewLineCheck(ref Span<char> BufferToCheck, ref OTextProcessParams tpp, ref string PreLineTerminator, ref string Excess)
        {
            PreLineTerminator = String.Empty;
            Excess = String.Empty;

            // Check all the simple cases first
            int u;

            // Is the line terminator string present at all
            u = BufferToCheck.IndexOf(tpp.UseLineTerminators);
            if (u == -1) return false;

            if (u == 0)
            {
                Excess = BufferToCheck.Slice(tpp.n_ult).ToString();
                return true;
            }

            // If there are no field containers, then the line terminators are valid
            if (tpp.n_fc == 0)
            {
                PreLineTerminator = BufferToCheck.Slice(0, u).ToString();
                if (u + tpp.n_ult <= BufferToCheck.Length) Excess = BufferToCheck.Slice(u + tpp.n_ult).ToString();
                return true;
            }



            // From here there are field containers, so need to handle them

            if (u == tpp.n_fc)
            {
                if (BufferToCheck.Slice(0, tpp.n_fc).SequenceEqual(tpp.FieldContainers) == false)
                {
                    return true;
                }
            }



            int p1 = -1;
            int p2 = -1;

            // Loop over the characters to try to find the first occurrence of a valid line terminator
            for (int i = 0; i < BufferToCheck.Length; i++)
            {

                // If the counter is not inside a pair of field containers, check if the line terminator exists
                if (p1 == -1)
                {
                    if (i + tpp.n_ult <= BufferToCheck.Length)
                    {
                        if (BufferToCheck.Slice(i, tpp.n_ult).SequenceEqual(tpp.UseLineTerminators))
                        {
                            PreLineTerminator = BufferToCheck.Slice(0, i).ToString();
                            if (i + tpp.n_ult <= BufferToCheck.Length) Excess = BufferToCheck.Slice(i + tpp.n_ult).ToString();
                            return true;
                        }
                    }
                }


                if (tpp.IsQuoteFieldContainers)
                {
                    if (i + tpp.n_fc + 1 <= BufferToCheck.Length)
                    {
                        if (tpp.SupportSlashEscapedQuotes)
                        {
                            if (BufferToCheck.Slice(i, tpp.n_fc + 1).SequenceEqual('\\' + tpp.FieldContainers))
                            {
                                i++;
                            }
                            else
                            {
                                if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                                {
                                    if (p1 == -1)
                                    {
                                        p1 = i;
                                    }
                                    else
                                    {
                                        p2 = i;
                                    }
                                    i += tpp.n_fc - 1;
                                }
                            }
                        }
                        else
                        {
                            if (BufferToCheck.Slice(i, tpp.n_fc + 1).SequenceEqual(str_escaped_dblquote) || BufferToCheck.Slice(i, tpp.n_fc + 1).SequenceEqual(str_escaped_sglquote))
                            {
                                i++;
                            }
                            else
                            {
                                if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                                {
                                    if (p1 == -1)
                                    {
                                        p1 = i;
                                    }
                                    else
                                    {
                                        p2 = i;
                                    }
                                    i += tpp.n_fc - 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (i + tpp.n_fc <= BufferToCheck.Length)
                    {
                        if (BufferToCheck.Slice(i, tpp.n_fc).SequenceEqual(tpp.FieldContainers))
                        {
                            if (p1 == -1)
                            {
                                p1 = i;
                            }
                            else
                            {
                                p2 = i;
                            }
                            i += tpp.n_fc - 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (p1 > -1 & p2 > -1)
                {
                    p1 = -1;
                    p2 = -1;
                }
            }






            return false;
        }



















        static bool BasicValidateObject_OSPF_Text_Tabulated(OSPF_Text_Tabulated o)
        {
            bool ret = false;
            if (o == null) return ret;
            if (o.FieldDelimiterCodes == null & string.IsNullOrEmpty(o.FieldDelimiters)) return ret;
            if (o.FieldDelimiterCodes != null)
            {
                if (o.FieldDelimiterCodes.Count == 0 & string.IsNullOrEmpty(o.FieldDelimiters)) return ret;
            }
            if (o.LineTerminatorCodes == null & string.IsNullOrEmpty(o.LineTerminators)) return ret;
            if (o.LineTerminatorCodes != null)
            {
                if (o.LineTerminatorCodes.Count == 0 & string.IsNullOrEmpty(o.LineTerminators)) return ret;
            }

            ret = true;
            return ret;
        }



        static bool BasicValidateObject_SourceParametersFile(OSourceParameters_File o)
        {
            bool ret = false;
            if (o == null) return ret;
            if (string.IsNullOrEmpty(o.FullPathAndFilename)) return ret;

            // Simple permissible combinations
            if (o.FileFormat == FileFormats.GenericDelimited & o.IsTextFile == false & o.IsStructured == false) return ret;
            if (o.FileFormat == FileFormats.CSV & o.IsTextFile == false & o.IsStructured == false) return ret;
            if (o.FileFormat == FileFormats.JSON & o.IsTextFile == false & o.IsStructured == false) return ret;
            if (o.FileFormat == FileFormats.XML & o.IsTextFile == false & o.IsStructured == false) return ret;
            if (o.FileFormat == FileFormats.FreeformText & o.IsTextFile == false & o.IsStructured == true) return ret;
            if (o.FileFormat == FileFormats.OrderedBinaryData & o.IsTextFile == true & o.IsStructured == false) return ret;
            if (o.FileFormat == FileFormats.Excel & o.IsTextFile == true & o.IsStructured == false) return ret;

            // Check appropriate object existence (order of checks matters here)
            if (o.Params_Text_Tabulated != null & o.IsTextFile == false) return ret;
            if (o.Params_Text_Tabulated != null & o.IsStructured == false) return ret;
            if (o.Params_Text_NonTabulated != null & o.IsTextFile == false) return ret;
            if (o.Params_Text_NonTabulated != null & o.IsStructured == false) return ret;
            if (o.Params_Binary_Repeating != null & o.IsTextFile == true) return ret;
            if (o.Params_Binary_Repeating != null & o.IsStructured == false) return ret;
            if (o.Params_Binary_Excel != null & o.IsTextFile == true) return ret;
            if (o.Params_Binary_Excel != null & o.IsStructured == false) return ret;

            ret = true;
            return ret;
        }






        static ofr ParseSourceDefinition(List<string> LS, ref OSource s)
        {
            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "ParseSourceDefinition";
            // Set the Processing Point description
            ret.ProcessingPoint = "Validate inputs";
            ret.AdditionalInfo = string.Empty;


            if (LS == null) return ret;
            //if (LS.Count == 0) return ret;


            //[************ Parameter names - These are the keywords users can use to define a data source
            const string pn_SourceType = "source_type";
            //const string pn_SourceName = "source_name";
            // File
            const string pn_File_FullPathAndFilename = "file_path";
            const string pn_File_FileFormat = "file_format";
            const string pn_File_FileEncoding = "file_encoding";
            const string pn_File_Text_FieldDelimiterCodes = "field_delimiter_codes";
            const string pn_File_Text_FieldDelimiters = "field_delimiter_string";
            const string pn_File_Text_FieldContainers = "field_container_string";
            const string pn_File_Text_LineTerminatorCodes = "line_terminator_codes";
            const string pn_File_Text_LineTerminators = "line_terminator_string";
            const string pn_File_Text_NumHeaderRows = "header_rows_count";
            const string pn_File_Text_HeaderRowWithFieldNames = "header_row_with_field_names";
            const string pn_File_Text_FixedNumFields = "fixed_field_count";
            const string pn_File_Text_NumFields = "field_count";
            const string pn_File_Text_TrimFieldWhiteSpace = "trim_field_white_space";
            const string pn_File_Text_SkipEmptyRows = "skip_empty_rows";
            const string pn_File_Text_SkipFinalEmptyRow = "skip_final_empty_row";
            const string pn_File_Text_SkipRowsWithDifferentNumFields = "skip_rows_with_different_number_of_fields";
            const string pn_File_Text_SchemaReferenceFilePath = "schema_reference_file_path";
            const string pn_File_Binary_DataTypesSequence = "data_types_sequence";
            const string pn_File_Binary_Excel_SheetName = "sheet_name";
            const string pn_File_Binary_Excel_Password = "password";
            //// Database
            //const string pn_DB_DBType = "db_type";
            //const string pn_DB_ConnectionStr = "connection_string";
            //const string pn_DB_SchemaName = "schema_name";
            //const string pn_DB_DBName = "db_name";
            //const string pn_DB_TableName = "table_name";
            //// API
            //const string pn_API_HostName = "host_name";
            //const string pn_API_Port = "post";
            //const string pn_API_EndPointPath = "endpoint_path";
            //const string pn_API_HeaderStr = "header_string";
            //const string pn_API_AuthenticationType = "authentication_type";
            //const string pn_API_UserName = "username";
            //const string pn_API_Password = "password";
            //************]


            // Output variables to obtain
            OSourceKeywordsObtained keywordsObtained = new OSourceKeywordsObtained();

            // Some constants and variables
            const string Str_Equals = "=";
            const int nb = 2;
            string[] r;
            string t;
            string pn;
            string pvalue;
            int iofequals;


            // Loop over the rows
            for (int i = 0; i < LS.Count; i++)
            {
                // Set the variables
                t = LS[i].Trim();
                pn = string.Empty;
                pvalue = string.Empty;

                // Only consider rows with content
                if (string.IsNullOrEmpty(t) == false)
                {
                    // Split by the equals character
                    r = t.Split(Str_Equals);

                    // Check the number of substrings found - only analyse those rows with at least the required number of strings
                    if (r.Count() >= nb)
                    {
                        // Get the parameter name and value strings for the simple case of only one equals character in the row
                        if (r.Count() == nb)
                        {
                            pn = r[0].Trim();
                            pvalue = r[1];
                        }
                        else
                        {
                            // Where there is more than one equals character, get the parameter name and get the appropriate parameter value string by only looking past the first equals character
                            if (r.Count() > nb)
                            {
                                pn = r[0].Trim();

                                iofequals = t.IndexOf(Str_Equals);
                                if (iofequals > 0)
                                {
                                    pvalue = t.Substring(iofequals + 1);
                                }


                            }
                        }


                        switch (pn.ToLowerInvariant())
                        {
                            case pn_SourceType:
                                keywordsObtained.SourceType = pvalue.Trim();
                                break;
                            //case pn_SourceName:
                            //    keywordsObtained.SourceName = pvalue.Trim();
                            //    break;
                            case pn_File_FullPathAndFilename:
                                keywordsObtained.File_FullPathAndFilename = pvalue.Trim();
                                break;
                            case pn_File_FileFormat:
                                keywordsObtained.File_FileFormat = pvalue.Trim();
                                break;
                            case pn_File_FileEncoding:
                                keywordsObtained.File_FileEncoding = pvalue.Trim();
                                break;
                            case pn_File_Text_FieldDelimiterCodes:
                                keywordsObtained.File_Text_FieldDelimiterCodes = pvalue.Trim();
                                break;
                            case pn_File_Text_FieldDelimiters:
                                keywordsObtained.File_Text_FieldDelimiters = pvalue.Trim();
                                break;
                            case pn_File_Text_FieldContainers:
                                keywordsObtained.File_Text_FieldContainers = pvalue.Trim();
                                break;
                            case pn_File_Text_LineTerminatorCodes:
                                keywordsObtained.File_Text_LineTerminatorCodes = pvalue.Trim();
                                break;
                            case pn_File_Text_LineTerminators:
                                keywordsObtained.File_Text_LineTerminators = pvalue.Trim();
                                break;
                            case pn_File_Text_NumHeaderRows:
                                keywordsObtained.File_Text_NumHeaderRows = pvalue.Trim();
                                break;
                            case pn_File_Text_HeaderRowWithFieldNames:
                                keywordsObtained.File_Text_HeaderRowWithFieldNames = pvalue.Trim();
                                break;
                            case pn_File_Text_FixedNumFields:
                                keywordsObtained.File_Text_FixedNumFields = pvalue.Trim();
                                break;
                            case pn_File_Text_NumFields:
                                keywordsObtained.File_Text_NumFields = pvalue.Trim();
                                break;
                            case pn_File_Text_TrimFieldWhiteSpace:
                                keywordsObtained.File_Text_TrimFieldWhiteSpace = pvalue.Trim();
                                break;
                            case pn_File_Text_SkipEmptyRows:
                                keywordsObtained.File_Text_SkipEmptyRows = pvalue.Trim();
                                break;
                            case pn_File_Text_SkipFinalEmptyRow:
                                keywordsObtained.File_Text_SkipFinalEmptyRow = pvalue.Trim();
                                break;
                            case pn_File_Text_SkipRowsWithDifferentNumFields:
                                keywordsObtained.File_Text_SkipRowsWithDifferentNumFields = pvalue.Trim();
                                break;
                            case pn_File_Text_SchemaReferenceFilePath:
                                keywordsObtained.File_Text_SchemaReferenceFilePath = pvalue.Trim();
                                break;
                            case pn_File_Binary_DataTypesSequence:
                                keywordsObtained.File_Binary_DataTypesSequence = pvalue.Trim();
                                break;
                            case pn_File_Binary_Excel_SheetName:
                                keywordsObtained.File_Binary_Excel_SheetName = pvalue.Trim();
                                break;
                            case pn_File_Binary_Excel_Password:
                                keywordsObtained.File_Binary_Excel_Password = pvalue.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }



            s = new OSource();


            //[*********** Core validations and trims
            const int MaxSourceNameLength = 2048;
            const int MaxFilePathLength = 32000;

            //if (string.IsNullOrEmpty(keywordsObtained.SourceName.Trim()))
            //{
            //    ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
            //    ret.AdditionalInfo = "No " + pn_SourceName + " specified";
            //    return ret;
            //}

            if (string.IsNullOrEmpty(keywordsObtained.SourceType.Trim()))
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                ret.AdditionalInfo = "No " + pn_SourceType + " specified";
                return ret;
            }

            if (string.IsNullOrEmpty(keywordsObtained.File_FullPathAndFilename.Trim()))
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                ret.AdditionalInfo = "No " + pn_File_FullPathAndFilename + " specified";
                return ret;
            }

            if (string.IsNullOrEmpty(keywordsObtained.File_FileFormat.Trim()))
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                ret.AdditionalInfo = "No " + pn_File_FileFormat + " specified";
                return ret;
            }


            //keywordsObtained.SourceName = keywordsObtained.SourceName.Trim();
            //if (keywordsObtained.SourceName.Length > MaxSourceNameLength)
            //{
            //    ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
            //    ret.AdditionalInfo = "Parameter " + pn_SourceName + " is longer than the maximum allowed string length: " + MaxSourceNameLength.ToString() + " characters";
            //    return ret;
            //}

            keywordsObtained.File_FullPathAndFilename = keywordsObtained.File_FullPathAndFilename.Trim();
            if (keywordsObtained.File_FullPathAndFilename.Length > MaxFilePathLength)
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                ret.AdditionalInfo = "Parameter " + pn_File_FullPathAndFilename + " is longer than the maximum allowed string length: " + MaxSourceNameLength.ToString() + " characters";
                return ret;
            }
            //***********]

            //s.Name = keywordsObtained.SourceName.Trim().Replace(' ', '_');
            bool GetParams = false;
            OSourceParameters_File spf = new OSourceParameters_File();


            if (string.Equals(keywordsObtained.SourceType, nameof(SourceTypes.File), StringComparison.OrdinalIgnoreCase))
            {
                s.SourceType = SourceTypes.File;
                GetParams = CreateSource_File(keywordsObtained, ref spf);
                if (GetParams)
                {
                    s.SourceParameters = spf;
                }
                else
                {
                    ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                    ret.AdditionalInfo = "One or more invalid values specified for the Source parameters. Please see the documentation.";
                    return ret;
                }
            }
            else
            {
                ret.PrimaryReturnValue = RefReturnValues.InvalidConfigurationSpecification;
                ret.AdditionalInfo = "Invalid value specified for Source parameter " + pn_SourceType;
                return ret;
            }


            if (string.Equals(keywordsObtained.SourceType, nameof(SourceTypes.Database), StringComparison.OrdinalIgnoreCase))
            {
                s.SourceType = SourceTypes.Database;
            }


            if (string.Equals(keywordsObtained.SourceType, nameof(SourceTypes.API), StringComparison.OrdinalIgnoreCase))
            {
                s.SourceType = SourceTypes.API;
            }




            // Now pass the source object 's' on to where it's needed...

            ret.PrimaryReturnValue = RefReturnValues.Success;
            return ret;
        }










        static bool CreateSource_File(OSourceKeywordsObtained kwo, ref OSourceParameters_File spf)
        {
            bool ret = false;
            spf = new OSourceParameters_File();

            const string Str_Y = "Y";
            const string Str_YES = "YES";
            const string Str_TRUE = "TRUE";
            const char Char_Comma = ',';


            //[*********** Core validations and trims
            kwo.File_FullPathAndFilename = kwo.File_FullPathAndFilename.Trim();
            kwo.File_IsStructured = kwo.File_IsStructured.Trim();
            kwo.File_FileFormat = kwo.File_FileFormat.Trim();
            kwo.File_FileEncoding = kwo.File_FileEncoding.Trim();
            kwo.File_Text_FieldDelimiterCodes = kwo.File_Text_FieldDelimiterCodes.Trim();
            kwo.File_Text_FieldDelimiters = kwo.File_Text_FieldDelimiters.Trim();
            kwo.File_Text_FieldContainers = kwo.File_Text_FieldContainers.Trim();
            kwo.File_Text_LineTerminatorCodes = kwo.File_Text_LineTerminatorCodes.Trim();
            kwo.File_Text_LineTerminators = kwo.File_Text_LineTerminators.Trim();
            kwo.File_Text_NumHeaderRows = kwo.File_Text_NumHeaderRows.Trim();
            kwo.File_Text_HeaderRowWithFieldNames = kwo.File_Text_HeaderRowWithFieldNames.Trim();
            kwo.File_Text_FixedNumFields = kwo.File_Text_FixedNumFields.Trim();
            kwo.File_Text_NumFields = kwo.File_Text_NumFields.Trim();
            kwo.File_Text_TrimFieldWhiteSpace = kwo.File_Text_TrimFieldWhiteSpace.Trim();
            kwo.File_Text_SkipEmptyRows = kwo.File_Text_SkipEmptyRows.Trim();
            kwo.File_Text_SkipFinalEmptyRow = kwo.File_Text_SkipFinalEmptyRow.Trim();
            kwo.File_Text_SkipRowsWithDifferentNumFields = kwo.File_Text_SkipRowsWithDifferentNumFields.Trim();
            //***********]



            spf.FullPathAndFilename = kwo.File_FullPathAndFilename;






            //[******************************************* GenericDelimited and CSV
            if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.GenericDelimited), StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_FileFormat, nameof(FileFormats.CSV), StringComparison.OrdinalIgnoreCase))
            {
                // Set the file format parameter
                if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.GenericDelimited), StringComparison.OrdinalIgnoreCase))
                {
                    spf.FileFormat = FileFormats.GenericDelimited;
                }
                if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.CSV), StringComparison.OrdinalIgnoreCase))
                {
                    spf.FileFormat = FileFormats.CSV;
                }

                // Set the structuring parameters
                spf.IsTextFile = true;
                spf.IsStructured = true;
                spf.Text_IsTabulated = true;



                // ************ FileEncoding
                if (string.IsNullOrEmpty(kwo.File_FileEncoding))
                {
                    spf.Encoding = DefaultSourceParameter_File_Encoding();
                }
                else
                {
                    FileEncodings ts;
                    if (kwo.File_FileEncoding.StartsWith("code_page", StringComparison.OrdinalIgnoreCase))
                    {
                        //  Provide support for user-specified code pages here, e.g. "code_page XXXX"
                    }
                    else
                    {
                        if (Enum.TryParse(kwo.File_FileEncoding.ToUpperInvariant(), out ts))
                        {
                            spf.Encoding = ts;
                        }
                        else
                        {
                            spf.Encoding = FileEncodings.InvalidMatch;
                        }
                    }
                }


                // Create the specific parameter object for this file type
                OSPF_Text_Tabulated SPFtt = new OSPF_Text_Tabulated();



                //[*********************************************************************** Parse the parameters for the File > Text > Tabulated configuration

                // ************ FieldDelimiterCodes
                if (string.IsNullOrEmpty(kwo.File_Text_FieldDelimiterCodes))
                {
                    SPFtt.FieldDelimiterCodes = DefaultSourceParameter_File_FieldDelimiterCodes();
                }
                else
                {
                    // Create a fresh codes list
                    SPFtt.FieldDelimiterCodes = new List<uint>();
                    uint fdc;

                    // Break up the list into an array
                    List<string> tl = kwo.File_Text_FieldDelimiterCodes.Split(Char_Comma).ToList();
                    foreach (string k in tl)
                    {
                        // Try to convert each item in the list into integers
                        if (uint.TryParse(k.Trim(), out fdc))
                        {
                            SPFtt.FieldDelimiterCodes.Add(fdc);
                        }
                    }
                }


                // ************ FieldDelimiters
                if (string.IsNullOrEmpty(kwo.File_Text_FieldDelimiters))
                {
                    if (SPFtt.FieldDelimiterCodes == null)
                    {
                        if (spf.FileFormat == FileFormats.CSV)
                        {
                            SPFtt.FieldDelimiters = ",";
                        }
                        else
                        {
                            SPFtt.FieldDelimiters = DefaultSourceParameter_File_FieldDelimiters();
                        }
                    }
                    else
                    {
                        SPFtt.FieldDelimiters = String.Empty;
                    }
                }
                else
                {
                    SPFtt.FieldDelimiters = kwo.File_Text_FieldDelimiters;
                }


                // ************ FieldContainers
                if (string.IsNullOrEmpty(kwo.File_Text_FieldContainers))
                {
                    if (spf.FileFormat == FileFormats.CSV)
                    {
                        SPFtt.FieldContainers = "\"";
                    }
                    else
                    {
                        SPFtt.FieldContainers = DefaultSourceParameter_File_FieldContainers();
                    }
                }
                else
                {
                    SPFtt.FieldContainers = kwo.File_Text_FieldContainers;
                }



                // ************ LineTerminatorCodes
                if (string.IsNullOrEmpty(kwo.File_Text_LineTerminatorCodes))
                {
                    if (string.IsNullOrEmpty(kwo.File_Text_LineTerminators))
                    {
                        SPFtt.LineTerminatorCodes = DefaultSourceParameter_File_LineTerminatorCodes();
                    }
                }
                else
                {
                    // Create a fresh codes list
                    SPFtt.LineTerminatorCodes = new List<uint>();
                    uint ltc;

                    // Break up the list into an array
                    List<string> tl = kwo.File_Text_LineTerminatorCodes.Split(Char_Comma).ToList();
                    foreach (string k in tl)
                    {
                        // Try to convert each item in the list into integers
                        if (uint.TryParse(k.Trim(), out ltc))
                        {
                            SPFtt.LineTerminatorCodes.Add(ltc);
                        }
                    }
                }


                // ************ LineTerminators
                if (string.IsNullOrEmpty(kwo.File_Text_LineTerminators))
                {
                    SPFtt.LineTerminators = DefaultSourceParameter_File_LineTerminators();

                    // In case the line terminator codes are also null, then give them the default
                    if (SPFtt.LineTerminatorCodes == null)
                    {
                        SPFtt.LineTerminatorCodes = DefaultSourceParameter_File_LineTerminatorCodes();
                    }
                }
                else
                {
                    SPFtt.LineTerminators = kwo.File_Text_LineTerminators;
                }


                // ************ NumHeaderRows
                if (string.IsNullOrEmpty(kwo.File_Text_NumHeaderRows))
                {
                    SPFtt.NumHeaderRows = DefaultSourceParameter_File_NumHeaderRows();
                }
                else
                {
                    uint n;
                    if (uint.TryParse(kwo.File_Text_NumHeaderRows, out n))
                    {
                        SPFtt.NumHeaderRows = n;
                    }
                    else
                    {
                        SPFtt.NumHeaderRows = DefaultSourceParameter_File_NumHeaderRows();
                    }
                }



                // ************ HeaderRowWithFieldNames
                if (string.IsNullOrEmpty(kwo.File_Text_HeaderRowWithFieldNames))
                {
                    SPFtt.HeaderRowWithFieldNames = DefaultSourceParameter_File_HeaderRowWithFieldNames();
                }
                else
                {
                    uint n;
                    if (uint.TryParse(kwo.File_Text_HeaderRowWithFieldNames, out n))
                    {
                        SPFtt.HeaderRowWithFieldNames = n;
                    }
                    else
                    {
                        SPFtt.HeaderRowWithFieldNames = DefaultSourceParameter_File_HeaderRowWithFieldNames();
                    }
                }


                // Make a sensible adjustment if necessary
                if (SPFtt.NumHeaderRows > 0 & SPFtt.HeaderRowWithFieldNames == 0) SPFtt.HeaderRowWithFieldNames = 1;


                // ************ FixedNumFields
                if (string.IsNullOrEmpty(kwo.File_Text_FixedNumFields))
                {
                    SPFtt.FixedNumFields = DefaultSourceParameter_File_FixedNumFields();
                }
                else
                {
                    if (string.Equals(kwo.File_Text_FixedNumFields, Str_Y, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_FixedNumFields, Str_YES, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_FixedNumFields, Str_TRUE, StringComparison.OrdinalIgnoreCase))
                    {
                        SPFtt.FixedNumFields = true;
                    }
                    else
                    {
                        SPFtt.FixedNumFields = false;
                    }
                }




                // ************ NumFields
                if (string.IsNullOrEmpty(kwo.File_Text_NumFields))
                {
                    SPFtt.NumFields = DefaultSourceParameter_File_NumFields();
                }
                else
                {
                    uint n;
                    if (uint.TryParse(kwo.File_Text_NumFields, out n))
                    {
                        SPFtt.NumFields = n;
                    }
                    else
                    {
                        SPFtt.NumFields = DefaultSourceParameter_File_NumFields();
                    }
                }





                // ************ TrimFieldWhiteSpace
                if (string.IsNullOrEmpty(kwo.File_Text_TrimFieldWhiteSpace))
                {
                    SPFtt.TrimFieldWhiteSpace = DefaultSourceParameter_File_TrimFieldWhiteSpace();
                }
                else
                {
                    if (string.Equals(kwo.File_Text_TrimFieldWhiteSpace, Str_Y, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_TrimFieldWhiteSpace, Str_YES, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_TrimFieldWhiteSpace, Str_TRUE, StringComparison.OrdinalIgnoreCase))
                    {
                        SPFtt.TrimFieldWhiteSpace = true;
                    }
                    else
                    {
                        SPFtt.TrimFieldWhiteSpace = false;
                    }
                }




                // ************ SkipEmptyRows
                if (string.IsNullOrEmpty(kwo.File_Text_SkipEmptyRows))
                {
                    SPFtt.SkipEmptyRows = DefaultSourceParameter_File_SkipEmptyRows();
                }
                else
                {
                    if (string.Equals(kwo.File_Text_SkipEmptyRows, Str_Y, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_SkipEmptyRows, Str_YES, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_SkipEmptyRows, Str_TRUE, StringComparison.OrdinalIgnoreCase))
                    {
                        SPFtt.SkipEmptyRows = true;
                    }
                    else
                    {
                        SPFtt.SkipEmptyRows = false;
                    }
                }



                // ************ SkipFinalEmptyRow
                if (string.IsNullOrEmpty(kwo.File_Text_SkipFinalEmptyRow))
                {
                    SPFtt.SkipFinalEmptyRow = DefaultSourceParameter_File_SkipFinalEmptyRow();
                }
                else
                {
                    if (string.Equals(kwo.File_Text_SkipFinalEmptyRow, Str_Y, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_SkipFinalEmptyRow, Str_YES, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_SkipFinalEmptyRow, Str_TRUE, StringComparison.OrdinalIgnoreCase))
                    {
                        SPFtt.SkipFinalEmptyRow = true;
                    }
                    else
                    {
                        SPFtt.SkipFinalEmptyRow = false;
                    }
                }



                // ************ SkipRowsWithDifferentNumFields
                if (string.IsNullOrEmpty(kwo.File_Text_SkipRowsWithDifferentNumFields))
                {
                    SPFtt.SkipRowsWithDifferentNumFields = DefaultSourceParameter_File_SkipRowsWithDifferentNumFields();
                }
                else
                {
                    if (string.Equals(kwo.File_Text_SkipRowsWithDifferentNumFields, Str_Y, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_SkipRowsWithDifferentNumFields, Str_YES, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_Text_SkipRowsWithDifferentNumFields, Str_TRUE, StringComparison.OrdinalIgnoreCase))
                    {
                        SPFtt.SkipRowsWithDifferentNumFields = true;
                    }
                    else
                    {
                        SPFtt.SkipRowsWithDifferentNumFields = false;
                    }
                }

                //***********************************************************************] Parse the parameters for the File > Text > Tabulated configuration



                // Set the specific parameter object into the main parameters object
                spf.Params_Text_Tabulated = SPFtt;


                ret = true;
                return ret;
            }
            //*******************************************]






            //[******************************************* JSON and XML
            if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.JSON), StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.File_FileFormat, nameof(FileFormats.XML), StringComparison.OrdinalIgnoreCase))
            {
                // Set the file format parameter
                if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.JSON), StringComparison.OrdinalIgnoreCase))
                {
                    spf.FileFormat = FileFormats.JSON;
                }
                if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.XML), StringComparison.OrdinalIgnoreCase))
                {
                    spf.FileFormat = FileFormats.XML;
                }

                // Set the structuring parameters
                spf.IsTextFile = true;
                spf.IsStructured = true;
                spf.Text_IsTabulated = false;

                //kwo.File_Text_SchemaReferenceFilePath

                ret = true;
                return ret;
            }
            //*******************************************]






            //[******************************************* FreeformText
            if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.FreeformText), StringComparison.OrdinalIgnoreCase))
            {
                // Set the file format parameter
                spf.FileFormat = FileFormats.FreeformText;

                // Set the structuring parameters
                spf.IsTextFile = true;
                spf.IsStructured = false;
                spf.Text_IsTabulated = false;


                ret = true;
                return ret;
            }
            //*******************************************]






            //[******************************************* OrderedBinaryData
            if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.OrderedBinaryData), StringComparison.OrdinalIgnoreCase))
            {
                // Set the file format parameter
                spf.FileFormat = FileFormats.OrderedBinaryData;

                // Set the structuring parameters
                spf.IsTextFile = false;
                spf.IsStructured = true;
                spf.Binary_IsRepeating = true;

                //kwo.File_Binary_DataTypesSequence

                ret = true;
                return ret;
            }
            //*******************************************]





            //[******************************************* Excel
            if (string.Equals(kwo.File_FileFormat, nameof(FileFormats.Excel), StringComparison.OrdinalIgnoreCase))
            {
                // Set the file format parameter
                spf.FileFormat = FileFormats.Excel;

                // Set the structuring parameters
                spf.IsTextFile = false;
                spf.IsStructured = true;
                spf.Binary_IsRepeating = false;

                //kwo.File_Binary_Excel_SheetName
                //kwo.File_Binary_Excel_Password

                ret = true;
                return ret;
            }
            //*******************************************]




            return ret;


        }










        static FileEncodings DefaultSourceParameter_File_Encoding()
        {
            return FileEncodings.UTF8;
        }

        static List<uint>? DefaultSourceParameter_File_FieldDelimiterCodes()
        {
            return null;
        }

        static string DefaultSourceParameter_File_FieldDelimiters()
        {
            return string.Empty;
        }

        static string DefaultSourceParameter_File_FieldContainers()
        {
            return string.Empty;
        }

        static List<uint>? DefaultSourceParameter_File_LineTerminatorCodes()
        {
            return new List<uint>() { 13, 10 };
        }

        static string DefaultSourceParameter_File_LineTerminators()
        {
            return string.Empty;
        }

        static uint DefaultSourceParameter_File_NumHeaderRows()
        {
            return 0;
        }

        static uint DefaultSourceParameter_File_HeaderRowWithFieldNames()
        {
            return 0;
        }

        static bool DefaultSourceParameter_File_FixedNumFields()
        {
            return true;
        }

        static uint? DefaultSourceParameter_File_NumFields()
        {
            return null;
        }

        static bool DefaultSourceParameter_File_TrimFieldWhiteSpace()
        {
            return false;
        }

        static bool DefaultSourceParameter_File_SkipEmptyRows()
        {
            return false;
        }

        static bool DefaultSourceParameter_File_SkipFinalEmptyRow()
        {
            return false;
        }

        static bool DefaultSourceParameter_File_SkipRowsWithDifferentNumFields()
        {
            return false;
        }















        static bool ParseAnalysisWindowDefinition(List<string> LS, ref OAnalysisWindow AW)
        {
            bool ret = false;
            if (LS == null) return ret;
            if (LS.Count == 0) return ret;


            //[************ Parameter names - These are the keywords users can use to define a data source
            const string pn_Fields = "fields";
            const string pn_Rows = "rows";
            const string pn_FieldConditions = "field_conditions";
            const string pn_ApplyFieldsConditionsFirst = "apply_fields_conditions_first";
            //************]


            // Output variables to obtain
            OAnalysisWindowKeywordsObtained keywordsObtained = new OAnalysisWindowKeywordsObtained();

            // Some constants and variables
            const string Str_Equals = "=";
            const int nb = 2;
            string[] r;
            string t;
            string pn;
            string pvalue;
            int iofequals;


            // Loop over the rows
            for (int i = 0; i < LS.Count; i++)
            {
                // Set the variables
                t = LS[i].Trim();
                pn = string.Empty;
                pvalue = string.Empty;

                // Only consider rows with content
                if (string.IsNullOrEmpty(t) == false)
                {
                    // Split by the equals character
                    r = t.Split(Str_Equals);

                    // Check the number of substrings found - only analyse those rows with at least the required number of strings
                    if (r.Count() >= nb)
                    {
                        // Get the parameter name and value strings for the simple case of only one equals character in the row
                        if (r.Count() == nb)
                        {
                            pn = r[0].Trim();
                            pvalue = r[1];
                        }
                        else
                        {
                            // Where there is more than one equals character, get the parameter name and get the appropriate parameter value string by only looking past the first equals character
                            if (r.Count() > nb)
                            {
                                pn = r[0].Trim();

                                iofequals = t.IndexOf(Str_Equals);
                                if (iofequals > 0)
                                {
                                    pvalue = t.Substring(iofequals + 1);
                                }


                            }
                        }


                        switch (pn.ToLowerInvariant())
                        {
                            case pn_Fields:
                                keywordsObtained.Fields = pvalue.Trim();
                                break;
                            case pn_Rows:
                                keywordsObtained.Rows = pvalue.Trim();
                                break;
                            case pn_FieldConditions:
                                keywordsObtained.FieldConditions.Add(pvalue.TrimStart());
                                break;
                            case pn_ApplyFieldsConditionsFirst:
                                keywordsObtained.ApplyFieldsConditionsFirst = pvalue.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }



            AW = new OAnalysisWindow();


            //[*********** Core validations and trims
            // No validations required for the Analysis Window
            // Trims will be done in the function that creates the Analysis Windows object
            //***********]


            bool GetParams = false;

            GetParams = CreateAnalysisWindow(keywordsObtained, ref AW);
            if (GetParams)
            {
            }
            else
            {
                return ret;
            }





            // Now pass the object on to where it's needed...

            ret = true;
            return ret;
        }















        static bool ParseCommandDefinition(List<string> LS, ref List<OStrKeyValuePair> CConfig)
        {
            // This function takes a list of string and parses them into string key-value pairs for generic command usage
            // The input list of string ("LS") should just be the command parameters, e.g. "setting_1 = adsf". It should not include
            // any framing content such as ":COMMAND_NAME" and opening and closing braces. It may include empty rows.

            bool ret = false;
            if (LS == null) return ret;
            if (LS.Count == 0) return ret;


            // Some constants and variables
            const string Str_Equals = "=";
            const int nb = 2;
            string[] r;
            string t;
            string pn;
            string pvalue;
            int iofequals;
            OStrKeyValuePair kvp;

            CConfig = new List<OStrKeyValuePair>();


            // Loop over the rows
            for (int i = 0; i < LS.Count; i++)
            {
                // Set the variables
                t = LS[i].Trim();
                pn = string.Empty;
                pvalue = string.Empty;

                // Only consider rows with content
                if (string.IsNullOrEmpty(t) == false)
                {
                    // Split by the equals character
                    r = t.Split(Str_Equals);

                    // Check the number of substrings found - only analyse those rows with at least the required number of strings
                    if (r.Count() >= nb)
                    {
                        // Get the parameter name and value strings for the simple case of only one equals character in the row
                        if (r.Count() == nb)
                        {
                            pn = r[0].Trim();
                            pvalue = r[1].TrimStart();
                        }
                        else
                        {
                            // Where there is more than one equals character, get the parameter name and get the appropriate parameter value string by only looking past the first equals character
                            if (r.Count() > nb)
                            {
                                pn = r[0].Trim();

                                iofequals = t.IndexOf(Str_Equals);
                                if (iofequals > 0)
                                {
                                    pvalue = t.Substring(iofequals + 1);
                                }


                            }
                        }

                        // Only keep keys which have some content
                        if (string.IsNullOrEmpty(pn) == false)
                        {
                            // Create the string key value pair
                            kvp = new OStrKeyValuePair();
                            kvp.KeyName = pn;
                            kvp.ValueStr = pvalue;

                            // Add the string key value pair to the list
                            CConfig.Add(kvp);
                        }
                    }
                }
            }



            ret = true;
            return ret;
        }




























        static bool CreateAnalysisWindow(OAnalysisWindowKeywordsObtained kwo, ref OAnalysisWindow AW)
        {
            bool ret = false;

            const string Str_N = "N";
            const string Str_NO = "NO";
            const string Str_FALSE = "FALSE";
            const char Char_Comma = ',';
            const string Str_Comma = ",";
            const string Str_To = "to";
            const string Str_DoubleQuote = "\"";
            const char Char_Space = ' ';
            const string Str_Space = " ";
            const string Str_First = "first";
            const string Str_Random = "random";
            const string Str_Static = "static";
            const string Str_Percent = "%";
            bool cont;
            List<string> tl;
            int n;
            long ln;
            OFieldIDPair fip;


            //[*********** Core validations and trims
            kwo.Fields = kwo.Fields.Trim();
            kwo.Rows = kwo.Rows.Trim();
            kwo.ApplyFieldsConditionsFirst = kwo.ApplyFieldsConditionsFirst.Trim();
            //***********]




            // Create the default Analysis Window object
            AW = CreateDefaultAnalysisWindow();






            // ************ Fields
            if (string.IsNullOrEmpty(kwo.Fields) == false)
            {
                if (kwo.Fields.Contains(Str_To, StringComparison.OrdinalIgnoreCase))
                {
                    // For a range
                    string t = kwo.Fields;
                    n = 0;
                    int n2 = 0;
                    // Remove any other commas first (just in case)
                    t = t.Replace(Str_Comma, string.Empty);

                    // Replace the "to" with a comma, to ease the splitting
                    t = t.Replace(Str_To, Str_Comma);

                    tl = t.Split(Char_Comma).ToList();

                    if (tl.Count >= 2)
                    {
                        cont = false;

                        // Try to convert the string into an integer
                        if (int.TryParse(tl[0].Trim(), out n))
                        {
                            if (n > 0) cont = true;
                        }
                        if (cont)
                        {
                            cont = false;

                            // Try to convert the string into an integer
                            if (int.TryParse(tl[1].Trim(), out n2))
                            {
                                if (n2 > 0) cont = true;
                            }
                        }

                        if (cont)
                        {
                            for (int i = n; i <= n2; i++)
                            {
                                fip = new OFieldIDPair();
                                fip.ID = i;
                                AW.UseFields.Add(fip);
                            }
                        }
                    }
                }
                else
                {
                    // For a comma-separated list
                    tl = kwo.Fields.Split(Char_Comma).ToList();

                    // Loop over the items in the list
                    foreach (string k in tl)
                    {
                        fip = new OFieldIDPair();  // Note that new FieldIDPairs have default empty values
                        cont = true;

                        // Try to convert each item in the list into integers
                        if (int.TryParse(k.Trim(), out n))
                        {
                            if (n > 0)
                            {
                                fip.ID = n;
                            }
                            else
                            {
                                cont = false;
                            }
                        }
                        else
                        {
                            fip.FieldName = k.Trim().Replace(Str_DoubleQuote, string.Empty);
                        }

                        if (cont) AW.UseFields.Add(fip);
                    }
                }
            }









            // ************ Rows
            if (string.IsNullOrEmpty(kwo.Rows) == false)
            {
                // If it's a comma-separated list of numbers
                if (kwo.Rows.Contains(Str_Comma))
                {
                    AW.RowsSamplingType = SamplingType.NumbersList;
                    AW.RowsSamplingTypeQualifier = SamplingTypeQualifier.NoQualifier;

                    // For a comma-separated list
                    tl = kwo.Rows.Split(Char_Comma).ToList();

                    // Loop over the items in the list
                    foreach (string k in tl)
                    {
                        cont = false;

                        // Try to convert each item in the list into integers
                        if (long.TryParse(k.Trim(), out ln))
                        {
                            if (ln > 0) cont = true;
                        }

                        if (cont) AW.RowsList.Add(ln);
                    }


                }
                else
                {
                    // For a range
                    if (kwo.Rows.Contains(Str_To, StringComparison.OrdinalIgnoreCase))
                    {
                        AW.RowsSamplingType = SamplingType.NumbersRange;

                        string t = kwo.Rows;
                        // Remove any other commas first (just in case)
                        t = t.Replace(Str_Comma, string.Empty);

                        // Replace the "to" with a comma, to ease the splitting
                        t = t.Replace(Str_To, Str_Comma);

                        tl = t.Split(Char_Comma).ToList();

                        if (tl.Count >= 2)
                        {
                            // Loop over the first two entries
                            for (int i = 0; i < 2; i++)
                            {
                                cont = false;

                                // Try to convert the string into an integer
                                if (long.TryParse(tl[i].Trim(), out ln))
                                {
                                    if (ln > 0) cont = true;
                                }

                                if (cont) AW.RowsList.Add(ln);
                            }
                        }
                    }
                    else
                    {
                        tl = kwo.Rows.Split(Char_Space).ToList();

                        // If there's just one item in the list then it should just be a single row number that's requested
                        if (tl.Count == 1)
                        {
                            AW.RowsSamplingType = SamplingType.NumbersList;
                            AW.RowsSamplingTypeQualifier = SamplingTypeQualifier.NoQualifier;

                            cont = false;

                            // Try to convert the string into an integer
                            if (long.TryParse(tl[0].Trim(), out ln))
                            {
                                if (ln > 0) cont = true;
                            }

                            if (cont) AW.RowsList.Add(ln);
                        }
                        else
                        {
                            switch (tl[0].Trim().ToLowerInvariant())
                            {
                                case Str_First:
                                    AW.RowsSamplingType = SamplingType.NumbersList;
                                    AW.RowsSamplingTypeQualifier = SamplingTypeQualifier.First;
                                    cont = false;

                                    // Try to convert the string into an integer
                                    if (long.TryParse(tl[1].Trim(), out ln))
                                    {
                                        if (ln > 0) cont = true;
                                    }

                                    if (cont) AW.RowsList.Add(ln);
                                    break;

                                case Str_Random:
                                    // If there's just one item after the "random" first word, then it will random dynamic
                                    if (tl.Count == 2)
                                    {
                                        AW.RowsSamplingType = SamplingType.Percentage;
                                        AW.RowsSamplingTypeQualifier = SamplingTypeQualifier.RandomDynamic;
                                        cont = false;
                                        double d;
                                        string tt = tl[1].Trim();
                                        tt = tt.Replace(Str_Space, string.Empty);
                                        tt = tt.Replace(Str_Percent, string.Empty);

                                        // Try to convert the string into an double
                                        if (double.TryParse(tt.Trim(), out d))
                                        {
                                            // Scale down the percentage value
                                            d = d / 100.0d;

                                            if (d >= 0.0d & d <= 1.0d)
                                            {
                                                cont = true;
                                            }
                                        }

                                        if (cont) AW.RowsPercentage = d;
                                    }
                                    else
                                    {
                                        // If there are two items after the "random" first word, then it should be random static
                                        if (tl.Count == 3)
                                        {
                                            string tt = tl[1].Trim();

                                            // Check that it's static
                                            if (string.Equals(tt, Str_Static, StringComparison.OrdinalIgnoreCase))
                                            {
                                                AW.RowsSamplingType = SamplingType.Percentage;
                                                AW.RowsSamplingTypeQualifier = SamplingTypeQualifier.RandomStatic;
                                                cont = false;
                                                double d;
                                                tt = tl[2].Trim();
                                                tt = tt.Replace(Str_Space, string.Empty);
                                                tt = tt.Replace(Str_Percent, string.Empty);

                                                // Try to convert the string into a double
                                                if (double.TryParse(tt.Trim(), out d))
                                                {
                                                    // Scale down the percentage value
                                                    d = d / 100.0d;

                                                    if (d >= 0.0d & d <= 1.0d)
                                                    {
                                                        cont = true;
                                                    }
                                                }

                                                if (cont) AW.RowsPercentage = d;

                                            }

                                        }
                                    }
                                    break;
                            }
                        }

                    }
                }
            }











            // ************ FieldConditions
            if (kwo.FieldConditions.Count > 0)
            {
                foreach (string kfc in kwo.FieldConditions)
                {

                    if (string.IsNullOrEmpty(kfc) == false)
                    {
                        string t = kfc;
                        OFieldAnalysisContext FAC = new OFieldAnalysisContext();
                        fip = new OFieldIDPair();
                        string tRemain = string.Empty;
                        double d;
                        bool BadCondition = false;


                        // If the first item is a field name...
                        if (string.Equals(t.Substring(0, 1), Str_DoubleQuote, StringComparison.InvariantCulture))
                        {
                            int u;

                            u = t.Substring(1).IndexOf(Str_DoubleQuote);
                            if (u > -1)
                            {
                                fip.FieldName = t.Substring(1, u);

                                if (t.Length - 1 > u) tRemain = t.Substring(u + 2).Trim();
                            }
                        }
                        else
                        {
                            // The first item should be number in this case
                            tl = t.Split(Char_Space).ToList();

                            cont = false;

                            // Try to convert the string into an integer
                            if (int.TryParse(tl[0].Trim(), out n))
                            {
                                if (n > 0) cont = true;
                            }

                            if (cont)
                            {
                                fip.ID = n;

                                tRemain = t.Substring(tl[0].Length).Trim();
                            }
                        }



                        // If we've been able to parse the field name or number correctly, then try to parse the rest
                        if (string.IsNullOrEmpty(tRemain) == false)
                        {
                            FAC.FieldIDPair = fip;

                            tl = tRemain.Split(Char_Space).ToList();


                            if (tl.Count > 1)
                            {
                                // Check for a "between" condition first
                                if (tl.Count == 4)
                                {
                                    if (string.Equals(tl[0], "between", StringComparison.OrdinalIgnoreCase) & string.Equals(tl[2], "and", StringComparison.OrdinalIgnoreCase))
                                    {
                                        FAC.Condition = SamplingConditionType.AbsoluteBetween;
                                        FAC.IsNumber = true;

                                        cont = false;
                                        double d2;

                                        // Try to convert the string into a double
                                        if (double.TryParse(tl[1].Trim(), out d))
                                        {
                                            cont = true;
                                        }

                                        if (cont)
                                        {
                                            // Try to convert the string into a double
                                            if (double.TryParse(tl[3].Trim(), out d2))
                                            {
                                                FAC.NumericConditionValue1 = d;
                                                FAC.NumericConditionValue2 = d2;
                                            }

                                        }

                                    }
                                }
                                else
                                {



                                    switch (tl[0].ToLowerInvariant())
                                    {
                                        case "equals":
                                            FAC.Condition = SamplingConditionType.AbsoluteEquals;
                                            break;
                                        case "does_not_equal":
                                            FAC.Condition = SamplingConditionType.AbsoluteNotEquals;
                                            break;
                                        case "greater_than":
                                            FAC.Condition = SamplingConditionType.AbsoluteGreaterThan;
                                            break;
                                        case "greater_than_or_equal":
                                            FAC.Condition = SamplingConditionType.AbsoluteGreaterThanOrEqual;
                                            break;
                                        case "less_than":
                                            FAC.Condition = SamplingConditionType.AbsoluteLessThan;
                                            break;
                                        case "less_than_or_equal":
                                            FAC.Condition = SamplingConditionType.AbsoluteLessThanOrEqual;
                                            break;
                                        case "contains":
                                            FAC.Condition = SamplingConditionType.AbsoluteContains;
                                            break;
                                        case "does_not_contain":
                                            FAC.Condition = SamplingConditionType.AbsoluteNotContains;
                                            break;
                                        case "starts_with":
                                            FAC.Condition = SamplingConditionType.AbsoluteStartsWith;
                                            break;
                                        case "does_not_starts_with":
                                            FAC.Condition = SamplingConditionType.AbsoluteNotStartsWith;
                                            break;
                                        case "ends_with":
                                            FAC.Condition = SamplingConditionType.AbsoluteEndsWith;
                                            break;
                                        case "does_not_end_with":
                                            FAC.Condition = SamplingConditionType.AbsoluteNotEndsWith;
                                            break;
                                        default:
                                            BadCondition = true;
                                            break;
                                    }


                                    // If there's only one item after the condition item, then it's either a number or a text string
                                    if (tl.Count == 2)
                                    {
                                        // Try to convert the string into a double
                                        if (double.TryParse(tl[1].Trim(), out d))
                                        {
                                            if (FAC.Condition == SamplingConditionType.AbsoluteContains || FAC.Condition == SamplingConditionType.AbsoluteNotContains || FAC.Condition == SamplingConditionType.AbsoluteStartsWith || FAC.Condition == SamplingConditionType.AbsoluteNotStartsWith || FAC.Condition == SamplingConditionType.AbsoluteEndsWith || FAC.Condition == SamplingConditionType.AbsoluteNotEndsWith)
                                            {
                                                FAC.IsNumber = false;
                                                FAC.StrConditionValue = tl[1].Trim();  // Trim, as we're not currently handling scenarios where the user can check for leading and trailing spaces around a numeric value
                                            }
                                            else
                                            {
                                                FAC.IsNumber = true;
                                                FAC.NumericConditionValue1 = d;
                                            }
                                        }
                                        else
                                        {
                                            // The string didn't convert to a double, so treat as a text string
                                            FAC.IsNumber = false;
                                            FAC.StrConditionValue = tl[1];  // Don't trim
                                        }

                                    }
                                    else
                                    {
                                        // If there's more than one item after the condition item, then treat as a text string
                                        FAC.IsNumber = false;

                                        // Get the remaining text as originally provided by the user (i.e. don't try to concatenate the split string)
                                        int u = tRemain.IndexOf(Str_Space);
                                        if (u > -1)
                                        {
                                            if (u + 1 <= tRemain.Length - 1) FAC.StrConditionValue = tRemain.Substring(u + 1);
                                        }
                                    }

                                }

                            }
                            else
                            {
                                switch (tl[0].ToLowerInvariant())
                                {
                                    case "is_empty":
                                        FAC.Condition = SamplingConditionType.AbsoluteIsEmpty;
                                        break;
                                    case "is_not_empty":
                                        FAC.Condition = SamplingConditionType.AbsoluteIsNotEmpty;
                                        break;
                                    default:
                                        BadCondition = true;
                                        break;
                                }

                                FAC.StrConditionValue = String.Empty;
                                FAC.IsNumber = false;
                            }

                            // Add the object to the list
                            if (BadCondition == false) AW.FieldSamplingConditions.Add(FAC);

                        }
                    }
                }
            }





            // ************ ApplyFieldsConditionsFirst
            if (string.IsNullOrEmpty(kwo.ApplyFieldsConditionsFirst) == false)
            {
                if (string.Equals(kwo.ApplyFieldsConditionsFirst, Str_N, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.ApplyFieldsConditionsFirst, Str_NO, StringComparison.OrdinalIgnoreCase) || string.Equals(kwo.ApplyFieldsConditionsFirst, Str_FALSE, StringComparison.OrdinalIgnoreCase))
                {
                    AW.ApplyRowSamplingAfterFieldSampling = false;
                }
                else
                {
                    AW.ApplyRowSamplingAfterFieldSampling = true;
                }
            }




            ret = true;
            return ret;

        }
















        // Populate an OFR object with specific values
        static void CreateOFRErrorResponse(ref ofr OFRToFill, RefReturnValues PrimaryReturnValue, string ProcessingPoint, string AdditionalInfo, List<RefReturnValues>? AdditionalReturnValues = null, Exception? e = null)
        {
            OFRToFill.DT = DateTime.Now;
            OFRToFill.PrimaryReturnValue = PrimaryReturnValue;
            OFRToFill.AdditionalReturnValues = AdditionalReturnValues;
            OFRToFill.ProcessingPoint = ProcessingPoint;
            OFRToFill.AdditionalInfo = AdditionalInfo;
            if (e != null)
            {
                OFRToFill.ErrorMessage = e.Message;
                if (e.InnerException != null)
                {
                    OFRToFill.ErrorDetails = e.InnerException.ToString();
                }
                else
                {
                    OFRToFill.ErrorDetails = string.Empty;
                }
            }
            else
            {
                OFRToFill.ErrorMessage = string.Empty;
                OFRToFill.ErrorDetails = string.Empty;
            }
        }




    }











    public class OSource
    {
        public string Name;
        public SourceTypes SourceType;
        public object SourceParameters;
    }


    public class OSourceParameters_File
    {
        public string FullPathAndFilename;
        public bool IsTextFile;
        public bool IsStructured;
        public bool? Text_IsTabulated;
        public bool? Binary_IsRepeating;
        public FileFormats FileFormat;
        public FileEncodings Encoding;
        public OSPF_Text_Tabulated Params_Text_Tabulated;
        public OSPF_Text_NonTabulated Params_Text_NonTabulated;
        public OSPF_Binary_Repeating Params_Binary_Repeating;
        public OSPF_Binary_NonRepeating_Excel Params_Binary_Excel;
    }


    public class OSPF_Text_Tabulated
    {
        public List<uint>? FieldDelimiterCodes;  // The integer character codes used as field delimiters. * This takes precedence over the field delimiters string. *
        public string FieldDelimiters; // Converted from the Field Delimiter Codes if they're populated, otherwise this string is used.
        public string FieldContainers; // e.g. double quote marks containing each field content
        public List<uint>? LineTerminatorCodes;  // The integer character codes used as line terminators. * This takes precedence over the line terminators string. *
        public string LineTerminators; // Converted from the Line Terminator Codes if they're populated
        public uint NumHeaderRows;
        public uint HeaderRowWithFieldNames;
        public bool FixedNumFields;
        public uint? NumFields;
        public bool? SkipRowsWithDifferentNumFields;  // Where there it is specified that there are a fixed number of fields, this indicates whether or not to skip rows that have a differing number of fields than that specified.
        public bool TrimFieldWhiteSpace;
        public bool SkipEmptyRows;
        public bool SkipFinalEmptyRow;
    }


    public class OSPF_Text_NonTabulated
    {
        public string SchemaReferenceFilePath;
    }


    public class OSPF_Binary_Repeating
    {
        public List<object> DataTypesSequence;
    }

    public class OSPF_Binary_NonRepeating_Excel
    {
        public string SheetName;
        public string Password;
    }


    public class OSourceKeywordsObtained
    {
        public string SourceName = string.Empty;
        public string SourceType = string.Empty;

        //[********* For a file
        public string File_FullPathAndFilename = string.Empty;
        public string File_IsStructured = string.Empty;
        public string File_FileFormat = string.Empty;
        public string File_FileEncoding = string.Empty;
        public string File_Text_FieldDelimiterCodes = string.Empty;
        public string File_Text_FieldDelimiters = string.Empty;
        public string File_Text_FieldContainers = string.Empty;
        public string File_Text_LineTerminatorCodes = string.Empty;
        public string File_Text_LineTerminators = string.Empty;
        public string File_Text_NumHeaderRows = string.Empty;
        public string File_Text_HeaderRowWithFieldNames = string.Empty;
        public string File_Text_FixedNumFields = string.Empty;
        public string File_Text_NumFields = string.Empty;
        public string File_Text_TrimFieldWhiteSpace = string.Empty;
        public string File_Text_SkipEmptyRows = string.Empty;
        public string File_Text_SkipFinalEmptyRow = string.Empty;
        public string File_Text_SkipRowsWithDifferentNumFields = string.Empty;
        public string File_Text_SchemaReferenceFilePath = string.Empty;
        public string File_Binary_DataTypesSequence = string.Empty;
        public string File_Binary_Excel_SheetName = string.Empty;
        public string File_Binary_Excel_Password = string.Empty;
        //*********]

        //[********* For a database
        public string DB_DBType = string.Empty;
        public string DB_ConnectionStr = string.Empty;
        public string DB_SchemaName = string.Empty;
        public string DB_DBName = string.Empty;
        public string DB_TableName = string.Empty;
        //*********]


        //[********* For an API
        public string API_HostName = string.Empty;
        public string API_Port = string.Empty;
        public string API_EndPointPath = string.Empty;
        public string API_HeaderStr = string.Empty;
        public string API_AuthenticationType = string.Empty;
        public string API_UserName = string.Empty;
        public string API_Password = string.Empty;
        //*********]
    }




    public class OAnalysisWindowKeywordsObtained
    {
        public string Fields = string.Empty;
        public string Rows = string.Empty;
        public List<string> FieldConditions = new List<string>();
        public string ApplyFieldsConditionsFirst = string.Empty;
    }





    //public class OActivityState
    //{
    //    public string ActivityName;
    //    public object ConfigObj;
    //    public bool HasInitialisationCompleted;
    //    public bool Started;
    //    public DateTime StartedDT;
    //    public bool DataExtractComplete;
    //    public DateTime DataExtractCompleteDT;
    //    public bool Completed;
    //    public DateTime CompletedDT;
    //    public List<string> SourceOriginalFieldHeaders;
    //    public object OutputObj;
    //    public List<string> OutputMessagesToUser;
    //    public ofr ofr;  // Object Return Function - the outcome of the process
    //    public bool EarlyTermination = false;  // Set and used by the program: when row sampling of explicit rows is used, and the program determines that all of the requested rows have been obtained. This allows the file reading process to terminate early, rather than continuing reading the file for no reason.
    //    public int ProcessedRowsCount = 0;
    //}







    public class OAnalysisWindow
    {
        public List<OFieldIDPair> UseFields;
        public List<OFieldAnalysisContext> FieldSamplingConditions;
        public SamplingType RowsSamplingType;
        public SamplingTypeQualifier RowsSamplingTypeQualifier;
        public List<long> RowsList;  // Requires RowsSamplingType = NumbersList or NumbersRange
        public double RowsPercentage;  // Requires RowsSamplingType = Percentage
        public bool ApplyRowSamplingAfterFieldSampling;
    }


    //public class OIncrementalDataObject
    //{
    //    public object IDO;
    //    public List<OFieldIDPair> LFieldsNameID;  // The ID in here is the original ID of the field
    //    public long ProcessedRowID;
    //    public bool SourceIsText;
    //}


    //public class OFieldIDPair
    //{
    //    public string FieldName = String.Empty;
    //    public int ID = 0;
    //}

    public class OFieldAnalysisContext
    {
        public OFieldIDPair FieldIDPair;
        public SamplingConditionType Condition;
        public bool IsNumber;
        public double NumericConditionValue1;
        public double NumericConditionValue2;
        public string StrConditionValue;
    }



    public class OTextProcessParams
    {
        public char[] UseLineTerminators;
        public int n_ult;
        public char[] UseDelimiters;
        public int n_delim;
        public string FieldContainers;
        public int n_fc;
        public bool IsQuoteFieldContainers;
        public bool SupportSlashEscapedQuotes;
    }


    //// Class Object Function Return
    //public class ofr
    //{
    //    public RefReturnValues PrimaryReturnValue;
    //    public List<RefReturnValues>? AdditionalReturnValues;
    //    public string ErrorMessage;
    //    public string ErrorDetails;
    //    public string ProcessingPoint;
    //    public string AdditionalInfo;
    //    public string FunctionName;
    //    public DateTime DT;
    //    public ofr ChildReturnObject;
    //}


    //public enum RefReturnValues : int
    //{
    //    Indeterminate = 0,
    //    Success = 1,
    //    ConditionalSuccess = 2,
    //    InvalidInputParameters = -1,
    //    ErrorWithinFunction = -2,
    //    ErrorInCodeOutsideOfThisFunction = -3,
    //    ErrorInExternalFile = -4,
    //    ErrorInExternalDB = -5,
    //    ErrorInExternalAPI = -6,
    //    ErrorInExternalObject = -7,
    //    OtherError = -8,
    //    ExpectedResponseNotFound = -9,
    //    InvalidCredentials = -10,
    //    FileNotFound = -11,
    //    InvalidConfigurationSpecification = -12  // E.g. if a connection string has an error in it
    //}


    public enum SourceTypes
    {
        File = 0,
        Database = 1,
        API = 2
    }

    public enum FileFormats
    {
        InvalidMatch = -1,
        GenericDelimited = 0,
        CSV = 1,  // This format uses the strict CSV definition where field values may have containing characters and that they may contain line breaks
        JSON = 3,
        XML = 4,
        FreeformText = 5,
        OrderedBinaryData = 6,
        Excel = 7
    }

    public enum FileEncodings
    {
        InvalidMatch = -1,
        ASCII = 0,
        UTF7 = 1,
        UTF8 = 2,
        UTF16 = 3,
        UTF16BE = 4,
        UTF32 = 5,
        UTF32BE = 6,
        ANSI = 7,
        SpecificCodePage = 8
    }


    public enum SamplingType
    {
        All = 0,  // Valid SamplingTypeQualifier: NoQualifier (all rows - no sampling)
        NumbersList = 1,  // Valid SamplingTypeQualifier: NoQualifier (a static list of desired rows), First (the first X rows), Last (the last X rows)
        NumbersRange = 2,  // Valid SamplingTypeQualifier: NoQualifier (a desired set of rows, given by explicit upper and lower row numbers)
        Percentage = 3  // Valid SamplingTypeQualifier: First (first X%), Last (last X%), RandomStatic (random across all rows), RandomDynamic (random across all rows)
    }

    public enum SamplingTypeQualifier
    {
        NoQualifier = -1,
        First = 0,
        Last = 1,
        RandomStatic = 2,
        RandomDynamic = 3
    }

    public enum SamplingConditionType
    {
        NoConditions = -1,
        AbsoluteEquals = 0,
        AbsoluteNotEquals = 1,
        AbsoluteGreaterThan = 2,
        AbsoluteGreaterThanOrEqual = 3,
        AbsoluteLessThan = 4,
        AbsoluteLessThanOrEqual = 5,
        AbsoluteBetween = 6,
        AbsoluteContains = 7,
        AbsoluteNotContains = 8,
        AbsoluteStartsWith = 9,
        AbsoluteNotStartsWith = 10,
        AbsoluteEndsWith = 11,
        AbsoluteNotEndsWith = 12,
        AbsoluteIsEmpty = 13,
        AbsoluteIsNotEmpty = 14
    }



    //public class OCommandConfig_Generic
    //{
    //    public List<OStrKeyValuePair> LKeyValuePairs;
    //}


    //public class OStrKeyValuePair
    //{
    //    public string KeyName;
    //    public string ValueStr;
    //}


}