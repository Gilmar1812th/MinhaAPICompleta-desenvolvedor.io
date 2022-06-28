using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.API.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Components;

namespace DevIO.API.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        // injetar repository
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        // Se quiser retornar tudo como código 200
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            // await entende que já o resultado do método
            var fornecedor = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return fornecedor;
        }
    }  
}