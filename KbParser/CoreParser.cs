using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KbParser.Dto;
using System.Threading.Tasks;
using System.Xml.Linq;
using KbParser.XmlHelpers;
using System.Text.RegularExpressions;

namespace KbParser
{
    public sealed class CoreParser
    {
        private static volatile CoreParser instance;
        private static object syncRoot = new Object();

        private CoreParser() { }

        public static CoreParser Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CoreParser();
                    }
                }

                return instance;
            }
        }

        public async Task<LcKbsDto> MapStringData(string data)
        {
            data = CleanData(data);
            var doc = XDocument.Parse(data);

            var result = MapLc(doc.Element("lc"));

            return await Task.FromResult<LcKbsDto>(result);
        }

        private string CleanData(string data)
        {
            return Regex.Replace(data, "&.*?;", string.Empty);
        }

        #region Mappers

        public static Func<XElement, LcKbsDto> MapLc
        {
            get
            {
                return elem => new LcKbsDto
                {
                    Days = elem.Descendants("day").Select(MapDay).ToDictionary(i => i.Date)
            };
            }
        }



        public static Func<XElement, LcDayDto> MapDay
        {
            get
            {
                return elem => new LcDayDto
                {
                    Date = elem.AttributeValueToDateTime("date"),
                    LiturgicalYear = elem.AttributeValue("liturgical_year"),
                    Week = elem.AttributeValueToInt("week"),
                    Weekday = elem.AttributeValue("weekday"),
                    NameDay = elem.AttributeValue("name_day"),
                    HolyObligation = elem.AttributeValue("holy_obligation"),
                    Celebrations = elem.Descendants("celebration").Select(MapCelebration).ToEnumerableDto()
                };
            }
        }

        public static Func<XElement, LcDayCelebrationDto> MapCelebration
        {
            get
            {
                return elem =>
                {
                    var ldc = new LcDayCelebrationDto
                    {
                        Memo = elem.ElementValue("memo"),
                        Directory = elem.AttributeValue("directory"),
                        Heads = elem.Descendants("head").Select(MapHead).ToEnumerableDto(),
                        Groups = elem.Descendants("group").Select(MapGroup).ToEnumerableDto(),
                    };

                    // nie vsetky citania su v skupinach
                    if(ldc.Groups.IsEmpty)
                    {
                        ldc.Groups = new EnumerableDto<LcDayCelebrationGroupDto>
                        {
                            Items = new List<LcDayCelebrationGroupDto>
                            {
                               new LcDayCelebrationGroupDto
                               {
                                   Readings = elem.Descendants("reading").Select(MapReading).ToEnumerableDto(),
                                   Psalms = elem.Descendants("psalm").Select(MapPsalm).ToEnumerableDto(),
                                   Gospels = elem.Descendants("gospel").Select(MapGospel).ToEnumerableDto()
                               }
                            }
                        };
                    }

                    return ldc;
                };
            }
        }

        public static Func<XElement, LcDayCelebrationHeadDto> MapHead
        {
            get
            {
                return elem => new LcDayCelebrationHeadDto
                {
                    Title = elem.ElementValue("title"),
                    LiturgicalColor = elem.Element("title") != null ? elem.Element("title").AttributeValue("liturgical_color") : string.Empty,
                    Solemnity = elem.Element("title") != null ? elem.Element("title").AttributeValue("solemnity") : string.Empty
                };
            }
        }

        public static Func<XElement, LcDayCelebrationGroupDto> MapGroup
        {
            get
            {
                return elem => new LcDayCelebrationGroupDto
                {
                    GroupTitle = elem.ElementValue("group_title"),
                    Readings = elem.Descendants("reading").Select(MapReading).ToEnumerableDto(),
                    Psalms = elem.Descendants("psalm").Select(MapPsalm).ToEnumerableDto(),
                    Gospels = elem.Descendants("gospel").Select(MapGospel).ToEnumerableDto()
                };
            }
        }

        public static Func<XElement, LcDayCelebrationGroupReadingDto> MapReading
        {
            get
            {
                return elem => new LcDayCelebrationGroupReadingDto
                {
                    Book = elem.ElementValue("book"),
                    Coordinate = elem.ElementValue("coordinate"),
                    Text = elem.Element("text") != null ? MapText(elem.Element("text")) : new TextDto()
                };
            }
        }

        public static Func<XElement, LcDayCelebrationGroupPsalmDto> MapPsalm
        {
            get
            {
                return elem => new LcDayCelebrationGroupPsalmDto
                {
                    Book = elem.ElementValue("book"),
                    Coordinate = elem.ElementValue("coordinate"),
                    Text = elem.ElementValue("text")
                };
            }
        }

        public static Func<XElement, LcDayCelebrationGroupGospelDto> MapGospel
        {
            get
            {
                return elem => new LcDayCelebrationGroupGospelDto
                {
                    Book = elem.ElementValue("book"),
                    Coordinate = elem.ElementValue("coordinate"),
                    Text = elem.Element("text") != null ? MapText(elem.Element("text")) : new TextDto()
                };
            }
        }

        public static Func<XElement, TextDto> MapText
        {
            get
            {
                return elem => new TextDto
                {
                    Title = elem.ElementValue("title"),
                    Value = elem.Value
                };
            }
        }

        #endregion
    }
}
