using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using ds_common;
using static System.Windows.Forms.AxHost;


namespace Data_Scratchpad
{
    internal static class Program
    {






        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            DataProcessing.ProcessingInitialisations();


            //[************ For debugging non-UI stuff. Remove when finished.
            //CheckStuff();
            //return;
            //************]


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

        }













    }
}
