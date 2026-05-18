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
    return (
        <div>
      
          <textarea
            rows={5}
            value={content}
            onChange={(e) => setContent(e.target.value)}
            className="
              w-full
              border
              rounded-lg
              p-3
              focus:outline-none
              focus:ring
            "
            placeholder="Write your note here..."
          />
      
          <button
            onClick={handleCreate}
            className="
              mt-4
              bg-black
              text-white
              px-4
              py-2
              rounded-lg
            "
          >
            Create Note
          </button>
        </div>
      );

}

