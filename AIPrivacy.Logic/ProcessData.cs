using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AIPrivacy.Logic
{
    public static class ProcessData
    {
        public static Process Excute(Process process, string progName, string netStatArgs)
        {
            ProcessStartInfo psf = new ProcessStartInfo();
            psf.Arguments = netStatArgs;
            psf.FileName = progName;
            psf.UseShellExecute = false;
            psf.WindowStyle = ProcessWindowStyle.Hidden;
            psf.RedirectStandardInput = true;
            psf.RedirectStandardOutput = true;
            psf.RedirectStandardError = true;

            process.StartInfo = psf;
            process.Start();

            return process;
        }

        public static string[] GetProcessData(Process process)
        {
            // Get process data information
            StreamReader stdOutput = process.StandardOutput;
            StreamReader stdError = process.StandardError;

            string content = stdOutput.ReadToEnd() + stdError.ReadToEnd();
            string exitStatus = process.ExitCode.ToString();

            if (exitStatus != "0")
            {
                // Command Errored. Handle Here If Need Be
            }

            string[] rows = Regex.Split(content, "\r\n");

            return rows;
        }
    }
}
