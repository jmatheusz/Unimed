using System;
using System.Collections.Generic;
using ClinicQueue.Models;
using ClinicQueue.Services;
using ClinicQueue.Data;

namespace ClinicQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            // CONFIGURAÇÃO DO BANCO DE DADOS
            // Altere os valores abaixo para conectar no seu MySQL local
            string host = "localhost";
            string database = "hospital";
            string user = "root";
            string password = "#LeavemeaLone9393";

            var repo = new PatientRepository(host, database, user, password);
            var service = new QueueService();

            Console.WriteLine("=== SISTEMA DE FILA DE TRIAGEM (MySQL) ===");
            
            try 
            {
                Console.WriteLine($"\nTentando conectar ao banco '{database}' em '{host}'...");
                
                // Popula o banco com dados de teste se estiver vazio
                repo.SeedData();

                // Busca os pacientes que estão com status 'Waiting' no MySQL
                List<Patient> patients = repo.GetPendingPatients();

                if (patients.Count == 0)
                {
                    Console.WriteLine("\n[AVISO] Nenhum paciente encontrado com status 'Waiting' no banco de dados.");
                    Console.WriteLine("Certifique-se de que você inseriu dados nas tabelas Patients e Attendances.");
                    return;
                }

                Console.WriteLine($"\n{patients.Count} pacientes encontrados. Processando triagem...");

                // Aplica a lógica de ordenação (Regras 1 a 5)
                var orderedQueue = service.OrderQueue(patients);

                Console.WriteLine("\nFILA ORDENADA PARA ATENDIMENTO:");
                Console.WriteLine("---------------------------------------------------------------------------");
                Console.WriteLine($"{"Chegada",-10} | {"Nome",-25} | {"Idade",-6} | {"Urgência Final",-15}");
                Console.WriteLine("---------------------------------------------------------------------------");

                foreach (var p in orderedQueue)
                {
                    string horario = p.ArrivalTime.ToString(@"hh\:mm");
                    Console.WriteLine($"{horario,-10} | {p.Name,-25} | {p.Age,-6} | {p.UrgencyFinal,-15}");
                }
                Console.WriteLine("---------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n[ERRO] Não foi possível conectar ao MySQL.");
                Console.WriteLine($"Detalhes: {ex.Message}");
                Console.WriteLine("\nPASSO A PASSO PARA CORREÇÃO:");
                Console.WriteLine("1. Verifique se o MySQL está rodando.");
                Console.WriteLine("2. Verifique se o banco 'clinic_db' foi criado.");
                Console.WriteLine("3. Ajuste o usuário e a senha no arquivo Program.cs.");
            }
        }
    }
}
