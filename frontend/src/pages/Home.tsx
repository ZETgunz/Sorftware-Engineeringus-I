import { useState, useEffect } from 'react';
import { Tile } from '../components/Tile/tile';
import './Home.css';

interface Game {
    id: number;
    name: string;
    description: string;
    route: string;
}

export const Home = () => {
    const [games, setGames] = useState<Game[]>([]);

    useEffect(() => {
        const fetchGames = async () => {
            try {
                const response = await fetch('http://localhost:5071/api/games');
                if (!response.ok) {
                    throw new Error(`Error: ${response.statusText}`);
                }
                const data: Game[] = await response.json();
                setGames(data);
            } catch (error) {
                console.error('Error fetching games:', error);
            }
        };

        fetchGames();
    }, []);

    return (
        <div className="tiles-container">
            {games.map((game) => (
                <Tile
                    key={game.id}
                    title={game.name}
                    description={game.description}
                    route={game.route}
                />
            ))}
        </div>
    );
};