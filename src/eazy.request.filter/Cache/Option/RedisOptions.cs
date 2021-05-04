using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.request.filter.Cache.Option
{
    public class RedisOptions
    {
        public bool Enable { get; set; }
        public string Connection { get; set; }
        public string InstanceName { get; set; }
    }
}
