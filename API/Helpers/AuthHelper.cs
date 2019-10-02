using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;

namespace API.Helpers
{
  public class AuthHelper
  {
    PasswordHasher<Member> _passwordHasher = new PasswordHasher<Member>();
    private readonly IStorage<Member> _storage;

    public AuthHelper(IStorage<Member> storage)
    {
      this._storage = storage;
    }

    public async Task<ActionResult<JwtAuthDto>> LoginMemberAsync(LoginDto loginDto)
    {
      PasswordVerificationResult verificationResult;
      Member member;
      try
      {
        member = (await _storage.GetItemsAsync(m => m.Email == loginDto.Email)).First();
      }
      catch
      {
        return new BadRequestObjectResult(new ErrorResponseDto(ValidationResponseHelper.InvalidCredentials));
      }

      verificationResult = ValidatePassword(member, member.PasswordHashed, loginDto.Password);
      JwtAuthDto jwtAuthToken = GenerateJsonWebToken(new JwtUserClaimsDto(member.Id.ToString()));
      jwtAuthToken.RefreshToken = GenerateToken();
      member.RefreshToken = jwtAuthToken.RefreshToken;

      switch (verificationResult)
      {
        case PasswordVerificationResult.Success:
          await _storage.UpdateItemAsync(member.Id.ToString(), member);
          return new OkObjectResult(jwtAuthToken);
        case PasswordVerificationResult.Failed:
          return new BadRequestObjectResult(new ErrorResponseDto(ValidationResponseHelper.InvalidCredentials));
        case PasswordVerificationResult.SuccessRehashNeeded:
          member.PasswordHashed = HashPassword(member, loginDto.Password);
          await _storage.UpdateItemAsync(member.Id.ToString(), member);
          return new OkObjectResult(jwtAuthToken);
        default:
          return new BadRequestResult();
      }
    }

    public async Task<ActionResult<JwtAuthDto>> RefreshSessionAsync(JwtAuthDto jwtAuthDto)
    {
      ClaimsPrincipal principal = null;
      try
      {
        principal = GetPrincipalFromExpiredToken(jwtAuthDto.Token);
      }
      catch
      {
        return new BadRequestObjectResult(new ErrorResponseDto(ValidationResponseHelper.InvalidSessionToken));
      }

      string newRefreshToken = GenerateToken();
      IEnumerable<Claim> claims = principal.Claims;

      object claimsData = null;
      Member dtoUser = null;

      foreach (Claim claim in claims)
      {
        if (claim.Type == ClaimTypes.UserData)
        {
          claimsData = JsonConvert.DeserializeObject<Member>(claim.Value);
          dtoUser = (Member)claimsData;
        }
      }

      if (claimsData == null)
      {
        return new BadRequestObjectResult(new ErrorResponseDto(ValidationResponseHelper.NoClaimsData));
      }

      Guid id = dtoUser.Id;
      Member user = await _storage.GetItemAsync(id.ToString());

      JwtAuthDto newJwtAuthToken = null;

      if (user != null)
      {
        if (jwtAuthDto.RefreshToken == user.RefreshToken)
        {
          user.RefreshToken = newRefreshToken;
          newJwtAuthToken = GenerateJsonWebToken(new JwtUserClaimsDto(dtoUser.Id.ToString()));
          newJwtAuthToken.RefreshToken = newRefreshToken;
          await _storage.UpdateItemAsync(user.Id.ToString(), user);
        }
        else
        {
          return new BadRequestObjectResult(new ErrorResponseDto(ValidationResponseHelper.InvalidSessionToken));
        }
      }
      else
      {
        return new BadRequestObjectResult(new ErrorResponseDto(ValidationResponseHelper.InvalidSessionToken));
      }

      return new OkObjectResult(newJwtAuthToken);
    }
    public async Task<ActionResult<JwtAuthDto>> RegisterMemberAsync(RegisterDto registerDto)
    {
      Member member = new Member
      {
        Id = Guid.NewGuid(),
        FullName = registerDto.FullName,
        Email = registerDto.Email,
        PasswordHashed = HashPassword(null, registerDto.Password),
        RefreshToken = GenerateToken(),
        MemberType = registerDto.MemberType
      };

      JwtAuthDto jwtAuthToken = GenerateJsonWebToken(new JwtUserClaimsDto(member.Id.ToString()));
      jwtAuthToken.RefreshToken = member.RefreshToken;

      try
      {
        await _storage.CreateItemAsync(member);
      }
      catch (Microsoft.Azure.Documents.DocumentClientException ex)
      {
        if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
          return new BadRequestObjectResult(new ErrorResponseDto(ValidationResponseHelper.InvalidDuplicateEmail));
        }
        else
        {
          return new BadRequestResult();
        }
      }

      return new OkObjectResult(jwtAuthToken);
    }
    private PasswordVerificationResult ValidatePassword(Member member, string hashedPassword, string providedPassword)
    {
      return _passwordHasher.VerifyHashedPassword(member, hashedPassword, providedPassword);
    }

    private string HashPassword(Member member, string password)
    {
      return _passwordHasher.HashPassword(member, password);
    }

    public static string GenerateToken()
    {
      var randomNumber = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Settings.JwtIssuer,
        ValidAudience = Settings.JwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.JwtSecretKey)),
        ValidateLifetime = false
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken securityToken;
      var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
      var jwtSecurityToken = securityToken as JwtSecurityToken;
      if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        throw new SecurityTokenException("Invalid token");

      return principal;
    }

    public static JwtAuthDto GenerateJsonWebToken(object jwtTokenData)
    {
      var claims = new[]
      {
        new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(jwtTokenData, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }), null)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.JwtSecretKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: Settings.JwtIssuer,
          audience: Settings.JwtAudience,
          claims: claims,
          expires: DateTime.Now.AddMinutes(1),
          signingCredentials: creds);

      string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
      JwtAuthDto jwtAuthToken = new JwtAuthDto(jwtToken);
      return jwtAuthToken;
    }
  }
}
