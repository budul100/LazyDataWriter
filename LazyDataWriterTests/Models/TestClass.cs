using System;
using System.Xml.Serialization;

namespace LazyDataWriterTests.Models
{
    [XmlRoot(ElementName = "SubTestClass")]
    [XmlType(Namespace = "http://www.subtest.de")]
    public class SubTestClass
    {
        #region Public Properties

        public bool BoolProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public int IntegerProperty { get; set; }

        public string StringProperty { get; set; }

        #endregion Public Properties
    }

    [XmlRoot(Namespace = "http://www.test.de")]
    [XmlInclude(typeof(SubTestClass))]
    public class TestClass
    {
        #region Public Properties

        public bool BoolProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public int IntegerProperty { get; set; }

        public string StringProperty { get; set; }

        [XmlElement(ElementName = "SubTestClass", Namespace = "http://www.subtest.de")]
        public SubTestClass SubTestClass { get; set; }

        #endregion Public Properties
    }
}