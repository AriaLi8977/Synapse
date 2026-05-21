import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import { getNoteById } from "../api/notesApi";
import type { Note } from "../types/note";

export function NoteDetailPage() {
    const { id } = useParams();
    const [note,setNote] = useState<Note | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(""); 

    useEffect(()=>{
        async function loadNote(){
            try{
                setLoading(true);
                const data = await getNoteById(id!);
                setNote(data);
            }catch(error){
                setError("Failed to load note details.");
            }finally{
                setLoading(false);
            }
        }
        loadNote();
    },[])
    if (loading) {
        return (
          <div className="p-8">
            Loading note...
          </div>
        );
      }
    
      if (error) {
        return (
          <div className="p-8 text-red-500">
            {error}
          </div>
        );
      }
    
      if (!note) {
        return (
          <div className="p-8">
            Note not found
          </div>
        );
      }

    return (
        <div className="min-h-screen bg-gray-100 p-8">

        <div className="max-w-3xl mx-auto">
  
          <Link
            to="/"
            className="text-sm text-blue-500 hover:underline"
          >
            ← Back
          </Link>
  
          <div className="bg-white rounded-xl shadow p-6 mt-4">
  
            <div className="flex justify-between items-center">
  
              <h1 className="text-2xl font-bold">
                Note Detail
              </h1>
  
              <span className="text-sm text-gray-500">
                {note.status}
              </span>
            </div>
  
            <div className="mt-6">
  
              <h2 className="font-semibold mb-2">
                Content
              </h2>
  
              <p className="text-gray-700 whitespace-pre-wrap">
                {note.content}
              </p>
            </div>
  
            {note.summary && (
              <div className="mt-6">
  
                <h2 className="font-semibold mb-2">
                  AI Summary
                </h2>
  
                <p className="text-gray-700">
                  {note.summary}
                </p>
              </div>
            )}
  
            <div className="mt-6 text-sm text-gray-500">
  
              Created at:
              {" "}
              {new Date(note.createdAt)
                .toLocaleString()}
            </div>
          </div>
        </div>
      </div>
    );
}