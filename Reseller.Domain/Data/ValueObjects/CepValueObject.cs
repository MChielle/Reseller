namespace Reseller.Domain.Data.ValueObjects
{
    public class CepValueObject
    {
        public const int Length = 8;
        public string Value { get; }

        public CepValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("CEP não pode ser vazio.");

            var cepNumbers = GetCepNumbers(value);

            if (!IsValidCep(cepNumbers))
                throw new ArgumentException("CEP inválido.");

            Value = cepNumbers;
        }

        private static string GetCepNumbers(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }

        private static bool IsValidCep(string value)
        {
            return value.Length == Length;
        }
    }
}