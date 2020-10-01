using System;
using System.Xml.Serialization;

namespace LazyDataWriterTests.Models
{
    [XmlType(Namespace = "http://www.test.de")]
    public class TestClass
    {
        #region Public Properties

        public bool BoolProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public int IntegerProperty { get; set; }

        public string StringProperty { get; set; }

        #endregion Public Properties
    }
}