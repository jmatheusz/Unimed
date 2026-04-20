using System;

namespace ClinicQueue.Models
{
    public class Patient
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public UrgencyLevel UrgencyBase { get; set; }
        public TimeSpan ArrivalTime { get; set; }

        public Patient(string name, int age, UrgencyLevel urgencyBase, TimeSpan arrivalTime)
        {
            Name = name;
            Age = age;
            UrgencyBase = urgencyBase;
            ArrivalTime = arrivalTime;
        }

        // Propriedade calculada para a lógica de negócio
        public UrgencyLevel UrgencyFinal => CalculateUrgencyFinal();

        private UrgencyLevel CalculateUrgencyFinal()
        {
            var urgency = UrgencyBase;

            // Regra 4: Idosos (60+) com urgência MÉDIA sobem para ALTA
            if (Age >= 60 && urgency == UrgencyLevel.MEDIA)
                urgency = UrgencyLevel.ALTA;

            // Regra 5: Menores de 18 ganham +1 nível (até o teto de CRÍTICA)
            if (Age < 18)
            {
                urgency = urgency switch
                {
                    UrgencyLevel.BAIXA => UrgencyLevel.MEDIA,
                    UrgencyLevel.MEDIA => UrgencyLevel.ALTA,
                    UrgencyLevel.ALTA => UrgencyLevel.CRITICA,
                    _ => urgency
                };
            }

            return urgency;
        }
    }
}
