using System;

namespace Kundenkomponente.DataAccessLayer.Datatypes
{
    [Serializable]
    public class AdressTyp
    {
        public string Strasse { get; }
        public string Hausnummer { get; }
        public string PLZ { get; }
        public string Ort { get; }

        public AdressTyp(string strasse, string hausnummer, string plz, string ort)
        {
            Strasse = strasse;
            Hausnummer = hausnummer;
            PLZ = plz;
            Ort = ort;
        }
    }
}
