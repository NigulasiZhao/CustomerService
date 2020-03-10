using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Person
{
    public class CustomerService
    {
        public string nickName { get; set; }
        public string faceImg { get; set; }
        public string servicerId { get; set; }
        public string deviceId { get; set; }

        public string ConnectionId { get; set; }

        public int ConnectionCount { get; set; }
    }
}
