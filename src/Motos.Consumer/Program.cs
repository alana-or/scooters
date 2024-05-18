using Microsoft.Extensions.DependencyInjection;
using Motos.Consumer;
using Motos.Data;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Configuração do serviço de injeção de dependência
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMotosRepository, MotosRepository>()
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
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
