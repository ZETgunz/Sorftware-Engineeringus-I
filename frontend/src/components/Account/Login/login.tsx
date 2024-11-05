import React from 'react';
import { Link } from 'react-router-dom';
import './login.css'; // Make sure to create a corresponding CSS file for styling

export const Login: React.FC = () => {

    const validate = async () => {

        const username = (document.getElementById("username") as HTMLInputElement).value;
        const password = (document.getElementById("password") as HTMLInputElement).value;
        try {
            const response = await fetch('http://localhost:5071/api/Account/' + username + '/' + password);
            if (!response.ok) {
                alert("User was not found")
                throw new Error(`Error: ${response.statusText}`);
            }

            const data = await response.json();
            
            console.log(data);
            
            localStorage.setItem('username', data.username);
            localStorage.setItem('password', data.password);
            localStorage.setItem('score', data.score);
            localStorage.setItem('role', data.role);    
            location.reload();
        
            return;
        } catch (error) {
            console.error('Error creating account:', error);
        }
    }

    const logOut = () => {
        localStorage.removeItem('username');
        localStorage.removeItem('password');
        localStorage.removeItem('score');
        localStorage.removeItem('role');
        location.reload();
    }

    return (
        <>
        {localStorage.getItem('username') != null ? <div>
        <h2>Welcome {localStorage.getItem('username')}</h2> 
        <h3>Your current score is {localStorage.getItem('score')}</h3>
        <h3>Your current role is {localStorage.getItem('role')}</h3>
        <button onClick={() => logOut()} className="logoutButton">Logout</button>
        </ div>
        
        : 
        
        
        <div className="login">
            <h1>Melon Login</h1>
            <label>Username:</label>
            <input type="text" id="username" name="username" placeholder="Enter your username"></input>
            <br />
            <label>Password:</label>
            <input type="password" id="password" name="password" placeholder="Enter your password"></input>
            <br />
            <button onClick={() => validate()} className="loginButton">Login</button>
            <br />
            <br />
            <label>Don't have an account?</label>
            <Link to="/account/register" className="nav-link">Click to create one</Link>
        </div >}</>
    );
}