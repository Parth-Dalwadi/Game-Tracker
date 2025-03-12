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
    class GameClient : Client, IClient<Game>
    {
        public GameClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Game[]> GetAsync(object item)
        {
            return await Task.Run(() =>
            {
                var content = new StringContent("fields *;\nwhere name = \"" + item + "\";", Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://api.igdb.com/v4/games", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    Trace.WriteLine("Success");
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    var jsonStringResult = jsonTask.Result;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Game[] games = js.Deserialize<Game[]>(jsonStringResult);
                    return games;
                }
                else
                {
                    Game[] games = new Game[0];
                    return games;
                }
            });
        }
    }
}
