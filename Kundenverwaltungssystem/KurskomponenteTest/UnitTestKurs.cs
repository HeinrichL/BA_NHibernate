using System;
using System.Collections.Generic;
using System.Linq;
using Kundenkomponente.Accesslayer;
using Kundenkomponente.DataAccessLayer.Datatypes;
using Kundenkomponente.DataAccessLayer.Entities;
using KursKomponente.AccessLayer;
using KursKomponente.AccessLayer.Exceptions;
using KursKomponente.DataAccessLayer;
using KursKomponente.DataAccessLayer.Datatypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MitarbeiterKomponente.AccessLayer;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using Moq;
using PersistenceService;

namespace KurskomponenteTest
{
    [TestClass]
    public class UnitTestKurs
    {
        private static IPersistenceService ps;
        private static ITransactionService ts;
        private static IKursServices kursServices;
        private static IMitarbeiterServices ms;
        private static IKundenServices kundenServices;

        private static Kurs k1;
        private static Kurs k2;
        private static Rezeptionist r1;
        private static Trainer t1;

        private static Kunde ku1;
        private static Kunde ku2;

        [ClassInitialize]
        public static void ClassInit(TestContext t)
        {
            ps = new HibernateService();
            ts = (ITransactionService)ps;

            ms = new MitarbeiterkomponenteFacade(ps ,ts);

            kundenServices = new KundenkomponenteFacade(ps, ts, (IMitarbeiterServicesFuerKunden) ms);
            kursServices = new KurskomponenteFacade(ps, ts, kundenServices as IKundenServicesFuerKurse, ms as IMitarbeiterServicesFuerKurse);

        }

        [TestInitialize]
        public void Before()
        {
            t1 = new Trainer()
            {
                Vorname = "Guter",
                Nachname = "Trainer",
                Personalnummer = "1122"
            };
            r1 = new Rezeptionist()
            {
                Vorname = "Guter",
                Nachname = "Rezeptionist",
                Personalnummer = "1122"
            };

            ms.CreateRezeptionist(r1);
            ms.CreateTrainer(t1);

            ku1 = new Kunde()
            {
                Vorname = "Klaus",
                Nachname = "Müller",
                Adresse = new AdressTyp("Berliner Tor", "7", "22091", "Hamburg"),
                EmailAdresse = new EmailTyp("bla@test.de"),
                Geburtsdatum = new DateTime(1990, 01, 01),
                Kundenstatus = Kundenstatus.Basic,
                Telefonnummer = "123456"
            };
            kundenServices.CreateKunde(ku1, r1.ID);
            ku2 = new Kunde()
            {
                Vorname = "Heinz",
                Nachname = "Schmidt",
                Adresse = new AdressTyp("Berliner Tor", "7", "22091", "Hamburg"),
                EmailAdresse = new EmailTyp("bla2@test.de"),
                Geburtsdatum = new DateTime(1995, 01, 01),
                Kundenstatus = Kundenstatus.Premium,
                Telefonnummer = "654321"
            };
            kundenServices.CreateKunde(ku2, r1.ID);
            k1 = new Kurs()
                 {
                     Titel = "Cooler Kurs",
                     Beschreibung = "Dies ist eine Kursbeschreibung",
                     MaximaleTeilnehmeranzahl = 3,
                     Veranstaltungszeit =
                         new VeranstaltungszeitTyp(DateTime.Today.AddHours(13), DateTime.Today.AddHours(14)),
                     Kursstatus = Kursstatus.Geplant
                 };

            k2 = new Kurs()
            {
                Titel = "Anderer cooler Kurs",
                Beschreibung = "Dies ist eine Kursbeschreibung",
                MaximaleTeilnehmeranzahl = 1,
                Veranstaltungszeit =
                         new VeranstaltungszeitTyp(DateTime.Today.AddHours(13), DateTime.Today.AddHours(14)),
                Kursstatus = Kursstatus.Geplant
            };
        }

        [TestCleanup]
        public void After()
        {
            ps.DeleteAll<Kurs>();
            ps.DeleteAll<Kunde>();
            ps.DeleteAll<Trainer>();
            ps.DeleteAll<Rezeptionist>();
            
        }

        [TestMethod]
        public void TestMethodCreateKurs()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);

            Assert.IsTrue(k1.ID != 0);
        }

        [TestMethod]
        public void TestFindKurs()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);

            Kurs kurs2 = kursServices.FindKursById(k1.ID);

            Assert.AreEqual(k1, kurs2);
        }

        [TestMethod]
        public void TestGetAlleKurse()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            kursServices.CreateKurs(k2, r1.ID, t1.ID);

            IList<Kurs> kurse = new List<Kurs>();
            kurse.Add(k1);
            kurse.Add(k2);

            IList<Kurs> alleKurse = kursServices.GetAlleKurse();
            CollectionAssert.AreEqual(kurse.ToList(), alleKurse.ToList());
        }

        [TestMethod]
        public void TestUpdateKurs()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);

            k1.Beschreibung = "Neue Beschreibung";
            kursServices.UpdateKurs(k1);

            Kurs kurs2 = kursServices.FindKursById(k1.ID);
            Assert.AreEqual(k1, kurs2);
        }

        [TestMethod]
        public void TestDeleteKurs()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);

            kursServices.DeleteKurs(k1);

            Assert.AreEqual(0, kursServices.GetAlleKurse().Count);
        }

        [TestMethod]
        public void TestBucheKursFuerEinenKundenSuccess()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            kursServices.BucheKurs(ku1.Kundennummer, k1);

            Assert.IsTrue(k1.Teilnehmer.Contains(ku1));
            Assert.IsTrue(k1.HatFreiePlaetze());
        }

        [TestMethod]
        public void TestBucheKursFuerEinenKundenKursVoll()
        {
            kursServices.CreateKurs(k2, r1.ID, t1.ID);

            kursServices.BucheKurs(ku1.Kundennummer, k2);

            try
            {
                kursServices.BucheKurs(ku2.Kundennummer, k2);
                Assert.Fail("Kurs sollte hier voll sein");
            }
            catch(KursUeberfuelltException e)
            {
                Assert.IsFalse(k2.HatFreiePlaetze());
            }
        }

        [TestMethod]
        public void TestBucheKursFuerZweiKundenSuccess()
        {

        }

        [TestMethod]
        public void TestBucheKursFuerZweiKundenNurEinPlatzFrei()
        {

        }

        [TestMethod]
        public void TestBucheEinenKundenAufAnderenKursUmSuccess()
        {

        }

        [TestMethod]
        public void TestBucheEinenKundenAufAnderenKursUmKursVoll()
        {

        }

        [TestMethod]
        public void TestBucheZweiKundenAufAnderenKursUmSuccess()
        {

        }

        [TestMethod]
        public void TestBucheZweiKundenAufAnderenKursUmNurEinPlatzFrei()
        {

        }
    }
}
