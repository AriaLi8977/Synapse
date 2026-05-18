import { useState } from "react";
import { register } from "../api/authApi";
import { saveToken } from "../auth/tokenStorage";

interface Props{
    onRegisterSuccess: () => void;
    onGoToLogin: () => void;
}

export function RegisterPage({
    onRegisterSuccess,
    onGoToLogin,
}: Props){

    const [email,setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setUserName] = useState("");

    const handleRegister = async () => {
        try{
            const data = await register(name, email, password);
            saveToken(data.token);
            onRegisterSuccess();
        }catch(error){
            alert("Registration failed: " + error.message);
        }
    };
    return(
        <div style={{ padding: 24}}>
            <h1>Register</h1>
            <input
                placeholder="UserName"
                value={name}
                onChange={(e) => setUserName(e.target.value)}></input>
            <br/><br/>
            <input
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}></input>
            <br/><br/>
            <input
                placeholder="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}></input>
            <br/><br/>
            <button onClick={handleRegister}>Register</button>
            <br/><br/>
            <button onClick={onGoToLogin}>Back to Login</button>
        </div>
    )
}