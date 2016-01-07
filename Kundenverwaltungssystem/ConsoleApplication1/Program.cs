using System;
using Kundenkomponente.DataAccessLayer.Datatypes;
using Kundenkomponente.DataAccessLayer.Entities;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using AnwendungskernFassade;

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
                Adresse = new AdressTyp("Berliner Tor", "7", "22091", "Hamburg"),
                EmailAdresse = new EmailTyp("bla@test.de"),
                Geburtsdatum = new DateTime(1990, 01, 01),
                Kundenstatus = Kundenstatus.Basic,
                Telefonnummer = "123456"
            };
            anwendungskern.CreateKunde(k, r.ID);

            Console.ReadLine();
        }
    }
}
