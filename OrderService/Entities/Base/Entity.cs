using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities.Base
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
