using Reseller.Domain.Data;
using Reseller.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Models.RevendaModels.Create
{
    public sealed class RevendaCreateContatoModel
    {
        [Required(ErrorMessage = "Principal é obrigatório")]
        public bool Principal { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(NomeContatoValueObject.MaxLength, ErrorMessage = "Nome contato deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }
    }
}