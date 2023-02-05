using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace NetworkDeviceMonitor.Lib.Model
{
    [XmlRoot(ElementName = "dev")]
    public class StDevResponseRoot
    {
        [XmlElement(ElementName = "batt_per")]
        public int BatteryPercentage { get; set; }
        [XmlElement(ElementName = "batt_st")]
        public int Batt_st { get; set; }
        [XmlElement(ElementName = "dsc")]
        public string Dsc { get; set; }
        [XmlElement(ElementName = "freq")]
        public string Freq { get; set; }
        [XmlElement(ElementName = "hwver")]
        public string Hwver { get; set; }
        [XmlElement(ElementName = "iccid")]
        public string Iccid { get; set; }
        [XmlElement(ElementName = "imei")]
        public string Imei { get; set; }
        [XmlElement(ElementName = "imsi")]
        public string Imsi { get; set; }
        [XmlElement(ElementName = "macaddr")]
        public string Macaddr { get; set; }
        [XmlElement(ElementName = "make")]
        public string Make { get; set; }
        [XmlElement(ElementName = "model")]
        public string Model { get; set; }
        [XmlElement(ElementName = "msisdn")]
        public string Msisdn { get; set; }
        [XmlElement(ElementName = "odm")]
        public string Odm { get; set; }
        [XmlElement(ElementName = "oui")]
        public string Oui { get; set; }
        [XmlElement(ElementName = "refresh")]
        public string Refresh { get; set; }
        [XmlElement(ElementName = "serial")]
        public string Serial { get; set; }
        [XmlElement(ElementName = "swdate")]
        public string Swdate { get; set; }
        [XmlElement(ElementName = "swver")]
        public string Swver { get; set; }
        [XmlElement(ElementName = "time")]
        public string Time { get; set; }
        [XmlElement(ElementName = "uiver")]
        public string Uiver { get; set; }
    }

}
