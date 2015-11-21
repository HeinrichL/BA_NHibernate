using System;

namespace Kundenkomponente.DataAccessLayer.Datatypes
{
    [Serializable]
    public class EmailTyp
    {
        public string Email { get; }

        public EmailTyp(string email)
        {
            if (EmailValid(email))
                Email = email;
            else
                throw new ArgumentException($"Email {email} hat ein ungüliges Format.");
        }

        private bool EmailValid(string mail)
        {
            return true;
        }
    }
}
