﻿using LazyDataWriter.Extensions;
using LazyDataWriter.Writers;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace LazyDataWriter
{
    public class XmlWriter<T>
    {
        #region Private Fields

        private string defaultNamespace;

        private XmlSerializer serializer;

        #endregion Private Fields

        #region Public Constructors

        public XmlWriter(string rootElement, string rootNamespace = default, bool withoutXmlHeader = false)
        {
            WithoutXmlHeader = withoutXmlHeader;

            CreateSerializer<T>(
                rootElement: rootElement,
                rootNamespace: rootNamespace,
                overridesGetter: r => r.GetOverridesXml<T>());
        }

        public XmlWriter(bool withoutXmlHeader = false)
            : this(default, default, withoutXmlHeader)
        { }

        #endregion Public Constructors

        #region Protected Properties

        protected XmlSerializerNamespaces Namespaces { get; } = new XmlSerializerNamespaces();

        protected bool WithoutXmlHeader { get; set; }

        #endregion Protected Properties

        #region Public Methods

        public void AddNamespace(string ns, string prefix = default)
        {
            if (!string.IsNullOrWhiteSpace(ns) && ns != defaultNamespace)
            {
                if (string.IsNullOrWhiteSpace(prefix))
                {
                    defaultNamespace = ns;
                }

                Namespaces.Add(
                    prefix: prefix,
                    ns: ns);
            }
        }

        public virtual string Write(T content)
        {
            var result = GetString(content);

            return result;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void CreateSerializer<U>(string rootElement, string rootNamespace,
            Func<XmlRootAttribute, XmlAttributeOverrides> overridesGetter)
        {
            if (overridesGetter == default)
            {
                throw new ArgumentNullException(nameof(overridesGetter));
            }

            var root = GetRootAttribute(
                rootElement: rootElement,
                rootNamespace: rootNamespace);

            var overrides = overridesGetter.Invoke(root);

            var result = new XmlSerializer(
                type: typeof(U),
                overrides: overrides,
                extraTypes: null,
                root: root,
                defaultNamespace: defaultNamespace);

            serializer = result;
        }

        protected XmlRootAttribute GetRootAttribute(string rootElement, string rootNamespace)
        {
            var result = new XmlRootAttribute();

            if (!string.IsNullOrWhiteSpace(rootNamespace))
            {
                if (defaultNamespace == default)
                {
                    AddNamespace(
                        prefix: string.Empty,
                        ns: rootNamespace);

                    defaultNamespace = rootNamespace;
                }

                result.Namespace = rootNamespace;
            }

            if (!string.IsNullOrWhiteSpace(rootElement))
            {
                result.ElementName = rootElement;
            }

            return result;
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
                            namespaces: Namespaces);
                    }
                }
                else
                {
                    serializer.Serialize(
                        o: content,
                        textWriter: textWriter,
                        namespaces: Namespaces);
                }

                result = textWriter.ToString();
            }

            return result;
        }

        #endregion Protected Methods
    }
}