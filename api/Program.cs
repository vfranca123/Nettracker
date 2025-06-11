using Api.Models;
//testando caso da varredura sem threade deu certo
/*
class Program
{
    static void Main(string[] args)
    {

        //testando caso da varredura sem thread
        var rede = new Rede();

        rede.inicio = 1;
        rede.fim = 10;

        rede.gatilhoVarrefuraSemThread();
        rede.ExibirResultados();

        
        
        
    }
}*/

using Microsoft.OpenApi.Models; // IMPORTANTE para Swagger

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

// 3. Adiciona suporte ao Swagger (interface gráfica para testes)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });
});

// 4. Constrói o app
var app = builder.Build();

// 5. Ativa o Swagger em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
    });
}

// 6. Usa o CORS com a política definida
app.UseCors("AllowAngularApp");

// 7. Usa roteamento de controladores
app.MapControllers();

// 8. Inicia o servidor
app.Run();
