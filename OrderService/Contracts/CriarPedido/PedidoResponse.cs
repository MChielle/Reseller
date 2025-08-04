namespace OrderService.Contracts.CriarPedido
{
    public class PedidoResponse
    {
        public Guid PedidoId { get; set; }
        public ICollection<CriarPedidoItemResponse> Itens { get; set; }
    }

    public class CriarPedidoItemResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }
}