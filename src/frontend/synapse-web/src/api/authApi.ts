const API_BASE = "https://localhost:5001/api";

export async function login(email: string, password: string){
    const response = await fetch(`${API_BASE}/login`,{
        method: "POST",
        headers:{
            "Content-Type":"application/json",
        },
        body: JSON.stringify({
            email, 
            password,
        }),
    });
    if (!response.ok){
        throw new Error("Login failed");
    }

    return response.json();
}

export async function register(email: string, password: string){
    const response = await fetch(`${API_BASE}/register`,{
        method: "POST",
        headers:{
            "Content-Type":"application/json",
        },
        body: JSON.stringify({
            email, 
            password,
        }),
    });
    if (!response.ok){
        throw new Error("Register failed");
    }

    return response.json();
}