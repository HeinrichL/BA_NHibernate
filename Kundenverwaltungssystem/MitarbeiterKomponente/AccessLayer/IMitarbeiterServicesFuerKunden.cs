using MitarbeiterKomponente.DataAccessLayer.Entities;

namespace MitarbeiterKomponente.AccessLayer
{
    public interface IMitarbeiterServicesFuerKunden
    {
        Rezeptionist FindRezeptionistById(int id);
        Trainer FindTrainerById(int id);
    }
}