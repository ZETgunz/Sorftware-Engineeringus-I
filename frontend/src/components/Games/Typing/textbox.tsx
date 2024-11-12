import React, { useState, useEffect } from 'react';
import './textbox.css'; // Make sure to create a corresponding CSS file for styling

export const Textbox: React.FC = () => {
    const [text, setText] = useState("");
    //const [typing, setTyping] = useState("");
    const [isPlaying, setIsPlaying] = useState(false);

    const playGame = async () => {
        setIsPlaying(true);
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
            //console.log(response.text());
            return data;
        } catch (error) {
            console.error('Error fetching text:', error);
        }
    };

    const finish = () => {
        setIsPlaying(false);
        //setTyping((document.getElementById("typing") as HTMLInputElement).value);
        const arrText = text.split('');
        const arrTyping = (document.getElementById("typing") as HTMLInputElement).value.split('');
        var score = 0;
        const scoreMax = 1000;
        const scoreRate = scoreMax/text.length;
        for(var i=0; i<arrText.length; i++){
            if(arrText[i]==arrTyping[i]){
                score+=scoreRate;
            }
        }
        setText("");
        (document.getElementById("typing") as HTMLInputElement).value="";
        alert("Your score is "+score+"!");
    }

    return (
        <div className="textbox">
            <h1>Melon Typing</h1>
            <label>{text}</label>
            <br />        
            <input type="text" id="typing"></input>
            <br />
            <button onClick={() => finish()} className={isPlaying ? 'active' : 'disabled'}>Finish</button>
            <button onClick={() => playGame()} className={isPlaying ? 'disabled' : 'active'}>Start</button>
        </div >
    );
}