using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace webApi.Controllers
{
    [Route("api/coinmarketcap/[controller]")]
    public class TickerController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<TickerModel[]> Get()
        {
            var response = await "https://api.coinmarketcap.com/v1/ticker/".GetAsync();
            var content = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TickerModel[]>(content);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<TickerModel> Get(int id)
        {
            var response = await $"https://api.coinmarketcap.com/v1/ticker/{id}".GetAsync();
            var content = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TickerModel>(content);
        }        
    }

    public class TickerModel
{
    public string id { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public string rank { get; set; }
    public string price_usd { get; set; }
    public string price_btc { get; set; }
    public string __invalid_name__24h_volume_usd { get; set; }
    public string market_cap_usd { get; set; }
    public string available_supply { get; set; }
    public string total_supply { get; set; }
    public string max_supply { get; set; }
    public string percent_change_1h { get; set; }
    public string percent_change_24h { get; set; }
    public string percent_change_7d { get; set; }
    public string last_updated { get; set; }
}
}
