using System;
using Kundenkomponente.DataAccessLayer.Datatypes;
using Kundenkomponente.DataAccessLayer.Entities;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using AnwendungskernFassade;
using PersistenceService;
using System.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            AnwendungskernFacade anwendungskern = new AnwendungskernFacade();

            Rezeptionist r = new Rezeptionist()
            {
                Vorname = "Hee",
                Nachname = "Haa"
            };
            anwendungskern.CreateRezeptionist(r);

            Kunde k = new Kunde()
            {
                Vorname = "Klaus",
                Nachname = "Müller",
                Adresse = new AdresseTyp("Berliner Tor", "7", "22091", "Hamburg"),
                EmailAdresse = new EmailTyp("bla@test.de"),
                Geburtsdatum = new DateTime(1990, 01, 01),
                Kundenstatus = Kundenstatus.Basic,
                Telefonnummer = "123456"
            };
            //anwendungskern.CreateKunde(k, r.ID);

            AnwendungskernFacade aw2 = new AnwendungskernFacade();

            var k2 = aw2.FindRezeptionistById(r.ID);

            NHibernateService h = new NHibernateService();
            var ma = h.GetAll<Mitarbeiter>();

            var sasdads = h.Query<Kunde>().Count();

            k.AngelegtVon = r;
            h.Save(k);

            Console.ReadLine();
        }
    }
}
