namespace Reseller.Domain.Data.ValueObjects
{
    public class TelefoneValueObject
    {
        public const int MaxLength = 11;

        public const int MinLength = 10;

        public string Value { get; }

        public TelefoneValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Telefone não pode ser vazio.");

            var phoneNumbers = GetPhoneNumbers(value);

            if (!IsValidPhone(phoneNumbers))
                throw new ArgumentException("Telefone inválido.");

            Value = phoneNumbers;
        }

        private static string GetPhoneNumbers(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }

        private static bool IsValidPhone(string value)
        {
            return value.Length == MinLength || value.Length == MaxLength;
        }

    }
}