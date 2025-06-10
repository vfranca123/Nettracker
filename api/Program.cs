using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona os serviços dos controladores (os atendentes da API)
builder.Services.AddControllers();

// 2. Habilita o CORS (para o Angular conseguir conversar com o back-end)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // endereço do Angular
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// 3. Usa o CORS com a política definida
app.UseCors("AllowAngularApp");

// 4. Usa roteamento de controladores (para que as rotas tipo /api/rede funcionem)
app.MapControllers();

// 5. Inicia o servidor
app.Run();