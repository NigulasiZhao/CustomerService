using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.CommandModel
{
    public class CommandUserConnection : Command<CommandUserConnection>
    {
        public string nickName { get; set; }

        public string faceImg { get; set; }

        public string userId { get; set; }

        public string deviceId { get; set; }
    }
}
