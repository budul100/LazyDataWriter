using LazyDataWriter;
using LazyDataWriterTests.Models;
using NUnit.Framework;
using System;

namespace LazyDataWriterTests
{
    public class Tests
    {
        #region Public Methods

        [Test]
        public void SimpleWriteSoap()
        {
            var test = new TestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
            };

            var writer = new SoapWriter<TestClass>();
            //writer.AddNamespace("http://www.test.de", "test");
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void SimpleWriteSoapWithoutHeader()
        {
            var test = new TestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
            };

            var writer = new SoapWriter<TestClass>(withoutXmlHeader: true);
            writer.AddNamespace("http://www.test.de", "test");
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void SimpleWriteXml()
        {
            var test = new TestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
            };

            var writer = new XmlWriter<TestClass>();
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void SimpleWriteXmlWithoutHeader()
        {
            var test = new TestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
            };

            var writer = new XmlWriter<TestClass>(withoutXmlHeader: true);
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        #endregion Public Methods
    }
}