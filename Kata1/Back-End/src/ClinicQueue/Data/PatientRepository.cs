using System;
using System.Collections.Generic;
using MySqlConnector;
using ClinicQueue.Models;

namespace ClinicQueue.Data
{
    public class PatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository(string host, string database, string user, string password)
        {
            // Exemplo de Connection String para MySQL
            _connectionString = $"Server={host};Database={database};User ID={user};Password={password};";
        }

        public List<Patient> GetPendingPatients()
        {
            var patients = new List<Patient>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT p.Name, p.BirthDate, a.OriginalUrgencyId, a.ArrivalTime 
                    FROM Attendances a
                    JOIN Patients p ON a.PatientId = p.Id
                    WHERE a.Status = 'Waiting'";

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        DateTime birthDate = reader.GetDateTime(1);
                        int urgencyId = reader.GetInt32(2);
                        DateTime arrivalTime = reader.GetDateTime(3);

                        // Cálculo simples de idade
                        int age = DateTime.Today.Year - birthDate.Year;
                        if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

                        patients.Add(new Patient(
                            name,
                            age,
                            (UrgencyLevel)urgencyId,
                            arrivalTime.TimeOfDay
                        ));
                    }
                }
            }

            return patients;
        }

        public void SeedData()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Verifica se já existem dados para não duplicar
                var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Patients", connection);
                if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0) return;

                Console.WriteLine("Semeando dados iniciais no MySQL...");

                string sql = @"
                    INSERT INTO Patients (Name, BirthDate) VALUES 
                    ('João Silva', '1994-05-20'),
                    ('Maria Oliveira', '1958-10-15'),
                    ('Pedro Santos', '1983-03-12'),
                    ('Ana Costa', '2009-07-25'),
                    ('Carlos Pereira', '1974-12-01');

                    INSERT INTO Attendances (PatientId, OriginalUrgencyId, ArrivalTime, Status) VALUES 
                    (1, 1, '2024-04-20 09:00:00', 'Waiting'),
                    (2, 2, '2024-04-20 09:05:00', 'Waiting'),
                    (3, 3, '2024-04-20 09:10:00', 'Waiting'),
                    (4, 1, '2024-04-20 09:15:00', 'Waiting'),
                    (5, 4, '2024-04-20 09:20:00', 'Waiting');";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
