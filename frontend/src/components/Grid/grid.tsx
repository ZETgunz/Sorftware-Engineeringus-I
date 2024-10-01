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
    const [level, setLevel] = useState(1);
    const [click, setClick] = useState(0);
    const [wrongClick, setWrongClick] = useState<cell>();

    const handleClick = (rowIndex: number, colIndex: number) => {
        if (!isPlaying || isShowing) return;
        if (rowIndex === cellsToActivate[click].row && colIndex === cellsToActivate[click].column) {
            setClick(click + 1);
            setActiveCells([{ row: rowIndex, column: colIndex }]);
            setTimeout(() => setActiveCells([]), 300);
            if (click === cellsToActivate.length - 1) {
                setLevel(level + 1);
                setClick(0);
                playGame();
            }
        }
        else {
            setWrongClick({ row: rowIndex, column: colIndex });
            setClick(0);
            gameOver();
            cellsToActivate.length = 0;
            setIsPlaying(false);
        }

    };

    const gameOver = () => {
        return; // Implement game over logic here
    };

    const fetchCell = async () => {
        try {
            const response = await fetch('http://localhost:5071/api/Sequance');
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }
            const data: cell = await response.json();
            console.log(data);
            cellsToActivate.push(data);


        } catch (error) {
            console.error('Error fetching cell:', error);
        }
    };



    const playGame = async () => {
        setIsPlaying(true);
        setIsShowing(true);
        setWrongClick(undefined);



        await fetchCell();
        console.log(cellsToActivate);
        setTimeout(() => {
            cellsToActivate.forEach((cell, index) => {
                setTimeout(() => {
                    setActiveCells([cell]);
                    setTimeout(() => setActiveCells([]), 300);
                }, index * 1000);
            });
        }, 1000);


        setTimeout(() => setIsShowing(false), cellsToActivate.length * 1000 + 1000);

    };

    return (
        <div className="grid">
            <h1>Sequance</h1>
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
};