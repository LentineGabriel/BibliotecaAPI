using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Services.UseCases.Auth.UsersUC;

public class PatchUsersUseCase : IPatchUsersUseCase
{
    #region PROS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public PatchUsersUseCase(UserManager<ApplicationUser> userManager , IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<UsersResponseDTO> PatchAsync(string id , JsonPatchDocument<UsersResponseDTO> patchDoc)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) throw new ArgumentException($"Não foi possível encontrar o usuário com ID {id}. Por favor, verifique o id digitado e tente novamente!");
        if (patchDoc == null) throw new ArgumentNullException(nameof(patchDoc));

        var userToPatch = _mapper.Map<UsersResponseDTO>(user);

        // Não deixando o EmailConfirmed ser alterado via patch
        if (patchDoc.Operations.Any(op => op.path.Equals("/emailConfirmed", StringComparison.OrdinalIgnoreCase))) throw new InvalidOperationException("A propriedade 'EmailConfirmed' não pode ser alterada via patch.");

        // Validando o DTO com DataAnnotations
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(userToPatch);
        if (!Validator.TryValidateObject(userToPatch, validationContext, validationResults, validateAllProperties: true))
        {
            var errors = string.Join("; ", validationResults.Where(r => !string.IsNullOrEmpty(r.ErrorMessage)).Select(r => r.ErrorMessage));
            throw new ValidationException($"Validação falhou: {errors}");
        }

        // Caso o email tenha sido alterado
        if (!string.Equals(userToPatch.Email, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            var setEmail = await _userManager.SetEmailAsync(user, userToPatch.Email!);
            if (!setEmail.Succeeded)
            {
                var descr = string.Join("; ", setEmail.Errors.Select(e => e.Description));
                throw new InvalidOperationException(descr);
            }
        }

        // Caso o username tenha sido alterado
        if (!string.Equals(userToPatch.Username, user.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var setUserName = await _userManager.SetUserNameAsync(user, userToPatch.Username!);
            if (!setUserName.Succeeded)
            {
                var descr = string.Join("; ", setUserName.Errors.Select(e => e.Description));
                throw new InvalidOperationException(descr);
            }
        }

        // P/ outros campos "não sensíveis"
        _mapper.Map(userToPatch, user);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var descr = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(descr);
        }

        return _mapper.Map<UsersResponseDTO>(user);
    }
}
