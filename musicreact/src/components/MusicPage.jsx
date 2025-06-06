import React, { useEffect, useState } from "react";
import musicApi from "../api/axiosMusic2";
import MusicList from "./MusicList";
import UploadMusicButton from "./UploadMusicButton";
import "./MusicPage.css";

const MusicPage = () => {
    const [musicList, setMusicList] = useState([]);

    const fetchMusics = async () => {
        try {
            const res = await musicApi.get("/music");
            setMusicList(res.data);
        } catch (error) {
            console.error("Musiqiləri yükləmək mümkün olmadı.", error);
        }
    };

    useEffect(() => {
        fetchMusics();
    }, []);

    const handleUploadSuccess = () => {
        fetchMusics(); // Uğurla yüklənəndən sonra siyahını yenilə
    };

    return (
        <div className="music-page">
            <div className="music-header">
                <h2>Musiqi Siyahısı</h2>
                <UploadMusicButton onSuccess={handleUploadSuccess} className="upload-wrapper" />
            </div>
            <div className="music-list-container">
                <MusicList musicList={musicList} />
            </div>
        </div>
    );
};

export default MusicPage;
