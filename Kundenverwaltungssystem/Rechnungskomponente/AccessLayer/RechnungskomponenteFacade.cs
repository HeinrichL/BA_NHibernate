using System.Collections.Generic;
using Common;
using Kundenkomponente.Accesslayer;
using KursKomponente.AccessLayer;
using PersistenceService;
using Rechnungskomponente.BusinessLogicLayer;
using Rechnungskomponente.DataAccessLayer;
using Rechnungskomponente.DataAccessLayer.Datatypes;
using Rechnungskomponente.DataAccessLayer.Entities;

namespace Rechnungskomponente.AccessLayer
{
    public class RechnungskomponenteFacade : IRechnungsServices
    {
        private RechnungsRepo rechnungsRepo;
        private RechnungsBusinessLogic businessLogic;

        public RechnungskomponenteFacade(IPersistenceService ps, ITransactionService ts, IKursServicesFuerRechnungen ks)
        {
            rechnungsRepo = new RechnungsRepo(ps);
            businessLogic = new RechnungsBusinessLogic(ts, rechnungsRepo, ks);
        }

        public IList<Rechnung> ErstelleRechnungen()
        {
            return businessLogic.ErstelleRechnungen();
        }

        public Rechnung FindRechnungById(int id)
        {
            Check.Argument(id > 0, "ID muss größer als 0 sein");

            return rechnungsRepo.FindRechnungById(id);
        }

        public IList<Rechnung> GetAlleRechnungen()
        {
            return rechnungsRepo.GetAlleRechnungen();
        }

        public IList<Rechnung> GetRechnungByAbrechnungsZeitraum(AbrechnungsZeitraumTyp abrechnungsZeitraum)
        {
            return rechnungsRepo.GetRechnungenByAbrechnungszeitraum(abrechnungsZeitraum);
        }
    }
}