using System.IO;
using System.Text;

namespace DataWriter.Models
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