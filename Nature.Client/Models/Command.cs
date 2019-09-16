using Nature.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nature.Client.Models
{
    public class Command: IMessage
    {
        public string CmdMsg { get; set; }
        public Guid Guid { get; set; }
        public DateTime Time { get; set; }
        public Node Node { get; set; }
    }
}
