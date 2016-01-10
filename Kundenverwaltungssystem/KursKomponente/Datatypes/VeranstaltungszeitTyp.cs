using System;

namespace KursKomponente.DataAccessLayer.Datatypes
{
    [Serializable]
    public class VeranstaltungszeitTyp
    {
        public DateTime StartZeitpunkt { get; private set; }
        public DateTime EndZeitpunkt { get; }

        public VeranstaltungszeitTyp(DateTime start, DateTime end)
        {
            StartZeitpunkt = start;
            EndZeitpunkt = end;
        }

        private VeranstaltungszeitTyp() { }
    }
}