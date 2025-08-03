namespace OrderService.Contracts
{
    public class CriarPedidoRequest
    {
        public string NomeCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public List<CriarPedidoItemRequest> Itens { get; set; }
    }

    public sealed class CriarPedidoItemRequest
    {
        public Guid Referencia { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }
}
