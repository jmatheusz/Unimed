import { useState, useEffect } from 'react'
import { taskApi, type Task } from './api'
import './App.css'

function App() {
  const [tasks, setTasks] = useState<Task[]>([])
  const [title, setTitle] = useState('')
  const [priority, setPriority] = useState('Média')
  const [filter, setFilter] = useState('all')
  const [loading, setLoading] = useState(false)

  const fetchTasks = async () => {
    setLoading(true)
    try {
      const data = await taskApi.getAll(filter)
      setTasks(data)
    } catch (error) {
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchTasks()
  }, [filter])

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!title.trim()) return
    try {
      await taskApi.create(title, priority)
      setTitle('')
      fetchTasks()
    } catch (error) {
      console.error(error)
    }
  }

  const toggleTask = async (task: Task) => {
    try {
      await taskApi.update(task.id, { completed: !task.completed })
      fetchTasks()
    } catch (error) {
      console.error(error)
    }
  }

  const deleteTask = async (id: string) => {
    try {
      await taskApi.delete(id)
      fetchTasks()
    } catch (error) {
      console.error(error)
    }
  }

  return (
    <div className="container">
      <header>
        <div className="logo">
          <span className="icon">⚡</span>
          <h1>TaskFlow</h1>
        </div>
        <p className="subtitle">Gerencie suas tarefas com .NET & React.</p>
      </header>

      <main>
        <section className="add-task-card glass">
          <form onSubmit={handleSubmit}>
            <input 
              type="text" 
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="O que precisa ser feito?" 
              required 
            />
            <select value={priority} onChange={(e) => setPriority(e.target.value)}>
              <option value="Baixa">Baixa</option>
              <option value="Média">Média</option>
              <option value="Alta">Alta</option>
            </select>
            <button type="submit" id="add-btn">Adicionar</button>
          </form>
        </section>

        <section className="filter-section">
          <div className="filters">
            {['all', 'pending', 'completed'].map((f) => (
              <button 
                key={f}
                className={`filter-btn ${filter === f ? 'active' : ''}`}
                onClick={() => setFilter(f)}
              >
                {f === 'all' ? 'Todas' : f === 'pending' ? 'Pendentes' : 'Concluídas'}
              </button>
            ))}
          </div>
        </section>

        <section className="task-list-container">
          <div className="task-list">
            {loading ? (
              <p className="loading">Carregando...</p>
            ) : tasks.length === 0 ? (
              <div className="empty-state">
                <p>Nenhuma tarefa encontrada.</p>
              </div>
            ) : (
              tasks.map(task => (
                <div key={task.id} className={`task-item glass ${task.completed ? 'completed' : ''}`}>
                  <input 
                    type="checkbox" 
                    className="task-checkbox" 
                    checked={task.completed}
                    onChange={() => toggleTask(task)}
                  />
                  <div className="task-content">
                    <div className="task-title">{task.title}</div>
                    <span className={`priority-badge priority-${task.priority}`}>{task.priority}</span>
                  </div>
                  <button className="delete-btn" onClick={() => deleteTask(task.id)}>
                    &times;
                  </button>
                </div>
              ))
            )}
          </div>
        </section>
      </main>

      <footer>
        <p>&copy; 2026 TaskFlow | Professional Stack</p>
      </footer>
    </div>
  )
}

export default App
