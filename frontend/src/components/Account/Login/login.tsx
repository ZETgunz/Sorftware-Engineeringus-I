import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './login.css';
import { createSession, getSession, logout } from '../../../Utils/Session';
import { hashString } from '../../../Utils/Hash';

export const Login: React.FC = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [username, setUsername] = useState('');
    const [score, setScore] = useState(0);
    const [rank, setRank] = useState(0);
    const [loading, setLoading] = useState(true);

    // Check session on component mount
    useEffect(() => {
        const checkSession = async () => {
            try {
                const session = await getSession();
                if (session) {
                    setUsername(session.username);
                    setIsLoggedIn(true);
                    handleScore(session.username);
                }
                else {
                    throw new Error('No session found');
                }
            } catch {
                setIsLoggedIn(false);
            } finally {
                setLoading(false);
            }
        };

        checkSession();
    }, []);

    const validate = async () => {
        const usernameInput = (document.getElementById('username') as HTMLInputElement).value;
        const passwordInput = (document.getElementById('password') as HTMLInputElement).value;

        try {
            const response = await fetch('http://localhost:5071/api/Account/getAccount', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username: usernameInput, password: hashString(passwordInput) }),
            });

            if (!response.ok) {
                alert('User not found or invalid credentials.');
                throw new Error(`Error: ${response.statusText}`);
            }

            // Create session and update state
            await createSession({ username: usernameInput });
            const session = await getSession();
            setUsername(session.username);
            setIsLoggedIn(true);
            handleScore(session.username);
        } catch (error) {
            console.error('Error logging in:', error);
        }
    };

    const handleScore = async (Username) => {
        try{
            const data = await fetch('http://localhost:5071/api/Leaderboard/'+Username);
            if (!data.ok) {
                throw new Error(`Error: ${data.statusText}`);
            }
            const scoreData = await data.json();
            console.log(scoreData);
            setScore(scoreData.score);
            setRank(scoreData.rank);
        } catch (error) {
            console.error('Error fetching score:', error);
        }
    };

    const handleLogout = async () => {
        try {
            await logout();
            setIsLoggedIn(false);
            setUsername('');
        } catch (error) {
            console.error('Error logging out:', error);
        }
    };
    return (

    <>
            {loading ? (
                <div>Loading...</div>
            ) : isLoggedIn ? (
       
                <div>
                    <h2 className="h2_welcome">Welcome {username}</h2>
                    <h3 className="h3_score">Your score is {score}</h3>
                    <h3 className="h3_role">Your rank is {rank}</h3>
                    <button onClick={handleLogout} className="logoutButton">
                        Logout
                    </button>
                </div>
            ) : (
                <div className="login">
                    <h1>Melon Login</h1>
                    <label>Username:</label>
                    <input
                        type="text"
                        id="username"
                        name="username"
                        placeholder="Enter your username"
                    />
                    <br />
                    <label>Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        placeholder="Enter your password"
                    />
                    <br />
                    <button onClick={validate} className="loginButton">
                        Login
                    </button>
                    <br />
                    <br />
                    <label>Don't have an account?</label>
                    <Link to="/account/register" className="nav-link">
                        Click to create one
                    </Link>
                </div>
            )}
        </>
    );
};
