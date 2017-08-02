using Newtonsoft.Json;
using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace EWiki.Sniper.PokeFeeder
{
    public class SniperInfo
    {
        public PokemonId PokemonId { get; set; } = PokemonId.Missingno;
        public string Number
        {
            get
            {
                var number = "";
                for (var i = 0; i < 3 - PokemonId.ToString().Length; i++)
                {
                    number += "0";
                }
                return $"{number}{(int)PokemonId}";
            }
        }
        [JsonIgnore]
        public ulong EncounterId { get; set; }
        public DateTime ExpirationTimestamp { get; set; } = default(DateTime);
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [JsonIgnore]
        public string SpawnPointId { get; set; } = null;
        public PokemonMove Move1 { get; set; }
        public PokemonMove Move2 { get; set; }
        public double IV { get; set; }
        public bool Verified { get; set; } = false;
        public DateTime VerifiedOn { get; set; } = default(DateTime);
        public ChannelInfo ChannelInfo { get; set; }
        [JsonIgnore]
        public List<ChannelInfo> OtherChannelInfos { get; set; } = new List<ChannelInfo>();
        public DateTime ReceivedTimeStamp { get; set; } = DateTime.Now;
        public bool NeedVerification { get; set; } = false;

        public override string ToString()
        {
            return FormatPokemonLog(this);
        }

        private static string FormatPokemonLog(SniperInfo sniperInfo)
        {
            const string timeFormat = "HH:mm:ss";
            return $"{sniperInfo.ChannelInfo}: {sniperInfo.PokemonId} at {sniperInfo.Latitude.ToString("N6", CultureInfo.InvariantCulture)},{sniperInfo.Longitude.ToString("N6", CultureInfo.InvariantCulture)}"
                   + " with " +
                   (!sniperInfo.IV.Equals(default(double))
                       ? $"{sniperInfo.IV}% IV"
                       : "unknown IV")
                   + (sniperInfo.Move1 != null ? $" Move1: {sniperInfo.Move1}" : "")
                   + (sniperInfo.Move2 != null ? $" Move2: {sniperInfo.Move2}" : "")
                   + (sniperInfo.EncounterId != default(ulong) && sniperInfo.EncounterId != null ? $" EncounterId: {sniperInfo.EncounterId.ToString()}" : "")
                   + (sniperInfo.ExpirationTimestamp != default(DateTime)
                       ? $" until {sniperInfo.ExpirationTimestamp.ToString(timeFormat)}"
                       : "");
        }

        public List<ChannelInfo> GetAllChannelInfos()
        {
            var channelInfos = new List<ChannelInfo>();
            if (ChannelInfo != null)
            {
                channelInfos.Add(ChannelInfo);
            }
            channelInfos.AddRange(OtherChannelInfos);
            return channelInfos;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return SniperInfoEquals(this, (SniperInfo)obj);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + Math.Round(Latitude, 4).GetHashCode();
            hash = (hash * 7) + Math.Round(Longitude, 4).GetHashCode();
            return hash;
        }

        private static bool SniperInfoEquals(SniperInfo a, SniperInfo b)
        {
            if (Math.Abs(a.Latitude - b.Latitude) <= Constants.CoordinatesOffsetAllowed
                && Math.Abs(a.Longitude - b.Longitude) <= Constants.CoordinatesOffsetAllowed)
            {
                if (a.PokemonId.Equals(PokemonId.Missingno) || b.PokemonId.Equals(PokemonId.Missingno))
                {
                    return true;
                }
                if (a.PokemonId.Equals(b.PokemonId))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
