using LHPets.Data;

var builder = WebApplication.CreateBuilder(args);

// Habilita Controllers e documentação interativa com Swagger.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Um repositório compartilhado mantém as listas em memória durante a execução.
builder.Services.AddSingleton<CadastroEmMemoria>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
