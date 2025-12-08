using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BibliotecaAPI.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult("Ocorreu um erro ao tratar a solicitação, tente novamente mais tarde!")
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
