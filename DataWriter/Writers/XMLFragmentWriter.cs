using System;
using System.IO;
using System.Text;
using System.Xml;

namespace DataWriter.Writers
{
    internal class XmlFragmentWriter
        : XmlTextWriter, IDisposable
    {
        #region Private Fields

        private bool skip;

        #endregion Private Fields

        #region Public Constructors

        public XmlFragmentWriter(TextWriter writer) : base(writer)
        { }

        public XmlFragmentWriter(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }

        public XmlFragmentWriter(string filename, Encoding encoding)
            : base(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None), encoding)
        { }

        #endregion Public Constructors

        #region Public Methods

        public override void WriteEndAttribute()
        {
            if (skip)
            {
                // Reset the flag, so we keep writing.
                skip = false;
                return;
            }

            base.WriteEndAttribute();
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)

        {
            // STEP 1 - Omits XSD and XSI declarations.
            // From Kzu - http://weblogs.asp.net/cazzu/archive/2004/01/23/62141.aspx

            if (prefix == "xmlns" && (localName == "xsd" || localName == "xsi"))
            {
                skip = true;
                return;
            }

            base.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument()
        {
            // STEP 2: Do nothing so we omit the xml declaration.
        }

        public override void WriteString(string text)
        {
            if (skip) return;

            base.WriteString(text);
        }

        #endregion Public Methods
    }
}