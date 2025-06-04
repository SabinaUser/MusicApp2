import React, { useEffect, useState } from "react";
import axios from "../api/axiosMusic2";
import "./MusicList.css"; // CSS faylını import etməyi unutma!

const MusicList = () => {
    const [musicList, setMusicList] = useState([]);

    useEffect(() => {
        const fetchMusics = async () => {
            try {
                const res = await axios.get("/music");
                setMusicList(res.data);
            } catch (error) {
                alert("Musiqiləri yükləmək mümkün olmadı.");
                console.error(error);
            }
        };

        fetchMusics();
    }, []);

    const handleDelete = async (id) => {
        const confirm = window.confirm("Bu musiqini silmək istədiyinizə əminsiniz?");
        if (!confirm) return;

        try {
            await axios.delete(`/music/delete-music/${id}`);
            setMusicList(prev => prev.filter(music => music.id !== id));
            alert("Musiqi silindi.");
        } catch (error) {
            alert("Silinmə zamanı xəta baş verdi.");
            console.error(error);
        }
    };

    const handleAddFavorite = async (musicId) => {
        try {
            await axios.post(`/music/${musicId}`);
            alert("Musiqi favoritlərə əlavə olundu.");
        } catch (error) {
            if (error.response && error.response.data) {
                alert(error.response.data.message || "Favoritə əlavə zamanı xəta baş verdi.");
            } else {
                alert("Favoritə əlavə zamanı xəta baş verdi.");
            }
            console.error(error);
        }
    };

    return (
        <div className="music-container">
            <h2 className="page-title">Bütün Musiqilər</h2>
            <div className="music-grid">
                {musicList.map((music) => (
                    <div key={music.id} className="music-card">
                        {music.posterImagePath && (
                            <img
                                src={`https://localhost:7037/${music.posterImagePath}`}
                                alt="poster"
                                className="music-poster"
                            />
                        )}
                        <div className="music-info">
                            <h3>{music.title}</h3>
                            <p>{music.artist}</p>
                        </div>
                        <audio controls className="music-player">
                            <source src={`https://localhost:7037/${music.filePath}`} type="audio/mpeg" />
                            Your browser does not support the audio element.
                        </audio>
                        <div className="music-actions">
                            <button onClick={() => handleDelete(music.id)} className="delete-btn">Sil</button>
                            <button onClick={() => handleAddFavorite(music.id)} className="fav-btn">❤️ Favorit</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default MusicList;
