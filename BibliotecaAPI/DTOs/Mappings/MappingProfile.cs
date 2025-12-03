using AutoMapper;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Livros , LivrosDTO>().ForMember(dest => dest.NomeAutor ,
                                                      opt => opt.MapFrom
                                                      (src => src.Autor!.PrimeiroNome + " " + src.Autor.Sobrenome)).ReverseMap();
            CreateMap<Autor , AutorDTO>().ReverseMap();
            CreateMap<Categorias , CategoriasDTO>().ReverseMap();
        }
    }
}
