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
            //File.Delete(DatabaseConfig.ConnStringSQLite);
            ps = new HibernateService();
            ts = (ITransactionService)ps;

            ms = new MitarbeiterkomponenteFacade(ps ,ts);

            kundenServices = new KundenkomponenteFacade(ps, ts, (IMitarbeiterServicesFuerKunden) ms);
            kursServices = new KurskomponenteFacade(ps, ts, kundenServices as IKundenServicesFuerKurse, ms as IMitarbeiterServicesFuerKurse);

            t1 = new Trainer()
            {
                Vorname = "Guter",
                Nachname = "Trainer"
            };
            r1 = new Rezeptionist()
            {
                Vorname = "Guter",
                Nachname = "Rezeptionist"
            };

            ms.CreateRezeptionist(r1);
            ms.CreateTrainer(t1);
        }

        [TestInitialize]
        public void Before()
        {
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
            if (ps.GetById<Kurs, int>(k1.ID) != null) ps.Delete(k1);
            if (ps.GetById<Kurs, int>(k2.ID) != null) ps.Delete(k2);
            kundenServices.DeleteKunde(ku1);
            kundenServices.DeleteKunde(ku2);
            //ps.DeleteAll<Trainer>();
            //ps.DeleteAll<Rezeptionist>();

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

            Assert.IsFalse(kursServices.GetAlleKurse().Contains(k1));
        }

        [TestMethod]
        public void TestBucheKursFuerEinenKundenSuccess()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            kursServices.BucheKurs(ku1.Kundennummer, k1);

            Assert.IsTrue(k1.Teilnehmer.Contains(ku1));
            Assert.IsTrue(k1.HatFreiePlaetze(2));
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
            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            List<int> kunden = new List<int>(new [] {ku1.Kundennummer, ku2.Kundennummer});
            kursServices.BucheKurs(kunden, k1);

            Assert.IsTrue(k1.Teilnehmer.Contains(ku1));
            Assert.IsTrue(k1.Teilnehmer.Contains(ku2));
            Assert.IsTrue(k1.HatFreiePlaetze(1));
        }

        [TestMethod]
        public void TestBucheKursFuerZweiKundenNurEinPlatzFrei()
        {
            kursServices.CreateKurs(k2, r1.ID, t1.ID);
            List<int> kunden = new List<int>(new[] { ku1.Kundennummer, ku2.Kundennummer });

            try
            {
                kursServices.BucheKurs(kunden, k2);
                Assert.Fail("Kurs sollte hier nicht genug Plätze haben");
            }
            catch(KursUeberfuelltException)
            {
                Assert.IsTrue(k2.HatFreiePlaetze(1));
            }
        }

        [TestMethod]
        public void TestBucheEinenKundenAufAnderenKursUmSuccess()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            kursServices.CreateKurs(k2, r1.ID, t1.ID);

            kursServices.BucheKurs(ku1.Kundennummer, k1);
            kursServices.BucheKundenAufAnderenKursUm(ku1.Kundennummer, k1, k2);
            Assert.IsFalse(k1.Teilnehmer.Contains(ku1));
            Assert.IsTrue(k2.Teilnehmer.Contains(ku1));
        }

        [TestMethod]
        public void TestBucheEinenKundenAufAnderenKursUmKursVoll()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            kursServices.CreateKurs(k2, r1.ID, t1.ID);

            kursServices.BucheKurs(ku1.Kundennummer, k2);
            kursServices.BucheKurs(ku2.Kundennummer, k1);
            try
            {
                kursServices.BucheKundenAufAnderenKursUm(ku2.Kundennummer, k1, k2);
                Assert.Fail("Zielkurs sollte hier voll sein");
            }
            catch(KursUeberfuelltException)
            {
                Assert.IsFalse(k2.HatFreiePlaetze());
            }
            
        }

        [TestMethod]
        public void TestBucheZweiKundenAufAnderenKursUmSuccess()
        {
            k2.MaximaleTeilnehmeranzahl = 2;

            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            kursServices.CreateKurs(k2, r1.ID, t1.ID);

            List<int> kunden = new List<int>(new[] { ku1.Kundennummer, ku2.Kundennummer });
            kursServices.BucheKurs(kunden, k1);
            kursServices.BucheKundenAufAnderenKursUm(kunden, k1, k2);

            Assert.IsFalse(k1.Teilnehmer.Contains(ku1));
            Assert.IsFalse(k1.Teilnehmer.Contains(ku2));
            Assert.IsTrue(k2.Teilnehmer.Contains(ku1));
            Assert.IsTrue(k2.Teilnehmer.Contains(ku1));
        }

        [TestMethod]
        public void TestBucheZweiKundenAufAnderenKursUmNurEinPlatzFrei()
        {
            kursServices.CreateKurs(k1, r1.ID, t1.ID);
            kursServices.CreateKurs(k2, r1.ID, t1.ID);

            List<int> kunden = new List<int>(new[] { ku1.Kundennummer, ku2.Kundennummer });
            kursServices.BucheKurs(kunden, k1);

            try
            {
                kursServices.BucheKundenAufAnderenKursUm(kunden, k1, k2);
                Assert.Fail("Zielkurs sollte hier nicht genug Platz haben");
            }
            catch(KursUeberfuelltException)
            {
                Assert.IsTrue(k1.HatFreiePlaetze(1));
                Assert.IsTrue(k2.HatFreiePlaetze(1));
            }


        }
    }
}
