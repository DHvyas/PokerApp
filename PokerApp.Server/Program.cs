using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Repositories;
using PokerApp.Server.Services;
using System.Text;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Configure DbContext with connection string
        services.Configure<DatabaseSettings>(Configuration.GetSection("ConnectionStrings"));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHandService, HandService>();
        services.AddSingleton<IDealerService, DealerService>();
        services.AddScoped<IRoundService, RoundService>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IHandRepository, HandRepository>();
        services.AddSingleton<IGameRepository, GameRepository>();
        services.AddSingleton<IGamePlayerRepository, GamePlayerRepository>();
        services.AddSingleton<IRoundRepository, RoundRepository>();
        services.AddSingleton<ICommunityCardsRepository, CommunityCardsRepository>();
        services.AddSingleton<IBetRepository, BetRepository>();
        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtBearer:ValidIssuer"],
                ValidAudience = Configuration["JwtBearer:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtBearer:IssuerSigningKey"]!))
            };
        }).AddIdentityCookies();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseCors("AllowOrigin");
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication(); // Add this line to enable authentication

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("/index.html");
        });
        app.UseHttpsRedirection();
        app.UseStaticFiles();


    }
}
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

