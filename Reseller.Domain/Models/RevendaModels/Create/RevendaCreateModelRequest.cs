using Reseller.Domain.Data;
using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;
using Reseller.Domain.Models.RevendaModels.Create;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Models.RevendaModels.Input
{
    public class RevendaCreateModelRequest : IValidatableObject
    {
        [Required(ErrorMessage = "CNPJ é obrigatório.")]
        [MaxLength(CnpjValueObject.Length, ErrorMessage = "CNPJ deve ter 14 caracteres")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "Razão social é obrigatória.")]
        [MaxLength(RazaoSocialRevendaValueObject.MaxLength, ErrorMessage = "Razão Social deve ter no máximo 100 caracteres")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "Nome fantasia é obrigatório.")]
        [MaxLength(NomeFantasiaRevendaValueObject.MaxLength, ErrorMessage = "Nome fantasia deve ter no máximo 100 caracteres")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [MaxLength(EmailValueObject.MaxLength, ErrorMessage = "CNPJ deve ter no máximo 100 caracteres")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        public List<string> Telefones { get; set; }

        public List<RevendaCreateContatoModel> Contatos { get; set; }

        public List<RevendaCreateEnderecoModel> Enderecos { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            try
            {
                var cnpjValidation = new CnpjValueObject(Cnpj);
            }
            catch
            {
                results.Add(new ValidationResult("CNPJ inválido.", new[] { nameof(Cnpj) }));
            }

            ValidateTelefones(validationContext, results);
            ValidateContatos(validationContext, results);

            return results;
        }

        private void ValidateTelefones(ValidationContext validationContext, List<ValidationResult> results)
        {
            Telefones.ForEach(telefone =>
            {
                try
                {
                    var telefoneValidation = new TelefoneValueObject(telefone);
                }
                catch
                {
                    results.Add(new ValidationResult($"Telefone {telefone} é inválido.", new[] { nameof(Telefones) }));
                }
            });
        }

        private void ValidateContatos(ValidationContext validationContext, List<ValidationResult> results)
        {
            if (Contatos.Count == 0)
                results.Add(new ValidationResult("Adicione pelo menos um contato.", new[] { nameof(Contatos) }));

            if (Contatos.Count > 0 && Contatos.Count(contato => contato.Principal) != 1)
                results.Add(new ValidationResult("Selecione um contato como principal.", new[] { nameof(Contatos) }));
        }
    }
}