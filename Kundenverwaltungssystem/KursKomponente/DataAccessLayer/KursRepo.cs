using System.Collections.Generic;
using System.Linq;
using PersistenceService;

namespace KursKomponente.DataAccessLayer.Repository
{
    public class KursRepo
    {
        private IPersistenceService ps;

        public KursRepo(IPersistenceService ps)
        {
            this.ps = ps;
        }

        public Kurs Save(Kurs kurs)
        {
            return ps.Save(kurs);
        }

        public Kurs FindById(int id)
        {
            return ps.GetById<Kurs, int>(id);
        }

        public IList<Kurs> GetAllKurse()
        {
            return ps.GetAll<Kurs>();
        }

        public IList<Kurs> GetKurseByVeransaltungszeit(int monat, int jahr)
        {
            //return (from kurse in ps.Query<Kurs>()
            //        where kurse.Veranstaltungszeit.StartZeitpunkt.Month == monat
            //              && kurse.Veranstaltungszeit.StartZeitpunkt.Year == jahr
            //        select kurse).ToList();
            return GetAllKurse().Where(k => k.Veranstaltungszeit.StartZeitpunkt.Month == monat &&
            k.Veranstaltungszeit.StartZeitpunkt.Year == jahr).ToList();
        }

        public void DeleteKurs(Kurs kurs)
        {
            ps.Delete(kurs);
        }


    }
}