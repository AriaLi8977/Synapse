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
        <div className="min-h-screen bg-gray-100 p-6">

        <div className="max-w-3xl mx-auto">

            <div className="bg-white rounded-2xl shadow-sm p-8">

            <div className="
                flex flex-col
                items-end
                gap-3
            ">
                <Link
                to="/"
                className="
                    inline-flex items-center
                    text-sm text-blue-600
                    hover:underline mb-4
                "
            >
                ← Back
            </Link>

                    <div className="flex-1 min-w-0">

                        <p className="
                            text-s uppercase tracking-wide
                            text-gray-400 mb-2
                        ">
                            Note Detail
                        </p>

                        <h1 className="
                            text-lg 
                            text-gray-900
                            break-words
                            leading-tight
                        ">
                            {note.title || "Untitled Note"}
                        </h1>
                    </div>

                    {/* <div>

                        <span className="
                            inline-flex items-center
                            rounded-full
                            bg-gray-100
                            px-3 py-1
                            text-xs font-medium
                            text-gray-700
                        ">
                            {note.status}
                        </span>
                    </div> */}
                </div>

                <div className="mt-8">

                    <h2 className="
                        text-sm font-semibold
                        text-gray-500 uppercase
                        tracking-wide mb-3
                    ">
                        Content
                    </h2>

                    <div className="
                        text-gray-700
                        whitespace-pre-wrap
                        leading-7
                    ">
                        {note.content}
                    </div>
                </div>

                {note.summary && (

                    <div className="
                        mt-8
                        rounded-xl
                        bg-gray-50
                        border border-gray-200
                        p-5
                    ">

                        <h2 className="
                            text-sm font-semibold
                            text-gray-500 uppercase
                            tracking-wide mb-3
                        ">
                            AI Summary
                        </h2>

                        <div className="
                            text-gray-700
                            whitespace-pre-wrap
                            leading-7
                        ">
                            {note.summary}
                        </div>
                    </div>
                )}

                <div className="
                    mt-8 pt-6
                    border-t border-gray-200
                    text-sm text-gray-500
                ">
                    Created at{" "}
                    {new Date(note.createdAt)
                        .toLocaleString()}
                </div>
            </div>
        </div>
    </div>
    );
}