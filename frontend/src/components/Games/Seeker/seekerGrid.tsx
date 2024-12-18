import React, { useState, useEffect } from 'react';
import { getSession } from '../../../Utils/Session';
import './seekerGrid.css'; // Make sure to create a corresponding CSS file for styling

interface cell {
    row: number;
    column: number;
}

export const SeekerGrid: React.FC = () => {
    const initialGrid = Array(10).fill(Array(10).fill(0));
    const [grid, setGrid] = useState(initialGrid);
    const [isPlaying, setIsPlaying] = useState(false);
    const [rotten, setRotten] = useState([0,0]);
    const [timeElapsed, setTimeElapsed] = useState(0);
    const [timeTotal, setTimeTotal] = useState(0);
    const [level, setLevel] = useState(0);

    useEffect(()=> {
        let timer: NodeJS.Interval;
        if (isPlaying) {
            if(level == 10){
                finish();
            }
            if(timeElapsed==(timeElapsed%100) && timeElapsed!=0) {
                setTimeTotal(timeTotal+timeElapsed);
                setTimeElapsed(0);
                playGame();
            }
            timer = setInterval(() => {setTimeElapsed((timeElapsed) => timeElapsed + 1); console.log("Time passed!");}, 100);
        }
        return () => clearInterval(timer);
    }, [isPlaying])

    const playGame = async () => {
        setIsPlaying(true);
        setLevel(level+1);
        await fetchCell();
    };

    const fetchCell = async () => {
        try {
            const response = await fetch('http://localhost:5071/api/Seeker/');
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }
            const data = await response.json();
            setRotten(data);
            console.log(data);
            //console.log(response.text());
            return data;
        } catch (error) {
            console.error('Error fetching text:', error);
        }
    };

    const finish = () => {
        setIsPlaying(false);
        var score = 1000;
        score -= timeTotal*10;
        alert("You score is "+score+"!");
        getSession().then(async (session) => {
            if (session) {
                const response = await fetch('http://localhost:5071/api/Leaderboard/'+ session.username);
                if (!response.ok) {
                    throw new Error(`Error: ${response.statusText}`);
                }
                const data = await response.json();
                score += data.score;

                await fetch('http://localhost:5071/api/Account/' + session.username, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        score: score
                    }),
                });
            }
        });
    }

    const handleClick = (rowIndex: number, colIndex: number) => {
        if (!isPlaying) return;
        //jei ne rotten - setTimeTotal(timeTotal+10); setTimeElapsed(0);
        //jei rotten - setTimeTotal(timeTotal+timeElapsed); setTimeElapsed(0);
        playGame();
    };

    return (
        <div className="seekergrid">
            <h1>Melon Seeker</h1>
            <h2>Level: {level}</h2>
            {grid.map((row, rowIndex) => (
                <div key={rowIndex} className="row">
                    {row.map((cell: number, colIndex: number) => (
                        <div
                            key={colIndex}
                            className={`${(rotten => rotten[0] === rowIndex && rotten[1] === colIndex) ? 'rotten' : ''} ${(rotten => rotten[0] != rowIndex && rotten[1] != colIndex) ? 'good' : ''}`}

                            onClick={() => handleClick(rowIndex, colIndex)}
                        >
                        </div>
                    ))}
                </div>
            ))}
            <button onClick={() => playGame()} className={isPlaying ? 'disabled' : 'active'}>Start</button>
        </div >
    );
}