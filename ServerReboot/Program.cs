using System;
using System.IO;
using System.Diagnostics;

namespace ServerReboot
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;

            string machineName = System.Environment.MachineName;

            string path = Environment.CurrentDirectory + @"\Logs\log.txt";
            string errorPath = Environment.CurrentDirectory + @"\Logs\errorlog.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                    }

                    sw.WriteLine("The current time is " + dt);
                    sw.WriteLine("Rebooting " + machineName);
                    sw.WriteLine("Reboot reason: " + args[0]);
                    sw.WriteLine("************");
                }
            }
            catch(Exception e)
            {
                using (StreamWriter sw = new StreamWriter(errorPath))
                {
                    if (!File.Exists(errorPath))
                    {
                        File.Create(errorPath);
                    }

                    sw.WriteLine("An error has occurred at " + dt);
                    sw.WriteLine(e.Message);
                    sw.WriteLine("**********");

                    Environment.Exit(1);
                }
            }

            try
            {
                using(Process pr = new Process())
                {
                    pr.StartInfo.FileName = "cmd.exe";
                    pr.StartInfo.UseShellExecute = true;
                    pr.StartInfo.CreateNoWindow = true;
                    pr.StartInfo.Arguments = @"shutdown -r -t 0 -d p:1:1 -c '" + args[0] + "'";
                }
            }
            catch(Exception e)
            {
                using(StreamWriter sw = new StreamWriter(errorPath))
                {
                    if (!File.Exists(errorPath))
                    {
                        File.Create(errorPath);
                    }

                    sw.WriteLine("An error has occurred at " + dt);
                    sw.WriteLine(e.Message);
                    sw.WriteLine("**********");

                    Environment.Exit(1);
                }
            }

            Environment.Exit(0);
        }
    }
}
