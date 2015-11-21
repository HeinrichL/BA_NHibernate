using FluentNHibernate.Mapping;

namespace MitarbeiterKomponente.DataAccessLayer.Entities
{
    public class Rezeptionist : Mitarbeiter
    {
         public Rezeptionist() { }
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