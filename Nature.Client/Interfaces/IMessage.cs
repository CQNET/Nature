using Nature.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nature.Client.Interfaces
{
    public interface IMessage
    {
        Guid Guid { get; set; }

        DateTime Time { get; set; }

        Node Node { get; set; }
    }
}
