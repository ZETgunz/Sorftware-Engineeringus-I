import React, { useState, useEffect, useRef } from "react";
import { getSession } from "../../../Utils/Session";
import "./AimTrainer.css";
import melonImage from "./melon.png"; // Import the image

interface Target {
  x: number;
  y: number;
}

const AimTrainer: React.FC = () => {
  const [target, setTarget] = useState<Target | null>(null);
  const [nextTarget, setNextTarget] = useState<Target | null>(null); 
  const [score, setScore] = useState(0);
  const [timeLeft, setTimeLeft] = useState(30); 
  const [isGameActive, setIsGameActive] = useState(false);
  const gameAreaRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (isGameActive && timeLeft > 0) {
      timer = setTimeout(() => setTimeLeft((prev) => prev - 1), 1000);
    } else if (timeLeft === 0 && isGameActive) {
      
      alert(`Game over! Your score is ${score}`);
      setIsGameActive(false);
      getSession().then(async (session) => {
                  if (session) {
                      const response = await fetch('http://localhost:5071/api/Leaderboard/'+ session.username);
                      if (!response.ok) {
                          throw new Error(`Error: ${response.statusText}`);
                      }
                      const data = await response.json();
                      setScore(score + data.score);
      
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
    return () => clearTimeout(timer);
  }, [timeLeft, isGameActive]);

  const startGame = async () => {
    setScore(0);
    setTimeLeft(30);
    setIsGameActive(true);

    const initialTarget = await fetchTarget();
    setTarget(initialTarget);

    preloadNextTarget(); 
  };

  const fetchTarget = async (): Promise<Target> => {
    if (gameAreaRef.current) {
      const gameArea = gameAreaRef.current.getBoundingClientRect();

      try {
        console.log(gameArea.width, gameArea.height);

        const response = await fetch("http://localhost:5071/api/aimtrainer/target", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            x: Math.round(gameArea.width),
            y: Math.round(gameArea.height)
          }),
        });

        if (response.ok) {
          return await response.json();
        } else {
          console.error("Failed to fetch target");
        }
      } catch (error) {
        console.error("Error fetching target:", error);
      }
    }

    return { x: 0, y: 0 }; 
  };

  const preloadNextTarget = async () => {
    const next = await fetchTarget();
    setNextTarget(next);
  };

  const handleTargetClick = async () => {
    if (isGameActive && nextTarget) {
      setScore((prev) => prev + 25);
      setTarget(nextTarget); 
      preloadNextTarget(); 
    }
  };

  return (
    <div className="aim-trainer">
      <h1>Aim Trainer</h1>
      {!isGameActive ? (
        <button onClick={startGame}>Start Game</button>
      ) : (
        <>
          <div className="stats">
            <p>Score: {score}</p>
            <p>Time Left: {timeLeft}s</p>
          </div>
          <div
            className="game-area"
            ref={gameAreaRef}
            style={{
              position: "relative",
              width: "60vw",
              height: "60vh",
              border: "2px solid black",
              margin: "0 auto",
            }}
          >
            {target && (
              <div
                className="target"
                onClick={handleTargetClick}
                style={{
                  position: "absolute",
                  width: 50,
                  height: 50,
                  borderRadius: "50%",
                  backgroundImage: `url(${melonImage})`, // Use the imported image
                  backgroundSize: "cover",
                  top: target.y,
                  left: target.x,
                  cursor: "pointer",
                }}
              ></div>
            )}
          </div>
        </>
      )}
    </div>
  );
};

export default AimTrainer;
