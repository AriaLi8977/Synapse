import * as signalR from "@microsoft/signalr";
import { getToken } from "../auth/tokenStorage";

class NoteHubService {
    private connection: signalR.HubConnection;

    constructor(){
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:5001/hubs/notes",{
                accessTokenFactory: () => getToken() || "", //provide token for authentication
            })
            .withAutomaticReconnect()
            .build();
    }

    async start(){
        if(this.connection.state === signalR.HubConnectionState.Disconnected){
            await this.connection.start();
        }
    }
    async joinNoteGroup(noteId: string){
        await this.start();
        await this.connection.invoke("JoinNoteGroup", noteId);
    }

    onNoteProcessing(callback: (data: {noteId: string, summary: string}) => void){
        this.connection.on("NoteProcessing", callback);
    }

    onNoteCompleted(callback: (data: {noteId: string, summary: string}) => void){
        this.connection.on("NoteCompleted", callback);
    }
}

export const noteHub = new NoteHubService();