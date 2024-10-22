import React, { useState, useEffect } from 'react';
import { AccountRegister } from '../../../pages/Account/Register/Register';
import { Link } from 'react-router-dom';
import './login.css'; // Make sure to create a corresponding CSS file for styling

interface account {
    username: string;
    password: string;
}

export const Login: React.FC = () => {

    const validate = async () => {
        
        const username = (document.getElementById("username") as HTMLInputElement).value;
        try {
            const response = await fetch('http://localhost:5071/api/Account/' + username);
            if (!response.ok) {
                alert("Username not found")
                throw new Error(`Error: ${response.statusText}`);
            }
            alert(`Welcome, ${username}!`)
            return;
        } catch (error) {
            console.error('Error creating account:', error);
        }
    }

    return (
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
            <Link to="/account/register" element={<AccountRegister />} className="nav-link">Click to create one</Link>
        </div >
    );
}