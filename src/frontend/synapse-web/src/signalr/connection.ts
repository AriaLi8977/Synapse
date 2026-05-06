import * as signalR from "@microsoft/signalr";

let connection: signalR.HubConnection | null = null;

export function createConnection(token?: string) {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:8080/hubs/notes", {
            accessTokenFactory: () => token || "",
        })
        .withAutomaticReconnect()
        .build();

    return connection
}

export function getConnection() {
    if (!connection) {
        throw new Error("Connection has not been created yet.");
    }
    return connection;
}