using Microsoft.VisualStudio.TestTools.UnitTesting;
using MitarbeiterKomponente.AccessLayer;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using PersistenceService;

namespace MitarbeiterkomponenteTest
{
    [TestClass]
    public class UnitTestMitarbeiter
    {
        static IPersistenceService ps;
        static ITransactionService ts;
        static IMitarbeiterServices ms;

        static Rezeptionist r1;
        static Trainer t1;

        [ClassInitialize]
        public static void ClassInit(TestContext t)
        {
            ps = new HibernateService();
            ts = (ITransactionService)ps;
            ms = new MitarbeiterkomponenteFacade(ps, ts);

        }

        [TestInitialize]
        public void Before()
        {
            r1 = new Rezeptionist()
            {
                Vorname = "Klaus",
                Nachname = "Müller"
            };

            t1 = new Trainer()
            {
                Vorname = "Hans",
                Nachname = "Schneider"
            };
        }

        [TestCleanup]
        public void After()
        {
            ps.DeleteAll<Rezeptionist>();
            ps.DeleteAll<Trainer>();
        }

        [TestMethod]
        public void TestCreateRezeptionist()
        {
            ms.CreateRezeptionist(r1);
            Assert.IsTrue(r1.ID != 0);
        }

        [TestMethod]
        public void TestCreateTrainer()
        {
            ms.CreateTrainer(t1);
            Assert.IsTrue(t1.ID != 0);
        }

        [TestMethod]
        public void TestFindRezeptionist()
        {
            ms.CreateRezeptionist(r1);

            Rezeptionist r2 = ms.FindRezeptionistById(r1.ID);
            Assert.AreEqual(r1, r2);
        }

        [TestMethod]
        public void TestFindTrainer()
        {
            ms.CreateTrainer(t1);

            Trainer t2 = ms.FindTrainerById(t1.ID);
            Assert.AreEqual(t1, t2);
        }

        [TestMethod]
        public void TestUpdateRezeptionist()
        {
            ms.CreateRezeptionist(r1);

            r1.Nachname = "Neuu";
            ms.UpdateRezeptionist(r1);

            Rezeptionist r2 = ms.FindRezeptionistById(r1.ID);
            Assert.AreEqual(r1, r2);
        }

        [TestMethod]
        public void TestUpdateTrainer()
        {
            ms.CreateTrainer(t1);

            t1.Nachname = "Neuu";
            ms.UpdateTrainer(t1);

            Trainer t2 = ms.FindTrainerById(t1.ID);
            Assert.AreEqual(t1, t2);
        }

        [TestMethod]
        public void TestDeleteRezeptionist()
        {
            ms.CreateRezeptionist(r1);
            ms.DeleteRezeptionist(r1);
            Assert.AreEqual(null, ms.FindRezeptionistById(r1.ID));
        }

        [TestMethod]
        public void TestDeleteTrainer()
        {
            ms.CreateTrainer(t1);
            ms.DeleteTrainer(t1);
            Assert.AreEqual(null, ms.FindTrainerById(t1.ID));
        }
    }
}
