using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PokemonGo.RocketAPI;
using EWiki.Sniper.PokeFeeder;
using System.Collections.Generic;
using EWiki.SignalR.Hubs.Models;
using EWiki.Sniper;

namespace EWiki.SignalR.Hubs
{
    public class SniperHub : Hub
    {
        public void SendMessage(string connectionId, SniperMessage message)
        {
            Clients.Client(connectionId).GetMessage(JsonConvert.SerializeObject(message));
        }

        public async Task SniperMessage(string message)
        {
            var connectionId = Context.ConnectionId;

            // Disable snipping feature
            Clients.Client(connectionId).GetSniperStatus(JsonConvert.SerializeObject(new SniperStatus
            {
                IsAvailable = false
            }));
            return;

            try
            {
                var snipeRq = JsonConvert.DeserializeObject<SnipeRq>(message);
                await Snipe(snipeRq);
            }
            catch (Exception e)
            {
                Logger.Error($"Error: {e.Message}");
            }
        }

        public void FetchSniperInfos()
        {
            try
            {
                var sniperInfos = PokeFeeder.FetchPokemon();
                SendSniperInfos(sniperInfos);
            }
            catch(Exception e)
            {
                Logger.Error($"Error: {e.Message}");
            }
        }

        private void SendSniperInfos(List<SniperInfo> sniperInfos)
        {
            var connectionId = Context.ConnectionId;
            Clients.Client(connectionId).GetSniperInfos(JsonConvert.SerializeObject(sniperInfos));
        }

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            SendMessage(connectionId, new SniperMessage()
            {
                Color = "Magenta",
                Content = "Connected",
                CreatedDate = DateTime.Now
            });

            return base.OnConnected();
        }

        public async Task Snipe(SnipeRq snipeRq)
        {
            var connectionId = Context.ConnectionId;
            await Task.Run(() =>
            {
                SniperFunc.Execute(snipeRq, connectionId);
            });
        }
    }
}
