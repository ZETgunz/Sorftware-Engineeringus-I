const BASE_URL = 'http://localhost:5071/api/Session';

interface UserData {
    username: string;
}

interface SessionData {
    sessionId: string;
}

export const createSession = async (userData: UserData): Promise<SessionData> => {
    try {
        const response = await fetch(`${BASE_URL}/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include', // Ensure cookies are included in the request
            body: JSON.stringify(userData),
        });

        console.log(JSON.stringify(userData));

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        const data: SessionData = await response.json();
        return data; // { sessionId: "session-id" }
    } catch (error) {
        console.error('Error creating session:', error);
        throw error;
    }
};

export const getSession = async () => {
    try {
        const response = await fetch(`${BASE_URL}/get`, {
            method: 'GET',
            credentials: 'include',
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        const data = await response.json();
        return data; // Session data
    } catch (error) {
        console.error('Error retrieving session:', error);
        throw error;
    }
};

export const logout = async () => {
    try {
        const response = await fetch(`${BASE_URL}/logout`, {
            method: 'POST',
            credentials: 'include',
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }

        const message = await response.text();
        return message; // Logout success message
    } catch (error) {
        console.error('Error logging out:', error);
        throw error;
    }
};