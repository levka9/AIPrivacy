using EsteblishedProcessKiller.Logic;
using EsteblishedProcessKiller.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsteblishedProcessKiller
{
    class Program
    {
        static void Main(string[] args)
        {   
            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.Start();

            var result = scheduler.DeleteJob(new JobKey("AIPrivacy"));

            IJobDetail job = JobBuilder.Create<KillProcess>()
                .WithIdentity("AIPrivacy")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                        .StartNow()                                        
                                        .WithSimpleSchedule(x => x
                                            .WithIntervalInSeconds(10)
                                            .RepeatForever())
                                        .Build();
            
            scheduler.ScheduleJob(job, trigger);            

            Console.ReadKey();
        }
    }
}
