using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Kundenkomponente.Accesslayer;
using KursKomponente.BusinessLogicLayer;
using KursKomponente.DataAccessLayer;
using KursKomponente.DataAccessLayer.Repository;
using MitarbeiterKomponente.AccessLayer;
using PersistenceService;

namespace KursKomponente.AccessLayer
{
    public class KurskomponenteFacade : IKursServices, IKursServicesFuerRechnungen
    {
        private IMitarbeiterServicesFuerKurse mitarbeiterServices;
        private KursBusinessLogic kursBusinessLogic;
        private KursRepo kursRepo;

        public KurskomponenteFacade(IPersistenceService ps, ITransactionService ts, IKundenServicesFuerKurse ks,
                                    IMitarbeiterServicesFuerKurse ms)
        {
            mitarbeiterServices = ms;
            kursRepo = new KursRepo(ps);
            kursBusinessLogic = new KursBusinessLogic(ts, ks, kursRepo);
        }

        public Kurs CreateKurs(Kurs kurs, int idRezeptionist, int idTrainer)
        {
            Check.Argument(kurs != null, "Kurs darf nicht null sein");
            Check.Argument(kurs.ID == 0, "KursID muss 0 sein");
            Check.Argument(idRezeptionist != 0, "Rezeptionist darf nicht 0 sein");
            Check.Argument(idTrainer != 0, "Trainer darf nicht 0 sein");

            kurs.AngelegtVon = mitarbeiterServices.FindRezeptionistById(idRezeptionist);
            kurs.Kursleiter = mitarbeiterServices.FindTrainerById(idTrainer);

            return kursRepo.Save(kurs);
        }

        public Kurs FindKursById(int id)
        {
            Check.Argument(id != 0, "ID darf nicht 0 sein");

            return kursRepo.FindById(id);
        }

        public IList<Kurs> GetAlleKurse()
        {
            return kursRepo.GetAllKurse();
        }

        public IList<Kurs> GetKurseByVeranstaltungszeit(int monat, int jahr)
        {
            Check.Argument(monat > 0 && monat <= 12, "Ungültiger Monat angegeben");
            Check.Argument(jahr > 0 && DateTime.MinValue.Year <= DateTime.MaxValue.Year, "Ungültiges Jahr angegeben");

            return kursRepo.GetKurseByVeransaltungszeit(monat, jahr);
        }

        public Kurs UpdateKurs(Kurs kurs)
        {
            Check.Argument(kurs != null, "Kurs darf nicht null sein");
            Check.Argument(kurs.ID != 0, "KursID darf nicht 0 sein");
            Check.Argument(kursRepo.FindById(kurs.ID) != null, $"Kurs mit ID {kurs.ID} existiert nicht in der Datenbank");

            return kursRepo.Save(kurs);
        }

        public void DeleteKurs(Kurs kurs)
        {
            Check.Argument(kurs != null, "Kurs darf nicht null sein");

            kursRepo.DeleteKurs(kurs);
        }

        public void BucheKurs(int idKunde, Kurs kurs)
        {
            Check.Argument(idKunde != 0, "Kundennummer darf nicht 0 sein");
            Check.Argument(kurs != null, "Kurs darf nicht null sein");

            kursBusinessLogic.BucheKurs(idKunde, kurs);
        }

        public void BucheKurs(List<int> idKunden, Kurs kurs)
        {
            Check.Argument(idKunden.All(x => x != 0), "Kundennummern dürfen nicht 0 sein");
            Check.Argument(kurs != null, "Kurs darf nicht null sein");

            kursBusinessLogic.BucheKurs(idKunden, kurs);
        }

        public void BucheKundenAufAnderenKursUm(int idKunde, Kurs kursVon, Kurs kursNach)
        {
            Check.Argument(idKunde != 0, "Kundennummer darf nicht 0 sein");
            Check.Argument(kursVon != null, "Kurs darf nicht null sein");
            Check.Argument(kursNach != null, "Zielkurs darf nicht null sein");

            kursBusinessLogic.BucheKundenAufAnderenKursUm(idKunde, kursVon, kursNach);
        }

        public void BucheKundenAufAnderenKursUm(List<int> idKunden, Kurs kursVon, Kurs kursNach)
        {
            Check.Argument(idKunden.All(x => x != 0), "Kundennummern dürfen nicht 0 sein");
            Check.Argument(kursVon != null, "Kurs darf nicht null sein");
            Check.Argument(kursNach != null, "Zielkurs darf nicht null sein");

            kursBusinessLogic.BucheKundenAufAnderenKursUm(idKunden, kursVon, kursNach);
        }
    }
}