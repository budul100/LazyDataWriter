using LazyDataWriter.Extensions;
using LazyDataWriter.Soap;
using LazyDataWriter.Writers;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace LazyDataWriter
{
    public class XmlWriter<T>
    {
        #region Private Fields

        private readonly XmlSerializerNamespaces allNamespaces = new XmlSerializerNamespaces();
        private readonly string rootElement;
        private readonly string rootNamespace;

        private string defaultNamespace;
        private XmlSerializerNamespaces namespaces;
        private XmlSerializer serializer;

        #endregion Private Fields

        #region Public Constructors

        public XmlWriter(string rootElement, string rootNamespace = default, bool withoutXmlHeader = false)
        {
            this.rootElement = rootElement;
            this.rootNamespace = rootNamespace;

            WithoutXmlHeader = withoutXmlHeader;
        }

        public XmlWriter(bool withoutXmlHeader = false)
            : this(default, default, withoutXmlHeader)
        { }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlWriter()
        { }

        #endregion Protected Constructors

        #region Protected Properties

        protected XmlAttributeOverrides overrides { get; } = new XmlAttributeOverrides();

        protected bool WithoutXmlHeader { get; set; }

        #endregion Protected Properties

        #region Public Methods

        public void AddNamespace(string ns, string prefix = default)
        {
            if (!string.IsNullOrWhiteSpace(ns))
            {
                allNamespaces.Add(
                    prefix: prefix ?? string.Empty,
                    ns: ns);
            }
        }

        public virtual string Write(T content)
        {
            CreateSerializer<T>(
                rootElement: rootElement,
                rootNamespace: rootNamespace);

            var result = GetString(content);

            return result;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void CreateSerializer<TSerialize>(string rootElement, string rootNamespace)
        {
            if (serializer == default)
            {
                var root = GetRootAttribute(
                    rootElement: rootElement,
                    rootNamespace: rootNamespace);

                SetOverrides(root);

                SetNamespaces();

                serializer = new XmlSerializer(
                    type: typeof(TSerialize),
                    overrides: overrides,
                    extraTypes: null,
                    root: root,
                    defaultNamespace: defaultNamespace);
            }
        }

        protected string GetString(object content)
        {
            var result = default(string);

            using (var textWriter = new UTF8Writer())
            {
                if (WithoutXmlHeader)
                {
                    using (var fragementWriter = new XmlFragmentWriter(textWriter))
                    {
                        fragementWriter.Formatting = Formatting.Indented;

                        serializer.Serialize(
                            o: content,
                            xmlWriter: fragementWriter,
                            namespaces: namespaces);
                    }
                }
                else
                {
                    serializer.Serialize(
                        o: content,
                        textWriter: textWriter,
                        namespaces: namespaces);
                }

                result = textWriter.ToString();
            }

            return result;
        }

        protected virtual void SetOverrides(XmlRootAttribute root)
        {
            var rootAttributes = new XmlAttributes
            {
                XmlRoot = root,
            };

            overrides.Add(
                type: typeof(T),
                attributes: rootAttributes);

            SetOverrides(typeof(T));
        }

        protected void SetOverrides(Type type)
        {
            if (type == default)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var properties = type.GetProperties()
                .Where(m => !m.PropertyType.IsValueType)
                .Where(m => m.PropertyType != typeof(string)).ToArray();

            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    var attributes = property.PropertyType
                        .GetAttributes(property.PropertyType == typeof(Body<T>));

                    overrides.Add(
                        type: type,
                        member: property.Name,
                        attributes: attributes);

                    SetOverrides(property.PropertyType);
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private XmlRootAttribute GetRootAttribute(string rootElement, string rootNamespace)
        {
            var result = new XmlRootAttribute();

            if (!string.IsNullOrWhiteSpace(rootNamespace))
            {
                AddNamespace(
                    prefix: string.Empty,
                    ns: rootNamespace);

                result.Namespace = rootNamespace;
            }

            if (!string.IsNullOrWhiteSpace(rootElement))
            {
                result.ElementName = rootElement;
            }

            return result;
        }

        private void SetNamespaces()
        {
            var result = new XmlSerializerNamespaces();

            var resultingNamespaces = allNamespaces
                .ToArray().GroupBy(n => n.Namespace)
                .Select(g => g.OrderBy(n => string.IsNullOrWhiteSpace(n.Name)).First()).ToArray();

            foreach (var resultingNamespace in resultingNamespaces)
            {
                result.Add(
                    prefix: resultingNamespace.Name,
                    ns: resultingNamespace.Namespace);

                if (defaultNamespace == default
                    && string.IsNullOrWhiteSpace(resultingNamespace.Name))
                {
                    defaultNamespace = resultingNamespace.Namespace;
                }
            }

            namespaces = result;
        }

        #endregion Private Methods
    }
}