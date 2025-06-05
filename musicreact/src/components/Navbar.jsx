import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "../api/axios";
import "./Navbar.css";

const Navbar = ({ onLogout }) => {
    const navigate = useNavigate();
    const [profile, setProfile] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const res = await axios.get("/account/profile");
                setProfile(res.data);
            } catch (error) {
                console.error("Profil yüklənə bilmədi", error);
            }
        };
        fetchProfile();
    }, []);

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

            <div className="nav-right">
            
                {profile && (
                    <div className="profile-section">
                        <Link to="/profile" className="profile-link">
                            <img
                                src={`https://localhost:7147${profile.profileImageUrl}`}
                                alt="Profile"
                                className="profile-image"
                            />
                        </Link>
                    </div>
                )}
                <button className="logout-btn" onClick={handleLogout}>
                    Logout
                </button>
            </div>
        </nav>
    );
};

export default Navbar;
