using System.Runtime.Serialization;

namespace BibliotecaAPI.Enums;
public enum StatusLivroEstante
{
    [EnumMember(Value = "Quero Ler")]
    QueroLer = 1,

    [EnumMember(Value = "Lendo")]
    Lendo = 2,

    [EnumMember(Value = "Lido")]
    Lido = 3
}
