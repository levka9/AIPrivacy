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
        #region Properties
        List<string> lstPermittedProcess;
        List<string> lstBlockedProcess;

        public List<string> PermittedProcess
        {
            get { return lstPermittedProcess; }
            set { lstPermittedProcess = value; }
        }
        public List<string> BlockedProcess
        {
            get { return lstBlockedProcess; }
            set { lstBlockedProcess = value; }
        } 
        #endregion

        public ProcessList()
        {
            ParseFile("BlockedProcess.txt", out lstBlockedProcess);
            ParseFile("PermittedProcess.txt", out lstPermittedProcess);
        }
        
        private void ParseFile(string FileName, out List<string> ProcessList)
        {
            ProcessList = new List<string>();

            string filePath = string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), FileName);

            using (var fs = new StreamReader(filePath))
            {
                string processName = fs.ReadLine();
                while (!string.IsNullOrEmpty(processName))
                {
                    ProcessList.Add(processName.Split(' ')[0].Replace("\t", "").Replace("//", ""));

                    processName = fs.ReadLine();
                }
            }
        }
    }
}
