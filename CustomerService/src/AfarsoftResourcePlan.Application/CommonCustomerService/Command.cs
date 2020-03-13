using System;
using System.Collections.Generic;
using System.Text;

namespace AfarsoftResourcePlan.CommonCustomerService
{
    public class Command<T>
    {
        public string command { get; set; }

        public T data { get; set; }
    }
}
