using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EditorasLivros;

namespace BibliotecaAPI.Services.UseCases.EditorasLivros
{
    public class CreateEditorasUseCase : ICreateEditorasUseCase
    {
        #region PROPS/CTOR
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;

        public CreateEditorasUseCase(IUnityOfWork uof , IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }
        #endregion

        public async Task<EditorasDTOResponse> PostAsync(EditorasDTORequest editorasDTO)
        {
            if(editorasDTO == null) throw new Exception("Não foi possível adicionar uma nova editora. Tente novamente mais tarde!");

            var editoraNova = _mapper.Map<Editoras>(editorasDTO);
            var editoraCriada = _uof.EditorasRepositorio.Create(editoraNova);
            await _uof.CommitAsync();

            return _mapper.Map<EditorasDTOResponse>(editoraCriada);
        }
    }
}
