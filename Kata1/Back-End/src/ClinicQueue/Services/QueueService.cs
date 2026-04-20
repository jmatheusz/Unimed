using System.Collections.Generic;
using System.Linq;
using ClinicQueue.Models;

namespace ClinicQueue.Services
{
    public class QueueService
    {
        public List<Patient> OrderQueue(List<Patient> patients)
        {
            if (patients == null) return new List<Patient>();

            return patients
                .OrderByDescending(p => (int)p.UrgencyFinal) // Regras 1, 2, 4 e 5
                .ThenBy(p => p.ArrivalTime)                // Regra 3 (FIFO)
                .ToList();
        }
    }
}
