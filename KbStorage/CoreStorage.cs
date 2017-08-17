using KbParser;
using KbParser.Dto;
using KbStorage.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KbStorage
{
    public sealed class CoreStorage
    {
        private static volatile CoreStorage instance;
        private static object syncRoot = new Object();

        private const int monthRange = 3;

        public LcKbsDto Data { get; set; }
        public string LinkUrl { get; set; }

        private CoreStorage()
        {
            Data = new LcKbsDto();
            Data.Days = new Dictionary<DateTime, LcDayDto>();
            LinkUrl = "https://lc.kbs.sk";
        }

        public static CoreStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CoreStorage();
                    }
                }

                return instance;
            }
        }

        public async Task<LcDayDto> GetData(DateTime date)
        {
            var result = new LcDayDto();

            if (Data.Days.ContainsKey(date))
            {
                result = Data.Days[date];
            }
            else
            {
                try
                {
                    result = await DownloadDateExact(date);
                    Data.Days.Add(date, result);
                }
                catch
                {
                    return null;
                }
            }

            return result;
        }

        private async Task<LcDayDto> DownloadDateExact(DateTime date)
        {
            //TODO zmenit na povodne
            var webClient = new HttpClient();
            var url = new Uri(string.Format("{0}/?den={1}&format=xml", LinkUrl, date.ToString("yyyyMMdd")));
            
            var text = await webClient.GetStringAsync(url);

            var allData = await CoreParser.Instance.MapStringData(text);

            var day = allData != null && allData.Days != null && allData.Days.Any() ? allData.Days.FirstOrDefault().Value : null;

            return day;
        }

        private async Task<LcKbsDto> DownloadDateRange(DateTime dateFrom, DateTime dateTo)
        {
            //TODO zmenit na povodne
            var webClient = new HttpClient();
            var allData = new LcKbsDto();
            var date = dateFrom;

            while(date.Year <= dateTo.Year && date.Month <= dateTo.Month)
            {
                var url = new Uri(string.Format("{0}/?mesiac={1}&format=xml", LinkUrl, date.ToString("yyyyMM")));
                var text = await webClient.GetStringAsync(url);
                var tempData = await CoreParser.Instance.MapStringData(text);
                foreach(var item in tempData.Days)
                {
                    allData.Days.Add(item.Key, item.Value);
                }
                date = date.AddMonths(1);
            }

            return allData;
        }

        public void Load()
        {
            //TODO zmenit na povodne
            if (Application.Current.Properties.ContainsKey("DataStorage"))
            {
                var serializedStorage = Application.Current.Properties["DataStorage"] as string;
                if(string.IsNullOrEmpty(serializedStorage))
                {
                    return;
                }

                var serializer = new XmlSerializer(typeof(Storer[]),
                                 new XmlRootAttribute() { ElementName = "items" });
                using (var reader = new StringReader(serializedStorage))
                {
                    var ser = new XmlSerializer(typeof(LcDayDto));

                    Data = new LcKbsDto();
                    Data.Days = (serializer.Deserialize(reader) as Storer[]).ToDictionary(i => i.Key, i => {
                        var res = new LcDayDto();
                        using (var readerStorer = new StringReader(i.LcDay))
                        {
                            res = ser.Deserialize(readerStorer) as LcDayDto;
                        }
                        return res; });
                }
            }
            else
            {
                Data = new LcKbsDto();
            }
        }

        public void Save()
        {
            //TODO zmenit na povodne
            var serializer = new XmlSerializer(typeof(Storer[]),
                                 new XmlRootAttribute() { ElementName = "items" });
            var result = string.Empty;
            using (var writer = new StringWriter())
            {
                var ser = new XmlSerializer(typeof(LcDayDto));

                serializer.Serialize(writer,
                    Data.Days.Select(i => {
                    var res = string.Empty;
                    using (var writerStorer = new StringWriter())
                    {
                        ser.Serialize(writerStorer, i.Value);
                        res = writerStorer.ToString();
                    }

                    return new Storer { Key = i.Key, LcDay = res }; }).ToArray());

                result = writer.ToString();
            }

            Application.Current.Properties["DataStorage"] = result;
        }

        public async Task<bool> Actualize()
        {
            try
            {
                Load();
                Data = await DownloadDateRange(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(monthRange));
                Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
