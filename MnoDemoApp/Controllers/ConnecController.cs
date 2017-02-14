using Maestrano;
using Maestrano.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MnoDemoApp.Controllers
{
    public class ConnecController : Controller
    {
        // GET: Connec
        public ActionResult Index()
        {
            // Check session is still valid (only if user logged in)
            var session = System.Web.HttpContext.Current.Session;
            if (session["loggedIn"] != null)
            {
                string marketplace = (String)session["marketplace"];
                var preset = MnoHelper.With(marketplace);
                var mnoSession = new Maestrano.Sso.Session(preset, session);
                if (!mnoSession.IsValid())
                {
                    Response.Redirect(preset.Sso.InitUrl());
                }
                string groupId = (String)session["groupId"];
                var client = preset.ConnecClient(groupId);
                var itemsResponse = client.Get<ItemsResult>("/items");
                ViewBag.Items = itemsResponse.Data.Items;
                var response = client.Get("/company");
                var parsedJson = JsonConvert.DeserializeObject(response.Content);
                var formattedJson = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                ViewBag.RawConnecResponse = formattedJson;
            }

            return View();
        }
    }
    public class ItemsResult
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }


    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("sale_price")]
        public Price SalePrice { get; set; }

        [JsonProperty("purchase_price")]
        public Price PurchasePrice { get; set; }

    }
    public class Price
    {
        [JsonProperty("total_amount")]
        public string TotalAmount { get; set; }

        [JsonProperty("net_amount")]
        public DateTime? NetAmount { get; set; }

        [JsonProperty("tax_amount")]
        public DateTime? TaxAmount { get; set; }

        [JsonProperty("tax_rate")]
        public String TaxRate { get; set; }

        [JsonProperty("currency")]
        public String Currency { get; set; }

    }

}