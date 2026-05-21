export type NoteStatus =
    | "Pending"
    | "Processing"
    | "Completed"
    | "Failed";

export interface Note {
    id: string;
    content: string;
    title:string;
    summary?: string;
    status: NoteStatus;
    createdAt: string; // ISO date string
}