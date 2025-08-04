using OrderService.Entities.Base;

namespace OrderService.Entities
{
    public class Cliente : Entity
    {
        public string? Cpf { get; set; }
        public string? Cnpj { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }
}
