import React, { useState, useEffect } from 'react';
import { PlusCircle, Users, Activity, Clock, RefreshCw, Calendar } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { UrgencyLevel, getUrgencyFinal } from './types';
import type { Patient, UrgencyLevelType } from './types';
import './index.css';

const API_URL = 'http://localhost:5001/api/patients'; // Usando a porta 5001 que sugerimos

const App: React.FC = () => {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    name: '',
    birthDate: '',
    urgency: UrgencyLevel.BAIXA.toString(),
  });

  const fetchPatients = async () => {
    setLoading(true);
    try {
      const response = await fetch(API_URL);
      if (response.ok) {
        const data = await response.json();
        setPatients(data);
      }
    } catch (error) {
      console.error('Erro ao buscar pacientes:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPatients();
    const interval = setInterval(fetchPatients, 10000);
    return () => clearInterval(interval);
  }, []);

  const handleAddPatient = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.name || !formData.birthDate) return;

    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: formData.name,
          birthDate: formData.birthDate,
          urgencyBase: parseInt(formData.urgency)
        }),
      });

      if (response.ok) {
        setFormData({ name: '', birthDate: '', urgency: UrgencyLevel.BAIXA.toString() });
        fetchPatients();
      }
    } catch (error) {
      console.error('Erro ao adicionar paciente:', error);
    }
  };

  const getUrgencyName = (level: UrgencyLevelType): string => {
    return Object.keys(UrgencyLevel).find(key => UrgencyLevel[key as keyof typeof UrgencyLevel] === level) || '';
  };

  return (
    <div className="container">
      <header>
        <motion.h1 
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
        >
          Hospital Triagem Pro
        </motion.h1>
        <p>Conectado ao Banco de Dados MySQL</p>
      </header>

      <div className="grid">
        {/* Form Card */}
        <motion.div 
          className="card"
          initial={{ opacity: 0, x: -30 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ delay: 0.2 }}
        >
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', marginBottom: '1.5rem' }}>
            <PlusCircle className="text-primary" />
            <h2 style={{ fontSize: '1.25rem' }}>Novo Paciente</h2>
          </div>

          <form onSubmit={handleAddPatient}>
            <div className="input-group">
              <label>Nome do Paciente</label>
              <input 
                type="text" 
                className="input-field" 
                placeholder="Ex: João Silva"
                value={formData.name}
                onChange={e => setFormData({...formData, name: e.target.value})}
                required
              />
            </div>

            <div className="input-group">
              <label>Data de Nascimento</label>
              <input 
                type="date" 
                className="input-field" 
                value={formData.birthDate}
                onChange={e => setFormData({...formData, birthDate: e.target.value})}
                required
              />
            </div>

            <div className="input-group">
              <label>Nível de Urgência</label>
              <select 
                className="input-field"
                value={formData.urgency}
                onChange={e => setFormData({...formData, urgency: e.target.value})}
              >
                <option value={UrgencyLevel.BAIXA}>BAIXA</option>
                <option value={UrgencyLevel.MEDIA}>MÉDIA</option>
                <option value={UrgencyLevel.ALTA}>ALTA</option>
                <option value={UrgencyLevel.CRITICA}>CRÍTICA</option>
              </select>
            </div>

            <button type="submit" className="btn-submit">
              <PlusCircle size={20} />
              Salvar no MySQL
            </button>
          </form>
        </motion.div>

        {/* Queue List */}
        <motion.div 
          className="card"
          initial={{ opacity: 0, x: 30 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ delay: 0.3 }}
        >
          <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '1.5rem' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
              <Users className="text-primary" />
              <h2 style={{ fontSize: '1.25rem' }}>Fila em Tempo Real</h2>
            </div>
            <button 
              onClick={fetchPatients} 
              style={{ background: 'none', border: 'none', color: 'var(--text-muted)', cursor: 'pointer' }}
              title="Atualizar agora"
            >
              <RefreshCw size={18} className={loading ? 'animate-spin' : ''} />
            </button>
          </div>

          <div className="queue-list">
            <AnimatePresence>
              {patients.length === 0 ? (
                <motion.div 
                  className="empty-state"
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  key="empty"
                >
                  {loading ? 'Carregando fila...' : 'Nenhum paciente aguardando no banco'}
                </motion.div>
              ) : (
                patients.map((patient) => {
                  const finalUrgency = getUrgencyFinal(patient);
                  const urgencyName = getUrgencyName(finalUrgency);
                  return (
                    <motion.div 
                      key={patient.id}
                      layout
                      initial={{ opacity: 0, scale: 0.9 }}
                      animate={{ opacity: 1, scale: 1 }}
                      exit={{ opacity: 0, scale: 0.9 }}
                      className="patient-item"
                    >
                      <div className="patient-info">
                        <span className="patient-name">{patient.name}</span>
                        <div className="patient-meta">
                          <span style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                            <Calendar size={14} /> {patient.age} anos
                          </span>
                          <span style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                            <Clock size={14} /> {patient.arrivalTime.substring(0, 5)}
                          </span>
                        </div>
                      </div>
                      <div className={`urgency-badge badge-${urgencyName.toLowerCase()}`}>
                        {urgencyName}
                      </div>
                    </motion.div>
                  );
                })
              )}
            </AnimatePresence>
          </div>
        </motion.div>
      </div>
    </div>
  );
};

export default App;
