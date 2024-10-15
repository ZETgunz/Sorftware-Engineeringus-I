import React, { useState, useEffect } from 'react';
import './leaderboard.css'; // Make sure to create a corresponding CSS file for styling

interface board {
    username: string;
    score: number;
}

export const Leaderboard: React.FC = () => {

    //var [string, setString] = useState([""]);
    var string = [""];

    const fetchLeaderboard = async () => {
        try {
            const response = await fetch('http://localhost:5071/api/Leaderboard');
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }
            const data: board[] = await response.json();
            string.pop();
            for(var i=0; i<data.length; i++){
                string.push(`${data[i].username}: ${data[i].score}`);
                //str += `${i.username}: ${i.score}`;
            }
            console.log(string);
            return string;
        } catch (error) {
            console.error('Error fetching leaderboard:', error);
        }
    }

    fetchLeaderboard();

    return (
        <div className="leaderboard">
            <h1>Melon Leaderboards</h1>
            {string.map((i: number) => (
                <div key={i}>
                    <label>{string}</label>
                </div>
            ))}
        </div >
    );
}