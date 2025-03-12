using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MyGameList.API.Client
{
    class Client
    {
        protected HttpClient client = new HttpClient();

        public async Task<Item[]> Get(int id)
        {
            return await Task.Run(() =>
            {
                var content = new StringContent("fields *;\nwhere id = " + id + ";", Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://api.igdb.com/v4/artworks", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    Trace.WriteLine("Success");
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    var jsonStringResult = jsonTask.Result;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Item[] items = js.Deserialize<Game[]>(jsonStringResult);
                    return items;
                }
                else
                {
                    Item[] items = new Item[0];
                    return items;
                }
            });
        }
    }
}
