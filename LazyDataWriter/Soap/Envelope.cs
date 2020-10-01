using System.Xml.Serialization;

namespace LazyDataWriter.Soap
{
    [XmlRoot(ElementName = RootElementName, Namespace = SoapEnvelopeNs)]
    public class Envelope<T>
    {
        #region Public Fields

        [XmlIgnore]
        public const string RootElementName = "Envelope";

        [XmlIgnore]
        public const string SoapEnvelopeNs = "http://schemas.xmlsoap.org/soap/envelope/";

        #endregion Public Fields

        #region Public Properties

        [XmlElement("Body", Namespace = SoapEnvelopeNs, Order = 2)]
        public Body<T> Body { get; set; }

        [XmlElement("Header", Namespace = SoapEnvelopeNs, Order = 1)]
        public Header Header { get; set; } = new Header();

        #endregion Public Properties
    }
}