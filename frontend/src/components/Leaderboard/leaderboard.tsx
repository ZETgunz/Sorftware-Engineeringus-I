import React, { useState } from 'react';
import './leaderboard.css'; // Make sure to create a corresponding CSS file for styling

interface board {
    username: string;
    score: number;
}

export const Leaderboard: React.FC = () => {

    const [string, setString] = useState([""]);
    var stringArr = [""];
    const [revealed, setRevealed] = useState(false);

    const fetchLeaderboard = async () => {
        if(revealed==false){
            try {
                const response = await fetch('http://localhost:5071/api/Leaderboard');
                if (!response.ok) {
                    throw new Error(`Error: ${response.statusText}`);
                }
                const data: board[] = await response.json();
                string.length = 0;
                for (var i = 0; i < data.length; i++) {
                    stringArr[i] = (`${data[i].username}: ${data[i].score}`);
                    //str += `${i.username}: ${i.score}`;
                }
                setRevealed(true);
                (document.getElementById("leaderboardButton") as HTMLInputElement).innerHTML="Hide";
                setString(stringArr);
                return;
            } catch (error) {
                console.error('Error fetching leaderboard:', error);
            }
        }
        if(revealed==true){
            setRevealed(false);
            (document.getElementById("leaderboardButton") as HTMLInputElement).innerHTML="Reveal";
            setString([]);
            return;
        }
    }

    return (
        <div className="leaderboard">
            <h1>Melon Leaderboards</h1>
            <button id="leaderboardButton" onClick={() => fetchLeaderboard()}>Reveal</button>
            {string.map((s, i) => (
                <div key={i}>
                    <label>{i+1}. {s}</label>
                </div>
            ))}

        </div >
    );

}