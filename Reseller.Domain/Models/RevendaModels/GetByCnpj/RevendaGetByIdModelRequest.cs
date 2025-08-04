using Reseller.Domain.Data.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Models.RevendaModels.Get
{
    public sealed class RevendaGetByIdModelRequest
    {
        [Required(ErrorMessage = "Id é obrigatório.")]
        public Guid Id { get; set; }
    }
}