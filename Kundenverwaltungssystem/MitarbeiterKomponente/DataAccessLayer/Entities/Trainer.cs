using FluentNHibernate.Mapping;

namespace MitarbeiterKomponente.DataAccessLayer.Entities
{
    public class Trainer : Mitarbeiter
    {
        public Trainer() { }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (typeof(Trainer) != obj.GetType()) return false;

            Trainer t = (Trainer)obj;

            return ID == t.ID &&
                   Vorname == t.Vorname &&
                   Nachname == t.Nachname;
        }
    }

    public class TrainerMap : ClassMap<Trainer>
    {
        public TrainerMap()
        {
            Id(x => x.ID).GeneratedBy.Increment();

            Map(x => x.Vorname);
            Map(x => x.Nachname);
        }
    }
}