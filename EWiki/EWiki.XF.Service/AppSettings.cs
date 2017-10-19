using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWiki.XF.Service
{
    public class AppSettings
    {
#if DEBUG
        public const string WEB_API_URL = "http://pokitappapi.azurewebsites.net/api/";
#else
        public const string WEB_API_URL = "http://esquare.io/api/";
#endif
    }
}
