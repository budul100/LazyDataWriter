using System.IO;
using System.Text;

namespace LazyDataWriter.Writers
{
    internal class UTF8Writer
        : StringWriter
    {
        #region Public Properties

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        #endregion Public Properties
    }
}