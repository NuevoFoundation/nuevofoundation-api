using System;
using Newtonsoft.Json;

namespace API.DTOs
{
  public class OnlineMeetingDto
  {
    [JsonProperty("accessLevel")]
    public string AccessLevel { get; set; }

    [JsonProperty("audioConferencing")]
    public AudioConferencing AudioConferencing { get; set; }

    [JsonProperty("canceledDateTime")]
    public DateTimeOffset? CanceledDateTime { get; set; }

    [JsonProperty("chatInfo")]
    public ChatInfo ChatInfo { get; set; }

    [JsonProperty("creationDateTime")]
    public DateTimeOffset CreationDateTime { get; set; }

    [JsonProperty("endDateTime")]
    public DateTimeOffset EndDateTime { get; set; }

    [JsonProperty("entryExitAnnouncement")]
    public bool? EntryExitAnnouncement { get; set; }

    [JsonProperty("expirationDateTime")]
    public DateTimeOffset ExpirationDateTime { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("isCancelled")]
    public bool IsCancelled { get; set; }

    [JsonProperty("joinUrl")]
    public Uri JoinUrl { get; set; }

    [JsonProperty("meetingType")]
    public string MeetingType { get; set; }

    [JsonProperty("participants")]
    public Participants Participants { get; set; }

    [JsonProperty("startDateTime")]
    public DateTimeOffset StartDateTime { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }
  }

  public class AudioConferencing
  {
    [JsonProperty("tollNumber")]
    public string TollNumber { get; set; }

    [JsonProperty("tollFreeNumber")]
    public string TollFreeNumber { get; set; }

    [JsonProperty("participantPasscode")]
    public long ParticipantPasscode { get; set; }

    [JsonProperty("leaderPasscode")]
    public object LeaderPasscode { get; set; }

    [JsonProperty("dialinUrl")]
    public Uri DialinUrl { get; set; }
  }

  public class ChatInfo
  {
    [JsonProperty("threadId")]
    public string ThreadId { get; set; }

    [JsonProperty("messageId")]
    public long MessageId { get; set; }

    [JsonProperty("replyChainMessageId")]
    public long? ReplyChainMessageId { get; set; }
  }

  public class Participants
  {
    [JsonProperty("organizer")]
    public Organizer Organizer { get; set; }
  }

  public class Organizer
  {
    [JsonProperty("identity")]
    public Identity Identity { get; set; }

    [JsonProperty("upn")]
    public string Upn { get; set; }
  }

  public class Identity
  {
    [JsonProperty("user")]
    public User User { get; set; }
  }

  public class User
  {
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("tenantId")]
    public Guid TenantId { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }
  }
}
