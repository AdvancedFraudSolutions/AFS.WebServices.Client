using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AFS.WebServices.Client
{
    public static class ResponseXmlParser
    {
        public static DateTimeOffset ParseDateTimeOffset(XElement el)
        {
            var ns = XNamespace.Get("http://schemas.datacontract.org/2004/07/System");
            var strOffsetMinutes = el.Element(ns + "OffsetMinutes").Value;
            var offsetMins = strOffsetMinutes.Parse(Convert.ToInt32);
            var offset = TimeSpan.FromMinutes(offsetMins);
            var strDateTime = el.Element(ns + "DateTime").Value;
            var dto = strDateTime.Parse(DateTimeOffset.Parse).ToOffset(offset);
            return dto;
        }

        public static KeyValuePair<string, int> ParseKeyValueOfStringInt(XElement el)
        {
            var ns = XNamespace.Get("http://schemas.microsoft.com/2003/10/Serialization/Arrays");

            var key = el.Element(ns + "Key").Value;
            var value = el.Element(ns + "Value").Value.Parse(Convert.ToInt32);

            return new KeyValuePair<string, int>(key, value);
        }

        public static IEnumerable<KeyValuePair<string, int>> ParseManyKeyValueOfStringInts(XElement el)
        {
            var ns = XNamespace.Get("http://schemas.microsoft.com/2003/10/Serialization/Arrays");
            return el.Elements(ns + "KeyValueOfstringint").Select(ParseKeyValueOfStringInt);
        }
    }
}