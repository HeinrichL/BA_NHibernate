using Kundenkomponente.DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Kundenkomponente.Accesslayer
{
    public interface IKundenServices
    {
        Kunde CreateKunde(Kunde k, int mitarbeiterId);

        Kunde FindKundeById(int id);

        IList<Kunde> GetAlleKunden();

        Kunde UpdateKunde(Kunde k);

        void DeleteKunde(Kunde k);

        Kunde SetzeKundenStatus(Kunde k, Kundenstatus status);
    }
}
