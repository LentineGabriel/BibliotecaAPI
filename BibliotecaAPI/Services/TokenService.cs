#region USINGS
using BibliotecaAPI.Services.Interfaces.TokenJWT;
using BibliotecaAPI.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace BibliotecaAPI.Services;

public class TokenService : ITokenService
{
    #region PROPS/CTOR
    private readonly JwtSettings _jwt;

    public TokenService(IOptions<JwtSettings> op)
    {
        _jwt = op.Value;

        if(string.IsNullOrWhiteSpace(_jwt.SecretKey)) throw new InvalidOperationException("Chave JWT não configurada!");
    }
    #endregion

    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_jwt.SecretKey ?? throw new InvalidOperationException("Chave secreta inválida!"));

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes) , SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims) ,
            Expires = DateTime.UtcNow.AddMinutes(_jwt.TokenValidityInMinutes) ,
            Audience = _jwt.ValidAudience ,
            Issuer = _jwt.ValidIssuer ,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];
        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);

        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        // validar o token de acesso expirado
        var secretKey = _jwt.SecretKey ?? throw new InvalidOperationException("Chave secreta inválida!");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true ,
            ValidateIssuer = true ,
            ValidateIssuerSigningKey = true ,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) ,
            ValidateLifetime = false ,
            ValidIssuer = _jwt.ValidIssuer ,
            ValidAudience = _jwt.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token , tokenValidationParameters , out SecurityToken securityToken);
        if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256 , StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Token inválido!");

        return principal;
    }

    public DateTime GetRefreshTokenExpiry() => DateTime.UtcNow.AddMinutes(_jwt.TokenValidityInMinutes);
}
