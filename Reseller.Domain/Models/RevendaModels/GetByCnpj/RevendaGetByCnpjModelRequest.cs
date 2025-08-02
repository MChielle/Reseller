using Reseller.Domain.Data.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Models.RevendaModels.Get
{
    public sealed class RevendaGetByCnpjModelRequest : IValidatableObject
    {
        [Required(ErrorMessage = "CNPJ é obrigatório.")]
        [MaxLength(CnpjValueObject.Length, ErrorMessage = "CNPJ deve ter 14 caracteres")]
        public string Cnpj { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            try
            {
                var cnpjValidation = new CnpjValueObject(Cnpj);
            }
            catch
            {
                results.Add(new ValidationResult("Cnpj inválido.", new[] { nameof(Cnpj) }));
            }

            return results;
        }
    }
}