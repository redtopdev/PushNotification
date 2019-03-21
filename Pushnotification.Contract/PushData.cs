using System;
using System.Collections.Generic;
using System.Text;

namespace Pushnotification.Contract
{
    public class PushData
    {
        public PushData(string type)
        {
            this.Type = type;
        }
        public string Type { get; private set; }
    }
}
