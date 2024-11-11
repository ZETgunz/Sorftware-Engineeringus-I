import React, { useState, useEffect } from 'react';
import './textbox.css'; // Make sure to create a corresponding CSS file for styling

export const Textbox: React.FC = () => {
    const [text, setText] = useState("");
    const [typing, setTyping] = useState("");

    return (
        <div className="textbox">
            <h1>Melon Typing</h1>
            

            <button onClick={() => playGame()} className={isPlaying ? 'disabled' : 'active'}>Start</button>
        </div >
    );
}