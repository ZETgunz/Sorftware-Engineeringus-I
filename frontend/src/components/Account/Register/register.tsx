import React from 'react';
import './register.css'; // Make sure to create a corresponding CSS file for styling

export const Register: React.FC = () => {

    const validate = async () => {


    }

    return (
        <div className="register">
            <h1>Melon Register</h1>
            <label>Username:</label>
            <input type="text" id="username" name="username" placeholder="Enter your username"></input>
            <br />
            <label>Password:</label>
            <input type="password" id="password" name="password" placeholder="Enter your password"></input>
            <br />
            <button onClick={() => validate()} className="Register">Register</button>
        </div >
    );
}