using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWiki.Api.Models
{
    public class FeedChanel : EntityBase
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public int Priority { get; set; }

        public ICollection<FeedInfo> FeedInfos { get; set; }
    }
}
