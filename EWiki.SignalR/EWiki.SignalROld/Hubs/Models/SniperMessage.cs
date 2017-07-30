using System;

namespace EWiki.SignalR.Hubs.Models
{
    public class SniperMessage
    {
        public string Content { get; set; }
        public string Color { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
