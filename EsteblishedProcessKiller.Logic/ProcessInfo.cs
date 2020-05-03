using EsteblishedProcessKiller.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EsteblishedProcessKiller.Logic
{
    public class ProcessInfo
    {
        #region Properties

        readonly string netStatArgs = "-a -n -o";
        readonly string progName = "netstat.exe";
        const string ESTABLISHED = "ESTABLISHED";
        List<ProcessDetails> lstProcessDetails;
        public List<ProcessDetails> ProcessesDetails
        {
            get
            {
                return lstProcessDetails;
            }
        }
        public List<ProcessDetails> EstablishedProcessesDetails { get; set; }
        #endregion

        public void Run()
        {
            try
            {
                using (var process = new Process())
                {
                    ExcuteNetstat(process);

                    var rows = GetProcessData(process);

                    ParseData(rows);

                    EstablishedProcessesDetails = lstProcessDetails.Where(x => x.State == ESTABLISHED).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string LookupProcess(int pid)
        {
            string procName;

            try
            {
                procName = Process.GetProcessById(pid).ProcessName;
            }
            catch (Exception)
            {
                procName = "-";
            }
            return procName;
        }

        private Process ExcuteNetstat(Process process)
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

        private string[] GetProcessData(Process process)
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

            // Parse all data
            //Get The Rows
            lstProcessDetails = new List<ProcessDetails>();
            string[] rows = Regex.Split(content, "\r\n");

            return rows;
        }

        private void ParseData(string[] rows)
        {
            foreach (string row in rows)
            {
                //Split it baby
                string[] tokens = Regex.Split(row, "\\s+");
                if (tokens.Length > 5 && (tokens[1].Equals("UDP") || tokens[1].Equals("TCP")))
                {
                    string localAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");
                    string foreignAddress = Regex.Replace(tokens[3], @"\[(.*?)\]", "1.1.1.1");

                    lstProcessDetails.Add(new ProcessDetails
                    {
                        Protocol = localAddress.Contains("1.1.1.1") ? String.Format("{0}v6", tokens[1]) : String.Format("{0}v4", tokens[1]),
                        PortNumber = localAddress.Split(':')[1],
                        ForeignAddress = foreignAddress.Split(':')[0],
                        ProcessName = tokens[1] == "UDP" ? LookupProcess(Convert.ToInt16(tokens[4])) : LookupProcess(Convert.ToInt16(tokens[5])),
                        State = tokens[4]
                    });
                }
            }
        }
    }    
}
