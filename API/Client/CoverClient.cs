using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MyGameList.API.Client
{
    class CoverClient : Client, IClient<Cover>
    {
        public CoverClient(HttpClient client)
        {
            this.client = client;
        }
        public async Task<Cover[]> GetAsync(object item)
        {
            return await Task.Run(() =>
            {
                var content = new StringContent("fields *;\nwhere id = " + item + ";", Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://api.igdb.com/v4/covers", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    var jsonStringResult = jsonTask.Result;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Cover[] covers = js.Deserialize<Cover[]>(jsonStringResult);
                    return covers;
                }
                else
                {
                    Cover[] covers = new Cover[0];
                    return covers;
                }
            });
        }
    }
}
