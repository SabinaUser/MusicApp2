import React, { useEffect, useState } from "react";
import axios from "../api/axios";
import "./Profile.css";

const Profile = () => {
    const [profile, setProfile] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const res = await axios.get("/account/profile");
                setProfile(res.data);
            } catch (err) {
                console.error("Profil məlumatı alınmadı", err);
            }
        };
        fetchProfile();
    }, []);

    if (!profile) return <div>Yüklənir...</div>;

    return (
        <div className="profile-page">
            <h2>Profil Məlumatları</h2>
            <img
                src={`https://localhost:7147${profile.profileImageUrl}`}
                alt="Profil şəkli"
                className="profile-avatar"
            />
            <p><strong>Ad Soyad:</strong> {profile.fullName}</p>
            <p><strong>Email:</strong> {profile.email}</p>
            <p><strong>Qeydiyyat Tarixi:</strong> {new Date(profile.createdAt).toLocaleDateString()}</p>
        </div>
    );
};

export default Profile;
