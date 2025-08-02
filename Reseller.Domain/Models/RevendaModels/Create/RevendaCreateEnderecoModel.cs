using Reseller.Domain.Data;
using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Models.RevendaModels.Create
{
    public class RevendaCreateEnderecoModel
    {
        [MaxLength(LogradouroEnderecoValueObject.MaxLength, ErrorMessage = "Excedido o limite de caracteres da propriedade Logradouro.")]
        [Required(ErrorMessage = "Logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        [MaxLength(NumeroEnderecoValueObject.MaxLength, ErrorMessage = "Excedido o limite de caracteres da propriedade Número.")]
        [Required(ErrorMessage = "Número é obrigatório.")]
        public string Numero { get; set; }

        [MaxLength(ComplementoEnderecoValueObject.MaxLength, ErrorMessage = "Excedido o limite de caracteres da propriedade Complemento.")]
        public string Complemento { get; set; }

        [MaxLength(CidadeEnderecoValueObject.MaxLength, ErrorMessage = "Excedido o limite de caracteres da propriedade Cidade.")]
        [Required(ErrorMessage = "Cidade é obrigatorio.")]
        public string Cidade { get; set; }

        [MaxLength(EstadoEnderecoValueObject.MaxLength, ErrorMessage = "Excedido o limite de caracteres da propriedade Estado.")]
        [Required(ErrorMessage = "Estado é obrigatorio.")]
        public string Estado { get; set; }

        [MaxLength(CepValueObject.Length, ErrorMessage = "Excedido o limite de caracteres da propriedade Cep.")]
        [Required(ErrorMessage = "Cep é obrigatorio.")]
        public string Cep { get; set; }
    }
}
