import React from 'react';
import { Link } from 'react-router-dom';
import './Nav.css';

export const Nav: React.FC = () => {
    return (
        <nav className="navbar">
            <ul className="nav-left">
                <li className="nav-item">
                    <Link to="/" className="nav-link">
                        HOME
                    </Link>
                </li>
            </ul>
            <ul className="nav-right">
                <li className="nav-item">
                    <Link to="/leaderboard" className="nav-link">
                        LEADERBOARD
                    </Link>
                </li>
                <li className="nav-item">
                    <Link to="/account/login" className="nav-link">
                        LOGIN
                    </Link>
                </li>
            </ul>
        </nav>
    );
};