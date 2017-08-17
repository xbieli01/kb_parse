using KbParser.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KbStorage.Data
{
    public class Storer
    {
        [XmlAttribute]
        public DateTime Key { get; set; }
        [XmlAttribute]
        public string LcDay { get; set; }
    }
}
