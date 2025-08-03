namespace Shared
{
    public static class Validations
    {
        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            var numbers = cpf.Select(x => int.Parse(x.ToString())).ToArray();

            var sum1 = Enumerable.Range(0, 9).Sum(i => numbers[i] * (10 - i));
            var check1 = sum1 % 11 < 2 ? 0 : 11 - sum1 % 11;

            var sum2 = Enumerable.Range(0, 10).Sum(i => numbers[i] * (11 - i));
            var check2 = sum2 % 11 < 2 ? 0 : 11 - sum2 % 11;

            return numbers[9] == check1 && numbers[10] == check2;
        }

        public static bool IsValidCnpj(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj)) return false;


            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1)
                return false;

            var numbers = cnpj.Select(x => int.Parse(x.ToString())).ToArray();
            var mult1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var mult2 = new int[] { 6 }.Concat(mult1).ToArray();

            int sum1 = mult1.Select((m, i) => m * numbers[i]).Sum();
            int check1 = sum1 % 11 < 2 ? 0 : 11 - sum1 % 11;

            int sum2 = mult2.Select((m, i) => m * numbers[i]).Sum();
            int check2 = sum2 % 11 < 2 ? 0 : 11 - sum2 % 11;

            return numbers[12] == check1 && numbers[13] == check2;
        }

        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}