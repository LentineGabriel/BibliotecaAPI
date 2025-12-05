using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region LIVROS
            // Livros -> Response
            CreateMap<Livros , LivrosDTOResponse>().ForMember(dest => dest.NomeAutor , opt => opt.MapFrom(src => src.Autor != null ? src.Autor!.PrimeiroNome + " " + src.Autor.Sobrenome : null))
                                                   .ForMember(dest => dest.NomeEditora , opt => opt.MapFrom(src => src.Editora != null ? src.Editora!.NomeEditora : null))
                                                   .ForMember(dest => dest.NomeCategoria , opt => opt.MapFrom(src => src.CategoriaLivro != null ? src.CategoriaLivro!.NomeCategoria : null));

            // Livros -> Request
            CreateMap<Livros , LivrosDTORequest>().ForMember(dest => dest.NomeAutor , opt => opt.MapFrom(src => src.Autor != null ? src.Autor.PrimeiroNome + " " + src.Autor.Sobrenome : null))
                                                  .ForMember(dest => dest.NomeEditora , opt => opt.MapFrom(src => src.Editora != null ? src.Editora.NomeEditora : null))
                                                  .ForMember(dest => dest.NomeCategoria , opt => opt.MapFrom(src => src.CategoriaLivro != null ? src.CategoriaLivro.NomeCategoria : null));
            CreateMap<LivrosDTORequest , Livros>().ForMember(dest => dest.Autor , opt => opt.Ignore())
                                                  .ForMember(dest => dest.Editora , opt => opt.Ignore())
                                                  .ForMember(dest => dest.CategoriaLivro , opt => opt.Ignore());
            #endregion

            #region AUTOR
            // Autor -> Response
            CreateMap<Autor , AutorDTOResponse>();

            // Autor -> Request
            CreateMap<AutorDTORequest , Autor>().ReverseMap();
            #endregion

            #region EDITORA
            // Editora -> Response
            CreateMap<Editoras , EditorasDTOResponse>();

            // Editora -> Request
            CreateMap<EditorasDTORequest , Editoras>().ReverseMap();
            #endregion

            #region CATEGORIA
            // Categoria -> Response
            CreateMap<Categorias , CategoriasDTOResponse>();

            // Categoria -> Request
            CreateMap<CategoriasDTORequest , Categorias>().ReverseMap();
            #endregion
        }
    }
}
