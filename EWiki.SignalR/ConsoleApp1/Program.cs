using EWiki.SignalR.Hubs.Models;
using EWiki.Sniper;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ewiki.Sniper.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var snipeRq = new SnipeRq()
            {
                UserName = "bongvl.vn@gmail.com",
                Password = "Asdcxz1+",
                PokemonId = POGOProtos.Enums.PokemonId.Xatu,
                Latitude = 51.4579544,
                Longitude = -0.2581807
            };

            SniperFunc.Execute(snipeRq, string.Empty);

            Console.ReadLine();
        }
    }
}
