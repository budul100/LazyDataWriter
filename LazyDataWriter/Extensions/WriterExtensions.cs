using LazyDataWriter.Soap;
using System;
using System.Reflection;
using System.Xml.Serialization;

namespace LazyDataWriter.Extensions
{
    internal static class WriterExtensions
    {
        #region Public Methods

        public static XmlAttributeOverrides GetOverridesSoap<T>(this XmlRootAttribute root)
        {
            var result = new XmlAttributeOverrides();

            var rootAttributes = new XmlAttributes
            {
                XmlRoot = root,
            };

            result.Add(
                type: typeof(Envelope<T>),
                attributes: rootAttributes);

            var bodyAttributes = new XmlAttributes
            {
                XmlElements = { GetXmlElementAttribut<T>() },
            };

            result.Add(
                type: typeof(Body<T>),
                member: nameof(Body<T>.Content),
                attributes: bodyAttributes);

            return result;
        }

        public static XmlAttributeOverrides GetOverridesXml<T>(this XmlRootAttribute root)
        {
            var result = new XmlAttributeOverrides();

            var rootAttributes = new XmlAttributes
            {
                XmlRoot = root
            };

            result.Add(
                type: typeof(T),
                attributes: rootAttributes);

            return result;
        }

        public static Envelope<T> GetSoap<T>(this T content)
        {
            var body = new Body<T>
            {
                Content = content,
            };

            var soap = new Envelope<T>
            {
                Body = body,
            };

            return soap;
        }

        public static string GetXmlNamespace(this Type type)
        {
            var result = type.GetCustomAttribute<XmlRootAttribute>()?.Namespace;

            if (string.IsNullOrWhiteSpace(result))
            {
                result = type.GetCustomAttribute<XmlTypeAttribute>()?.Namespace;
            }

            return result;
        }

        public static string GetXmlRootElement(this Type type)
        {
            var result = type.GetCustomAttribute<XmlRootAttribute>()?.ElementName;

            if (string.IsNullOrWhiteSpace(result))
            {
                result = type.GetCustomAttribute<XmlTypeAttribute>()?.TypeName;
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = type.Name;
            }

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private static XmlElementAttribute GetXmlElementAttribut<T>()
        {
            var result = new XmlElementAttribute
            {
                ElementName = typeof(T).GetXmlRootElement(),
                Namespace = typeof(T).GetXmlNamespace()
            };

            return result;
        }

        #endregion Private Methods
    }
}