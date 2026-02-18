namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IDeleteUsersUseCase
{
    Task DeleteUser(string id);
}
