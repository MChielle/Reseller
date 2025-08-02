using Reseller.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Entities
{
    public sealed class Contato : Entity
    {
        internal Contato()
        { }

        public Contato(NomeContatoValueObject nome, bool principal)
        {
            Nome = nome.Value;
            Principal = principal;
        }

        public Guid RevendaId { get; set; }
        public Revenda Revenda { get; set; }

        [Required]
        [MaxLength(NomeContatoValueObject.MaxLength)]
        public string Nome { get; set; }

        [Required]
        public bool Principal { get; set; }
    }

    public sealed class NomeContatoValueObject
    {
        public const int MaxLength = 100;

        public NomeContatoValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MaxLength)
                throw new ArgumentNullException($"Nome deve ter no máximo {MaxLength} caracteres.");
            Value = value;
        }

        public string Value { get; }
    }
}