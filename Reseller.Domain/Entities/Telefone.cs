using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Entities
{
    public sealed class Telefone : Entity
    {
        public Guid RevendaId { get; set; }
        public Revenda Revenda { get; set; }

        [MaxLength(TelefoneValueObject.MaxLength)]
        [Required]
        public string Numero { get; set; }

        internal Telefone()
        { }

        public Telefone(TelefoneValueObject numero)
        {
            Numero = numero.Value;
        }
    }
}