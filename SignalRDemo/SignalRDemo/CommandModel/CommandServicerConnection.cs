using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.CommandModel
{
    public class CommandServicerConnection : Command<CommandServicerConnection>
    {
        public string nickName { get; set; }

        public string faceimg { get; set; }

        public string servicerId { get; set; }

        public string deviceId { get; set; }
    }
}
