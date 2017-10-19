using EWiki.Api.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using EWiki.Api.Utility;
using HtmlAgilityPack;

namespace EWiki.Api.DataAccess
{
    public class FeedInfoRepository : RepositoryBase<FeedInfo>, IFeedInfoRepository
    {
        public FeedInfoRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        public void ExecuteFeeder()
        {
            //IWebDriver webDriver = new ChromeDriver();
            //webDriver.Url = "http://pokezz.com";
            //webDriver.Navigate();
            //var elements = webDriver.FindElements(By.ClassName("collection-item"));
            //foreach (var element in elements)
            //{
            //    var name = element.FindElement(By.ClassName("avatar-text")).Text;
            //    var location = element.FindElement(By.ClassName("title")).Text;
            //    newSniperInfos.Add(new SniperInfo()
            //    {
            //        PokemonId = (PokemonId)Enum.Parse(typeof(PokemonId), name),
            //        Latitude = Convert.ToDouble(location.Split(',')[0].Trim(), CultureInfo.InvariantCulture),
            //        Longitude = Convert.ToDouble(location.Split(',')[1].Trim(), CultureInfo.InvariantCulture),
            //    });
            //}

            var url = "http://pokezz.com/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var elements = doc.DocumentNode.SelectNodes("//div[contains(@class, 'collection-item')]");
            foreach (var element in elements)
            {
                var name = element.SelectSingleNode("//div[contains(@class, 'avatar-text')]").InnerText;
                var location = element.SelectSingleNode("//span[contains(@class, 'title')]").InnerText;
                //newSniperInfos.Add(new SniperInfo()
                //{
                //    PokemonId = (PokemonId)Enum.Parse(typeof(PokemonId), name),
                //    Latitude = Convert.ToDouble(location.Split(',')[0].Trim(), CultureInfo.InvariantCulture),
                //    Longitude = Convert.ToDouble(location.Split(',')[1].Trim(), CultureInfo.InvariantCulture),
                //});
            }
        }
    }
}
