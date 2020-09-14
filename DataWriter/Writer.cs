using DataWriter.Writers;
using System.Xml;
using System.Xml.Serialization;

namespace DataWriter
{
    public class Writer<T>
    {
        #region Private Fields

        private readonly XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
        private readonly XmlAttributeOverrides overrides;
        private readonly XmlRootAttribute root;
        private readonly string rootNamespace;

        #endregion Private Fields

        #region Public Constructors

        public Writer(string rootElement, string rootNamespace)
        {
            this.rootNamespace = rootNamespace;

            root = GetRootAttribute(rootElement);
            overrides = GetOverrides(root);
        }

        public Writer()
            : this(default, default)
        { }

        #endregion Public Constructors

        #region Public Methods

        public void AddNamespace(string prefix, string ns)
        {
            namespaces.Add(
                prefix: prefix,
                ns: ns);
        }

        public string Get(T content, bool withoutXmlHeader = false)
        {
            var result = GetAsString(
                content: content,
                withoutXmlHeader: withoutXmlHeader);

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private static XmlAttributeOverrides GetOverrides(XmlRootAttribute root)
        {
            var result = new XmlAttributeOverrides();

            if (root != default)
            {
                var attributes = new XmlAttributes
                {
                    XmlRoot = root
                };

                result = new XmlAttributeOverrides();

                result.Add(
                    type: typeof(T),
                    attributes: attributes);
            }

            return result;
        }

        private string GetAsString(T content, bool withoutXmlHeader = false)
        {
            var result = default(string);

            var xmlSerializer = GetSerializer();

            using (var textWriter = new UTF8Writer())
            {
                if (withoutXmlHeader)
                {
                    using (var fragementWriter = new XmlFragmentWriter(textWriter))
                    {
                        fragementWriter.Formatting = Formatting.Indented;

                        xmlSerializer.Serialize(
                            xmlWriter: fragementWriter,
                            o: content,
                            namespaces: namespaces);
                    }
                }
                else
                {
                    xmlSerializer.Serialize(
                        textWriter: textWriter,
                        o: content,
                        namespaces: namespaces);
                }

                result = textWriter.ToString();
            }

            return result;
        }

        private XmlRootAttribute GetRootAttribute(string rootElement)
        {
            var result = default(XmlRootAttribute);

            if (!string.IsNullOrWhiteSpace(rootNamespace))
            {
                result = new XmlRootAttribute();

                AddNamespace(
                    prefix: string.Empty,
                    ns: rootNamespace);

                result.Namespace = rootNamespace;
            }

            if (!string.IsNullOrWhiteSpace(rootElement))
            {
                if (result == null)
                    result = new XmlRootAttribute();
                result.ElementName = rootElement;
            }

            return result;
        }

        private XmlSerializer GetSerializer()
        {
            var result = overrides != default
                ? new XmlSerializer(
                    type: typeof(T),
                    overrides: overrides,
                    extraTypes: null,
                    root: root,
                    defaultNamespace: rootNamespace)
                : new XmlSerializer(typeof(T));

            return result;
        }

        #endregion Private Methods
    }
}