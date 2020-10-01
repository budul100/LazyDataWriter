using LazyDataWriter.Extensions;
using LazyDataWriter.Soap;
using System.Xml.Serialization;

namespace LazyDataWriter
{
    public class SoapWriter<T>
        : XmlWriter<T>
    {
        #region Private Fields

        private const string SoapEnvelopeNsPrefix = "soap";

        #endregion Private Fields

        #region Public Constructors

        public SoapWriter(bool withoutXmlHeader = false)
        {
            WithoutXmlHeader = withoutXmlHeader;

            CreateSerializer<Envelope<T>>(
                rootElement: Envelope<T>.RootElementName,
                rootNamespace: Envelope<T>.SoapEnvelopeNs);

            Namespaces.Add(
                prefix: SoapEnvelopeNsPrefix,
                ns: Envelope<T>.SoapEnvelopeNs);
        }

        #endregion Public Constructors

        #region Public Methods

        public override string Write(T content)
        {
            var soap = content.GetSoap();

            var result = GetString(soap);

            return result;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override XmlAttributeOverrides GetOverrides(XmlRootAttribute root)
        {
            var result = new XmlAttributeOverrides();

            var rootAttributes = new XmlAttributes
            {
                XmlRoot = root,
            };

            result.Add(
                type: typeof(Envelope<T>),
                attributes: rootAttributes);

            var bodyAttributes = GetBodyAttributes();

            result.Add(
                type: typeof(Body<T>),
                member: nameof(Body<T>.Content),
                attributes: bodyAttributes);

            return result;
        }

        #endregion Protected Methods

        #region Private Methods

        private XmlAttributes GetBodyAttributes()
        {
            var ns = typeof(T).GetXmlNamespace();

            AddNamespace(ns);

            var elementAttribute = new XmlElementAttribute
            {
                ElementName = typeof(T).GetXmlRootElement(),
                Namespace = typeof(T).GetXmlNamespace()
            };

            var result = new XmlAttributes
            {
                XmlElements = { elementAttribute },
            };

            return result;
        }

        #endregion Private Methods
    }
}