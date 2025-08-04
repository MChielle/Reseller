namespace Reseller.Domain.Models.RevendaModels.GetByCnpj
{
    public sealed class RevendaGetByIdModelResponse
    {
        public RevendaGetByIdModelResponse(string cnpj, string razaoSocial, string nomeFantasia, string email)
        {
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            Email = email;
        }

        public string Cnpj { get; set; }

        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; }

        public string Email { get; set; }
    }
}