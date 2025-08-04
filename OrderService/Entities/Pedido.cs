using OrderService.Entities.Base;
using OrderService.Enums;

namespace OrderService.Entities
{
    public class Pedido : Entity
    {
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public Guid RevendaId { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataPedidoUTC { get; set; }
        public ICollection<Item> Itens { get; set; }
    }
}