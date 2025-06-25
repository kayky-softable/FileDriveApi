using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using FileDriveAPi.Data;
using FileDriveAPi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with PostgreSQL connection string
builder.Services.AddDbContext<FileDriveApiDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration["ConnectionStrings:DefaultConnection"]
        ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    )
);

// Add user secrets
builder.Configuration.AddUserSecrets<Program>();

// Configuring AWS credentials
var accessKey = builder.Configuration["AWS:AccessKey"];
var secretKey = builder.Configuration["AWS:SecretKey"];

if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("AWS Access Key or Secret Key is missing.");
}

// Register AmazonS3Client with the credentials
builder.Services.AddSingleton<AmazonS3Client>(provider =>
{
    var endpoint = RegionEndpoint.USEast2; // Defina conforme necessário
    var config = new AmazonS3Config { RegionEndpoint = endpoint };

    return new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), config);
});

// Register FileService for dependency injection
builder.Services.AddScoped<FileDriveService>();

// Add controllers and Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// === Adiciona configuração de CORS ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configura o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === Aplica a política de CORS ===
app.UseCors("AllowAll");

app.MapControllers();

app.Run();