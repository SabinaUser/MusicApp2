import React, { useState } from "react";
import LoginForm from "./LoginForm";
import RegisterForm from "./RegisterForm";

const AuthPage = ({ onLogin }) => {
    const [isLogin, setIsLogin] = useState(true);

    return (
        <div>
            {isLogin ? (
                <LoginForm onSwitch={() => setIsLogin(false)} onLogin={onLogin} />

            ) : (
                <RegisterForm onSwitch={() => setIsLogin(true)} />
            )}
        </div>
    );
};

export default AuthPage;
