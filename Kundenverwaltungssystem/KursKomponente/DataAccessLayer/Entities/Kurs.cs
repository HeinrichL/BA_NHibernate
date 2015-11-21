using System.Collections.Generic;
using FluentNHibernate.Mapping;
using Kundenkomponente.DataAccessLayer.Entities;
using KursKomponente.DataAccessLayer.Datatypes;
using MitarbeiterKomponente.DataAccessLayer.Entities;

namespace KursKomponente.DataAccessLayer
{
    public enum Kursstatus
    {
        Geplant, FindetStatt, Vorbei, Abgesagt
    }

    public class Kurs
    {
        public virtual int ID { get; set; }
        public virtual string Titel { get; set; }
        public virtual string Beschreibung { get; set; }
        public virtual int MaximaleTeilnehmeranzahl { get; set; }
        public virtual IList<Kunde> Teilnehmer { get; set; }
        public virtual VeranstaltungszeitTyp Veranstaltungszeit { get; set; }
        public virtual Kursstatus Kursstatus { get; set; }
        public virtual Trainer Kursleiter { get; set; }
        public virtual Rezeptionist AngelegtVon { get; set; }

        public Kurs()
        {
            Teilnehmer = new List<Kunde>();
        }

        public virtual bool HatFreiePlaetze(int anzahl = 1)
        {
            return Teilnehmer.Count + anzahl <= MaximaleTeilnehmeranzahl;
        }
    }

    public class KursMap : ClassMap<Kurs>
    {
        public KursMap()
        {
            Id(x => x.ID).GeneratedBy.Increment();

            Map(x => x.Titel);
            Map(x => x.Beschreibung);
            Map(x => x.MaximaleTeilnehmeranzahl);
            Map(x => x.Veranstaltungszeit);
            Map(x => x.Kursstatus);

            HasManyToMany(x => x.Teilnehmer);

            References(x => x.Kursleiter).Cascade.SaveUpdate();
            References(x => x.AngelegtVon).Cascade.SaveUpdate();

        }
    }
}