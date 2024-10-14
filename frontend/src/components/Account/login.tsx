import React, { useState, useEffect } from 'react';
import './login.css'; // Make sure to create a corresponding CSS file for styling

export const Login: React.FC = () => {

    const validate = async () => {

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
        </div >
    );
}