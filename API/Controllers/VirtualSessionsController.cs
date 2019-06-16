using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Mailer;
using API.Http;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{

  [Route("api/[controller]")]
  public class VirtualSessionsController : ControllerBase
  {
    private readonly IStorage<VirtualSession> _virtualSessionsStorage;
    private readonly IStorage<Member> _membersStorage;
    private readonly VirtualSessionsMailer _mailer;
    private readonly GraphService _graphService;

    public VirtualSessionsController(IStorage<VirtualSession> virtualSessionsStorage, IStorage<Member> membersStorage)
    {
      this._mailer = new VirtualSessionsMailer();
      this._virtualSessionsStorage = virtualSessionsStorage;
      this._membersStorage = membersStorage;
      this._graphService = new GraphService();
    }

    // GET api/virtualsessions/5
    [HttpGet("{id}")]
    public async Task<VirtualSession> Get(string id)
    {
      var virtualSession = await _virtualSessionsStorage.GetItemAsync(id);
      return virtualSession;
    }

    // POST api/virtualsessions
    [HttpPost]
    #if !DEBUG
    [Authorize]
    #endif
    public async Task Post([FromBody]VirtualSession virtualSession)
    {
      virtualSession.Id = Guid.NewGuid();
      virtualSession.VolunteerId = Guid.Empty;
      await _virtualSessionsStorage.CreateItemAsync(virtualSession);
      var volunteerEmails = await GetCurrentVolunteerEmails();
      await _mailer.SendNewVirtualSessionMail(volunteerEmails, virtualSession.Id);
    }

    // PUT api/virtualsessions/5
    [HttpPut("{id}")]
    #if !DEBUG
    [Authorize]
    #endif
    public async Task Put(string id, [FromBody]VirtualSession virtualSession)
    {
      var storedVirtualSession = await _virtualSessionsStorage.GetItemAsync(id);
      if (storedVirtualSession.VolunteerId == Guid.Empty)
      {
        storedVirtualSession.VolunteerId = virtualSession.VolunteerId;
        storedVirtualSession.TimePreferenceSelected = virtualSession.TimePreferenceSelected;
        var volunteer = await _membersStorage.GetItemAsync(virtualSession.VolunteerId.ToString());
        var educator = await _membersStorage.GetItemAsync(storedVirtualSession.EducatorId.ToString());
        await _virtualSessionsStorage.UpdateItemAsync(id, storedVirtualSession);
        var teamsMeeting = await _graphService.CreateTeamsMeeting();
        await _graphService.ScheduleOnlineMeeting(virtualSession.TimePreferenceSelected, volunteer, educator, teamsMeeting);
      }
    }

    private async Task<List<string>> GetCurrentVolunteerEmails()
    {
      var volunteerEmails = new List<string>();
      var volunteers = await _membersStorage.GetItemsAsync(m => m.MemberType == "volunteer");
      foreach(var volunteer in volunteers)
      {
        volunteerEmails.Add(volunteer.Email);
      }
      return volunteerEmails;
    }
  }
}
