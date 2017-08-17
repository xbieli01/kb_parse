using KbParser;
using KbParser.Dto;
using KbStorage;
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
            //var client = new WebClient();
            //client.Encoding = Encoding.UTF8;

            //var date = DateTime.Now;

            //for (int i = 1; i < 30; i++)
            //{
            //    var data = client.DownloadStringTaskAsync(string.Format("https://lc.kbs.sk/?mesiac={0}&format=xml", date.ToString("yyyyMM"))).GetAwaiter().GetResult();

            //    LcKbsDto mapped = CoreParser.Instance.MapStringData(data).GetAwaiter().GetResult();

            //    XmlSerializer ser = new XmlSerializer(typeof(List<LcDayDto>));
            //    string res = string.Empty;

            //    using (var writer = new StringWriter())
            //    {
            //        ser.Serialize(writer, mapped.Days.Values.ToList());
            //        //ser.Serialize(writer, mapped.Days.Values.Where(i => i.Celebrations.Items.Where(r => r.Groups.Items.Any()).Any()).ToList());
            //        res = writer.ToString();
            //    }

            //    Console.WriteLine(res);

            //    date = date.AddMonths(1);
            //}


            CoreStorage.Instance.Load();

            // test aktualnosti
            if(!CoreStorage.Instance.Data.Days.Any() || CoreStorage.Instance.Data.Days.Keys.Max() <= DateTime.Now.AddMonths(1))
            {
                if(!CoreStorage.Instance.Actualize().GetAwaiter().GetResult())
                {
                    Console.WriteLine("chyba aktualizacie");
                }
            }

            CoreStorage.Instance.Load();

            while (true)
            {
                var dateInp = Console.ReadLine();
                DateTime dateParse;
                var date = DateTime.TryParseExact(dateInp, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None, out dateParse) ? dateParse : DateTime.Now;

                XmlSerializer ser = new XmlSerializer(typeof(LcDayDto));
                var res = string.Empty;

                using (var writer = new StringWriter())
                {
                    ser.Serialize(writer, CoreStorage.Instance.GetData(date).GetAwaiter().GetResult());
                    res = writer.ToString();
                }

                Console.WriteLine(res);
            }
        }
    }
}
