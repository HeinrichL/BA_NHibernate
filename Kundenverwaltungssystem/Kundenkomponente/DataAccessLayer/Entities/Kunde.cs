using FluentNHibernate.Mapping;
using Kundenkomponente.DataAccessLayer.Datatypes;
using System;
using MitarbeiterKomponente.DataAccessLayer.Entities;
using Common;

namespace Kundenkomponente.DataAccessLayer.Entities
{
    public enum Kundenstatus
    {
        Basic, Premium, Gekuendigt
    }

    public class Kunde : Entity
    {
        public virtual int Kundennummer { get; set; }
        public virtual string Vorname { get; set; }
        public virtual string Nachname { get; set; }
        public virtual AdresseTyp Adresse { get; set; }
        public virtual EmailTyp EmailAdresse { get; set; }
        public virtual string Telefonnummer { get; set; }
        public virtual DateTime Geburtsdatum { get; set; }
        public virtual Kundenstatus Kundenstatus { get; set; }
        public virtual Rezeptionist AngelegtVon { get; set; }

        public Kunde() { }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (typeof(Kunde) != obj.GetType()) return false;

            Kunde k = (Kunde)obj;

            return Kundennummer == k.Kundennummer &&
                   Vorname == k.Vorname &&
                   Nachname == k.Nachname &&
                   Equals(Adresse, k.Adresse) &&
                   Equals(EmailAdresse, k.EmailAdresse) &&
                   Telefonnummer == k.Telefonnummer &&
                   Geburtsdatum == k.Geburtsdatum &&
                   Kundenstatus == k.Kundenstatus &&
                   Equals(AngelegtVon, k.AngelegtVon);
        }
    }

    public class KundeMap : ClassMap<Kunde>
    {
        public KundeMap()
        {
            Id(x => x.Kundennummer).GeneratedBy.Increment();
            Map(x => x.Vorname).Not.Nullable();
            Map(x => x.Nachname);
            Map(x => x.Adresse);
            Map(x => x.EmailAdresse);
            Map(x => x.Telefonnummer);
            Map(x => x.Geburtsdatum);
            Map(x => x.Kundenstatus);
            Version(x => x.Version);
            OptimisticLock.All().DynamicUpdate();
            //DynamicUpdate();
            References(x => x.AngelegtVon);
        }
    }
}
