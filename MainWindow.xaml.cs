using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Web.Script.Serialization;
using System.IO;


namespace MyGameList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public class Games
        {
            public int id { get; set; }
            public string name { get; set; }
            public string summary { get; set; }
        }

        public class Images
        {
            public int id { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public string url { get; set; }

        }

        public MainWindow()
        {
            InitializeComponent();
            string[] contents = File.ReadAllLines("..\\..\\secrets.txt");
            client.DefaultRequestHeaders.Add("Client-ID", contents[0].Split('=')[1]);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + contents[1].Split('=')[1]);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = new StringContent("fields *;\nwhere name = \"Super Mario 64\";", Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.igdb.com/v4/games", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine("Success");
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                var jsonStringResult = jsonTask.Result;
                JavaScriptSerializer js = new JavaScriptSerializer();
                Games[] games = js.Deserialize<Games[]>(jsonStringResult);
                content = new StringContent("fields *;\nwhere id = " + games[0].id + ";", Encoding.UTF8, "application/json");
                response = client.PostAsync("https://api.igdb.com/v4/artworks", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    jsonStringResult = jsonTask.Result;
                    Images[] images = js.Deserialize<Images[]>(jsonStringResult);
                    Trace.WriteLine(images[0].url);
                } else
                {
                    Trace.WriteLine("Artwork fail");
                }
                Trace.WriteLine(games[0].name);
            } else
            {
                Trace.WriteLine("Fail");
            }
        }
    }
}
