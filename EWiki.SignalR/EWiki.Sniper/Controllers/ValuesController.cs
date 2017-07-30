using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EWiki.Sniper.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Url = "http://pokezz.com";
            webDriver.Navigate();
            var str = new List<string>();
            var elements = webDriver.FindElements(By.ClassName("collection-item"));
            foreach(var element in elements)
            {
                var name = element.FindElement(By.ClassName("avatar-text")).Text;
                var location = element.FindElement(By.ClassName("title")).Text;
                str.Add($"{name} - {location}");
            }
            
            return str;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
