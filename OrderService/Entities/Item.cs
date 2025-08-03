using OrderService.Entities.Base;

namespace OrderService.Entities
{
    public class Item : Entity
    {
        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public Guid Referencia { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }
}