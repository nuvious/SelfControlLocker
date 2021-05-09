using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SelfControl
{
    public class TimeCheckerWorker
    {
        const string CONFIG_FILE = "selfcontrol.json";
        public static bool StopTimeChecker = false;
        private static SelfControlConfig config = null;

        private static void dumpConfigTemplateAndExit(string configPath, string message)
        {
            var config = new SelfControlConfig();
            File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
            MessageBox.Show(
                            message,
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
                        try
                        {
                            string jsonText = File.ReadAllText(configPath);
                            config = JsonConvert.DeserializeObject<SelfControlConfig>(jsonText);
                        } catch(Exception e) {
                            dumpConfigTemplateAndExit(
                                configPath,
                                "Error reading configuration. Dumping template. Please edit and restart the application."
                            );
                        }
                    }
                    else
                    {
                        dumpConfigTemplateAndExit(
                            configPath,
                            "Edit ~/selfcontrol.json and restart the application"
                        );
                    }
                }

                if (StopTimeChecker)
                {
                    break;
                }
                DateTime curr = DateTime.Now;
                DailyConfig currConfig = config[(int)curr.DayOfWeek];
                DateTime start = new DateTime(curr.Year, curr.Month, curr.Day, currConfig.Start, 0, 0);
                DateTime finish = new DateTime(curr.Year, curr.Month, curr.Day, currConfig.End, 0, 0);

                if (curr > start && curr < finish)
                {
                    if (!LockCheck.IsWorkstationLocked())
                    {
                        LockCheck.LockWorkStation();
                    }
                }

                Thread.Sleep(currConfig.Delay);
            }
        }
    }
}
