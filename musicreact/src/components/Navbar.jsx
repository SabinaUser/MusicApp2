import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "./Navbar.css"; // ⬅️ CSS faylını daxil edin

const Navbar = ({ onLogout }) => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem("token");
        onLogout();
        navigate("/");
    };

    return (
        <nav className="navbar">
            <div className="nav-links">
                <Link to="/home">Home</Link>
                <Link to="/favorites">Favorites</Link>
            </div>
            <button className="logout-btn" onClick={handleLogout}>
                Logout
            </button>
        </nav>
    );
};

export default Navbar;
