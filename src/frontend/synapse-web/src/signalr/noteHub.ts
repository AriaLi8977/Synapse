import * as signalR from "@microsoft/signalr";
import { getToken } from "../auth/tokenStorage";

class NoteHubService {
    private connection: signalR.HubConnection;
    private startPromise: Promise<void> | null = null;

    constructor(){
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:5001/hubs/notes",{
                accessTokenFactory: () => getToken() || "", //provide token for authentication
            })
            .withAutomaticReconnect()
            .build();
    }

    async start() {
        if (this.connection.state === signalR.HubConnectionState.Connected) {
            return;
        }

        if (this.startPromise) {
            return this.startPromise;
        }

        this.startPromise = this.connection
            .start()
            .then(() => {
                console.log("SignalR connected");
            })
            .catch((err) => {
                console.error("SignalR start failed:", err);
                this.startPromise = null;
                throw err;
            });

        return this.startPromise;
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