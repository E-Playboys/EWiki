﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWiki.XF.Service
{
    public class AppSettings
    {
#if DEBUG
        public const string WEB_API_URL = "http://esquare.io/api/";
#else
        public const string WebApiUrl = "";
#endif
    }
}
