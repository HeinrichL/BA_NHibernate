using System.Collections.Generic;
using Kundenkomponente.Accesslayer;
using Kundenkomponente.DataAccessLayer.Entities;
using KursKomponente.AccessLayer;
using KursKomponente.DataAccessLayer;
using MitarbeiterKomponente.AccessLayer;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using PersistenceService;
using Rechnungskomponente.AccessLayer;
using Rechnungskomponente.DataAccessLayer.Datatypes;
using Rechnungskomponente.DataAccessLayer.Entities;

namespace AnwendungskernFassade
{
    public class AnwendungskernFacade : IMitarbeiterServices, IKundenServices, IKursServices, IRechnungsServices
    {
        private IMitarbeiterServices mitarbeiterServices;
        private IKundenServices kundenServices;
        private IKursServices kursServices;
        private IRechnungsServices rechnungsServices;

        public AnwendungskernFacade()
        {
            IPersistenceService persistenceService = new HibernateService();
            ITransactionService transactionService = (ITransactionService)persistenceService;
            mitarbeiterServices = new MitarbeiterkomponenteFacade(persistenceService, transactionService);
            kundenServices = new KundenkomponenteFacade(persistenceService, transactionService, mitarbeiterServices as IMitarbeiterServicesFuerKunden);
            kursServices = new KurskomponenteFacade(persistenceService, transactionService, kundenServices as IKundenServicesFuerKurse, mitarbeiterServices as IMitarbeiterServicesFuerKurse);
            rechnungsServices = new RechnungskomponenteFacade(persistenceService, transactionService, kursServices as IKursServicesFuerRechnungen);
        }

        #region Mitarbeiterkomponente
        public Rezeptionist CreateRezeptionist(Rezeptionist r)
        {
            return mitarbeiterServices.CreateRezeptionist(r);
        }

        public Trainer CreateTrainer(Trainer t)
        {
            return mitarbeiterServices.CreateTrainer(t);
        }

        public Rezeptionist FindRezeptionistById(int id)
        {
            return mitarbeiterServices.FindRezeptionistById(id);
        }

        public Trainer FindTrainerById(int id)
        {
            return mitarbeiterServices.FindTrainerById(id);
        }

        public Rezeptionist UpdateRezeptionist(Rezeptionist r)
        {
            return mitarbeiterServices.UpdateRezeptionist(r);
        }

        public Trainer UpdateTrainer(Trainer t)
        {
            return mitarbeiterServices.UpdateTrainer(t);
        }

        public void DeleteRezeptionist(Rezeptionist r)
        {
            mitarbeiterServices.DeleteRezeptionist(r);
        }

        public void DeleteTrainer(Trainer t)
        {
            mitarbeiterServices.DeleteTrainer(t);
        }
        #endregion

        #region Kundenkomponente

        public Kunde CreateKunde(Kunde k, int mitarbeiterId)
        {
            return kundenServices.CreateKunde(k, mitarbeiterId);
        }

        public Kunde FindKundeById(int id)
        {
            return kundenServices.FindKundeById(id);
        }

        public IList<Kunde> GetAlleKunden()
        {
            return kundenServices.GetAlleKunden();
        }

        public Kunde UpdateKunde(Kunde k)
        {
            return kundenServices.UpdateKunde(k);
        }

        public void DeleteKunde(Kunde k)
        {
            kundenServices.DeleteKunde(k);
        }

        public Kunde SetzeKundenStatus(Kunde k, Kundenstatus status)
        {
            return kundenServices.SetzeKundenStatus(k, status);
        }

        #endregion

        #region Kurskomponente

        public Kurs CreateKurs(Kurs kurs, int idRezeptionist, int idTrainer)
        {
            return kursServices.CreateKurs(kurs, idRezeptionist, idTrainer);
        }

        public Kurs FindKursById(int id)
        {
            return kursServices.FindKursById(id);
        }

        public IList<Kurs> GetAlleKurse()
        {
            return kursServices.GetAlleKurse();
        }

        public IList<Kurs> GetKurseByVeranstaltungszeit(int monat, int jahr)
        {
            return kursServices.GetKurseByVeranstaltungszeit(monat, jahr);
        }

        public Kurs UpdateKurs(Kurs kurs)
        {
            return kursServices.UpdateKurs(kurs);
        }

        public void DeleteKurs(Kurs kurs)
        {
            kursServices.DeleteKurs(kurs);
        }

        public void BucheKurs(int idKunde, Kurs kurs)
        {
            kursServices.BucheKurs(idKunde, kurs);
        }

        public void BucheKurs(List<int> idKunden, Kurs kurs)
        {
            kursServices.BucheKurs(idKunden, kurs);
        }

        public void BucheKundenAufAnderenKursUm(int idKunde, Kurs kursVon, Kurs kursNach)
        {
            kursServices.BucheKundenAufAnderenKursUm(idKunde, kursVon, kursNach);
        }

        public void BucheKundenAufAnderenKursUm(List<int> idKunden, Kurs kursVon, Kurs kursNach)
        {
            kursServices.BucheKundenAufAnderenKursUm(idKunden, kursVon, kursNach);
        }
        #endregion

        #region Rechnungskomponente
        public IList<Rechnung> ErstelleRechnungen()
        {
            return rechnungsServices.ErstelleRechnungen();
        }

        public Rechnung FindRechnungById(int id)
        {
            return rechnungsServices.FindRechnungById(id);
        }

        public IList<Rechnung> GetAlleRechnungen()
        {
            return rechnungsServices.GetAlleRechnungen();
        }

        public IList<Rechnung> GetRechnungByAbrechnungsZeitraum(AbrechnungsZeitraumTyp abrechnungsZeitraum)
        {
            return rechnungsServices.GetRechnungByAbrechnungsZeitraum(abrechnungsZeitraum);
        }
        #endregion
    }
}
