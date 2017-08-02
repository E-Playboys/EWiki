using System;
using System.Collections.Generic;
using Quobject.Collections.Immutable;
using Quobject.SocketIoClientDotNet.Client;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using POGOProtos.Enums;

namespace EWiki.Sniper.PokeFeeder
{
    public class PokeFeeder
    {
        public static List<SniperInfo> FetchPokemon()
        {
            var newSniperInfos = new List<SniperInfo>();

            var options = new IO.Options
            {
                Transports = ImmutableList.Create("websocket")
            };

            //var socket = IO.Socket("http://pokezz.com", options);
            //socket.On("p", msg =>
            //{
            //    socket.Close();
            //    var pokemonDefinitions = ((string)msg).Split('~');
            //    foreach (var pokemonDefinition in pokemonDefinitions)
            //    {
            //        try
            //        {
            //            var pokemonDefinitionElements = pokemonDefinition.Split('|');
            //            var expiration =
            //                new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(
            //                        Convert.ToDouble(pokemonDefinitionElements[3], CultureInfo.InvariantCulture))
            //                    .ToLocalTime();
            //            var pokezzElement = new SniperInfo()
            //            {
            //                PokemonId =
            //                    (PokemonId)Convert.ToInt32(pokemonDefinitionElements[0], CultureInfo.InvariantCulture),
            //                Latitude = Convert.ToDouble(pokemonDefinitionElements[1], CultureInfo.InvariantCulture),
            //                Longitude = Convert.ToDouble(pokemonDefinitionElements[2], CultureInfo.InvariantCulture),
            //                ExpirationTimestamp = expiration,
            //                Verified = pokemonDefinitionElements[4] != "0",
            //                Move1 = string.IsNullOrEmpty(pokemonDefinitionElements[5]) ? PokemonMove.MoveUnset : (PokemonMove)Enum.Parse(typeof(PokemonMove), pokemonDefinitionElements[5].Split(',')[0].Trim()),
            //                Move2 = string.IsNullOrEmpty(pokemonDefinitionElements[5]) ? PokemonMove.MoveUnset : (PokemonMove)Enum.Parse(typeof(PokemonMove), pokemonDefinitionElements[5].Split(',')[1].Trim()),
            //                IV = string.IsNullOrEmpty(pokemonDefinitionElements[6]) ? 0 : Convert.ToDouble(pokemonDefinitionElements[6]),
            //                ChannelInfo = new ChannelInfo() { channel = "PokeZZ" }
            //            };
            //            newSniperInfos.Add(pokezzElement);
            //        }
            //        catch (Exception)
            //        {
            //            // Just in case Pokezz changes their implementation, let's catch the error and set the error flag.
            //            continue;
            //        }
            //    }
            //});

            IWebDriver webDriver = new ChromeDriver();
            webDriver.Url = "http://pokezz.com";
            webDriver.Navigate();
            var elements = webDriver.FindElements(By.ClassName("collection-item"));
            foreach (var element in elements)
            {
                var name = element.FindElement(By.ClassName("avatar-text")).Text;
                var location = element.FindElement(By.ClassName("title")).Text;
                newSniperInfos.Add(new SniperInfo() {
                    PokemonId = (PokemonId)Enum.Parse(typeof(PokemonId), name),
                    Latitude = Convert.ToDouble(location.Split(',')[0].Trim(), CultureInfo.InvariantCulture),
                    Longitude = Convert.ToDouble(location.Split(',')[1].Trim(), CultureInfo.InvariantCulture),
                });
            }

            return newSniperInfos;
        }
    }
}
