using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KbParser.XmlHelpers
{
    public static class XmlExtension
    {
        /// <summary>
        /// funkcia ziska hodnotu atributu
        /// </summary>
        /// <param name="elem">element s atributmi</param>
        /// <param name="attrName">nazov atributu</param>
        /// <returns>vratena hodnota</returns>
        public static DateTime AttributeValueToDateTime(this XElement elem, string attrName)
        {
            DateTime outParse;
            string[] format = { "yyyyMMdd" };
            return elem.Attribute(attrName) != null ? DateTime.TryParseExact((string)elem.Attribute(attrName), format, System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None, out outParse) ? outParse : default(DateTime) : default(DateTime);
        }

        /// <summary>
        /// funkcia ziska hodnotu atributu
        /// </summary>
        /// <param name="elem">element s atributmi</param>
        /// <param name="attrName">nazov atributu</param>
        /// <returns>vratena hodnota</returns>
        public static int AttributeValueToInt(this XElement elem, string attrName)
        {
            int outParse;
            return elem.Attribute(attrName) != null ? int.TryParse((string)elem.Attribute(attrName), out outParse) ? outParse : default(int) : default(int);
        }

        /// <summary>
        /// funkcia ziska hodnotu atributu
        /// </summary>
        /// <param name="elem">element s atributmi</param>
        /// <param name="attrName">nazov atributu</param>
        /// <returns>vratena hodnota</returns>
        public static string AttributeValue(this XElement elem, string attrName)
        {
            return elem.Attribute(attrName) != null ? (string)elem.Attribute(attrName) : string.Empty;
        }

        /// <summary>
        /// funkcia ziska hodnotu elemntu
        /// </summary>
        /// <param name="elem">element</param>
        /// <param name="elemName">nazov elementu</param>
        /// <returns>vratena hodnota</returns>
        public static string ElementValue(this XElement elem, string elemName)
        {
            return elem.Element(elemName) != null ? elem.Element(elemName).Value : string.Empty;
        }
    }
}
