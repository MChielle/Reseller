using Reseller.Domain.Data;
using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Entities
{
    public sealed class Revenda : Entity
    {
        internal Revenda()
        { }

        public Revenda(CnpjValueObject cnpj, RazaoSocialRevendaValueObject razaoSocial, NomeFantasiaRevendaValueObject nomeFantasia, EmailValueObject email, ICollection<Telefone> telefones, ICollection<Contato> contatos, ICollection<Endereco> enderecos)
        {
            Cnpj = cnpj.Value;
            RazaoSocial = razaoSocial.Value;
            NomeFantasia = nomeFantasia.Value;
            Email = email.Value;
            Telefones = telefones;
            Contatos = contatos;
            Enderecos = enderecos;
        }

        [MaxLength(CnpjValueObject.Length)]
        public string Cnpj { get; private set; }

        [Required]
        [MaxLength(RazaoSocialRevendaValueObject.MaxLength)]
        public string RazaoSocial { get; private set; }

        [Required]
        [MaxLength(NomeFantasiaRevendaValueObject.MaxLength)]
        public string NomeFantasia { get; private set; }

        [MaxLength(EmailValueObject.MaxLength)]
        public string Email { get; private set; }

        public ICollection<Telefone> Telefones { get; private set; }
        public ICollection<Contato> Contatos { get; private set; }
        public ICollection<Endereco> Enderecos { get; private set; }
    }

    public class RazaoSocialRevendaValueObject
    {
        public const int MaxLength = 100;
        public string Value { get; }

        public RazaoSocialRevendaValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Razão Social não pode ser nulo ou vazio.");

            if (value.Length > MaxLength)
                throw new ArgumentException($"Razão Social deve ter no máximo {MaxLength} caracteres.");

            Value = value;
        }
    }

    public class NomeFantasiaRevendaValueObject
    {
        public const int MaxLength = 100;
        public string Value { get; }

        public NomeFantasiaRevendaValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Nome Fantasia não pode ser nulo ou vazio.");

            if (value.Length > MaxLength)
                throw new ArgumentException($"Nome Fantasia deve ter no máximo {MaxLength} caracteres.");

            Value = value;
        }
    }
}