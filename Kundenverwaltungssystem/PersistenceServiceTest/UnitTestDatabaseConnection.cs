using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using PersistenceService.Implementation;

namespace PersistenceServiceTest
{
    [TestClass]
    public class UnitTestDatabaseConnection
    {
        private static ISession _session;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _session = HibernateSessionFactory.OpenSession();
            //_session.CreateSQLQuery("IF OBJECT_ID('dbo.Testtable') IS NOT NULL DROP TABLE Testtable").ExecuteUpdate();
            _session.CreateSQLQuery("CREATE TABLE Testtable (ID int not null, Bla nvarchar(200))").ExecuteUpdate();
        }

        [ClassCleanup]
        public static void Clean()
        {
            _session = HibernateSessionFactory.OpenSession();
            //_session.CreateSQLQuery("IF OBJECT_ID('dbo.Testtable') IS NOT NULL DROP TABLE Testtable").ExecuteUpdate();
        }

        [TestMethod]
        public void TestDatabaseConnection()
        {
            IList res;
            using (ITransaction transaction = _session.BeginTransaction())
            {
                _session.CreateSQLQuery("INSERT INTO Testtable (ID, Bla) VALUES (1,'asd'),(2,'asdf')").ExecuteUpdate();
                res = _session.CreateSQLQuery("SELECT * FROM Testtable").List();

                transaction.Commit();
            }

            Assert.AreEqual(2, res.Count);
        }
    }
}
