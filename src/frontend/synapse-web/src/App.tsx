import { useState } from 'react'
import { createNote } from './api/client'
import { useEffect } from 'react'
import { createConnection } from './signalr/connection'

import './App.css'

function App() {

  const [content, setContent] = useState("Hello, Synapse!")
  const [noteId, setNoteId] = useState<string | null>(null)
  const [status, setStatus] = useState("Idle")
  const [summary, setSummary] = useState("")

  const handleCreate = async () => {
    setStatus("Creating note...")
    const result = await createNote(content);
 
    setNoteId(result.id)
    const conn = createConnection();
    await conn.invoke("JoinNoteGroup", result.id)
    setStatus("Note created with ID: " + result.id)
  }

  useEffect(() => {
    const conn = createConnection();

    conn.start().then(() => console.log("Connected to SignalR hub"))
                .catch(err => console.error("Failed to connect to SignalR hub", err));
    
    conn.on("NoteCompleted", (data)=> {
        setSummary(data.summary)
        setStatus("Note completed!")
    });
    return () => {
        conn.stop()
    };},[]);

  return (
    <>
    <div style={{ padding: 20}}>
      <h1>AI Summary Notes</h1>
      <textarea 
        value={content} 
        onChange={(e) => setContent(e.target.value)} 
        rows={5} 
        cols={50}/>
      <br/>
    <button onClick={handleCreate}>CreateNote</button>

    <div>Status: {status}</div>
    {noteId && <div>Note ID: {noteId}</div>}
    {summary && <div>
      <h2>Summary:</h2>
      <p>{summary}</p>
      </div>}
    </div>
    </>
  )
}

export default App
