﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Req
    {
        public string path { get; set; }
        public int topics { get; set; }
        public List<string> flags { get; set; }

        public Req(string path, int num, List<string> flags)
        {
            this.path = path;
            this.topics = num;
            this.flags = flags;
        }
    }
}
