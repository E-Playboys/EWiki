using Android.Util;
using EWiki.XF.Droid.LocationFeeder.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;

namespace EWiki.XF.Droid.LocationFeeder.Common
{
    public class ChannelParser
    {
        public List<DiscordChannels> Settings;

        public static ChannelParser Default => new ChannelParser();

        public void LoadChannelSettings()
        {
            var configFile = Path.Combine(Directory.GetCurrentDirectory(), "Config", "discord_channels.json");

            if (File.Exists(configFile))
            {
                //if the file exists, load the Settings
                var input = File.ReadAllText(configFile);

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                Settings = JsonConvert.DeserializeObject<List<DiscordChannels>>(input, jsonSettings);
            }
            else
            {
                Settings = new List<DiscordChannels>();
                Log.Error("", $"Channel file \"{configFile}\" not found!");
            }

        }

        public ChannelInfo ToChannelInfo(string channelId)
        {
            var channelInfo = new ChannelInfo();
            if (channelId != null)
            {
                foreach (var channel in Settings)
                {
                    if (object.Equals(channelId, channel.id))
                    {
                        channelInfo.server = channel.Server;
                        channelInfo.channel = channel.Name;
                        channelInfo.isValid = true;
                        return channelInfo;
                    }
                }
            }
            channelInfo.server = "Unknown";
            channelInfo.channel = "Unknown";
            return channelInfo;
        }

        public class DiscordChannels
        {
            public string id;
            public string Name;
            public string Server;
        }
    }

    public class SourceInfo
    {
        public string server { get; set; }
        public string channelId { get; set; }
    }
}