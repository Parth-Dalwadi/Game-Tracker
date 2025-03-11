using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MyGameList.API
{
    class Game
    {
        public int id { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
    }
}
