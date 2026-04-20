using MySqlConnector;
using ClinicQueue.Models;
using ClinicQueue.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // URL do Vite
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection String carregada de forma segura via appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");

var queueService = new QueueService();

// ENDPOINTS

// 1. Listar Fila (Ordenada)
app.MapGet("/api/patients", async () =>
{
    var patients = new List<Patient>();
    using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    string sql = @"
        SELECT p.Name, p.BirthDate, a.OriginalUrgencyId, a.ArrivalTime 
        FROM Attendances a
        JOIN Patients p ON a.PatientId = p.Id
        WHERE a.Status = 'Waiting'";

    using var command = new MySqlCommand(sql, connection);
    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        string name = reader.GetString(0);
        DateTime birthDate = reader.GetDateTime(1);
        int urgencyId = reader.GetInt32(2);
        DateTime arrivalTime = reader.GetDateTime(3);

        int age = DateTime.Today.Year - birthDate.Year;
        if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

        patients.Add(new Patient(name, age, (UrgencyLevel)urgencyId, arrivalTime.TimeOfDay));
    }

    return Results.Ok(queueService.OrderQueue(patients));
});

// 2. Adicionar Paciente
app.MapPost("/api/patients", async ([FromBody] PatientRequest request) =>
{
    using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    using var transaction = await connection.BeginTransactionAsync();
    try
    {
        // 1. Inserir Paciente
        string sqlPatient = "INSERT INTO Patients (Name, BirthDate) VALUES (@name, @birthDate); SELECT LAST_INSERT_ID();";
        using var cmdPatient = new MySqlCommand(sqlPatient, connection, transaction);
        cmdPatient.Parameters.AddWithValue("@name", request.Name);
        cmdPatient.Parameters.AddWithValue("@birthDate", request.BirthDate);
        
        var patientId = await cmdPatient.ExecuteScalarAsync();

        // 2. Inserir Atendimento na Fila
        string sqlAttendance = "INSERT INTO Attendances (PatientId, OriginalUrgencyId, ArrivalTime, Status) VALUES (@pId, @urgency, NOW(), 'Waiting')";
        using var cmdAttendance = new MySqlCommand(sqlAttendance, connection, transaction);
        cmdAttendance.Parameters.AddWithValue("@pId", patientId);
        cmdAttendance.Parameters.AddWithValue("@urgency", (int)request.UrgencyBase);
        
        await cmdAttendance.ExecuteNonQueryAsync();
        await transaction.CommitAsync();

        Console.WriteLine($"[DB] Paciente {request.Name} salvo com sucesso!");

        return Results.Created($"/api/patients", request);
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        return Results.Problem(ex.Message);
    }
});

app.Run();

// DTO para o POST
public record PatientRequest(string Name, DateTime BirthDate, UrgencyLevel UrgencyBase);
