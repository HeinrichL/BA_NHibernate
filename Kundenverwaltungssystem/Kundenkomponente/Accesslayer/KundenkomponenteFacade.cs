using System.Collections.Generic;
using System.Linq;
using Common;
using Kundenkomponente.DataAccessLayer.Entities;
using Kundenkomponente.DataAccessLayer.Repository;
using MitarbeiterKomponente.AccessLayer;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using PersistenceService;

namespace Kundenkomponente.Accesslayer
{
    public class KundenkomponenteFacade : IKundenServices, IKundenServicesFuerRechnungen, IKundenServicesFuerKurse
    {
        private readonly KundenRepo _kundenRepo;
        private readonly ITransactionService _transactionService;
        private readonly IMitarbeiterServicesFuerKunden _mitarbeiterServices;

        public KundenkomponenteFacade(IPersistenceService ps, ITransactionService ts, IMitarbeiterServicesFuerKunden ms)
        {
            _kundenRepo = new KundenRepo(ps);
            _transactionService = ts;
            _mitarbeiterServices = ms;
        }

        public Kunde CreateKunde(Kunde k, int mitarbeiterId)
        {
            Check.Argument(k != null, "Kunde darf nicht null sein");
            Check.Argument(k.Kundennummer == 0, "Kundennummer muss 0 sein");
            Check.Argument(mitarbeiterId != 0, "Mitarbeiter ID darf nicht 0 sein");

            Rezeptionist r = _mitarbeiterServices.FindRezeptionistById(mitarbeiterId);
            k.AngelegtVon = r;

            return _kundenRepo.SaveKunde(k);
        }

        public Kunde FindKundeById(int id)
        {
            Check.Argument(id > 0, $"Kundennummer muss größer als 0 sein, ist aber {id}");

            return _kundenRepo.FindKundeById(id);
        }

        public Kunde UpdateKunde(Kunde k)
        {
            Check.Argument(k != null, "Kunde darf nicht null sein");
            Check.Argument(k.Kundennummer > 0, $"Kundennummer muss größer als 0 sein, ist aber {k.Kundennummer}");
            Check.Argument(_kundenRepo.FindKundeById(k.Kundennummer) != null, $"Kunde mit der Kundennummer {k.Kundennummer} existiert nicht in der Datenbank");

            return _kundenRepo.SaveKunde(k);
        }

        public void DeleteKunde(Kunde k)
        {
            Check.Argument(k != null, "Kunde darf nicht null sein");
            Check.Argument(k.Kundennummer > 0, $"Kundennummer muss größer als 0 sein, ist aber {k.Kundennummer}");
            _kundenRepo.DeleteKunde(k);
        }

        public Kunde SetzeKundenStatus(Kunde k, Kundenstatus status)
        {
            Check.Argument(k != null, "Kunde darf nicht null sein");
            k.Kundenstatus = status;
            return UpdateKunde(k);
        }

        public IList<Kunde> GetAlleKunden()
        {
            return _kundenRepo.GetAlleKunden();
        }

        public IList<Kunde> GetKundenByIds(List<int> ids)
        {
            Check.Argument(ids.All(x => x != 0), "Kundennummern dürfen nicht 0 sein");

            return _kundenRepo.GetKundenByIds(ids);
        }
    }
}
