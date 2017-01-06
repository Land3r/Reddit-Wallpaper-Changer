using System;
using System.Threading;
using System.Windows.Forms;

namespace Reddit_Wallpaper_Changer
{
    static class Program
    {
        //TODO : Check if another proper method exists
        /// <summary>
        /// This mutex is used to avoid lanching many instances by mistake
        /// </summary>
        static Mutex mutex = new Mutex(false, "RedditWallpaperChanger_byUgleh");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
        {
            DialogResult dialogResult = MessageBox.Show("Run another instance of RWC?", "Reddit Wallpaper Changer", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                    StartApplicationInstance();
            }
            return;
        }

        try
        {
            StartApplicationInstance();
        }
        finally { mutex.ReleaseMutex(); } // I find this more explicit
        }

        /// <summary>
        /// Start a new application instance
        /// </summary>
        static void StartApplicationInstance()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RWC());
        }
    }
}
