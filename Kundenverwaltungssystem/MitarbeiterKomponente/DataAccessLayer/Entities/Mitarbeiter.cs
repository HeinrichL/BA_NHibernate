using FluentNHibernate.Mapping;

namespace MitarbeiterKomponente.DataAccessLayer.Entities
{
    public class Mitarbeiter
    {
        public virtual int ID { get; set; }
        public virtual string Vorname { get; set; }
        public virtual string Nachname { get; set; }
    }

    public class MitarbeiterMap : ClassMap<Mitarbeiter>
    {
        public MitarbeiterMap()
        {
            Id(x => x.ID).GeneratedBy.Increment();
            Map(x => x.Vorname);
            Map(x => x.Nachname);
            UseUnionSubclassForInheritanceMapping();
        }
    }
}