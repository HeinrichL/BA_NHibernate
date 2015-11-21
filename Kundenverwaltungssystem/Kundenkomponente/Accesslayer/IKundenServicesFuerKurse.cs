using Kundenkomponente.DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Kundenkomponente.Accesslayer
{
    public interface IKundenServicesFuerKurse
    {
        Kunde FindKundeById(int id);

        IList<Kunde> GetAlleKunden();
        IList<Kunde> GetKundenByIds(List<int> ids);
    }
}
