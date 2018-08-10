using System.Collections.Generic;
using Application1.Repositories.Contract.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application1.Services.Services;
using System.Linq;

namespace Application1.Services.Tests.UnitTests
{
    [TestClass]
    public class Service1Tests
    {
        Service1 _service1;
        IRepository1 _repository1;
        IRepository2 _repository2;

        [TestInitialize]
        public void Setup()
        {
            _repository1 = new Repository1TestImplementation();
            _repository2 = new Repository2TestImplementation();
            _service1 = new Service1(_repository1, _repository2);
        }

        [TestMethod]
        public void GetMeSomeNamesActuallyGetsSomeNames()
        {
            var names = _service1.GetMeSomeNames();

            Assert.AreEqual(names.Count(), 6);
            Assert.AreEqual(names.ElementAt(0), "One");
            Assert.AreEqual(names.ElementAt(1), "Two");
            Assert.AreEqual(names.ElementAt(2), "Three");
            Assert.AreEqual(names.ElementAt(3), "Four");
            Assert.AreEqual(names.ElementAt(4), "Five");
            Assert.AreEqual(names.ElementAt(5), "Six");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _service1 = null;
            _repository1 = null;
            _repository2 = null;
        }
    }

    #region IRepository1Implementation

    public class Repository1TestImplementation : IRepository1
    {
        public IEnumerable<string> GetRepository1Names()
        {
            return new List<string> { "One", "Two", "Three" };
        }
    }

    #endregion IRepository1Implementation

    #region IRepository2Implementation

    public class Repository2TestImplementation : IRepository2
    {
        public IEnumerable<string> GetRepository2Names()
        {
            return new List<string> { "Four", "Five", "Six" };
        }
    }

    #endregion IRepository2Implementation
}
