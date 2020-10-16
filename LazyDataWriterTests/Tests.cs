using IVUTrafficNetworkImportService;
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
        public void WriteSoapComplex()
        {
            var test = new TrafficNetworkImportRequest
            {
                trafficNetwork = new trafficNetwork
                {
                    links = new Link[] { new Link { distance = "200" } }
                }
            };

            var writer = new SoapWriter<TrafficNetworkImportRequest>();
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void WriteSoapSimple()
        {
            var test = GetTestClass();

            var writer = new SoapWriter<TestClass>();
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void WriteSoapWithNamespace()
        {
            var test = GetTestClass();

            var writer = new SoapWriter<TestClass>(withoutXmlHeader: true);
            writer.AddNamespace("http://www.subtest.de", "test");
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void WriteXmlComplex()
        {
            var test = new TrafficNetworkImportRequest
            {
                trafficNetwork = new trafficNetwork
                {
                    links = new Link[] { new Link { distance = "200" } }
                }
            };

            var writer = new XmlWriter<TrafficNetworkImportRequest>(rootNamespace: "http://intf.mb.ivu.de/network/standardimport/remote");
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void WriteXmlSimple()
        {
            var test = GetTestClass();

            var writer = new XmlWriter<TestClass>();
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void WriteXmlSimpleWithoutHeader()
        {
            var test = GetTestClass();

            var writer = new XmlWriter<TestClass>(withoutXmlHeader: true);
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void WriteXmlWothNamespace()
        {
            var test = GetTestClass();

            var writer = new XmlWriter<TestClass>("test", "http://www.subtest.de");
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        #endregion Public Methods

        #region Private Methods

        private static SubTestClass GetSubTestClass()
        {
            return new SubTestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
            };
        }

        private static TestClass GetTestClass()
        {
            return new TestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
                SubTestClass = GetSubTestClass()
            };
        }

        #endregion Private Methods
    }
}