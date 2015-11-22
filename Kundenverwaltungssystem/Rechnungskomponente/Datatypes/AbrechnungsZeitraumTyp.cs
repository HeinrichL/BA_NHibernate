using System;
using Common;

namespace Rechnungskomponente.DataAccessLayer.Datatypes
{
    [Serializable]
    public struct AbrechnungsZeitraumTyp
    {
        public int Monat { get; }
        public int Jahr { get; }

        public AbrechnungsZeitraumTyp(int monat, int jahr)
        {
            if(IstGueltigerMonat(monat) && IstGueltigesJahr(jahr))
            {
                Monat = monat;
                Jahr = jahr;
            }
            else
            {
                throw new ArgumentException($"{monat}/{jahr} ist kein gültiger Abrechnungszeitraum");
            }
        }

        public static bool IstGueltigerMonat(int monat)
        {
            return monat > 0 && monat <= 12;
        }

        public static bool IstGueltigesJahr(int jahr)
        {
            return jahr >= DateTime.MinValue.Year && jahr <= DateTime.MaxValue.Year;
        }

    }
}