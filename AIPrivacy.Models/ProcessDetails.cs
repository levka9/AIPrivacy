using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsteblishedProcessKiller.Models
{
    public class ProcessDetails
    {
        public string Name
        {
            get
            {
                return $"{this.ProcessName} ({this.Protocol} port: {this.PortNumber} {this.ForeignAddress} processId: {this.ProcessId} state: {this.State})";
            }
            set { }
        }
        public string PortNumber { get; set; }
        public string ProcessName { get; set; }
        public string Protocol { get; set; }
        public string ForeignAddress { get; set; }
        public string ProcessId { get; set; }
        public string State { get; set; }
    }
}
