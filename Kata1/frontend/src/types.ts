export const UrgencyLevel = {
  BAIXA: 1,
  MEDIA: 2,
  ALTA: 3,
  CRITICA: 4
} as const;

export type UrgencyLevelType = typeof UrgencyLevel[keyof typeof UrgencyLevel];

export interface Patient {
  id: string;
  name: string;
  age: number;
  urgencyBase: UrgencyLevelType;
  arrivalTime: string; // HH:mm
}

export const getUrgencyFinal = (patient: Patient): UrgencyLevelType => {
  let urgency = patient.urgencyBase;

  // Regra 4: Idosos (60+) com urgência MÉDIA sobem para ALTA
  if (patient.age >= 60 && urgency === UrgencyLevel.MEDIA) {
    urgency = UrgencyLevel.ALTA;
  }

  // Regra 5: Menores de 18 ganham +1 nível
  if (patient.age < 18) {
    if (urgency === UrgencyLevel.BAIXA) urgency = UrgencyLevel.MEDIA;
    else if (urgency === UrgencyLevel.MEDIA) urgency = UrgencyLevel.ALTA;
    else if (urgency === UrgencyLevel.ALTA) urgency = UrgencyLevel.CRITICA;
  }

  return urgency;
};

export const sortQueue = (patients: Patient[]): Patient[] => {
  return [...patients].sort((a, b) => {
    const urgencyA = getUrgencyFinal(a);
    const urgencyB = getUrgencyFinal(b);

    if (urgencyA !== urgencyB) {
      return (urgencyB as number) - (urgencyA as number); // Descending
    }

    // FIFO: Arrival Time ascending
    return a.arrivalTime.localeCompare(b.arrivalTime);
  });
};
