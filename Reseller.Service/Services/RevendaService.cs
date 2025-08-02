using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;
using Reseller.Domain.Interfaces.Repositories;
using Reseller.Domain.Interfaces.Services;
using Reseller.Domain.Models.RevendaModels.Get;
using Reseller.Domain.Models.RevendaModels.GetByCnpj;
using Reseller.Domain.Models.RevendaModels.Input;

namespace Reseller.Service.Services
{
    public class RevendaService : IRevendaService
    {
        private readonly IRevendaRepository _revendaRepository;

        public RevendaService(IRevendaRepository revendaRepository)
        {
            _revendaRepository = revendaRepository;
        }

        public async Task<bool> CreateRevendaAsync(RevendaCreateModelRequest request)
        {
            var existeRevenda = await _revendaRepository.GetByCnpjAsync(new CnpjValueObject(request.Cnpj));

            if (existeRevenda != null) throw new Exception("Revenda já possui cadastro.");

            var telefones = request.Telefones.Select(telefone =>
                    new Telefone(new TelefoneValueObject(telefone))).ToArray();

            var contatos = request.Contatos.Select(contato =>
                    new Contato(new NomeContatoValueObject(contato.Nome), contato.Principal)).ToArray();

            var enderecos = request.Enderecos.Select(endereco =>
                    new Endereco(
                        new LogradouroEnderecoValueObject(endereco.Logradouro),
                        new NumeroEnderecoValueObject(endereco.Numero),
                        new ComplementoEnderecoValueObject(endereco.Complemento),
                        new CidadeEnderecoValueObject(endereco.Cidade),
                        new EstadoEnderecoValueObject(endereco.Estado),
                        new CepValueObject(endereco.Cep))).ToArray();

            var revenda = new Revenda(
                new CnpjValueObject(request.Cnpj),
                new RazaoSocialRevendaValueObject(request.RazaoSocial),
                new NomeFantasiaRevendaValueObject(request.NomeFantasia),
                new EmailValueObject(request.Email),
                telefones,
                contatos,
                enderecos);

            return await _revendaRepository.CreateAsync(revenda);
        }

        public async Task<RevendaGetByCnpjModelResponse> GetRevendaByCnpjAsync(RevendaGetByCnpjModelRequest request)
        {
            var revenda = await _revendaRepository.GetByCnpjAsync(new CnpjValueObject(request.Cnpj));

            if (revenda == null)
                throw new Exception("Revenda não encontrada.");

            var response = new RevendaGetByCnpjModelResponse(
                revenda.Cnpj,
                revenda.RazaoSocial,
                revenda.NomeFantasia,
                revenda.Email);

            return response;
        }
    }
}