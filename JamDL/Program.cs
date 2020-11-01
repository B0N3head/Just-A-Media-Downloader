using JAMDL;
using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MediaDL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
       {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ThemeCreator());
        }
    }
}
