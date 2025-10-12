namespace Authentica.Service.Identity;

/// <summary>
/// The entry point for the Web Application.
/// </summary>
public sealed class Program
{
    /// <summary>
    /// The entry method for the web application.
    /// </summary>
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        await app.RunAsync();
    }
}
