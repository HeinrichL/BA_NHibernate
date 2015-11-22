using System;
using System.Collections.Generic;
using System.Linq;
using Kundenkomponente.Accesslayer;
using Kundenkomponente.DataAccessLayer.Datatypes;
using Kundenkomponente.DataAccessLayer.Entities;
using MitarbeiterKomponente.AccessLayer;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using PersistenceService;
using Rechnungskomponente.DataAccessLayer.Datatypes;
using Rechnungskomponente.DataAccessLayer.Entities;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //IPersistenceService ps = new HibernateService();
            //ITransactionService ts = (ITransactionService) ps;
            //IMitarbeiterServices ms = new MitarbeiterkomponenteFacade(ps, ts);
            //IKundenServices ks = new KundenkomponenteFacade(ps, ts, ms as IMitarbeiterServicesFuerKunden);
            //IKundenServicesFuerKurse ksk = ks as IKundenServicesFuerKurse;

            //Rezeptionist r = new Rezeptionist()
            //                 {
            //                     Vorname = "Hee",
            //                     Nachname = "Haa",
            //                     Personalnummer = "1234856"
            //                 };
            //ms.CreateRezeptionist(r);

            //Kunde k = new Kunde()
            //            {
            //                Vorname = "Klaus",
            //                Nachname = "Müller",
            //                Adresse = new AdressTyp("Berliner Tor", "7", "22091", "Hamburg"),
            //                EmailAdresse = new EmailTyp("bla@test.de"),
            //                Geburtsdatum = new DateTime(1990, 01, 01),
            //                Kundenstatus = Kundenstatus.Basic,
            //                Telefonnummer = "123456"
            //            };
            //ks.CreateKunde(k, r.ID);
            //ksk.GetKundenByIds(new List<int>(new [] {1,2,3,4}));
            var a1 = new AbrechnungsZeitraumTyp(12, 2012);
            var a2 = new AbrechnungsZeitraumTyp(12, 2012);
            Console.WriteLine(a1.Equals(a2));
            List<Rechnungsposition> rPos = new List<Rechnungsposition>();
            List<Rechnungsposition> rPos2 = new List<Rechnungsposition>();

            Rechnung r1 = new Rechnung();
            r1.Rechnungspositionen = rPos;
            Rechnung r2 = new Rechnung();
            r2.Rechnungspositionen = rPos2;

            Console.WriteLine(r1.Equals(r2));
            Console.WriteLine(rPos.SequenceEqual(rPos2));
            Console.ReadLine();
        }
    }
}
