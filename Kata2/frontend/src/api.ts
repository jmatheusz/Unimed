const BASE_URL = 'http://localhost:5000/tasks';

export type Task = {
    id: string;
    title: string;
    completed: boolean;
    priority: string;
    createdAt: string;
}

export const taskApi = {
    async getAll(status?: string): Promise<Task[]> {
        const url = status && status !== 'all' ? `${BASE_URL}?status=${status}` : BASE_URL;
        const response = await fetch(url);
        if (!response.ok) throw new Error('Failed to fetch tasks');
        return response.json();
    },

    async create(title: string, priority: string): Promise<Task> {
        const response = await fetch(BASE_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title, priority })
        });
        if (!response.ok) throw new Error('Failed to create task');
        return response.json();
    },

    async update(id: string, updates: Partial<Task>): Promise<Task> {
        const response = await fetch(`${BASE_URL}/${id}`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updates)
        });
        if (!response.ok) throw new Error('Failed to update task');
        return response.json();
    },

    async delete(id: string): Promise<void> {
        const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
        if (!response.ok) throw new Error('Failed to delete task');
    }
};
