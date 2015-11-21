﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kundenkomponente.Accesslayer;
using Kundenkomponente.DataAccessLayer.Datatypes;
using Kundenkomponente.DataAccessLayer.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersistenceService;
using MitarbeiterKomponente.AccessLayer;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using Moq;

namespace KundenkomponenteTest
{
    [TestClass]
    public class UnittestKunde
    {
        static IPersistenceService ps;
        static ITransactionService ts;
        static IKundenServices ks;
        static IMitarbeiterServices ms;
        

        static Kunde k1;
        static Rezeptionist r1;

        [ClassInitialize]
        public static void ClassInit(TestContext t)
        {
            ps = new HibernateService();
            ts = (ITransactionService) ps;

            ms = new MitarbeiterkomponenteFacade(ps, ts);

            ks = new KundenkomponenteFacade(ps, ts, ms as IMitarbeiterServicesFuerKunden);
        }

        [TestInitialize]
        public void Before()
        {
            k1 = new Kunde()
            {
                Vorname = "Klaus",
                Nachname = "Müller",
                Adresse = new AdressTyp("Berliner Tor", "7", "22091", "Hamburg"),
                EmailAdresse = new EmailTyp("bla@test.de"),
                Geburtsdatum = new DateTime(1990, 01, 01),
                Kundenstatus = Kundenstatus.Basic,
                Telefonnummer = "123456"
            };

            r1 = new Rezeptionist()
                 {
                     Vorname = "Rezep",
                     Nachname = "tionist",
                     Personalnummer = "12345"
                 };
            ms.CreateRezeptionist(r1);
        }

        [TestCleanup]
        public void After()
        {
            ps.DeleteAll<Kunde>();
            ps.DeleteAll<Rezeptionist>();
        }

        [TestMethod]
        public void TestCreate()
        {
            ks.CreateKunde(k1, r1.ID);
            Assert.IsTrue(k1.Kundennummer != 0);
        }

        [TestMethod]
        public void TestFindKunde()
        {
            ks.CreateKunde(k1, r1.ID);

            Kunde k2 = ks.FindKundeById(k1.Kundennummer);
            Assert.AreEqual(k1, k2);
        }

        [TestMethod]
        public void TestGetKundenByIds()
        {
            ks.CreateKunde(k1, r1.ID);
            Kunde k2 = new Kunde()
                       {
                            Vorname = "Klaus",
                            Nachname = "ddd",
                            Adresse = new AdressTyp("Berliner Tor", "7", "22091", "Hamburg"),
                            EmailAdresse = new EmailTyp("dds@test.de"),
                            Geburtsdatum = new DateTime(1990, 01, 01),
                            Kundenstatus = Kundenstatus.Basic,
                            Telefonnummer = "123456"
                        };
            ks.CreateKunde(k2, r1.ID);
            IList<Kunde> kunden = ((IKundenServicesFuerKurse) ks).GetKundenByIds(new []{k1.Kundennummer, k2.Kundennummer}.ToList());
            //Assert.AreEqual(k1, k2);
        }

        [TestMethod]
        public void TestUpdate()
        {
            ks.CreateKunde(k1, r1.ID);

            k1.Nachname = "Neuer Nachname";
            ks.UpdateKunde(k1);

            Kunde k2 = ks.FindKundeById(k1.Kundennummer);
            Assert.AreEqual(k1, k2);
            //Assert.IsTrue(k1.Kundennummer != 0);
        }

        [TestMethod]
        public void TestDelete()
        {
            ks.CreateKunde(k1, r1.ID);
            ks.DeleteKunde(k1);
            Assert.AreEqual(null, ks.FindKundeById(k1.Kundennummer));
        }

        [TestMethod]
        public void TestSetzeKundenstatus()
        {
            ks.CreateKunde(k1, r1.ID);
            ks.SetzeKundenStatus(k1, Kundenstatus.Premium);
            Assert.IsTrue(k1.Kundennummer != 0);
        }
    }
}
