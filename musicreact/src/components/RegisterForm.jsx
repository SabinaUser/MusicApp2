import React, { useState } from "react";
import axios from "../api/axios";

const RegisterForm = ({ onSwitch }) => {
    const [email, setEmail] = useState("");
    const [fullName, setFullName] = useState("");
    const [password, setPassword] = useState("");

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await axios.post("/account/signup", {
                email,
                fullName,
                password,
            });
            alert("Registration successful!");
            onSwitch(); // switch back to login
        } catch (error) {
            alert("Registration failed: " + error.response?.data || "Server error");
        }
    };

    return (
        <div>
            <h2>Sign Up</h2>
            <form onSubmit={handleRegister}>
                <input
                    type="text"
                    placeholder="Full name"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                    required
                />
                <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit">Register</button>
            </form>
            <p>
                Already have an account? <button onClick={onSwitch}>Sign in</button>
            </p>
        </div>
    );
};

export default RegisterForm;
