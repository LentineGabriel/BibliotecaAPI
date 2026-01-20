#region USINGS
using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Roles;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Identity;
#endregion

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
                                                   .ForMember(dest => dest.Categorias , opt => opt.MapFrom(src => src.LivrosCategorias!.Select(lc => lc.Categorias.NomeCategoria)));
            // Request -> Livros
            CreateMap<LivrosDTORequest , Livros>().ForMember(dest => dest.Autor , opt => opt.Ignore())
                                                  .ForMember(dest => dest.Editora , opt => opt.Ignore())
                                                  .ForMember(dest => dest.LivrosCategorias , opt => opt.Ignore());
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

            #region CATEGORIAS
            // Categoria -> Response
            CreateMap<Categorias , CategoriasDTOResponse>();

            // Categoria -> Request
            CreateMap<CategoriasDTORequest , Categorias>().ReverseMap();
            #endregion

            #region AUTH
            // Users -> UsersDTO
            CreateMap<ApplicationUser , UsersRequestDTO>().ForMember(dest => dest.Username , opt => opt.MapFrom(src => src.UserName));
            CreateMap<UsersResponseDTO , ApplicationUser>().ForMember(dest => dest.UserName , opt => opt.MapFrom(src => src.Username)).ReverseMap().ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));

            // Roles -> RolesDTO
            CreateMap<IdentityRole , RolesRequestDTO>();
            CreateMap<RolesResponseDTO , IdentityRole>().ReverseMap();
            #endregion
        }
    }
}
