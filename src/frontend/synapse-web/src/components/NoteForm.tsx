import { useState } from "react";

interface Props{
    onCreate: (content: string) => Promise<void>;
}

export function NoteForm({ onCreate }: Props){
    const [content, setContent] = useState("");

    const handleCreate = async () => {
        if(!content.trim()) return;
        await onCreate(content);
        setContent("");
    };
    return(
        <div style={{ marginBottom: 20}}>
            <textarea 
                value={content} 
                onChange={(e) => setContent(e.target.value)} 
                rows={5} 
                cols={50}
                placeholder="Enter note content here..."/>
            <br/>
            <button onClick={handleCreate}>Create Note</button>
        </div>
    );

}

