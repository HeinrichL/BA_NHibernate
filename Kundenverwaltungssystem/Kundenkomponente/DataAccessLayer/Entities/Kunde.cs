﻿using FluentNHibernate.Mapping;
using Kundenkomponente.DataAccessLayer.Datatypes;
using System;
using MitarbeiterKomponente.DataAccessLayer.Entities;

namespace Kundenkomponente.DataAccessLayer.Entities
{
    public enum Kundenstatus
    {
        Basic, Premium, Gekuendigt
    }

    public class Kunde
    {
        public virtual int Kundennummer { get; set; }
        public virtual string Vorname { get; set; }
        public virtual string Nachname { get; set; }
        public virtual AdressTyp Adresse { get; set; }
        public virtual EmailTyp EmailAdresse { get; set; }
        public virtual string Telefonnummer { get; set; }
        public virtual DateTime Geburtsdatum { get; set; }
        public virtual Kundenstatus Kundenstatus { get; set; }
        public virtual Rezeptionist AngelegtVon { get; set; }

        public Kunde() { }
    }

    public class KundeMap : ClassMap<Kunde>
    {
        public KundeMap()
        {
            Id(x => x.Kundennummer).GeneratedBy.Increment();
            Map(x => x.Vorname);
            Map(x => x.Nachname);
            Map(x => x.Adresse);
            Map(x => x.EmailAdresse);
            Map(x => x.Telefonnummer);
            Map(x => x.Geburtsdatum);
            Map(x => x.Kundenstatus);
            References(x => x.AngelegtVon).Cascade.SaveUpdate();
        }
    }
}