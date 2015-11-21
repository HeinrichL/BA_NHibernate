using Kundenkomponente.DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Kundenkomponente.Accesslayer
{
    public interface IKundenServicesFuerRechnungen
    {
        Kunde FindKundeById(int id);

        IList<Kunde> GetAlleKunden();
    }
}
