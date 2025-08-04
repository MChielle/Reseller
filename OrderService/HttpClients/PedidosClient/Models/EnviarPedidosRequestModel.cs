namespace OrderService.HttpClients.PedidosClient.Models
{
    public class EnviarPedidosRequestModel
    {
        public Guid RevendaId { get; set; }

        public ICollection<EnviarPedidosPedidoModel> Pedidos { get; set; }
    }

    public class EnviarPedidosPedidoModel
    {
        public Guid RevendaId { get; set; }
        public DateTime DataPedidoUTC { get; set; }
        public ICollection<EnviarPedidosPedidoItemModel> Itens { get; set; }
    }

    public class EnviarPedidosPedidoItemModel
    {
        public Guid Referencia { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }
}