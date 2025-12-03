using AutoMapper;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Livros - sem ReverseMap
            CreateMap<Livros , LivrosDTO>().ForMember(dest => dest.NomeAutor ,
                                                      opt => opt.MapFrom
                                                      (src => src.Autor!.PrimeiroNome + " " + src.Autor.Sobrenome))
                                           .ForMember(dest => dest.NomeEditora,
                                                      opt => opt.MapFrom
                                                      (src => src.Editora!.NomeEditora));
            
            CreateMap<LivrosDTO , Livros>().ForMember(dest => dest.Autor , opt => opt.Ignore())
                                           .ForMember(dest => dest.Editora , opt => opt.Ignore());
            
            // Autor
            CreateMap<Autor , AutorDTO>().ReverseMap();

            // Editora
            CreateMap<Editoras , EditorasDTO>().ReverseMap();

            // Categoria
            CreateMap<Categorias , CategoriasDTO>().ReverseMap();
        }
    }
}
