using KbParser;
using KbParser.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async void Main(string[] args)
        {
            var client = new WebClient();
            var data = await client.DownloadStringTaskAsync("https://lc.kbs.sk/?mesiac=201708&format=xml");

            LcKbsDto mapped = await CoreParser.Instance.MapStringData(data);
        }
    }
}
