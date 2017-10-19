using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWiki.Api.Models
{
    public class FeedInfo : EntityBase
    {
        public PokemonId PokemonId { get; set; } = PokemonId.Missingno;
        public long EncounterId { get; set; }
        public DateTime ExpirationTimestamp { get; set; } = default(DateTime);
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SpawnPointId { get; set; } = null;
        public PokemonMove Move1 { get; set; }
        public PokemonMove Move2 { get; set; }
        public double IV { get; set; }
        public bool Verified { get; set; } = false;
        public DateTime VerifiedOn { get; set; } = default(DateTime);
        public DateTime ReceivedTimeStamp { get; set; } = DateTime.Now;
        public int? FeedChanelId { get; set; }
        public FeedChanel FeedChanel { get; set; }
    }
}
