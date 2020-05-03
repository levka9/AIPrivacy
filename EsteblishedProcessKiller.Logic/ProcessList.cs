using EsteblishedProcessKiller.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsteblishedProcessKiller.Logic
{
    public class ProcessList
    {
        public List<string> PermittedProcess { get; set; }
        public List<string> BlockedProcess { get; set; }

        public ProcessList()
        {
            this.SetBlockedProcess();
            this.SetPermittedProcess();
        }

        private void SetBlockedProcess()
        {
            BlockedProcess = new List<string>();

            string filePath = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), "BlockedProcess.txt");

            using (var fs = new StreamReader(filePath))
            {
                string processName = fs.ReadLine();
                while (!string.IsNullOrEmpty(processName))
                {
                    BlockedProcess.Add(processName);

                    processName = fs.ReadLine();
                }
            }            
        }

        private void SetPermittedProcess()
        {
            PermittedProcess = new List<string>();

            string filePath = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), "PermittedProcess.txt");

            using (var fs = new StreamReader(filePath))
            {
                string processName = fs.ReadLine();
                while (!string.IsNullOrEmpty(processName))
                {
                    PermittedProcess.Add(processName);

                    processName = fs.ReadLine();
                }
            }
        }
    }
}
