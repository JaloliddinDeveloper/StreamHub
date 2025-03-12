using Microsoft.AspNetCore.Http.Features;
using StreamHub.Brokers.Storages;
using StreamHub.Components;
using StreamHub.Services.Foundations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ApplicationName = typeof(Program).Assembly.FullName,
            EnvironmentName = Environments.Development,
            ContentRootPath = AppContext.BaseDirectory,
            WebRootPath = "wwwroot"
        });

        // Barcha IP-lardan ulanishga ruxsat berish
        builder.WebHost.UseUrls("http://0.0.0.0:5000");

        // Razor Components qo'shish
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Broker va Service'larni ro‘yxatdan o‘tkazish
        builder.Services.AddTransient<IStorageBroker, StorageBroker>();
        builder.Services.AddTransient<IVideoMetadataService, VideoMetadataService>();

        // Fayl yuklash limitini 1 GB ga o‘rnatish
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1073741824; // 1 GB
        });

        // CORS ni qo‘shish
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        // Statik fayllarni ulash
        app.UseStaticFiles();

        // Routingni ishga tushirish
        app.UseRouting();

        // CORS-ni ishga tushirish
        app.UseCors("AllowAll");

        // Antiforgeryni faollashtirish
        app.UseAntiforgery();

        // HTTPS-ni ishga tushirish
        app.UseHttpsRedirection();

        // Razor Components ni ulash
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Ilovani ishga tushirish
        app.Run();
    }
}
