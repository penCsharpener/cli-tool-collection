using TextCompAs.Worker.Extensions;

namespace TextCompAs.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.RegisterServices(builder.Configuration);

        var host = builder.Build();
        host.Run();
    }
}