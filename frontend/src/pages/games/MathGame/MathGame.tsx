import React, { useState, useEffect, useCallback } from 'react';
import { getSession } from '../../../Utils/Session';

interface Equation {
  formula: string;
  solution: number;
}

interface ScoreConfig {
  timeLimit: number;
  points: number;
  wrongPenalty: number;
}

const SCORING: Record<string, ScoreConfig> = {
  easy: { timeLimit: 30, points: 25, wrongPenalty: 5 },
  medium: { timeLimit: 30, points: 100, wrongPenalty: 10 },
  hard: { timeLimit: 60, points: 200, wrongPenalty: 15 }
};

const MathChallenge: React.FC = () => {
  const [gameState, setGameState] = useState<'menu' | 'playing' | 'gameOver'>('menu');
  const [difficulty, setDifficulty] = useState<string>('easy');
  const [currentProblem, setCurrentProblem] = useState<Equation | null>(null);
  const [userAnswer, setUserAnswer] = useState<string>('');
  const [score, setScore] = useState<number>(0);
  const [timeRemaining, setTimeRemaining] = useState<number>(0);
  const [consecutiveCorrect, setConsecutiveCorrect] = useState<number>(0);
  const [username, setUsername] = useState<string | null>(null);

  useEffect(() => {
    const loadSession = async () => {
      const session = await getSession();
      if (session) {
        setUsername(session.username);
      }
    };
    loadSession();
  }, []);

  useEffect(() => {
    let timerId: NodeJS.Timeout;

    if (gameState === 'playing' && timeRemaining > 0) {
      timerId = setTimeout(() => {
        setTimeRemaining(prev => prev - 1);
      }, 1000);
    } else if (timeRemaining <= 0 && gameState === 'playing') {
      endGame();
    }

    return () => clearTimeout(timerId);
  }, [gameState, timeRemaining]);

  const fetchEquation = useCallback(async (selectedDifficulty: string) => {
    try {
      const response = await fetch(
        `http://localhost:5071/api/MathGame/formula?difficulty=${selectedDifficulty}`
      );
      
      if (!response.ok) {
        throw new Error('Failed to fetch equation');
      }

      const data: Equation = await response.json();
      setCurrentProblem(data);
      setUserAnswer('');
    } catch (error) {
      console.error('Error fetching equation:', error);
      generateLocalProblem(selectedDifficulty);
    }
  }, []);

  const generateLocalProblem = (selectedDifficulty: string) => {
    const operators = ['+', '-', '*'];
    let num1 = 0, num2 = 0, operator = '', solution = 0;

    switch (selectedDifficulty) {
      case 'easy':
        num1 = Math.floor(Math.random() * 10);
        num2 = Math.floor(Math.random() * 10);
        operator = operators[Math.floor(Math.random() * 2)];
        break;
      case 'medium':
        num1 = Math.floor(Math.random() * 20);
        num2 = Math.floor(Math.random() * 20);
        operator = operators[Math.floor(Math.random() * 3)];
        break;
      case 'hard':
        num1 = Math.floor(Math.random() * 50);
        num2 = Math.floor(Math.random() * 50);
        operator = operators[Math.floor(Math.random() * 3)];
        break;
    }

    switch (operator) {
      case '+':
        solution = num1 + num2;
        break;
      case '-':
        solution = num1 - num2;
        break;
      case '*':
        solution = num1 * num2;
        break;
    }

    setCurrentProblem({
      formula: `${num1} ${operator} ${num2}`,
      solution
    });
  };

  const startGame = (selectedDifficulty: string) => {
    setDifficulty(selectedDifficulty);
    setScore(0);
    setConsecutiveCorrect(0);
    setGameState('playing');
    
    const config = SCORING[selectedDifficulty];
    setTimeRemaining(config.timeLimit);
    
    fetchEquation(selectedDifficulty);
  };

  const updateHighScore = async () => {
    if (!username) return;

    try {
      const response = await fetch(`http://localhost:5071/api/Leaderboard/${username}`);
      
      if (!response.ok) {
        throw new Error(`Error fetching current score: ${response.statusText}`);
      }
      
      const userData = await response.json();
      const currentTotalScore = userData.score;
      const updatedTotalScore = currentTotalScore + score;

      const updateResponse = await fetch(`http://localhost:5071/api/Account/${username}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ score: updatedTotalScore }),
      });

      if (!updateResponse.ok) {
        throw new Error(`Error updating score: ${updateResponse.statusText}`);
      }

      console.log(`High score updated successfully: ${updatedTotalScore}`);
    } catch (error) {
      console.error('Error updating high score:', error);
    }
  };

  const checkAnswer = () => {
    if (!currentProblem) return;

    const config = SCORING[difficulty];
    const parsedAnswer = parseInt(userAnswer);

    if (parsedAnswer === currentProblem.solution) {
      setScore(prev => prev + config.points);
      setConsecutiveCorrect(prev => prev + 1);
      

      fetchEquation(difficulty);
    } else {

      setScore(prev => Math.max(0, prev - config.wrongPenalty));
      fetchEquation(difficulty);
    }
  };
  const endGame = () => {
    updateHighScore();
    setGameState('gameOver');
  };

  const resetGame = () => {
    setGameState('menu');
    setScore(0);
    setConsecutiveCorrect(0);
  };

  const renderMenu = () => (
    <div className="text-center p-8">
      <h1 className="text-3xl mb-6">Math Challenge</h1>
      <div className="space-y-4">
        {['easy', 'medium', 'hard'].map(level => (
          <button 
            key={level}
            onClick={() => startGame(level)} 
            className={`
              text-white px-4 py-2 rounded capitalize
              ${level === 'easy' ? 'bg-green-500' : 
                level === 'medium' ? 'bg-yellow-500' : 'bg-red-500'}
            `}
          >
            {level} ({SCORING[level].timeLimit}s, {SCORING[level].points}pts)
          </button>
        ))}
      </div>
    </div>
  );

  // Render game play
  const renderGame = () => (
    <div className="text-center p-8">
      <div className="mb-4">
        <h2 className="text-2xl">Time: {timeRemaining}s</h2>
        <h2 className="text-xl">Score: {score}</h2>
      </div>
      {currentProblem && (
        <div>
          <h3 className="text-3xl mb-4">{currentProblem.formula} = ?</h3>
          <input 
            type="number" 
            value={userAnswer}
            onChange={(e) => setUserAnswer(e.target.value)}
            onKeyDown={(e) => e.key === 'Enter' && checkAnswer()}
            className="border p-2 mb-4"
            placeholder="Your answer"
          />
          <br />
          <button 
            onClick={checkAnswer}
            className="bg-blue-500 text-white px-4 py-2 rounded"
          >
            Submit
          </button>
        </div>
      )}
    </div>
  );

  // Render game over screen
  const renderGameOver = () => (
    <div className="text-center p-8">
      <h1 className="text-3xl mb-4">Game Over!</h1>
      <p className="text-xl mb-4">Your Score: {score}</p>
      <button 
        onClick={resetGame}
        className="bg-green-500 text-white px-4 py-2 rounded"
      >
        Play Again
      </button>
    </div>
  );

  // Main render
  return (
    <div className="max-w-md mx-auto mt-10 bg-gray-100 rounded-lg shadow-md">
      {gameState === 'menu' && renderMenu()}
      {gameState === 'playing' && renderGame()}
      {gameState === 'gameOver' && renderGameOver()}
    </div>
  );
};

export default MathChallenge;