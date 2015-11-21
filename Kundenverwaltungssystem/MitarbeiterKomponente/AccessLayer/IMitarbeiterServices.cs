using MitarbeiterKomponente.DataAccessLayer.Entities;

namespace MitarbeiterKomponente.AccessLayer
{
    public interface IMitarbeiterServices
    {
        Rezeptionist CreateRezeptionist(Rezeptionist r);
        Trainer CreateTrainer(Trainer t);

        Rezeptionist FindRezeptionistById(int id);
        Trainer FindTrainerById(int id);

        Rezeptionist UpdateRezeptionist(Rezeptionist r);
        Trainer UpdateTrainer(Trainer t);

        void DeleteRezeptionist(Rezeptionist r);
        void DeleteTrainer(Trainer t);
    }
}