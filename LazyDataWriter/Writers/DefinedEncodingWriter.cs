using System.IO;
using System.Text;

namespace LazyDataWriter.Writers
{
    internal class DefinedEncodingWriter
        : StringWriter
    {
        #region Public Constructors

        public DefinedEncodingWriter(Encoding encoding = default)
        {
            if (encoding == default)
            {
                encoding = Encoding.UTF8;
            }

            Encoding = encoding;
        }

        #endregion Public Constructors

        #region Public Properties

        public override Encoding Encoding { get; }

        #endregion Public Properties
    }
}