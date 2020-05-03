using AIPrivacy.Logic;
using EsteblishedProcessKiller.Logger;
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
                string[] commandResultRows = null;

                using (var process = new Process())
                {
                    ProcessData.Excute(process, progName, netStatArgs);

                    commandResultRows = ProcessData.GetProcessData(process);
                }

                ParseData(commandResultRows);

                EstablishedProcessesDetails = lstProcessDetails.Where(x => x.State == ESTABLISHED).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void ParseData(string[] rows)
        {
            lstProcessDetails = new List<ProcessDetails>();

            foreach (string row in rows)
            {
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
                        State = tokens[4],
                        ProcessId = tokens[5]
                    });
                }
            }
        }
    }
}
