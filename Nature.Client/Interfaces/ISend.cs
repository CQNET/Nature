using System;
using System.Collections.Generic;
using System.Text;

namespace Nature.Client.Interfaces
{
    interface ISend
    {
        void Send(IMessage message);
    }
}
