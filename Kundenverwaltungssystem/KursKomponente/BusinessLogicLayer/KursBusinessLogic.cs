using System.Collections.Generic;
using Kundenkomponente.Accesslayer;
using Kundenkomponente.DataAccessLayer.Entities;
using KursKomponente.AccessLayer.Exceptions;
using KursKomponente.DataAccessLayer;
using KursKomponente.DataAccessLayer.Repository;
using NHibernate.Util;
using PersistenceService;

namespace KursKomponente.BusinessLogicLayer
{
    public class KursBusinessLogic
    {
        private KursRepo kursRepo;
        private ITransactionService ts;
        private IKundenServicesFuerKurse kundenServices;

        public KursBusinessLogic(ITransactionService ts, IKundenServicesFuerKurse ks, KursRepo repo)
        {
            kursRepo = repo;
            this.ts = ts;
            kundenServices = ks;
        }

        public void BucheKurs(int idKunde, Kurs kurs)
        {
            ts.ExecuteInTransaction(() =>
            {
                if (kurs.HatFreiePlaetze())
                {
                    kurs.Teilnehmer.Add(kundenServices.FindKundeById(idKunde));
                    kursRepo.Save(kurs);
                }
                else
                {
                    throw new KursUeberfuelltException("Kurs ist bereits ausgebucht");
                }
            });
            
        }

        public void BucheKurs(List<int> idKunden, Kurs kurs)
        {
            ts.ExecuteInTransaction(() =>
            {
                if (kurs.HatFreiePlaetze(idKunden.Count))
                {
                    IList<Kunde> kunden = kundenServices.GetKundenByIds(idKunden);
                    kunden.ForEach(kunde => kurs.Teilnehmer.Add(kunde));
                    kursRepo.Save(kurs);
                }
                else
                {
                    throw new KursUeberfuelltException("Kurs ist bereits ausgebucht");
                }
            });
        }

        public void BucheKundenAufAnderenKursUm(int idKunde, Kurs kursVon, Kurs kursNach)
        {
            ts.ExecuteInTransaction(() =>
            {
                if(kursNach.HatFreiePlaetze())
                {
                    Kunde k = kundenServices.FindKundeById(idKunde);
                    kursVon.Teilnehmer.Remove(k);
                    kursNach.Teilnehmer.Add(k);
                    kursRepo.Save(kursVon);
                    kursRepo.Save(kursNach);
                }
                else
                {
                    throw new KursUeberfuelltException("Zielkurs ist bereits ausgebucht");
                }
            });
        }

        public void BucheKundenAufAnderenKursUm(List<int> idKunden, Kurs kursVon, Kurs kursNach)
        {
            ts.ExecuteInTransaction(() =>
            {
                if (kursNach.HatFreiePlaetze(idKunden.Count))
                {
                    IList<Kunde> kunden = kundenServices.GetKundenByIds(idKunden);
                    kunden.ForEach(kunde => kursVon.Teilnehmer.Remove(kunde));
                    kunden.ForEach(kunde => kursNach.Teilnehmer.Add(kunde));
                    kursRepo.Save(kursVon);
                    kursRepo.Save(kursNach);
                }
                else
                {
                    throw new KursUeberfuelltException("Zielkurs ist bereits ausgebucht");
                }
            });
        }
    }
}