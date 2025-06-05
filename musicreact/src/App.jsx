import React, { useEffect, useState } from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import AuthPage from "./components/AuthPage";
import MusicPage from "./components/MusicPage";
import FavoritesPage from "./components/FavoritesPage";
import Navbar from "./components/Navbar";
import './App.css';
import Profile from "./components/Profile";

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            setIsAuthenticated(true);
        }
    }, []);

    if (!isAuthenticated) {
        return <AuthPage onLogin={() => setIsAuthenticated(true)} />;
    }

    return (
        <div className="App">
            <Router>
                <Navbar onLogout={() => {
                    localStorage.removeItem("token");
                    setIsAuthenticated(false);
                }} />
                <Routes>
                    <Route path="/" element={<Navigate to="/home" />} />
                    <Route path="/home" element={<MusicPage />} />
                    <Route path="/favorites" element={<FavoritesPage />} />
                    <Route path="/profile" element={<Profile/>} />
                </Routes>
            </Router>
        </div>
    );
}

export default App;
