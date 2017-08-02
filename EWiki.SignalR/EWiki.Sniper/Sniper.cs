using EWiki.SignalR.Hubs.Models;
using POGOProtos.Enums;
using PokeMaster.Logic;
using PokeMaster.Logic.Functions;
using PokeMaster.Logic.Shared;
using PokeMaster.Logic.Utils;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EWiki.Sniper
{
    public static class SniperFunc
    {
        public static void Execute(SnipeRq snipeRq, string connectionId)
        {
            if (string.IsNullOrWhiteSpace(snipeRq.UserName) || string.IsNullOrWhiteSpace(snipeRq.Password))
            {
                Logger.Error("Please input UserName and Password.");
                return;
            }

            var settings = new Settings();

            if (snipeRq.UserName.Contains("@gmail.com"))
            {
                settings.AuthType = AuthType.Google;
            }
            else
            {
                settings.AuthType = AuthType.Ptc;
            }

            settings.pFHashKey = "7N2P8Z7D4W3V0N4U3N0E";
            settings.Username = snipeRq.UserName;
            settings.Password = snipeRq.Password;

            try
            {
                var deviceData = Path.Combine("DeviceData.json");
                DeviceSetup.SelectDevice("iPhone 7", "d9ec71a7bb96a35bd9ec71a7bb96a35b", deviceData);
                new Logic(settings, GlobalVars.infoObservable);
            }
            catch (PtcOfflineException)
            {
                Logger.Error("PTC Servers are probably down OR you credentials are wrong.");
                Logger.Error("Trying again in 20 seconds...");
                Thread.Sleep(20000);
                new Logic(settings, GlobalVars.infoObservable);
            }
            catch (AccountNotVerifiedException)
            {
                Logger.Error("Your PTC Account is not activated. Exiting in 10 Seconds.");
                Thread.Sleep(10000);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Logger.Error($"Unhandled exception: {ex}");
                Logger.Error("Restarting in 20 Seconds.");
                Thread.Sleep(20000);
                new Logic(settings, GlobalVars.infoObservable);
            }

            Logic.objClient.ClientId = connectionId;
            Logic.objClient.CurrentAltitude = Logic.Instance.BotSettings.DefaultAltitude;
            Logic.objClient.CurrentLongitude = Logic.Instance.BotSettings.DefaultLongitude;
            Logic.objClient.CurrentLatitude = Logic.Instance.BotSettings.DefaultLatitude;


            #region Fix Altitude

            if (Math.Abs(Logic.objClient.CurrentAltitude) < 0.001)
            {
                Logic.objClient.CurrentAltitude = LocationUtils.GetAltitude(Logic.objClient.CurrentLatitude, Logic.objClient.CurrentLongitude);
                Logic.Instance.BotSettings.DefaultAltitude = Logic.objClient.CurrentAltitude;

                Logger.Warning($"Altitude was 0, resolved that. New Altitude is now: {Logic.objClient.CurrentAltitude}");
            }

            Logic.objClient.Login.DoLogin().Wait();
            Logger.Debug("login done");

            #endregion
            GlobalVars.SnipeOpts.WaitSecond = 7;
            GlobalVars.SnipeOpts.NumTries = 3;
            GlobalVars.SnipeOpts.TransferIt = false;
            GlobalVars.SnipeOpts.UsePinap = false;
            GlobalVars.SnipeOpts.Enabled = true;
            GlobalVars.PokemonPinap = new List<PokemonId>();


            CatchingLogic.AllowCatchPokemon = false;
            CatchingLogic.Execute();

            Logic.Instance.sniperLogic.Execute(snipeRq.PokemonId, new GeoCoordinate()
            {
                Latitude = snipeRq.Latitude,
                Longitude = snipeRq.Longitude
            });
        }
    }
}
