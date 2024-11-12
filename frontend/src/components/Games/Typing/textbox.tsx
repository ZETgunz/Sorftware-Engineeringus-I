import React, { useState, useEffect } from 'react';
import './textbox.css'; // Make sure to create a corresponding CSS file for styling

export const Textbox: React.FC = () => {
    const [text, setText] = useState("");
    const [typing, setTyping] = useState("");
    const [isPlaying, setIsPlaying] = useState(false);

    const playGame = async () => {
        setIsPlaying(true);
        await fetchCell();
    };

    const fetchCell = async () => {
        try {
            const response = await fetch('http://localhost:5071/api/Typing/');
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }
            const data: string = await response.json();
            setText(data);
            console.log(data);
            return data;
        } catch (error) {
            console.error('Error fetching text:', error);
        }
    };

    const finish = () => {
        setIsPlaying(false);
        setTyping("WIP");//work in progress
        const arrText = text.split('');
        const arrTyping = typing.split('');
        var score = 0;
        const scoreMax = 1000;
        const scoreRate = text.length/scoreMax;
        for(var i=0; i<arrText.length; i++){
            if(arrTyping[i]==arrText[i]){
                score+=scoreRate;
            }
        }
    }

    return (
        <div className="textbox">
            <h1>Melon Typing</h1>
            <label>{text}</label>            
            <input></input>
            <button onClick={() => finish()} className={isPlaying ? 'active' : 'disabled'}>Finish</button>
            <button onClick={() => playGame()} className={isPlaying ? 'disabled' : 'active'}>Start</button>
        </div >
    );
}