using System.Collections.Generic;
using KursKomponente.DataAccessLayer;

namespace KursKomponente.AccessLayer
{
    public interface IKursServicesFuerRechnungen
    {
        Kurs FindKursById(int id);
        IList<Kurs> GetKurseByVeranstaltungszeit(int monat, int jahr);
    }
}