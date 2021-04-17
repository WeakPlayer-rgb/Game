using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewGame
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static ApplicationContext Context { get; set; }

        public static Size screenSize { get; set; }

        [STAThread]
        public static void Main()
        {
            // Application.SetHighDpiMode(HighDpiMode.SystemAware);
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Menu());
            screenSize = new Size(1280, 720);
            Context = new ApplicationContext(new Menu());
            Application.Run(Context);
        }
    }
}