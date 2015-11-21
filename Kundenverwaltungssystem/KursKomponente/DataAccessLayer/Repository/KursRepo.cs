using System.Collections.Generic;
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

        public void DeleteKurs(Kurs kurs)
        {
            ps.Delete(kurs);
        }
    }
}