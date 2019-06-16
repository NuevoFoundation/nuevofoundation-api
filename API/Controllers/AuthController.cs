using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private AuthHelper _authHelper;

    public AuthController(IStorage<Member> storage)
    {
      _authHelper = new AuthHelper(storage);
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<JwtAuthDto>> LoginAsync([FromBody]LoginDto loginDto)
    {
      return await _authHelper.LoginMemberAsync(loginDto);
    }

    // POST api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<JwtAuthDto>> RegisterAsync([FromBody]RegisterDto registerDto)
    {
      return await _authHelper.RegisterMemberAsync(registerDto);
    }

    // POST api/auth/refresh
    [HttpPost("refresh")]
    public async Task<ActionResult<JwtAuthDto>> RefreshSessionAsync([FromBody]JwtAuthDto jwtAuthDto)
    {
      return await _authHelper.RefreshSessionAsync(jwtAuthDto);
    }
  }
}
