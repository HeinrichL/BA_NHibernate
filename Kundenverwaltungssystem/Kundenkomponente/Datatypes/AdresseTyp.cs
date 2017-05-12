using System;

namespace Kundenkomponente.DataAccessLayer.Datatypes
{
    [Serializable]
    public class AdresseTyp
    {
        public string Strasse { get; private set; }
        public string Hausnummer { get; private set; }
        public string PLZ { get; private set; }
        public string Ort { get; private set; }

        public AdresseTyp(string strasse, string hausnummer, string plz, string ort)
        {
            Strasse = strasse;
            Hausnummer = hausnummer;
            PLZ = plz;
            Ort = ort;
        }

        private AdresseTyp() { }
    }
}
