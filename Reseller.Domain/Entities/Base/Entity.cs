using System.ComponentModel.DataAnnotations;

namespace Reseller.Domain.Entities.Base
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
