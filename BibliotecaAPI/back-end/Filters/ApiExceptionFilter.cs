#region USINGS
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#endregion

namespace BibliotecaAPI.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    #region ON EXCEPTION
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult("Ocorreu um erro ao tratar a solicitação, tente novamente mais tarde!")
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
    #endregion
}
