using Microsoft.AspNetCore.Http.Features;
using StreamHub.Brokers.Storages;
using StreamHub.Components;
using StreamHub.Services.Foundations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddTransient<IStorageBroker, StorageBroker>();
        builder.Services.AddTransient<IVideoMetadataService, VideoMetadataService>();

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1073741824;
        });

        var app = builder.Build();

        app.UseStaticFiles();
        app.UseRouting();


        app.UseAntiforgery();

        app.UseHttpsRedirection();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}