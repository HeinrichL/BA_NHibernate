using FluentNHibernate.Mapping;

namespace MitarbeiterKomponente.DataAccessLayer.Entities
{
    public class Rezeptionist : Mitarbeiter
    {
         public Rezeptionist() { }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (typeof(Rezeptionist) != obj.GetType()) return false;

            Rezeptionist r = (Rezeptionist) obj;

            return ID == r.ID &&
                   Vorname == r.Vorname &&
                   Nachname == r.Nachname;
        }
    }

    public class RezeptionistMap : SubclassMap<Rezeptionist>
    {
        public RezeptionistMap()
        {
            Table("Rezeptionist");
        }
    }
}