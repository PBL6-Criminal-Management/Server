using Application.Extensions;
using Application.Hubs.Notification;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Shared.Extensions;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using WebApi.Extensions;

Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .CreateBootstrapLogger();
var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} up");

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
   string wifiAddress = "localhost";

   // Get all available network interfaces
   NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

   foreach (NetworkInterface networkInterface in interfaces)
   {
       if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && networkInterface.OperationalStatus == OperationalStatus.Up)
       {
           IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
           foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
           {
               if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
               {
                   wifiAddress = ip.Address.ToString();
               }
           }
       }
   }
   builder.WebHost.UseUrls($"https://{wifiAddress}:1234", $"http://{wifiAddress}:5678"); //set server listen on wifi address
   Console.WriteLine("SWAGGER PATH: " + $"https://{wifiAddress}:1234/swagger/index.html" + " OR " + $"http://{wifiAddress}:5678/swagger/index.html");
}

// Add services to the container.
try
{
    builder.Host.AddAppConfigurations();

    builder.Services.AddApplicationExtensions();

    builder.Services.AddPersistenceInfrastructure(builder.Configuration);

    builder.Services.AddSharedExtensions(builder.Configuration);

    builder.Services.AddSwaggerExtension();

    builder.Services.AddHangFire(builder.Configuration);

    builder.Services.AddApiversioningExtension();

    builder.Services.AddCorsExtensions();

    builder.Services.AddRepositories();

    builder.Services.AddIdentityServices();

    builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));

    builder.Services.AddCurrentUserService();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<CustomValidationFilter>(int.MinValue);
    });
    //Apply converter for all datetime/dateonly fields
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    //    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    //});

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddLazyCache();

    //SignalR
    builder.Services.AddSignalR();
    builder.Services.AddResponseCompression(options =>
        options.MimeTypes = ResponseCompressionDefaults
        .MimeTypes
        .Concat(new[] { "application/octet-stream" })
    );

    var app = builder.Build();
    app.UseResponseCompression();

    app.UseRouting();

    app.UseFolderAsStatic();

    app.UseCors("CorsPolicy");

    app.UseSwaggerExtension();

    app.UseHangfireExtension();

    app.UseErrorHandlingMiddleware();

    //app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    //app.UseHealthChecks("/health");
    // app.MapHub<NotificationService>("/api/notification");

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<NotificationService>("/api/notification");
    });

    // Seeding data
    app.InitializeDb();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}