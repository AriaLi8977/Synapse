import { getToken } from "../auth/tokenStorage";

const API_BASE = "https://localhost:5001/api";

export async function createNote(content: string){
    const token = getToken();
    const response = await fetch(`${API_BASE}/notes`,{
        method:"POST",
        headers:{
            "Content-Type":"application/json",
            Authorization: `Bearer ${token}`, //include token for authentication
        },
        body: JSON.stringify({content})
    })
    if(!response.ok){
        throw new Error("Failed to create note");
    }
    return await response.json();
}

export async function getNotes(){
    const response = await fetch(`${API_BASE}/notes`,{
        method:"GET",
        headers:{
            "Content-Type":"application/json"
        }
    })
    if(!response.ok){
        throw new Error("Failed to fetch notes");
    }
    return await response.json();
}
