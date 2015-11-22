using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Kundenkomponente.DataAccessLayer.Datatypes
{
    [Serializable]
    public struct EmailTyp
    {
        public string Email { get; }

        public EmailTyp(string email)
        {
            if (EmailValid(email))
                Email = email;
            else
                throw new ArgumentException($"Email {email} hat ein ungüliges Format.");
        }

        private static bool EmailValid(string mail)
        {
            return Regex.IsMatch(mail, @"^[\w\.\-]+@[\w\-]+\.(\w){2,3}$");
        }
    }
}
