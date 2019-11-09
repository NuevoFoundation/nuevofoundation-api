<h1 align="center">

Nuevo Foundation API

</h1>

<h3 align="center">Backend for Nuevo Foundation website</h3>

<p align="center">
  <a href="https://dev.azure.com/project-unicorn/nuevo/_build/latest?definitionId=4&branchName=master">
    <img src="https://dev.azure.com/project-unicorn/nuevo/_apis/build/status/NuevoFoundation.nuevofoundation-api?branchName=master" alt="Azure DevOps">
  </a>
</p>
<hr />


## Quick Start

```bash
git clone git@github.com:NuevoFoundation/nuevofoundation-api.git
OR
git clone https://github.com/NuevoFoundation/nuevofoundation-api.git
cd nuevofoundation-api

# Install dependencies
dotnet restore

# Develop
dotnet run --project API

# Run tests
dotnet test
```

## Set App Secrets
Running the app locally requires a set of secrets. The secrets are stored locally on your system. Here's the sample file that is stored on the system with critical secrets omitted. More info for setting the secrets on your system can be found [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=linux#set-a-secret).

```
{
  "StorageKey": "omitted",
  "StorageEndpoint": "omitted",
  "StorageDatabaseId": "omitted",
  "ClientId": "omitted",
  "ClientSecret": "omitted",
  "Scope": "https://graph.microsoft.com/.default",
  "TenantId": "omitted",
  "Email": "omitted",
  "EmailPassword": "omitted",
  "JwtSecretKey": "omitted",
  "JwtSecretIssuer": "omitted",
  "JwtAudience": "omitted",
}
```

Below is a brief description for each secret


`StorageKey`, `StorageEndpoint`, `StorageDatabaseId` - Credentials for connecting to the cosmos db storage.

`ClientId`, `ClientSecret`, `TenantId`, `Scope` - Credentials for authenticating against the Microsoft Graph API - used for creating teams meeting

`Email`, `EmailPassword` - Credentials for sending email via Smtp client

`JwtSecretKey`, `JwtSecretIssuer`, `JwtAudience` - Values for jwt token validation

## Contributing

Please see our [Contributing Guide](CONTRIBUTING.md).
