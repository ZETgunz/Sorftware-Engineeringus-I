import { useNavigate } from 'react-router-dom';
import './tile.css';

interface TileProps {
    title: string;
    description: string;
    route: string;
}

export const Tile = ({ title, description, route }: TileProps) => {
    const navigate = useNavigate();

    return (
        <div className="tile" onClick={() => navigate(route)}>
            <div className="tile-content">
                <h3>{title}</h3>
                <p>{description}</p>
            </div>
        </div>
    );
};