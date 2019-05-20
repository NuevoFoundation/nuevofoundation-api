using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class MembersController : ControllerBase
  {
    private readonly IStorage<Member> _storage;

    public MembersController(IStorage<Member> storage)
    {
      this._storage = storage;
    }

    // GET api/members/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> Get(string id)
    {
      var member = await _storage.GetItemAsync(id);
      return new OkObjectResult(member);
    }

    // POST api/members
    [HttpPost]
    #if !DEBUG
    [Authorize]
    #endif
    public async Task Post([FromBody]Member member)
    {
      member.Id = Guid.NewGuid();
      await _storage.CreateItemAsync(member);
    }

    // PUT api/members/5
    [HttpPut("{id}")]
    #if !DEBUG
    [Authorize]
    #endif
    public void Put(int id, [FromBody]string value)
    {
    }
  }
}
