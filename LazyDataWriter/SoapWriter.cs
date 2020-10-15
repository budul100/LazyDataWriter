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

            AddNamespace(
                ns: Envelope<T>.SoapEnvelopeNs,
                prefix: SoapEnvelopeNsPrefix);
        }

        #endregion Public Constructors

        #region Public Methods

        public override string Write(T content)
        {
            CreateSerializer<Envelope<T>>(
                rootElement: Envelope<T>.RootElementName,
                rootNamespace: Envelope<T>.SoapEnvelopeNs);

            var soap = content.GetSoap();

            var result = GetString(soap);

            return result;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void SetOverrides(XmlRootAttribute root)
        {
            var rootAttributes = new XmlAttributes
            {
                XmlRoot = root,
            };

            overrides.Add(
                type: typeof(Envelope<T>),
                attributes: rootAttributes);

            SetOverrides(typeof(Envelope<T>));
        }

        #endregion Protected Methods
    }
}