using SelfControl.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfControl
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

            Application.Run(new SelControlApplicationContext());
        }
    }

    public class SelControlApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Thread timeCheckerThread;

        public SelControlApplicationContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.Icon,
                Visible = true
            };
            timeCheckerThread = new Thread(TimeCheckerWorker.DoWork);
            timeCheckerThread.Start();
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
