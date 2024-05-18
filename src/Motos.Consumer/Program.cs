using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Motos.Consumer;
using Motos.Data;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMotosRepository, MotosRepository>()
            .AddDbContext<MotosContext>(options =>
                options.UseNpgsql("Host=motos_db;Port=5432;Username=postgres;Password=postgrespw;Database=motos_db;Search Path=public"))
            .AddTransient<MotosListener>(provider =>
                new MotosListener(
                    hostname: "localhost",
                    queueName: "motos_queue",
                    username: "guest",
                    password: "guest",
                    motosRepository: provider.GetRequiredService<IMotosRepository>())
            )
            .BuildServiceProvider();

        using (var listener = serviceProvider.GetRequiredService<MotosListener>())
        {
            listener.StartListening();
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
