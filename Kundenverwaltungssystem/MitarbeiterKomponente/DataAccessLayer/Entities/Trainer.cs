using FluentNHibernate.Mapping;

namespace MitarbeiterKomponente.DataAccessLayer.Entities
{
    public class Trainer : Mitarbeiter
    {
         public Trainer() { }
    }

    public class TrainerMap : ClassMap<Trainer>
    {
        public TrainerMap()
        {
            Id(x => x.ID).GeneratedBy.Increment();

            Map(x => x.Personalnummer).Unique();
            Map(x => x.Vorname);
            Map(x => x.Nachname);
        }
    }
}