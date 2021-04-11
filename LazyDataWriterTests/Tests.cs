using IVUTrafficNetworkImportService;
using LazyDataWriter;
using LazyDataWriterTests.Models;
using NUnit.Framework;
using System;
using System.Text;

namespace LazyDataWriterTests
{
    public class Tests
    {
        #region Public Methods

        [Test]
        public void ChangeEncoding()
        {
            var test = GetTestClass();

            var encodingText = "ISO-8859-1";
            var encoding = Encoding.GetEncoding(encodingText);

            var writer = new XmlWriter<TestClass>(encoding: encoding);
            var result = writer.Write(test);

            Assert.IsTrue(result.ToLower().Contains(encodingText.ToLower()));
        }

        [Test]
        public void SoapComplex()
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
        public void SoapSimple()
        {
            var test = GetTestClass();

            var writer = new SoapWriter<TestClass>();
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void SoapWithNamespace()
        {
            var test = GetTestClass();

            var writer = new SoapWriter<TestClass>(withoutXmlHeader: true);
            writer.AddNamespace("http://www.subtest.de", "test");
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void XmlComplexTrafficNetwork()
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
        public void XmlComplexTrans402()
        {
            var test = new TRANS
            {
                DATA = new DATA
                {
                    GRUNDDATEN = new GRUNDDATEN
                    {
                        BETRIEBSBEREICHE = new BETRIEBSBEREICHE
                        {
                            BTB = new BTB[]
                               { new BTB
                                   {
                                       KBEZ="TEST",
                                   }
                               }
                        }
                    }
                }
            };

            var writer = new XmlWriter<TRANS>();
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void XmlSimple()
        {
            var test = GetTestClass();

            var writer = new XmlWriter<TestClass>();
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void XmlSimpleWithoutHeader()
        {
            var test = GetTestClass();

            var writer = new XmlWriter<TestClass>(withoutXmlHeader: true);
            var result = writer.Write(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void XmlWithNamespace()
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