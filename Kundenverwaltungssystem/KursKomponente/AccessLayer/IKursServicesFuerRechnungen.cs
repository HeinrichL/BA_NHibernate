﻿using System.Collections.Generic;
using KursKomponente.DataAccessLayer;

namespace KursKomponente.AccessLayer
{
    public interface IKursServicesFuerRechnungen
    {
        IList<Kurs> GetKurseByVeranstaltungszeit(int monat, int jahr);
    }
}