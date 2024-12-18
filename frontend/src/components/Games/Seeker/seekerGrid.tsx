import React, { useState, useEffect } from 'react';
import { getSession } from '../../../Utils/Session';

export const SeekerGrid = () => {
  const [grid, setGrid] = useState<number[][]>([]);
  const [targetTile, setTargetTile] = useState<{row: number, col: number} | null>(null);
  const [gameStarted, setGameStarted] = useState(false);
  const [timeRemaining, setTimeRemaining] = useState(30);
  const [score, setScore] = useState(0);
  const [gameOver, setGameOver] = useState(false);

  useEffect(() => {
    resetGrid();
  }, []);

  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (gameStarted && timeRemaining > 0) {
      timer = setInterval(() => {
        setTimeRemaining(prev => {
          if (prev <= 1) {
            clearInterval(timer);
            setGameStarted(false);
            setGameOver(true);
            handleGameEnd();
            return 0;
          }
          return prev - 1;
        });
      }, 1000);
    }
    return () => clearInterval(timer);
  }, [gameStarted, score]);

  const resetGrid = () => {
    const newGrid = Array.from({ length: 10 }, () => 
      Array.from({ length: 10 }, () => 0)
    );
    
    const targetRow = Math.floor(Math.random() * 10);
    const targetCol = Math.floor(Math.random() * 10);
    newGrid[targetRow][targetCol] = 1;

    setGrid(newGrid);
    setTargetTile({ row: targetRow, col: targetCol });
  };

  const handleGameEnd = () => {
    getSession().then(async (session) => {
      if (session) {
        try {
          const response = await fetch('http://localhost:5071/api/Leaderboard/' + session.username);
          let currentScore = 0;
          if (response.ok) {
            const data = await response.json();
            currentScore = data.score || 0;
          }

          const updatedScore = currentScore + score;

          const updateResponse = await fetch('http://localhost:5071/api/Account/' + session.username, {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify({
              score: updatedScore
            }),
          });

          if (!updateResponse.ok) {
            throw new Error(`Error updating score: ${updateResponse.statusText}`);
          }

          console.log('Score updated successfully:', updatedScore);
        } catch (error) {
          console.error('Error uploading score:', error);
        }
      }
    });
  };

  const startGame = () => {
    setGameStarted(true);
    setGameOver(false);
    setScore(0);
    setTimeRemaining(30);
    resetGrid();
  };

  const handleTileClick = (row: number, col: number) => {
    if (!gameStarted) return;

    if (targetTile && row === targetTile.row && col === targetTile.col) {
      setScore(prev => prev + 25);
      resetGrid();
    }
  };

  const renderGrid = () => {
    return grid.map((row, rowIndex) => (
      <div key={rowIndex} style={{ display: 'flex' }}>
        {row.map((tile, colIndex) => {
          const backgroundImage = tile === 1 
            ? 'url("melon_rotten.png")' // Slightly darker tile
            : 'url("melon_rotten.good")'; // Bright white for other tiles
          
          return (
            <div 
              key={colIndex} 
              onClick={() => handleTileClick(rowIndex, colIndex)}
              style={{
                width: '40px',
                height: '40px',
                backgroundImage: backgroundImage,
                border: '1px solid #e0e0e0',
                cursor: 'pointer'
              }}
            />
          );
        })}
      </div>
    ));
  };

  return (
    <div style={{
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      padding: '20px',
      fontFamily: 'Arial, sans-serif'
    }}>
      <h1>Extreme Tile Seeker</h1>
      
      {!gameStarted && !gameOver && (
        <button 
          onClick={startGame}
          style={{
            padding: '10px 20px',
            fontSize: '16px',
            margin: '20px 0'
          }}
        >
          Start Game
        </button>
      )}

      {gameStarted && (
        <div style={{ 
          display: 'flex', 
          gap: '20px', 
          margin: '10px 0' 
        }}>
          <span>Time: {timeRemaining} sec</span>
          <span>Score: {score}</span>
        </div>
      )}

      {gameOver && (
        <div style={{ textAlign: 'center' }}>
          <h2>Game Over!</h2>
          <p>Your Score: {score}</p>
          <button 
            onClick={startGame}
            style={{
              padding: '10px 20px',
              fontSize: '16px',
              margin: '20px 0'
            }}
          >
            Play Again
          </button>
        </div>
      )}

      <div style={{ 
        display: 'flex', 
        flexDirection: 'column',
        border: '2px solid #333' 
      }}>
        {renderGrid()}
      </div>
    </div>
  );
};

export default SeekerGrid;