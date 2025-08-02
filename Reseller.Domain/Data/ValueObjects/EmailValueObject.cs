using System.Net.Mail;

namespace Reseller.Domain.Data.ValueObjects
{
    public sealed class EmailValueObject
    {
        public const int MaxLength = 100;
        public string Value { get; }

        public EmailValueObject(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Email não pode ser vazio.");

            var email = valor.Trim().ToLowerInvariant();

            if (!IsValidEmail(email))
                throw new ArgumentException("Email inválido.");

            Value = email;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
