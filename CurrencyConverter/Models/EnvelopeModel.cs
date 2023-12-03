using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyConverter.Models
{
    /// <summary>
    /// Represents the XML envelope structure for currency exchange rate data.
    /// </summary>
    [XmlRoot("Envelope", Namespace = GesmesNameSpace)]
    public class EnvelopeModel
    {
        public const string GesmesNameSpace = "http://www.gesmes.org/xml/2002-08-01";
        public const string EcbNameSpace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

        [XmlElement("Sender", Namespace = GesmesNameSpace)]
        public SenderModel Sender { get; set; }
        [XmlElement("subject", Namespace = GesmesNameSpace)]
        public string Subject { get; set; }
        [XmlArray("Cube", Namespace = EcbNameSpace)]
        [XmlArrayItem("Cube")]
        public List<CubeModel> Cube { get; set; }
    }

    /// <summary>
    /// Represents the sender information in the XML envelope.
    /// </summary>
    [XmlRoot(ElementName = "Sender")]
    public class SenderModel
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
    [XmlRoot(ElementName = "Cube")]

    /// <summary>
    /// Represents a time-stamped cube in the XML envelope, containing currency exchange rate data.
    /// </summary>
    public class CubeModel
    {
        [XmlAttribute("time")]
        public DateTime Time { get; set; }
        [XmlElement("Cube")]
        public List<CubeItemModel> Cubes { get; set; }
    }

    /// <summary>
    /// Represents an individual currency exchange rate within a cube.
    /// </summary>
    public class CubeItemModel
    {
        [XmlAttribute("rate")]
        public decimal Rate { get; set; }
        [XmlAttribute("currency")]
        public string Currency { get; set; }
    }

}
