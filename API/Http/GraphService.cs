using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API;
using Newtonsoft.Json;
using API.Models;
using System.IO;

namespace API.Http
{
  public class GraphService
  {
    private Http _http = new Http();
    private Dictionary<string, string> _headers;
    private readonly string _baseAuthUri;
    private readonly string _accessToken;
    private readonly string _baseUri = "https://graph.microsoft.com";
    private readonly string _apiBetaVersion = "beta";
    private readonly string _apiVersion = "v1.0";
    private readonly Guid _organizerId = new Guid("1b03e5e9-65e6-473b-9882-baf1526f3d0b");
    private readonly string _grantType = "client_credentials";
    private readonly string _clientId;
    private readonly string _scope;
    private readonly string _clientSecret;
    private string _mailerViewsRoot = "./Mailer/MailerViews";

public GraphService()
    {
      _baseAuthUri = $"https://login.microsoftonline.com/{Settings.TenantId}/oauth2/v2.0/token";
      _headers = new Dictionary<string, string>();
      _clientId = Settings.ClientId;
      _clientSecret = Settings.ClientSecret;
      _scope = Settings.Scope;

      _accessToken = GetAuthToken().Result.AccessToken;
      UpdateAuthHeader();
    }

    public async Task<GraphAuthDto> GetAuthToken()
    {
      var graphAuthDto = await _http.Post<GraphAuthDto>($"{_baseAuthUri}", $"client_id={_clientId}&scope={_scope}&client_secret={_clientSecret}&grant_type={_grantType}", null, true);
      UpdateAuthHeader();
      return graphAuthDto;
    }

    public async Task<OnlineMeetingDto> CreateTeamsMeeting()
    {
      var teamsMeeting = new OnlineMeetingDto
      {
        MeetingType = "meetNow",
        Subject = "Nuevo Foundation - Virtual Session",
        AccessLevel = "everyone", 
        Participants = new Participants
        {
          Organizer = new Organizer
          {
            Identity = new Identity
            {
              User = new User
              {
                Id = _organizerId
              }
            }
          }
        }
      };

      var teamsMeetingResponse = await _http.Post<OnlineMeetingDto>($"{_baseUri}/{_apiBetaVersion}/app/onlineMeetings", JsonConvert.SerializeObject(teamsMeeting), _headers);
      return teamsMeetingResponse;
    }

    public async Task<CalendarEventDto> ScheduleOnlineMeeting(DateTimeOffset startTime, Member volunteer, Member educator, OnlineMeetingDto onlineMeeting)
    {
      var mailBody = ReadHtmlFile("ScheduledVirtualSession");
      mailBody = mailBody.Replace("<meetingConferenceUrl>", $"{onlineMeeting.JoinUrl}");

      var calendarEvent = new CalendarEventDto
      {
        Subject = "Nuevo Foundation Virtual Session",
        Body = new Body
        {
          ContentType = "HTML",
          Content = mailBody
        },
        Start = new End
        {
          DateTime = startTime,
          TimeZone = "Pacific Standard Time"
        },
        End = new End
        {
          DateTime = startTime.AddMinutes(45),
          TimeZone = "Pacific Standard Time"
        },
        Attendees = InitializeAttendees(volunteer, educator)
      };

      var calendarEventResponse = await _http.Post<CalendarEventDto>($"{_baseUri}/{_apiVersion}/users/{_organizerId}/calendar/events", JsonConvert.SerializeObject(calendarEvent), _headers);
      return calendarEventResponse;
    }

    private void UpdateAuthHeader()
    {
      if (_headers.ContainsKey("Authorization"))
      {
        _headers["Authorization"] = $"Bearer {_accessToken}";
      }
      else
      {
        _headers.Add("Authorization", $"Bearer {_accessToken}");
      }
    }

    private Attendee[] InitializeAttendees(Member volunteer, Member educator)
    {
      Attendee[] attendees = {
        new Attendee {
        EmailAddress = new EmailAddress
        {
          Address = volunteer.Email,
          Name = $"{volunteer.FullName}"
        },
        Type = "required"
      }, new Attendee {
        EmailAddress = new EmailAddress
        {
          Address = educator.Email,
          Name = $"{educator.FullName}"
        },
        Type = "required"
      } };

      return attendees;
    }

    private string ReadHtmlFile(string mailName)
    {
      var mailContent = File.ReadAllText($"{_mailerViewsRoot}/{mailName}.html");
      return mailContent;
    }
  }
}
