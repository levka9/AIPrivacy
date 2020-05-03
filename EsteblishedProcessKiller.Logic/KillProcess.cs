using EsteblishedProcessKiller.Logger;
using EsteblishedProcessKiller.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsteblishedProcessKiller.Logic
{
    public class KillProcess : IJob
    {
        ProcessList processList;
        ProcessInfo processInfo;

        public KillProcess()
        {
            processList = new ProcessList();
            processInfo = new ProcessInfo();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            this.Run();

            await Console.Out.WriteLineAsync(string.Format("Excuted at {0}", DateTime.UtcNow.ToString()));
        }

        public void Run()
        {
            processInfo.Run();

            foreach (var establishedProcess in processInfo.EstablishedProcessesDetails)
            {

                Kill(establishedProcess);
                Console.WriteLine(string.Format("{0}", establishedProcess.Name));
            }
        }

        private void Kill(ProcessDetails ProcessDetails)
        {
            bool IsKill = int.Parse(ProcessDetails.PortNumber) > 1024;
            IsKill = IsKill && !processList.PermittedProcess.Any(x => x == ProcessDetails.ProcessName);
            
            if (IsKill)
            {
                try
                {
                    foreach (var process in Process.GetProcessesByName(ProcessDetails.ProcessName))
                    {
                        process.Kill();

                        Console.WriteLine(string.Format("Process {0} killed", ProcessDetails.Name));
                        LogHelper.Info(string.Format("Process {0} killed", ProcessDetails.Name));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("{0} :{1}", ex.Message, ProcessDetails.Name));
                    LogHelper.Error(string.Format("{0} :{1}", ex.Message, ProcessDetails.Name));
                    //throw;
                }
            }            
        }
    }
}
