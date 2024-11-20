import React, { useState, useEffect } from 'react';
import './Grid.css'; // Make sure to create a corresponding CSS file for styling

interface cell {
    row: number;
    column: number;
}

export const Grid: React.FC = () => {
    const initialGrid = Array(3).fill(Array(3).fill(0));
    const [grid, setGrid] = useState(initialGrid);
    const [activeCells, setActiveCells] = useState<cell[]>([]);
    const [isPlaying, setIsPlaying] = useState(false);
    const [cellsToActivate, setCellsToActivate] = useState<cell[]>([]);
    const [isShowing, setIsShowing] = useState(false);
    const [level, setLevel] = useState(0);
    const [click, setClick] = useState(0);
    const [wrongClick, setWrongClick] = useState<cell>();

    const handleClick = (rowIndex: number, colIndex: number) => {
        if (!isPlaying || isShowing) return;
        if (rowIndex === cellsToActivate[click].row && colIndex === cellsToActivate[click].column) {
            setClick(click + 1);
            setActiveCells([{ row: rowIndex, column: colIndex }]);
            setTimeout(() => setActiveCells([]), 300);
            if (click === cellsToActivate.length - 1) {
                setClick(0);
                setIsShowing(true);
                setTimeout(() => playGame(), 500);
            }
        }
        else {
            setWrongClick({ row: rowIndex, column: colIndex });
            setClick(0);
            gameOver();
        }

    };

    const gameOver = () => {
        cellsToActivate.length = 0;
        setIsPlaying(false);
        var storedScore = localStorage.getItem('score'); 
        var score = (level-1)*150;
        alert("You score is "+score+"!");
      
        if (storedScore === null || score > parseInt(storedScore || '0')) {
            localStorage.setItem('score', score.toString());
            return;
        }
        if(localStorage.getItem('username') != null){
            const username = localStorage.getItem('username');
            const password = localStorage.getItem('password');
            const scoreUpdate = parseInt(localStorage.getItem('score') || '0');
            const data = {password, score: scoreUpdate};
            fetch('http://localhost:5071/api/Account/' + username, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
        setLevel(0);
        return;
    };
    };

    const fetchCell = async () => {
        try {
            const response = await fetch('http://localhost:5071/api/Sequence/' + (level + 1));
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }
            const data: cell[] = await response.json();
            setCellsToActivate(data);
            console.log(data);
            playCellAnimation(data);
            return data;
        } catch (error) {
            console.error('Error fetching cell:', error);
        }
    };

    const playGame = async () => {
        setLevel(level+1);
        setIsPlaying(true);
        setIsShowing(true);
        setWrongClick(undefined);
        await fetchCell();
    };

    const playCellAnimation = (data: cell[]) => {
        setTimeout(() => {
            data.forEach((cell, index) => {
                setTimeout(() => {
                    setActiveCells([cell]);
                    setTimeout(() => setActiveCells([]), 300);
                }, index * 600);
            });
        }, 500);
        setTimeout(() => setIsShowing(false), cellsToActivate.length * 500 + 500);
    };

    return (
        <div className="grid">
            <h1>Melon Sequence</h1>
            <h2>Level: {level}</h2>
            {grid.map((row, rowIndex) => (
                <div key={rowIndex} className="row">
                    {row.map((cell: number, colIndex: number) => (
                        <div
                            key={colIndex}
                            className={`cell ${activeCells.some(activeCell => activeCell.row === rowIndex && activeCell.column === colIndex) ? 'active' : ''} ${wrongClick?.row === rowIndex && wrongClick?.column === colIndex ? 'wrong' : ''}`}

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
