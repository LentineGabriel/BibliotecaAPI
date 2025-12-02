using AutoMapper;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Livros , LivrosDTO>().ReverseMap();
            CreateMap<Categorias , CategoriasDTO>().ReverseMap();
        }
    }
}
