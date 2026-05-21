import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { noteHub } from "../signalr/noteHub";
import { createNote, getNotes } from "../api/notesApi";
import type { Note } from "../types/note";
import { NoteCard } from "../components/NoteCard";
import { NoteForm } from "../components/NoteForm";
import { Navbar } from "../components/Navbar";

export function HomePage(){
    const [notes, setNotes] = useState<Note[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [creating, setCreating] = useState(false);
    const [successMessage, setSuccessMessage] = useState("");
    const navigate = useNavigate();

    useEffect(()=>{
        async function loadNotes(){ 
            try{
                setLoading(true);
                const data = await getNotes();
                setNotes(data);
            }catch{
                setError("Failed to load notes.");
            }finally{
                setLoading(false);
            }
        }
        loadNotes();

        noteHub.start();

        noteHub.onNoteProcessing((data)=>{
            setNotes((prev)=>
                prev.map((note)=>
                    note.id === data.noteId ? {...note, status: "Processing", summary: data.summary} : note)
            )
        })

        noteHub.onNoteCompleted((data)=>{
            setNotes((prev)=>
                prev.map((note)=>
                    note.id === data.noteId ? {...note, status: "Completed", summary: data.summary} : note)
            )
        })
    }, []);

    const handleCreate = async (content: string) => {
        try{
            setCreating(true);
            setSuccessMessage("");

            const result = await createNote(content);

            const newNote: Note = {
              id: result.noteId,
              content,
              status: "Processing",
              createdAt: new Date().toISOString(),
            };

            setNotes((prev) => [newNote,...prev]);
            await noteHub.joinNoteGroup(newNote.id);
            setSuccessMessage("Note created successfully!");
        }catch{
            alert("Failed to create note: ");
            return;
        }finally{
            setCreating(false);
        }
    };

    return (
        <div className="min-h-screen bg-gray-100 p-8">
          <div className="max-w-3xl mx-auto">
      
            <Navbar onLogout={()=>navigate("/login")} />
      
            <div className="bg-white rounded-xl shadow p-6">
      
              <h1 className="text-3xl font-bold mb-6">
                Synapse AI Notes
              </h1>
      
              <NoteForm onCreate={handleCreate} 
                        creating={creating}/>
      
            </div>
      
            {loading && (
              <div className="mt-6 text-gray-600">
                Loading notes...
              </div>
            )}
      
            {error && (
              <div className="mt-6 text-red-500">
                {error}
              </div>
            )}

            {successMessage && (
                <div className="mt-6 text-green-500">
                    {successMessage}
                </div>
            )}
      
            <div className="mt-6 space-y-4">
              {notes.map((note) => (
                <NoteCard key={note.id} note={note} />
              ))}
            </div>
          </div>
        </div>
      );
};