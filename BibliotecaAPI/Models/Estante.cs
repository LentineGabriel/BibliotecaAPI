using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;
public class Estante : IValidatableObject
{
    #region PROPS/CTOR
    
    #endregion

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}
