import React, { useState } from "react";
import musicAxios from "../api/axiosMusic2";


const UploadMusicButton = ({ onSuccess }) => {
    const [showForm, setShowForm] = useState(false);
    const [title, setTitle] = useState("");
    const [artist, setArtist] = useState("");
    const [file, setFile] = useState(null);
    const [poster, setPoster] = useState(null);

    const toggleForm = () => setShowForm(!showForm);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!file) {
            alert("Musiqi faylı seçilməlidir.");
            return;
        }

        const formData = new FormData();
        formData.append("title", title);
        formData.append("artist", artist);
        formData.append("file", file);
        if (poster) formData.append("poster", poster);

        try {
            const res = await musicAxios.post("/music", formData);
            alert(res.data.message || "Musiqi uğurla yükləndi!");
            setTitle("");
            setArtist("");
            setFile(null);
            setPoster(null);
            setShowForm(false); // Formu bağla

            if (onSuccess) onSuccess();
        } catch (err) {
            console.error(err);
            alert("Xəta baş verdi.");
        }
    };

    return (
        <div>
            <button onClick={toggleForm}>
                {showForm ? "Formu Gizlət" : "Musiqi Yüklə"}
            </button>

            {showForm && (
                <form onSubmit={handleSubmit} encType="multipart/form-data" style={{ marginTop: "1rem" }}>
                    <input
                        type="text"
                        placeholder="Başlıq"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                    <input
                        type="text"
                        placeholder="İfaçı"
                        value={artist}
                        onChange={(e) => setArtist(e.target.value)}
                        required
                    />
                    <input
                        type="file"
                        accept="audio/*"
                        onChange={(e) => setFile(e.target.files[0])}
                        required
                    />
                    <input
                        type="file"
                        accept="image/*"
                        onChange={(e) => setPoster(e.target.files[0])}
                    />
                    <button type="submit">Yüklə</button>
                </form>
            )}
        </div>
    );
};

export default UploadMusicButton;
