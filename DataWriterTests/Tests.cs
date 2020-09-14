using DataWriter;
using NUnit.Framework;
using System;

namespace DataWriterTests
{
    public class Tests
    {
        #region Public Methods

        [Test]
        public void SimpleWriteTest()
        {
            var test = new TestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
            };

            var writer = new Writer<TestClass>();
            var result = writer.Get(test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [Test]
        public void SimpleWriteTestWithoutXmlHeader()
        {
            var test = new TestClass
            {
                BoolProperty = true,
                DateTimeProperty = DateTime.Now,
                IntegerProperty = 12345,
                StringProperty = "Lorem ipsum",
            };

            var writer = new Writer<TestClass>(withoutXmlHeader: true);
            var result = writer.Get(
                content: test);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        #endregion Public Methods
    }
}