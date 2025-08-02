namespace Reseller.Domain.Data.ValueObjects
{
    public sealed class CnpjValueObject
    {
        public const int Length = 14;
        public string Value { get; }

        public CnpjValueObject(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("CNPJ não pode ser vazio.");

            var apenasNumeros = SomenteNumeros(valor);

            if (apenasNumeros.Length != Length)
                throw new ArgumentException($"CNPJ deve conter {Length} dígitos.");

            if (!Validar(apenasNumeros))
                throw new ArgumentException("CNPJ inválido.");

            Value = apenasNumeros;
        }

        private static string SomenteNumeros(string valor)
        {
            return new string(valor.Where(char.IsDigit).ToArray());
        }

        private static bool Validar(string valor)
        {
            if(valor.All(c => c.Equals('0'))) return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var temp = valor.Substring(0, 12);
            var soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(temp[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;

            temp += digito1;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(temp[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;

            return valor.EndsWith($"{digito1}{digito2}");
        }
    }
}