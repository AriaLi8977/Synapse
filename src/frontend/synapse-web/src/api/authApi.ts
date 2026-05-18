const API_BASE = "http://localhost:8080/api/auth";

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

export async function register(name: string, email: string, password: string){
    const response = await fetch(`${API_BASE}/register`,{
        method: "POST",
        headers:{
            "Content-Type":"application/json",
        },
        body: JSON.stringify({
            name,
            email, 
            password,
        }),
    });
    if (!response.ok){
        throw new Error("Register failed");
    }

    return response.json();
}