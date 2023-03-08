using System.Text;
using Data_Scratchpad;
using ds_common;


namespace Data_Scratchpad
{
    public static class Commands
    {









        public static ofr RunCommand_ANALYSE_FILE_FORMAT(ref OActivityState AState, OIncrementalDataObject? IDO = null)
        {
            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "RunCommand_ANALYSE_FILE_FORMAT";
            // Set the Processing Point description
            ret.ProcessingPoint = "Initialising function";
            ret.AdditionalInfo = string.Empty;

            if (AState == null) return ret;

            const string CommandName = "analyse_file_format";

            // Check that the right command is being called
            if (string.Equals(AState.ActivityName, CommandName, StringComparison.OrdinalIgnoreCase) == false)
            {
                ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                return ret;
            }


            // Record the first engagement with the command on first pass
            if (AState.Started == false)
            {
                AState.Started = true;
                AState.StartedDT = DateTime.Now;

                ofr aofr = new ofr();

                aofr.DT = DateTime.Now;
                aofr.PrimaryReturnValue = RefReturnValues.Indeterminate;
                aofr.FunctionName = AState.ActivityName.ToString();
                aofr.ProcessingPoint = "Command started";

                AState.ofr = aofr;

            }



            //[*************** Perform command initialisation activities if required
            if (AState.HasInitialisationCompleted == false)
            {
                AState.ofr.ProcessingPoint = "Initialisation started";


                //[******** Specific initialisations
                AState.OutputObj.Information = new List<string>();
                AState.OutputObj.PrimaryOutput = new List<object>();

                // Set the visualisation method
                AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Text);
                //********]


                AState.HasInitialisationCompleted = true;
                AState.ofr.ProcessingPoint = "Initialisation completed";

                if (IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.Success;
                    return ret;
                }
            }
            //***************]



            // Determine if this the incremental data processing has completed, by checking if no more data have been provided
            if (AState.HasInitialisationCompleted == true & IDO == null)
            {
                if (AState.DataExtractComplete == false)
                {
                    AState.DataExtractComplete = true;
                    AState.DataExtractCompleteDT = DateTime.Now;
                }
            }



            //[**************** If there are still more data points to accummulate then do the accummulation stuff here
            if (AState.DataExtractComplete == false)
            {
                if (IDO == null || IDO.IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                    return ret;
                }




                //[***************************** Do the relevant processing
                if (IDO.SourceIsText)
                {
                    // Process text-based incremental data objects
                    List<object> LFields = (List<object>)IDO.IDO;
                    string t;


                    t = "Likely file encoding: " + (string)LFields[0];
                    t += Environment.NewLine + "Likely line terminators: " + (string)LFields[1];
                    t += Environment.NewLine + "Likely field delimiters: " + (string)LFields[2];
                    t += Environment.NewLine + "Likely field containers: " + (string)LFields[3];

                    AState.OutputObj.PrimaryOutput.Add(t);
                }
                else
                {
                    // Process binary-based incremental data objects
                }
                //*****************************]




                ret.PrimaryReturnValue = RefReturnValues.Success;
                return ret;
            }
            //****************]






            //[********************** Do any final (end of incremental data provision) processing here
            //if (AState.DataExtractComplete == true)
            //{
            //}
            //**********************]


            // Do the necessary things here to display the outputs



            AState.Completed = true;
            AState.CompletedDT = DateTime.Now;
            AState.ofr.PrimaryReturnValue = RefReturnValues.Success;


            ret.PrimaryReturnValue = RefReturnValues.Success;
            return ret;
        }




















        public static ofr RunCommand_BYTE_INSPECT(ref OActivityState AState, OIncrementalDataObject? IDO = null)
        {
            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "RunCommand_BYTE_INSPECT";
            // Set the Processing Point description
            ret.ProcessingPoint = "Initialising function";
            ret.AdditionalInfo = string.Empty;

            if (AState == null) return ret;

            const string CommandName = "byte_inspect";

            // Check that the right command is being called
            if (string.Equals(AState.ActivityName, CommandName, StringComparison.OrdinalIgnoreCase) == false)
            {
                ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                return ret;
            }


            // Record the first engagement with the command on first pass
            if (AState.Started == false)
            {
                AState.Started = true;
                AState.StartedDT = DateTime.Now;

                ofr aofr = new ofr();

                aofr.DT = DateTime.Now;
                aofr.PrimaryReturnValue = RefReturnValues.Indeterminate;
                aofr.FunctionName = AState.ActivityName.ToString();
                aofr.ProcessingPoint = "Command started";

                AState.ofr = aofr;

            }



            //[*************** Perform command initialisation activities if required
            if (AState.HasInitialisationCompleted == false)
            {
                AState.ofr.ProcessingPoint = "Initialisation started";


                //[******** Specific initialisations
                AState.OutputObj.Information = new List<string>();
                AState.OutputObj.PrimaryOutput = new List<object>();


                // Check if the user specified an override on the visualisation method
                string vt = ds_common.Utilities.VisualisationType_Grid;
                string ct;
                ct = ds_common.Utilities.GetConfigParameter(ds_common.Utilities.VisualisationMethodKeyword, ref AState);
                if (ct == null) ct = string.Empty;

                switch (ct.ToLowerInvariant())
                {
                    case ds_common.Utilities.VisualisationType_Text:
                        vt = ds_common.Utilities.VisualisationType_Text;
                        break;

                    case ds_common.Utilities.VisualisationType_Grid:
                        vt = ds_common.Utilities.VisualisationType_Grid;
                        break;
                }

                AState.OutputObj.Information.Add(vt);


                // Add the column headers for this output
                AState.SourceOriginalFieldHeaders = new List<string>();
                AState.SourceOriginalFieldHeaders.Add("Byte position");
                AState.SourceOriginalFieldHeaders.Add("Decimal byte value");
                AState.SourceOriginalFieldHeaders.Add("Hex byte value");
                AState.SourceOriginalFieldHeaders.Add("Bits");
                AState.SourceOriginalFieldHeaders.Add("ASCII character");
                AState.SourceOriginalFieldHeaders.Add("ANSI (cp1252) character");
                AState.SourceOriginalFieldHeaders.Add("UTF7 character");
                AState.SourceOriginalFieldHeaders.Add("UTF8 character");
                AState.SourceOriginalFieldHeaders.Add("UTF16-LE character");
                AState.SourceOriginalFieldHeaders.Add("UTF16-BE character");
                AState.SourceOriginalFieldHeaders.Add("UTF32-LE character");
                AState.SourceOriginalFieldHeaders.Add("UTF32-BE character");
                //********]


                AState.HasInitialisationCompleted = true;
                AState.ofr.ProcessingPoint = "Initialisation completed";

                if (IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.Success;
                    return ret;
                }
            }
            //***************]



            // Determine if this the incremental data processing has completed, by checking if no more data have been provided
            if (AState.HasInitialisationCompleted == true & IDO == null)
            {
                if (AState.DataExtractComplete == false)
                {
                    AState.DataExtractComplete = true;
                    AState.DataExtractCompleteDT = DateTime.Now;
                }
            }



            //[**************** If there are still more data points to accummulate then do the accummulation stuff here
            if (AState.DataExtractComplete == false)
            {
                if (IDO == null || IDO.IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                    return ret;
                }




                //[***************************** Do the relevant processing
                if (IDO.SourceIsText)
                {
                    // Process text-based incremental data objects
                    List<object> LFields = (List<object>)IDO.IDO;

                    if (string.Equals(AState.OutputObj.Information[0], ds_common.Utilities.VisualisationType_Grid, StringComparison.OrdinalIgnoreCase))
                    {
                        AState.OutputObj.PrimaryOutput.Add(IDO.IDO);
                    }
                    else
                    {
                        if (string.Equals(AState.OutputObj.Information[0], ds_common.Utilities.VisualisationType_Text, StringComparison.OrdinalIgnoreCase))
                        {
                            StringBuilder sb = new StringBuilder();

                            if (LFields.Count == 12)
                            {
                                sb.Append((string)LFields[0]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[1]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[2]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[3]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[4]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[5]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[6]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[7]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[8]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[9]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[10]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[11]);

                                AState.OutputObj.PrimaryOutput.Add(sb.ToString());
                            }

                        }
                    }
                }
                else
                {
                    // Process binary-based incremental data objects
                }
                //*****************************]




                ret.PrimaryReturnValue = RefReturnValues.Success;
                return ret;
            }
            //****************]






            //[********************** Do any final (end of incremental data provision) processing here
            //if (AState.DataExtractComplete == true)
            //{
            //}
            //**********************]


            // Do the necessary things here to display the outputs



            AState.Completed = true;
            AState.CompletedDT = DateTime.Now;
            AState.ofr.PrimaryReturnValue = RefReturnValues.Success;


            ret.PrimaryReturnValue = RefReturnValues.Success;
            return ret;
        }


















        public static ofr RunCommand_CODE_POINT_INSPECT(ref OActivityState AState, OIncrementalDataObject? IDO = null)
        {
            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "RunCommand_CODE_POINT_INSPECT";
            // Set the Processing Point description
            ret.ProcessingPoint = "Initialising function";
            ret.AdditionalInfo = string.Empty;

            if (AState == null) return ret;

            const string CommandName = "code_point_inspect";

            // Check that the right command is being called
            if (string.Equals(AState.ActivityName, CommandName, StringComparison.OrdinalIgnoreCase) == false)
            {
                ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                return ret;
            }


            // Record the first engagement with the command on first pass
            if (AState.Started == false)
            {
                AState.Started = true;
                AState.StartedDT = DateTime.Now;

                ofr aofr = new ofr();

                aofr.DT = DateTime.Now;
                aofr.PrimaryReturnValue = RefReturnValues.Indeterminate;
                aofr.FunctionName = AState.ActivityName.ToString();
                aofr.ProcessingPoint = "Command started";

                AState.ofr = aofr;

            }



            //[*************** Perform command initialisation activities if required
            if (AState.HasInitialisationCompleted == false)
            {
                AState.ofr.ProcessingPoint = "Initialisation started";


                //[******** Specific initialisations
                AState.OutputObj.Information = new List<string>();
                AState.OutputObj.PrimaryOutput = new List<object>();


                // Check if the user specified an override on the visualisation method
                string vt = ds_common.Utilities.VisualisationType_Grid;
                string ct;
                ct = ds_common.Utilities.GetConfigParameter(ds_common.Utilities.VisualisationMethodKeyword, ref AState);
                if (ct == null) ct = string.Empty;

                switch (ct.ToLowerInvariant())
                {
                    case ds_common.Utilities.VisualisationType_Text:
                        vt = ds_common.Utilities.VisualisationType_Text;
                        break;

                    case ds_common.Utilities.VisualisationType_Grid:
                        vt = ds_common.Utilities.VisualisationType_Grid;
                        break;
                }

                AState.OutputObj.Information.Add(vt);


                // Add the column headers for this output
                AState.SourceOriginalFieldHeaders = new List<string>();
                AState.SourceOriginalFieldHeaders.Add("Character position");
                AState.SourceOriginalFieldHeaders.Add("Character width (bytes)");
                AState.SourceOriginalFieldHeaders.Add("Character");
                //AState.SourceOriginalFieldHeaders.Add("Byte position");
                AState.SourceOriginalFieldHeaders.Add("Code (decimal)");
                AState.SourceOriginalFieldHeaders.Add("Code (hex)");
                //********]


                AState.HasInitialisationCompleted = true;
                AState.ofr.ProcessingPoint = "Initialisation completed";

                if (IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.Success;
                    return ret;
                }
            }
            //***************]



            // Determine if this the incremental data processing has completed, by checking if no more data have been provided
            if (AState.HasInitialisationCompleted == true & IDO == null)
            {
                if (AState.DataExtractComplete == false)
                {
                    AState.DataExtractComplete = true;
                    AState.DataExtractCompleteDT = DateTime.Now;
                }
            }



            //[**************** If there are still more data points to accummulate then do the accummulation stuff here
            if (AState.DataExtractComplete == false)
            {
                if (IDO == null || IDO.IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                    return ret;
                }




                //[***************************** Do the relevant processing
                if (IDO.SourceIsText)
                {
                    // Process text-based incremental data objects
                    List<object> LFields = (List<object>)IDO.IDO;

                    if (string.Equals(AState.OutputObj.Information[0], ds_common.Utilities.VisualisationType_Grid, StringComparison.OrdinalIgnoreCase))
                    {
                        AState.OutputObj.PrimaryOutput.Add(IDO.IDO);
                    }
                    else
                    {
                        if (string.Equals(AState.OutputObj.Information[0], ds_common.Utilities.VisualisationType_Text, StringComparison.OrdinalIgnoreCase))
                        {
                            StringBuilder sb = new StringBuilder();

                            if (LFields.Count == 5)
                            {
                                sb.Append((string)LFields[0]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[1]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[2]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[3]);
                                sb.Append("  ,  ");
                                sb.Append((string)LFields[4]);

                                AState.OutputObj.PrimaryOutput.Add(sb.ToString());
                            }

                        }
                    }
                }
                else
                {
                    // Process binary-based incremental data objects
                }
                //*****************************]




                ret.PrimaryReturnValue = RefReturnValues.Success;
                return ret;
            }
            //****************]






            //[********************** Do any final (end of incremental data provision) processing here
            //if (AState.DataExtractComplete == true)
            //{
            //}
            //**********************]


            // Do the necessary things here to display the outputs



            AState.Completed = true;
            AState.CompletedDT = DateTime.Now;
            AState.ofr.PrimaryReturnValue = RefReturnValues.Success;


            ret.PrimaryReturnValue = RefReturnValues.Success;
            return ret;
        }

























        public static ofr RunCommand_INSPECT(ref OActivityState AState, OIncrementalDataObject? IDO = null)
        {
            // Prepare the return object
            ofr ret = new ofr();
            ret.DT = DateTime.Now;
            ret.PrimaryReturnValue = RefReturnValues.Indeterminate;
            ret.FunctionName = "RunCommand_INSPECT";
            // Set the Processing Point description
            ret.ProcessingPoint = "Initialising function";
            ret.AdditionalInfo = string.Empty;


            if (AState == null) return ret;


            const string CommandName = "inspect";

            // Check that the right command is being called
            if (string.Equals(AState.ActivityName, CommandName, StringComparison.OrdinalIgnoreCase) == false)
            {
                ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                return ret;
            }


            // Record the first engagement with the command on first pass
            if (AState.Started == false)
            {
                AState.Started = true;
                AState.StartedDT = DateTime.Now;

                ofr aofr = new ofr();

                aofr.DT = DateTime.Now;
                aofr.PrimaryReturnValue = RefReturnValues.Indeterminate;
                aofr.FunctionName = AState.ActivityName.ToString();
                aofr.ProcessingPoint = "Command started";

                AState.ofr = aofr;

            }



            //[*************** Perform command initialisation activities if required
            if (AState.HasInitialisationCompleted == false)
            {
                AState.ofr.ProcessingPoint = "Initialisation started";


                //[******** Specific initialisations
                AState.OutputObj = new OCommandOutput();
                AState.IntermediateObj = new List<object>();
                AState.OutputObj.Information = new List<string>();
                AState.OutputObj.PrimaryOutput = new List<object>();
                //********]


                AState.HasInitialisationCompleted = true;
                AState.ofr.ProcessingPoint = "Initialisation completed";

                if (IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.Success;
                    return ret;
                }
            }
            //***************]



            // Determine if this the incremental data processing has completed, by checking if no more data have been provided
            if (AState.HasInitialisationCompleted == true & IDO == null)
            {
                if (AState.DataExtractComplete == false)
                {
                    AState.DataExtractComplete = true;
                    AState.DataExtractCompleteDT = DateTime.Now;
                }
            }



            //[**************** If there are still more data points to accummulate then do the accummulation stuff here
            if (AState.DataExtractComplete == false)
            {
                if (IDO == null || IDO.IDO == null)
                {
                    ret.PrimaryReturnValue = RefReturnValues.ErrorInCodeOutsideOfThisFunction;
                    return ret;
                }



                //[***************************** Do the relevant processing
                if (IDO.SourceIsText)
                {
                    // Process text-based incremental data objects
                    string L = string.Empty;
                    const string d = "\t";
                    const int BulkTextTheshold = 100;

                    List<object> LFields = (List<object>)IDO.IDO;

                    // On first data row processing, determine the method of visualisation: first check if the user specified, if not, then determine from the data.
                    // In the case of determination, use a simple check of the length of the first string item. The assumption is that if it's very long, then the
                    // parsing didn't work as intended most likely because the delimiters or encoding where incorrect. As such, very long data strings
                    // would be put into the data grid, and hence the data grid display method is inappropriate. It would be better to use the simple
                    // text output for this scenario.
                    if (AState.ProcessedRowsCount == 1)
                    {

                        // Check if the user specified an override on the visualisation method
                        string ct;
                        ct = ds_common.Utilities.GetConfigParameter(ds_common.Utilities.VisualisationMethodKeyword, ref AState);
                        if (ct == null) ct = string.Empty;

                        switch (ct.ToLowerInvariant())
                        {
                            case ds_common.Utilities.VisualisationType_Text:
                                AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Text);
                                break;

                            case ds_common.Utilities.VisualisationType_Grid:
                                AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Grid);
                                break;

                            default:
                                if (LFields.Count > 0)
                                {
                                    if (((string)LFields[0]).Length > BulkTextTheshold)
                                    {
                                        AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Text);
                                    }
                                    else
                                    {
                                        AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Grid);
                                    }
                                }
                                else
                                {
                                    AState.OutputObj.Information.Add(ds_common.Utilities.VisualisationType_Text);
                                }
                                break;
                        }


                    }


                    if (string.Equals(AState.OutputObj.Information[0], ds_common.Utilities.VisualisationType_Text, StringComparison.OrdinalIgnoreCase))
                    {
                        for (int i = 0; i < LFields.Count; i++)
                        {
                            if (i > 0) L += d;

                            L += (string)LFields[i];

                            L = L.ReplaceLineEndings(" ");
                        }

                        AState.OutputObj.PrimaryOutput.Add(L);
                    }
                    else
                    {
                        if (string.Equals(AState.OutputObj.Information[0], ds_common.Utilities.VisualisationType_Grid, StringComparison.OrdinalIgnoreCase))
                        {
                            AState.OutputObj.PrimaryOutput.Add(IDO.IDO);
                        }
                    }
                }
                else
                {
                    // Process binary-based incremental data objects
                }


                //*****************************]



                ret.PrimaryReturnValue = RefReturnValues.Success;
                return ret;
            }
            //****************]






            //[********************** Do any final (end of incremental data provision) processing here
            //if (AState.DataExtractComplete == true)
            //{
            //}
            //**********************]


            // Do the necessary things here to display the outputs



            AState.Completed = true;
            AState.CompletedDT = DateTime.Now;
            AState.ofr.PrimaryReturnValue = RefReturnValues.Success;


            ret.PrimaryReturnValue = RefReturnValues.Success;
            return ret;
        }









    }

}
