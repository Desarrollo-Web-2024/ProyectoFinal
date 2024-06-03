using api.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddDbContext<ProjectContext>(c => {
    if (builder.Environment.IsDevelopment()) {
        c.EnableSensitiveDataLogging();
    }
});

builder.Services.AddCors(c => {
    c.AddDefaultPolicy(co => {
        co.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.MapControllers().WithOpenApi();
app.Run();
