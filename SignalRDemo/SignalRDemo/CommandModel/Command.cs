using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.CommandModel
{
    public class Command<T>
    {
        public string command { get; set; }

        public T data { get; set; }
    }
}
