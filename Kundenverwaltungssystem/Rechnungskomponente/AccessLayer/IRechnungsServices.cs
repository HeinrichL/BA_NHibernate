using System.Collections.Generic;
using Rechnungskomponente.DataAccessLayer.Datatypes;
using Rechnungskomponente.DataAccessLayer.Entities;

namespace Rechnungskomponente.AccessLayer
{
    public interface IRechnungsServices
    {
        IList<Rechnung> ErstelleRechnungen();
        Rechnung FindRechnungById(int id);
        IList<Rechnung> GetAlleRechnungen();
        IList<Rechnung> GetRechnungByAbrechnungsZeitraum(AbrechnungsZeitraumTyp abrechnungsZeitraum);
    }
}