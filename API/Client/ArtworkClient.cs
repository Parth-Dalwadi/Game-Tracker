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
    class ArtworkClient : Client, IClient<Artwork>
    {
        public ArtworkClient(HttpClient client)
        {
            this.client = client;
        }
        public async Task<Artwork[]> GetAsync(object item)
        {
            return await Task.Run(() =>
            {
                var content = new StringContent("fields *;\nwhere id = " + item + ";", Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://api.igdb.com/v4/artworks", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    var jsonStringResult = jsonTask.Result;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Artwork[] images = js.Deserialize<Artwork[]>(jsonStringResult);
                    return images;
                }
                else
                {
                    Artwork[] images = new Artwork[0];
                    return images;
                }
            });
        }
    }
}
