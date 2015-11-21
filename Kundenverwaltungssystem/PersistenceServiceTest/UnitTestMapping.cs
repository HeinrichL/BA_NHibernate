using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using PersistenceService.Implementation;

namespace PersistenceServiceTest
{
    [TestClass]
    public class UnitTestMapping
    {
        private static ISession _session;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _session = HibernateSessionFactory.OpenSession();
        }

        [ClassCleanup]
        public static void Clean()
        {
            _session.CreateSQLQuery("IF OBJECT_ID('dbo.TestMember') IS NOT NULL DROP TABLE TestMember").ExecuteUpdate();
            _session.CreateSQLQuery("IF OBJECT_ID('dbo.TestClass') IS NOT NULL DROP TABLE TestClass").ExecuteUpdate();
        }


        [TestMethod]
        public void TestSaveAndLoadObjectSuccess()
        {
            IList<TestMember> members = new List<TestMember>
                                    {
                                        new TestMember() {Hehe = DateTime.Now},
                                        new TestMember() {Hehe = DateTime.Today}
                                    };
            TestClass test = new TestClass { Bla = "Test", Members = members };
            IList<TestMember> members2 = new List<TestMember>
                                    {
                                        new TestMember() {Hehe = DateTime.Now},
                                        new TestMember() {Hehe = DateTime.Now.AddDays(12)}
                                    };
            TestClass test2 = new TestClass { Bla = "Test2", Members = members2 };
            _session.Save(test);
            _session.Save(test2);
            _session.Flush();
            TestClass testexp = _session.Get<TestClass>(test.ID);

            Assert.AreEqual(test, testexp);
            //CollectionAssert.AreEqual(test.Members, tests.Members);

            _session.Delete(test);
            _session.Flush();
        }
    }
}
