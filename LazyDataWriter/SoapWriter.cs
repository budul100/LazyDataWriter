using LazyDataWriter.Extensions;
using LazyDataWriter.Soap;

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
                rootNamespace: Envelope<T>.SoapEnvelopeNs,
                overridesGetter: r => r.GetOverridesSoap<T>());

            Namespaces.Add(
                prefix: SoapEnvelopeNsPrefix,
                ns: Envelope<T>.SoapEnvelopeNs);
        }

        #endregion Public Constructors

        #region Public Methods

        public override string Write(T content)
        {
            var soap = content.GetSoap<T>();

            var result = GetString(soap);

            return result;
        }

        #endregion Public Methods
    }
}