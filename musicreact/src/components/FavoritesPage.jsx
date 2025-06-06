﻿import React, { useEffect, useState } from "react";
import musicApi from "../api/axiosMusic2";
import "./FavoritesPage.css";


const FavoritesPage = () => {
    const [favorites, setFavorites] = useState([]);

    useEffect(() => {
        const fetchFavorites = async () => {
            try {
                const res = await musicApi.get("/music/my-favorites");
                setFavorites(res.data);
            } catch (error) {
                console.error("Favorit musiqilər yüklənmədi:", error);
            }
        };

        fetchFavorites();
    }, []);
    const handleRemove = async (musicId) => {
        try {
            await musicApi.delete(`/music/${musicId}`);
            // Silindikdən sonra siyahını yenilə
            setFavorites((prev) => prev.filter((m) => m.musicId !== musicId));
        } catch (error) {
            console.error("Favoritdən silmək mümkün olmadı:", error);
            alert("Favoritdən silmək mümkün olmadı.");
        }
    };
    return (
        <div className="favorites-container">
            <h2>Favorit Musiqilər</h2>
            {favorites.length === 0 ? (
                <p className="empty-message">Favorit musiqiniz yoxdur.</p>
            ) : (
                <ul className="favorites-list">
                    {favorites.map((music) => (
                        <li key={music.musicId}>
                            <span>
                                <strong>{music.title}</strong> - {music.artist}
                            </span>
                            <button
                                className="remove-btn"
                                onClick={() => handleRemove(music.musicId)}
                            >
                                Remove
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );

    
};

export default FavoritesPage;
