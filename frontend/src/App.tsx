import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Home } from './pages/Home';
import './App.css';
import { Nav } from './components/Nav/nav';

export const App = () => {
  return (

    <BrowserRouter>
      <Nav />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/leaderboard" element={<div>Leaderboard</div>} />
        <Route path="/account" element={<div>Account</div>} />
        <Route path="/sequance" element={<div>Sequance</div>} />
      </Routes>
    </BrowserRouter>
  );
}
