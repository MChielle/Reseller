namespace OrderService.Contracts.CriarPedido
{
    public class CriarPedidoRequest
    {
        public Guid RevendaId { get; set; }
        public CriarPedidoClienteRequest Cliente { get; set; }
        public List<CriarPedidoItemRequest> Itens { get; set; }
    }

    public sealed class  CriarPedidoClienteRequest
    {
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }

    public sealed class CriarPedidoItemRequest
    {
        public Guid Referencia { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }
}
