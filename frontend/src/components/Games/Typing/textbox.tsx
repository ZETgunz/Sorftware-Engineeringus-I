import React, { useState, useEffect } from 'react';
import './textbox.css'; // Make sure to create a corresponding CSS file for styling

export const Textbox: React.FC = () => {
    const [text, setText] = useState("Click Start to start (unexpected)!");
    //const [typing, setTyping] = useState("");
    const [isPlaying, setIsPlaying] = useState(false);
    const [timeElapsed, setTimeElapsed] = useState(0);

    const playGame = async () => {
        setIsPlaying(true);
        setTimeout(() => {timer();}, 500);
        await fetchText();
    };

    const fetchText = async () => {
        try {
            const response = await fetch('http://localhost:5071/api/Typing/');
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }
            const data: string = await response.text();
            setText(data);
            console.log(text);
            //console.log(response.text());
            return data;
        } catch (error) {
            console.error('Error fetching text:', error);
        }
    };

    const timer = () => {
        console.log("time passed");
        if (isPlaying) {
            setTimeElapsed(timeElapsed + 0.5);
            setTimeout(() => {timer();}, 500)
        }
        else return;
    }

    const finish = () => {
        setIsPlaying(false);
        //setTyping((document.getElementById("typing") as HTMLInputElement).value);
        const arrText = text.split('');
        const arrTyping = (document.getElementById("typing") as HTMLInputElement).value.split('');
        var score = 0;
        const scoreMax = 1000;
        const scoreRate = scoreMax / text.length;
        for (var i = 0; i < arrText.length; i++) {
            if (arrText[i] == arrTyping[i]) {
                score += scoreRate;
            }
        }
        if(timeElapsed>=5) score -= score - score/(1+((timeElapsed-5)/10)) ;
        score = Math.round(score);
        setText("Click Start to start (unexpected)!");
        (document.getElementById("typing") as HTMLInputElement).value = "";
        alert("Your score is " + score + "!");
    }

    return (
        <div className="textbox">
            <h1>Melon Typing</h1>
            <label className="text">{text}</label>
            <br />
            <textarea type="text" className="typing" id="typing"></textarea>
            <br />
            <button onClick={() => finish()} className={isPlaying ? 'active' : 'disabled'}>Finish</button>
            <button onClick={() => playGame()} className={isPlaying ? 'disabled' : 'active'}>Start</button>
        </div >
    );
}