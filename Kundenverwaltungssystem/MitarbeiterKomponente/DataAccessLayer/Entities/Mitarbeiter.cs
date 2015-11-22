using Common;

namespace MitarbeiterKomponente.DataAccessLayer.Entities
{
    public class Mitarbeiter
    {
        public virtual int ID { get; set; }
        public virtual string Personalnummer { get; set; }
        public virtual string Vorname { get; set; }
        public virtual string Nachname { get; set; }
    }
}