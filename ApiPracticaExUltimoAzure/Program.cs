using ApiPracticaExUltimoAzure.Data;
using ApiPracticaExUltimoAzure.Helpers;
using ApiPracticaExUltimoAzure.Repositories;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});

SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret vaultSecret = await secretClient.GetSecretAsync("SqlAzureCubos");
KeyVaultSecret issueSecret = await secretClient.GetSecretAsync("Issuer");
KeyVaultSecret audSecret = await secretClient.GetSecretAsync("Audience");
KeyVaultSecret keySecret = await secretClient.GetSecretAsync("SecretKey");

MyKeys.SqlAzureCubos = vaultSecret.Value;
MyKeys.Issue = issueSecret.Value;
MyKeys.Audience = audSecret.Value;
MyKeys.SecretKey = keySecret.Value;

HelperOAuth helperOAuth = new HelperOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperOAuth>(helperOAuth);
builder.Services.AddAuthentication(helperOAuth.GetAuthenticateSchema()).AddJwtBearer(helperOAuth.GetJwtBearerOptions());

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddTransient<CubosRepository>();
builder.Services.AddSingleton<HelperSecurityUser>();
builder.Services.AddDbContext<CubosContext>(x => x.UseSqlServer(MyKeys.SqlAzureCubos));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



var app = builder.Build();










// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();

app.UseHttpsRedirection();


app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/openapi/v1.json", "Api Mine2U");
    x.RoutePrefix = "";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
