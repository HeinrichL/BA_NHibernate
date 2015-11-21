using MitarbeiterKomponente.DataAccessLayer.Entities;

namespace MitarbeiterKomponente.AccessLayer
{
    public interface IMitarbeiterServicesFuerKurse
    {
        Rezeptionist FindRezeptionistById(int id);
        Trainer FindTrainerById(int id);
    }
}