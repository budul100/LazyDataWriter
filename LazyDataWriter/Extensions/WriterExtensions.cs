using LazyDataWriter.Soap;
using System;
using System.Reflection;
using System.Xml.Serialization;

namespace LazyDataWriter.Extensions
{
    internal static class WriterExtensions
    {
        #region Public Methods

        public static XmlAttributes GetAttributes(this Type type, bool leaveName)
        {
            var elementAttribute = new XmlElementAttribute
            {
                Namespace = type.GetXmlNamespace()
            };

            if (!leaveName)
            {
                elementAttribute.ElementName = type.GetXmlRootElement();
            }

            var result = new XmlAttributes
            {
                XmlElements = { elementAttribute },
            };

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
    }
}