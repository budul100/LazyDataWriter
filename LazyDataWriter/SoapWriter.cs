using LazyDataWriter.Extensions;
using LazyDataWriter.Soap;
using System.Linq;
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

        protected override void SetRoot(XmlRootAttribute root)
        {
            var rootAttributes = new XmlAttributes
            {
                XmlRoot = root,
            };

            Overrides.Add(
                type: typeof(Envelope<T>),
                attributes: rootAttributes);

            SetSoapOverrides();
            SetContentOverrides();
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetContentOverrides()
        {
            var type = typeof(Body<T>);

            var properties = type.GetProperties()
                .Where(m => !m.PropertyType.IsValueType)
                .Where(m => m.PropertyType != typeof(string)).ToArray();

            foreach (var property in properties)
            {
                var attributes = property.PropertyType
                    .GetAttributes(property.PropertyType == typeof(Body<T>));

                Overrides.Add(
                    type: type,
                    member: property.Name,
                    attributes: attributes);
            }
        }

        private void SetSoapOverrides()
        {
            var type = typeof(Envelope<T>);

            var properties = type.GetProperties()
                .Where(m => !m.PropertyType.IsValueType)
                .Where(m => m.PropertyType != typeof(string)).ToArray();

            foreach (var property in properties)
            {
                var attributes = property.GetAttributes();

                Overrides.Add(
                    type: type,
                    member: property.Name,
                    attributes: attributes);
            }
        }

        #endregion Private Methods
    }
}