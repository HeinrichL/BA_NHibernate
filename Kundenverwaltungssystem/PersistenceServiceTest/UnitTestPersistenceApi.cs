using System;
using System.IO;
using System.Linq;
using FluentNHibernate.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Util;
using PersistenceService;
using PersistenceService.Implementation;

namespace PersistenceServiceTest
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für UnitTestPersistenceApi
    /// </summary>
    [TestClass]
    public class UnitTestPersistenceApi
    {
        private static ISession _session;
        private static IPersistenceService _ps;
        private static ITransactionService _ts;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _session = HibernateSessionFactory.OpenSession();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            _session.CreateSQLQuery("IF OBJECT_ID('dbo.TestMember') IS NOT NULL DROP TABLE TestMember").ExecuteUpdate();
            _session.CreateSQLQuery("IF OBJECT_ID('dbo.TestClass') IS NOT NULL DROP TABLE TestClass").ExecuteUpdate();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _ps = new HibernateService();
            _ts = (ITransactionService)_ps;
        }


        [TestMethod]
        public void TestMethodSave()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            _ps.Save(m);
            Assert.IsTrue(m.ID != 0);
        }

        [TestMethod]
        public void TestMethodSaveAll()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            TestMember m2 = new TestMember() { Hehe = DateTime.Now };
            TestMember m3 = new TestMember() { Hehe = DateTime.Now };
            TestMember m4 = new TestMember() { Hehe = DateTime.Now };
            var res = _ps.SaveAll(new [] {m, m2, m3, m4});
            res.ForEach(x => Assert.IsTrue(x.ID != 0));
        }

        [TestMethod]
        public void TestMethodGetById()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            _ps.Save(m);
            TestMember res = _ps.GetById<TestMember, int>(m.ID);
            Assert.AreEqual(m, res);
            Assert.AreSame(m, res);
        }

        [TestMethod]
        public void TestMethodGetAll()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            TestMember m2 = new TestMember() { Hehe = DateTime.Now };
            TestMember m3 = new TestMember() { Hehe = DateTime.Now };
            TestMember m4 = new TestMember() { Hehe = DateTime.Now };
            var res = _ps.SaveAll(new[] { m, m2, m3, m4 });
            var all = _ps.GetAll<TestMember>();
            res.ForEach(elem => Assert.IsTrue(all.Contains(elem)));
        }

        [TestMethod]
        public void TestMethodDelete()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            _ps.Save(m);
            int id = m.ID;
            _ps.Delete(m);
            Assert.IsTrue(null == _ps.GetById<TestMember, int>(id));
        }

        [TestMethod]
        public void TestMethodDeleteAll()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            TestMember m2 = new TestMember() { Hehe = DateTime.Now };
            TestMember m3 = new TestMember() { Hehe = DateTime.Now };
            TestMember m4 = new TestMember() { Hehe = DateTime.Now };
            _ps.SaveAll(new[] { m, m2, m3, m4 });
            _ps.DeleteAll<TestMember>();
            var all = _ps.GetAll<TestMember>();
            Assert.AreEqual(0, all.Count);
        }

        [TestMethod]
        public void TestMethodDeleteRange()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            TestMember m2 = new TestMember() { Hehe = DateTime.Now };
            TestMember m3 = new TestMember() { Hehe = DateTime.Now };
            TestMember m4 = new TestMember() { Hehe = DateTime.Now };
            var res = _ps.SaveAll(new[] { m, m2, m3, m4 });
            var delete = new[] {m2, m3};
            _ps.DeleteRange(delete);
            var all = _ps.GetAll<TestMember>();
            Enumerable.Except(res, delete).ForEach(elem => Assert.IsTrue(all.Contains(elem)));
            res.Intersect(delete).ForEach(elem => Assert.IsFalse(all.Contains(elem)));
        }

        [TestMethod]
        public void TestMethodQuery()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };
            TestMember m2 = new TestMember() { Hehe = DateTime.Today.AddDays(2) };
            TestMember m3 = new TestMember() { Hehe = DateTime.Now };
            TestMember m4 = new TestMember() { Hehe = DateTime.Now };
            var res = _ps.SaveAll(new[] { m, m2, m3, m4 });

            var query = _ps.Query<TestMember>().Where(x => x.Hehe == DateTime.Today.AddDays(2)).ToList();
            var linqQuery = from tm in _ps.Query<TestMember>()
                            where tm.Hehe == DateTime.Today.AddDays(2)
                            select tm;
            Assert.AreEqual(1, query.Count);
            Assert.AreEqual(m2, linqQuery.First());

        }

        [TestMethod]
        public void TestTransaction()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now };

            _ps.Save(m);

            IPersistenceService ps2 = new HibernateService();
            ITransactionService ts2 = (ITransactionService)ps2;
            TestMember mConcurrent = ps2.GetById<TestMember, int>(m.ID);

            Assert.AreNotSame(m, mConcurrent);
            ts2.ExecuteInTransaction(() =>
            {
                mConcurrent.Hehe = DateTime.Today.AddDays(20);
            });
            Assert.AreNotSame(m, mConcurrent);
            try
            {
                m.Hehe = DateTime.Now.AddMinutes(5);
                _ps.Save(m);
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.AreEqual(typeof(StaleObjectStateException), e.GetType());
                Assert.AreEqual(_ps.Refresh(m), mConcurrent);
            }
        }

        [TestMethod]
        public void TestTransaction2()
        {
            TestMember m = new TestMember() { Hehe = DateTime.Now.AddDays(12) };
            _ps.Save(m);

            IPersistenceService ps2 = new HibernateService();
            ITransactionService ts2 = (ITransactionService)ps2;
            TestMember mConcurrent = ps2.GetById<TestMember, int>(m.ID);

            Assert.AreNotSame(m, mConcurrent);
            var mts = ts2.ExecuteInTransaction(() =>
            {
                mConcurrent.Hehe = DateTime.Today.AddDays(20);
                return true;
            });
            Assert.AreNotSame(m, mConcurrent);
            try
            {
                m.Hehe = DateTime.Now.AddMinutes(5);
                _ps.Save(m);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(StaleObjectStateException), e.GetType());
            }
        }
    }
}
