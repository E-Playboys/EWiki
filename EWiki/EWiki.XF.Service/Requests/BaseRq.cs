﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWiki.XF.Service.Requests
{
    public abstract class BaseRq
    {
        protected BaseRq()
        {
            Take = 5;
        }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
