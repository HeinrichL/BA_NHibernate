using System.Collections.Generic;
using KursKomponente.DataAccessLayer;

namespace KursKomponente.AccessLayer
{
    public interface IKursServices
    {
        Kurs CreateKurs(Kurs kurs, int idRezeptionist, int idTrainer);
        Kurs FindKursById(int id);
        IList<Kurs> GetAlleKurse();
        IList<Kurs> GetKurseByVeranstaltungszeit(int monat, int jahr);
        Kurs UpdateKurs(Kurs kurs);
        void DeleteKurs(Kurs kurs);

        void BucheKurs(int idKunde, Kurs kurs);
        void BucheKurs(List<int> idKunden, Kurs kurs);
        void BucheKundenAufAnderenKursUm(int idKunde, Kurs kursVon, Kurs kursNach);
        void BucheKundenAufAnderenKursUm(List<int> idKunden, Kurs kursVon, Kurs kursNach);
    }
}