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
using MyGameList.API;
using MyGameList.API.Client;
using Artwork = MyGameList.API.Artwork;


namespace MyGameList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private GameClient gameClient;
        private ArtworkClient imageClient;
        private CoverClient coverClient;

        public MainWindow()
        {
            InitializeComponent();
            string[] contents = File.ReadAllLines("..\\..\\secrets.txt");
            client.DefaultRequestHeaders.Add("Client-ID", contents[0].Split('=')[1]);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + contents[1].Split('=')[1]);
            gameClient = new GameClient(client);
            imageClient = new ArtworkClient(client);
            coverClient = new CoverClient(client);
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
            Game[] games = await gameClient.GetAsync("Xenoblade Chronicles 3");
            if (games != null && games.Length != 0)
            {
                Trace.WriteLine(games[0].id);
                Trace.WriteLine(games[0].name);
                Cover[] images = await coverClient.GetAsync(games[0].id);
                if (images != null && images.Length != 0)
                {
                    Trace.WriteLine(images[0].id);
                    Trace.WriteLine(images[0].url);
                } else
                {
                    Trace.WriteLine("Images is empty");
                }
            } else
            {
                Trace.WriteLine("Games is empty");
            }
        }
    }
}
