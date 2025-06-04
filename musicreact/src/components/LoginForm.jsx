import React, { useState } from "react";
import axios from "../api/axios";

const LoginForm = ({ onSwitch, onLogin }) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const res = await axios.post("/account/signin", {
                email,
                password,
            });

            localStorage.setItem("token", res.data.token);
            alert("Login successful!");
            onLogin();

        } catch (error) {
            alert("Login failed: " + error.response?.data || "Server error");
        }
    };

    return (
        <div>
            <h2>Sign In</h2>
            <form onSubmit={handleLogin}>
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
                <button type="submit">Login</button>
            </form>
            <p>
                Don't have an account? <button onClick={onSwitch}>Sign up</button>
            </p>
        </div>
    );
};

export default LoginForm;
