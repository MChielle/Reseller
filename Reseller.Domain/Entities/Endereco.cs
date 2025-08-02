using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Entities
{
    public sealed class Endereco : Entity
    {
        internal Endereco()
        { }

        public Endereco(LogradouroEnderecoValueObject logradouro, NumeroEnderecoValueObject numero, ComplementoEnderecoValueObject complemento, CidadeEnderecoValueObject cidade, EstadoEnderecoValueObject estado, CepValueObject cep)
        {
            Logradouro = logradouro.Value;
            Numero = numero.Value;
            Complemento = complemento.Value;
            Cidade = cidade.Value;
            Estado = estado.Value;
            Cep = cep.Value;
        }

        public Guid RevendaId { get; set; }
        public Revenda Revenda { get; set; }

        [MaxLength(LogradouroEnderecoValueObject.MaxLength)]
        [Required]
        public string Logradouro { get; set; }

        [MaxLength(NumeroEnderecoValueObject.MaxLength)]
        [Required]
        public string Numero { get; set; }

        [MaxLength(ComplementoEnderecoValueObject.MaxLength)]
        [Required]
        public string? Complemento { get; set; }

        [MaxLength(CidadeEnderecoValueObject.MaxLength)]
        [Required]
        public string Cidade { get; set; }

        [MaxLength(EstadoEnderecoValueObject.MaxLength)]
        [Required]
        public string Estado { get; set; }

        [MaxLength(CepValueObject.Length)]
        [Required]
        public string Cep { get; set; }
    }

    public sealed class LogradouroEnderecoValueObject
    {
        public const int MaxLength = 100;
        public string Value { get; }

        public LogradouroEnderecoValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Logradouro não pode ser nulo.");

            if (value.Length > MaxLength)
                throw new ArgumentException($"Logradouro deve ter no máximo {MaxLength} caracteres.");

            Value = value;
        }
    }

    public sealed class NumeroEnderecoValueObject
    {
        public const int MaxLength = 10;
        public string Value { get; }

        public NumeroEnderecoValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Número não pode ser nulo.");

            if (value.Length > MaxLength)
                throw new ArgumentException($"Número deve ter no máximo {MaxLength} caracteres.");

            Value = value;
        }
    }

    public sealed class ComplementoEnderecoValueObject
    {
        public const int MaxLength = 50;
        public string? Value { get; }

        public ComplementoEnderecoValueObject(string? value)
        {
            if (value.Length > MaxLength)
                throw new ArgumentException($"Complemento deve ter no máximo {MaxLength} caracteres.");

            Value = value;
        }
    }

    public sealed class CidadeEnderecoValueObject
    {
        public const int MaxLength = 100;
        public string Value { get; }

        public CidadeEnderecoValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Cidade não pode ser nulo.");

            if (value.Length > MaxLength)
                throw new ArgumentException($"Cidade deve ter no máximo {MaxLength} caracteres.");

            Value = value;
        }
    }

    public sealed class EstadoEnderecoValueObject
    {
        public const int MaxLength = 30;
        public string Value { get; }

        public EstadoEnderecoValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Estado não pode ser nulo.");

            if (value.Length > MaxLength)
                throw new ArgumentException($"Estado deve ter no máximo {MaxLength} caracteres.");

            Value = value;
        }
    }
}