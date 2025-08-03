using OrderService.Enums;

namespace OrderService.Entities
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public StatusPedido Status { get; set; }
        public string NomeCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public DateTime DataPedidoUTC { get; set; }
        public ICollection<Item> Itens { get; set; }
    }
}