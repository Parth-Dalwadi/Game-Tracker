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
    class ImageClient : Client
    {
        public ImageClient(HttpClient client)
        {
            this.client = client;
        }
        public async Task<Image[]> Get_Images(int id)
        {
            return await Task.Run(() =>
            {
                var content = new StringContent("fields *;\nwhere id = " + id + ";", Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://api.igdb.com/v4/artworks", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    var jsonStringResult = jsonTask.Result;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Image[] images = js.Deserialize<Image[]>(jsonStringResult);
                    return images;
                }
                else
                {
                    Image[] images = new Image[0];
                    return images;
                }
            });
        }
    }
}
