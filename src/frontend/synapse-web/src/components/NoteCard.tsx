import type { Note } from "../types/note";

interface Props{ note: Note;}

export function NoteCard({note}: Props){
    return(
        <div style={{
            border: "1px solid #ccc",
            borderRadius: 5,
            padding: 10,
            marginBottom: 10
        }}>
            <div><strong>Content:</strong> {note.content}</div>
            <div><strong>Status:</strong> {note.status}</div>
            {note.summary && <div><strong>Summary:</strong> {note.summary}</div>}
            <div><small>Created At: {new Date(note.createdAt).toLocaleString()}</small></div> 
        </div>  
    );

}