using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Person
{
    public class Customer
    {
        public string nickName { get; set; }
        public string faceimg { get; set; }
        public string userId { get; set; }
        public string deviceId { get; set; }

        public string ConnectionId { get; set; }

        public string servicerTerminalId { get; set; }
    }
}
