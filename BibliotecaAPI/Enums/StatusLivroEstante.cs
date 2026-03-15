using System.Runtime.Serialization;

namespace BibliotecaAPI.Enums;
public enum StatusLivroEstante
{
    [EnumMember(Value = "Quero Ler")]
    QueroLer,

    [EnumMember(Value = "Lendo")]
    Lendo,

    [EnumMember(Value = "Lido")]
    Lido
}
