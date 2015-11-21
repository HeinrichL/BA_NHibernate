using System;

namespace KursKomponente.AccessLayer.Exceptions
{
    public class KursUeberfuelltException : Exception
    {
         public KursUeberfuelltException(string message) : base(message) { }
    }
}