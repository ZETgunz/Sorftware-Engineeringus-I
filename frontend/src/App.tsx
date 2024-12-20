import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Home } from './pages/Home';
import './App.css';
import { Nav } from './components/Nav/nav';
import { LeaderboardPage } from './pages/Leaderboard/leaderboard';
import { AccountLogin } from './pages/Account/Login/login';
import { AccountRegister } from './pages/Account/Register/register';
import { Typing } from './pages/games/Typing/Typing';
import { Sequence } from './pages/games/Sequence/Sequence';
import { Seeker } from './pages/games/Seeker/Seeker';
import AimTrainer from './pages/games/AimTrainer/AimTrainer';
import MathGame from './pages/games/MathGame/MathGame';

export const App = () => {
  return (

    <BrowserRouter>
      <Nav />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/leaderboard" element={<LeaderboardPage />} />
        <Route path="/account/login" element={<AccountLogin />} />
        <Route path="/account/register" element={<AccountRegister />} />
        <Route path="/typing" element={<Typing />} />
        <Route path="/sequence" element={<Sequence />} />
        <Route path="/aimTrainer" element={<AimTrainer />} />
        <Route path="/seeker" element={<Seeker />} />
        <Route path="/mathGame" element={<MathGame />}/>
        <Route path="*" element={<h1>Not Found</h1>} />
      </Routes>
    </BrowserRouter>
  );
}
