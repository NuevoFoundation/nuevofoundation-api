using System;

namespace API
{
  public static class Settings
  {
    public static string StorageEndpoint { get; set; } = Environment.GetEnvironmentVariable("StorageEndpoint", EnvironmentVariableTarget.Process);
    public static string StorageKey { get; set; } = Environment.GetEnvironmentVariable("StorageKey", EnvironmentVariableTarget.Process);
    public static string StorageDatabaseId { get; set; } = Environment.GetEnvironmentVariable("StorageDatabaseId", EnvironmentVariableTarget.Process);
    public static string ClientId { get; set; } = Environment.GetEnvironmentVariable("ClientId", EnvironmentVariableTarget.Process);
    public static string ClientSecret { get; set; } = Environment.GetEnvironmentVariable("ClientSecret", EnvironmentVariableTarget.Process);
    public static string Scope { get; set; } = Environment.GetEnvironmentVariable("Scope", EnvironmentVariableTarget.Process);
    public static string Email { get; set; } = Environment.GetEnvironmentVariable("Email", EnvironmentVariableTarget.Process);
    public static string EmailPassword { get; set; } = Environment.GetEnvironmentVariable("EmailPassword", EnvironmentVariableTarget.Process);
    public static string TenantId { get; set; } = Environment.GetEnvironmentVariable("TenantId", EnvironmentVariableTarget.Process);
    public static string FrontendUrl { get; set; } = Environment.GetEnvironmentVariable("FrontendUrl", EnvironmentVariableTarget.Process);
    public static string JwtIssuer { get; set; } = Environment.GetEnvironmentVariable("JwtIssuer", EnvironmentVariableTarget.Process);
    public static string JwtAudience { get; set; } = Environment.GetEnvironmentVariable("JwtAudience", EnvironmentVariableTarget.Process);
    public static string JwtSecretKey { get; set; } = Environment.GetEnvironmentVariable("JwtSecretKey", EnvironmentVariableTarget.Process);
  }
}