import { useEffect, useState } from "react";
import { noteHub } from "../signalr/noteHub";
import { createNote } from "../api/notesApi";
import type { Note } from "../types/note";
import { NoteCard } from "../components/NoteCard";
import { NoteForm } from "../components/NoteForm";

export function HomePage(){
    const [notes, setNotes] = useState<Note[]>([]);
    useEffect(()=>{
        noteHub.start();

        noteHub.onNoteCompleted((data)=>{
            setNotes((prev)=>
                prev.map((note)=>
                    note.id === data.noteId ? {...note, status: "Completed", summary: data.summary} : note)
            )
        })
    });

    const handleCreate = async (content: string) => {
        const newNote = await createNote(content);
        setNotes((prev) => [...prev, newNote]);
        await noteHub.joinNoteGroup(newNote.id);
    };

    return(
        <div style={{ padding: 24}}>
            <h1>AI Summary Notes</h1>
            <NoteForm onCreate={handleCreate}/>
            {notes.map((note) => (
                <NoteCard key={note.id} note={note}/>
            ))}
        </div>
    )
};