using System;

namespace API.Helpers
{
  public static class ValidationResponseHelper
  {
    // Authentication Validation Message(s)
    public static string InvalidCredentials { get; } = "Invalid email/password combination.";
    public static string InvalidDuplicateEmail { get; } = "Invalid or duplicate email.";
    // Session Validation Message(s)
    public static string InvalidSessionToken { get; } = "Invalid session token";
    public static string NoClaimsData { get; } = "ClaimTypes.UserData not found.";

    // Virtual Sessions Validation Message(s)
    public static string MeetingCreationError { get; } = "Oops. Something went wrong confirming the virtual session. Please try again later soon.";    
    public static string VirtualSessionAlreadyConfirmed { get; } = "This virtual session has already been confirmed.";
  }
}