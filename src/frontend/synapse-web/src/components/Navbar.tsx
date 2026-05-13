import { clearToken } from "../auth/tokenStorage";

interface Props{
    onLogout: () => void;
}

export function Navbar({ onLogout }: Props){
    const handleLogout = () =>{
        clearToken();
        onLogout();
    }
    return(
        <div style={{ display: "flex", justifyContent: "space-between", marginBottom: 24}}>
            <h2>Synapse</h2>
            <button onClick={handleLogout}>Logout</button>
        </div>
    )
}