using Election.App.Features.Candidates.Commands;
using Election.App.Features.Candidates.Handlers;
using Election.Core;
using Election.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register ApplicationDbContext with InMemory provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("ElectionDb"));

// Register MediatR from Election.App

builder.Services.AddMediatR(
    typeof(CreateCandidateHandler).Assembly,
    typeof(DeleteCandidateHandler).Assembly,
    typeof(FilterCandidatesHandler).Assembly,
    typeof(GetAllCandidatesHandler).Assembly,
    typeof(GetCandidateByIdHandler).Assembly,
    typeof(UpdateCandidateHandler).Assembly
    );

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
    );
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<Election.Api.Services.CandidateGeneratorService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Seed initial data if database is empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Candidates.Any())
    {
        db.Candidates.AddRange(
            new Candidate { Id = Guid.NewGuid(), Name = "Andrei Popescu", Image = "https://randomuser.me/api/portraits/men/11.jpg", Party = "Green Party", Description = "Environmental activist and community leader." },
            new Candidate { Id = Guid.NewGuid(), Name = "Emily Johnson", Image = "https://randomuser.me/api/portraits/women/21.jpg", Party = "Liberal Alliance", Description = "Economist with a focus on education reform." },
            new Candidate { Id = Guid.NewGuid(), Name = "Maria Ionescu", Image = "https://randomuser.me/api/portraits/women/31.jpg", Party = "Social Democrats", Description = "Healthcare professional and advocate for public health." },
            new Candidate { Id = Guid.NewGuid(), Name = "James Smith", Image = "https://randomuser.me/api/portraits/men/41.jpg", Party = "Conservative Union", Description = "Entrepreneur and supporter of small businesses." },
            new Candidate { Id = Guid.NewGuid(), Name = "Ioana Dumitrescu", Image = "https://randomuser.me/api/portraits/women/51.jpg", Party = "Progressive Movement", Description = "Teacher and champion for youth programs." },
            new Candidate { Id = Guid.NewGuid(), Name = "David Brown", Image = "https://randomuser.me/api/portraits/men/61.jpg", Party = "People's Party", Description = "Retired engineer and infrastructure specialist." },
            new Candidate { Id = Guid.NewGuid(), Name = "Elena Marinescu", Image = "https://randomuser.me/api/portraits/women/71.jpg", Party = "Liberty Party", Description = "Lawyer and defender of civil rights." },
            new Candidate { Id = Guid.NewGuid(), Name = "Michael Williams", Image = "https://randomuser.me/api/portraits/men/81.jpg", Party = "Green Party", Description = "Urban planner and environmentalist." },
            new Candidate { Id = Guid.NewGuid(), Name = "Gabriel Stan", Image = "https://randomuser.me/api/portraits/men/91.jpg", Party = "Conservative Union", Description = "Former mayor and experienced policymaker." },
            new Candidate { Id = Guid.NewGuid(), Name = "Sophia Miller", Image = "https://randomuser.me/api/portraits/women/99.jpg", Party = "Social Democrats", Description = "Social worker and advocate for equality." }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("FrontendPolicy");
app.MapControllers();
app.MapHub<Election.Api.Hubs.CandidateHub>("/candidateHub");
app.MapPost("/api/candidates/generate/start", (Election.Api.Services.CandidateGeneratorService gen) => { gen.Start(); return Results.Ok(); });
app.MapPost("/api/candidates/generate/stop", (Election.Api.Services.CandidateGeneratorService gen) => { gen.Stop(); return Results.Ok(); });

app.Run();
