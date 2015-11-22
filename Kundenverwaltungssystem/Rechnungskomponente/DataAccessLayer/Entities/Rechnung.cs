using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Common;
using FluentNHibernate.Mapping;
using Kundenkomponente.DataAccessLayer.Entities;
using KursKomponente.DataAccessLayer;
using Rechnungskomponente.DataAccessLayer.Datatypes;

namespace Rechnungskomponente.DataAccessLayer.Entities
{
    public class Rechnung
    {
        public virtual int Rechnungsnummer { get; set; }
        public virtual Kunde Kunde { get; set; }
        public virtual AbrechnungsZeitraumTyp AbrechnungsZeitraum { get; set; }
        public virtual bool Bezahlt { get; set; }
        public virtual IList<Rechnungsposition> Rechnungspositionen { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (typeof(Rechnung) != obj.GetType()) return false;

            Rechnung r = (Rechnung)obj;

            return Rechnungsnummer == r.Rechnungsnummer &&
                   Equals(Kunde, r.Kunde) &&
                   Equals(AbrechnungsZeitraum, r.AbrechnungsZeitraum) &&
                   Bezahlt == r.Bezahlt &&
                   Rechnungspositionen.SequenceEqual(r.Rechnungspositionen);
        }
    }

    public class Rechnungsposition
    {
        public virtual int Rechnungspositionsnummer { get; set; }
        public virtual decimal Kosten { get; set; }
        public virtual Kurs Kurs { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (typeof(Rechnungsposition) != obj.GetType()) return false;

            Rechnungsposition r = (Rechnungsposition)obj;

            return Rechnungspositionsnummer == r.Rechnungspositionsnummer &&
                   Kosten == r.Kosten &&
                   Equals(Kurs, r.Kurs);
        }
    }

    public class RechnungMap : ClassMap<Rechnung>
    {
        public RechnungMap()
        {
            Id(x => x.Rechnungsnummer).GeneratedBy.Increment();

            Map(x => x.AbrechnungsZeitraum);
            Map(x => x.Bezahlt);

            References(x => x.Kunde);
            HasMany(x => x.Rechnungspositionen).Not.LazyLoad().Cascade.All();
        }
    }

    public class RechnungspositionMap : ClassMap<Rechnungsposition>
    {
        public RechnungspositionMap()
        {
            Id(x => x.Rechnungspositionsnummer).GeneratedBy.Increment();

            Map(x => x.Kosten);

            References(x => x.Kurs);
        }
    }
}