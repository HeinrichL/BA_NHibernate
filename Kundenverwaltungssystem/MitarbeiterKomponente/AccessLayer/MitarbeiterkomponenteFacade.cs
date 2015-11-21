using Common;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using MitarbeiterKomponente.DataAccessLayer.Repository;
using PersistenceService;

namespace MitarbeiterKomponente.AccessLayer
{
    public class MitarbeiterkomponenteFacade: IMitarbeiterServices, IMitarbeiterServicesFuerKurse, IMitarbeiterServicesFuerKunden
    {
        private MitarbeiterRepo _mitarbeiterRepo;

        public MitarbeiterkomponenteFacade(IPersistenceService ps, ITransactionService ts)
        {
            _mitarbeiterRepo = new MitarbeiterRepo(ps);
        }

        public Rezeptionist CreateRezeptionist(Rezeptionist r)
        {
            Check.Argument(r != null, "Rezeptionist darf nicht null sein");
            Check.Argument(r.ID == 0, "ID muss 0 sein");

            return _mitarbeiterRepo.SaveRezeptionist(r);
        }

        public Trainer CreateTrainer(Trainer t)
        {
            Check.Argument(t != null, "Trainer darf nicht null sein");
            Check.Argument(t.ID == 0, "ID muss 0 sein");

            return _mitarbeiterRepo.SaveTrainer(t);
        }

        public Rezeptionist FindRezeptionistById(int id)
        {
            Check.Argument(id > 0, $"ID muss größer als 0 sein, ist aber {id}");

            return _mitarbeiterRepo.FindRezeptionistById(id);
        }

        public Trainer FindTrainerById(int id)
        {
            Check.Argument(id > 0, $"ID muss größer als 0 sein, ist aber {id}");

            return _mitarbeiterRepo.FindTrainerById(id);
        }

        public Rezeptionist UpdateRezeptionist(Rezeptionist r)
        {
            Check.Argument(r != null, "Rezeptionist darf nicht null sein");
            Check.Argument(r.ID > 0, $"ID muss größer als 0 sein, ist aber {r.ID}");
            Check.Argument(_mitarbeiterRepo.FindRezeptionistById(r.ID) != null, $"Rezeptionist mit der ID {r.ID} existiert nicht in der Datenbank");

            return _mitarbeiterRepo.SaveRezeptionist(r);
        }

        public Trainer UpdateTrainer(Trainer t)
        {
            Check.Argument(t != null, "Trainer darf nicht null sein");
            Check.Argument(t.ID > 0, $"ID muss größer als 0 sein, ist aber {t.ID}");
            Check.Argument(_mitarbeiterRepo.FindTrainerById(t.ID) != null, $"Trainer mit der ID {t.ID} existiert nicht in der Datenbank");

            return _mitarbeiterRepo.SaveTrainer(t);
        }

        public void DeleteRezeptionist(Rezeptionist r)
        {
            Check.Argument(r != null, "Rezeptionist darf nicht null sein");
            Check.Argument(r.ID > 0, $"ID muss größer als 0 sein, ist aber {r.ID}");

            _mitarbeiterRepo.DeleteRezeptionist(r);
        }

        public void DeleteTrainer(Trainer t)
        {
            Check.Argument(t != null, "Trainer darf nicht null sein");
            Check.Argument(t.ID > 0, $"ID muss größer als 0 sein, ist aber {t.ID}");

            _mitarbeiterRepo.DeleteTrainer(t);
        }
    }
}