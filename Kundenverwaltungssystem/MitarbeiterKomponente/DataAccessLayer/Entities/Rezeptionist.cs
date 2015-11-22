using Common;
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
                   Nachname == r.Nachname &&
                   Personalnummer == r.Personalnummer;
        }
    }

    public class RezeptionistMap : ClassMap<Rezeptionist>
    {
        public RezeptionistMap()
        {
            Id(x => x.ID).GeneratedBy.Increment();

            Map(x => x.Personalnummer).Unique();
            Map(x => x.Vorname);
            Map(x => x.Nachname);
        }
    }
}