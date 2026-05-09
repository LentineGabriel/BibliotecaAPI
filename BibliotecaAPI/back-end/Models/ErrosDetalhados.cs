using System.Text.Json;

namespace BibliotecaAPI.Models;
public class ErrosDetalhados
{
    #region PROPS
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Trace { get; set; }
    #endregion

    #region METHODS
    public override string ToString() => JsonSerializer.Serialize(this);
    #endregion
}
