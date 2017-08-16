﻿using KbParser;
using KbParser.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;

            var date = DateTime.Now;

            for (int i = 1; i < 30; i++)
            {
                var data = client.DownloadStringTaskAsync(string.Format("https://lc.kbs.sk/?mesiac={0}&format=xml", date.ToString("yyyyMM"))).GetAwaiter().GetResult();
                
                LcKbsDto mapped = CoreParser.Instance.MapStringData(data).GetAwaiter().GetResult();

                XmlSerializer ser = new XmlSerializer(typeof(List<LcDayDto>));
                string res = string.Empty;

                using (var writer = new StringWriter())
                {
                    ser.Serialize(writer, mapped.Days.Values.ToList());
                    //ser.Serialize(writer, mapped.Days.Values.Where(i => i.Celebrations.Items.Where(r => r.Groups.Items.Any()).Any()).ToList());
                    res = writer.ToString();
                }

                Console.WriteLine(res);

                date = date.AddMonths(1);
            }

            
        }
    }
}
