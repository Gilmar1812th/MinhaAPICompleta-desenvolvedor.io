using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.API.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/fornecedores")]
    public class FornecedoresController : MainController    
    {
        // injetar repository
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,         
                                      IMapper mapper,
                                      INotificador notificador) : base(notificador)
        {            
            _fornecedorRepository = fornecedorRepository;            
            _mapper = mapper;
        }

        // Se quiser retornar tudo como código 200
        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            // await entende que já o resultado do método            
            return _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());                        
        }
    }
}