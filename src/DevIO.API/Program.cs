using System.Reflection;
using DevIO.API.Configuration;
using DevIO.Business.Intefaces;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

// Instância de [WebApplicationBuilder], 
// classe responsável por oferecer acesso a toda configuração da aplicação
var builder = WebApplication.CreateBuilder(args);

// configurar os serviços:
// Configurando o AppSettings.json conforme ambiente de execução
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// Adicionando suporte ao contexto do Identity via EF
// Add services to the container.
builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ResolveDependencies();

// Resolvendo classes para injeção de dependência
//builder.Services.AddScoped<IMinhaDependencia, MinhaDependencia>();

// Adicionando Suporte a controllers e Views (MVC) no pipeline
builder.Services.AddControllersWithViews();

// Adicionando suporte a componentes Razor (ex. Telas do Identity)
builder.Services.AddRazorPages();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Using the OpenApiInfo class, modify the information displayed in the UI:
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo   
    {
        Version = "v1",
        Title = "Todo API",
        Description = "An ASP.NET Core Web API for managing ToDo Items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {   
            Name = "Example license",
            Url = new Uri("https://example.com/license")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));    
});

// A última linha de código precisa ser sempre a última, pois é ai que é feito o build 
// de tudo para retornar a instância da aplicação que será configurada na sequência. 
// Já observei muitos erros de pessoas que tentaram adicionar serviços depois do build da App, 
// respeite sempre essa divisão de responsabilidades.
var app = builder.Build();

// *** Configurando o resquest dos serviços no pipeline *** 
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();    
}

// Redirecionamento para HTTPs
app.UseHttpsRedirection();

// Uso de arquivos estáticos (ex. CSS, JS)
app.UseStaticFiles();

// Adicionando suporte a rota
app.UseRouting();

// Autenticacao e autorização (Identity)
// A ordem das declarações AFETA TOTALMENTE o comportamento da aplicação, 
// pois a execução do pipeline segue a ordem da declaração da Program. 
// O erro mais comum de todos é chamar o UseAuthorization antes do UseAuthentication
app.UseAuthentication();
app.UseAuthorization();

// Rota padrão (no caso MVC)
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Mapeando componentes Razor Pages (ex: Identity)
app.MapRazorPages();

app.MapControllers();

// Coloca a aplicação para rodar.
app.Run();
