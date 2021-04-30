using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfControl
{
    public class TimeCheckerWorker
    {
        const string CONFIG_FILE = "selfcontrol.json";
        public static bool StopTimeChecker = false;
        private static SelfControlConfig config = null;

        public static void DoWork()
        {
            while (true)
            {
                string configPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\" + CONFIG_FILE;

                if (config == null)
                {
                    // Read config if it exists, if it doesn't dump a default one for the user to edit.
                    if (File.Exists(configPath))
                    {
                        string jsonText = File.ReadAllText(configPath);
                        config = JsonConvert.DeserializeObject<SelfControlConfig>(jsonText);
                    }
                    else
                    {
                        config = new SelfControlConfig();
                        File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
                        MessageBox.Show(
                            "Edit ~/selfcontrol.json and restart the application", 
                            "Self Control",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        new Process
                        {
                            StartInfo = new ProcessStartInfo(configPath)
                            {
                                UseShellExecute = true
                            }
                        }.Start();
                        Environment.Exit(0);
                    }
                }

                if (StopTimeChecker)
                {
                    break;
                }
                DateTime curr = DateTime.Now;
                DateTime start = new DateTime(curr.Year, curr.Month, curr.Day, config.Start, 0, 0);
                DateTime finish = new DateTime(curr.Year, curr.Month, curr.Day, config.End, 0, 0);

                if (config.Weekdays.Contains((int)curr.DayOfWeek) && curr > start && curr < finish)
                {
                    if (!LockCheck.IsWorkstationLocked())
                    {
                        LockCheck.LockWorkStation();
                    }
                }

                Thread.Sleep(config.Delay);
            }
        }
    }
}
