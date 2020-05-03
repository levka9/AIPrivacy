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
                return string.Format("{0} ({1} port {2} {3} state {4})", this.ProcessName, 
                    this.Protocol, this.PortNumber, this.ForeignAddress, this.State);
            }
            set { }
        }
        public string PortNumber { get; set; }
        public string ProcessName { get; set; }
        public string Protocol { get; set; }
        public string ForeignAddress { get; set; }
        public string State { get; set; }
    }
}
