import { getToken } from "../auth/tokenStorage";

const API_BASE = "http://localhost:8080/api/Notes";

export async function createNote(content: string){
    const token = getToken();
    const response = await fetch(`${API_BASE}/CreateNotes`,{
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
    const token = getToken();
    const response = await fetch(`${API_BASE}/GetNotes`,{
        method:"GET",
        headers:{
            Authorization: `Bearer ${token}`,
            //"Content-Type":"application/json"
        }
    })
    if(!response.ok){
        throw new Error("Failed to fetch notes");
    }
    return await response.json();
}
