import type { Note } from "../types/note";

interface Props{ note: Note;}

export function NoteCard({ note }: Props) {

    const statusMap: Record<number, {text: string; color:string}>={
        0:{
            text: "Pending",
            color: "bg-yellow-100 text-yellow-800"
        },
        1:{
            text: "Processing",
            color: "bg-blue-100 text-blue-800"
        },
        2:{
            text: "Completed",
            color: "bg-green-100 text-green-800"
        },
        3:{
            text: "Failed",
            color: "bg-red-100 text-red-800"
        }
    }

    const statusInfo = statusMap[note.status] || {text: "Unknown", color: "bg-gray-100 text-gray-800"};

    return (
      <div className="bg-white rounded-xl shadow p-5">
  
        <div className="flex justify-between items-center">
  
          <h3 className="font-semibold text-lg">
            Note
          </h3>
  
          <span className={`text-sm rounded-full font-medium ${statusInfo.color} px-2 py-1`}>
            {statusInfo.text}
          </span>
        </div>
  
        <p className="mt-4 text-gray-700 whitespace-pre-wrap">
          {note.content}
        </p>
  
        {note.summary && (
          <>
            <hr className="my-4" />
  
            <h4 className="font-medium mb-2">
              AI Summary
            </h4>
  
            <p className="text-gray-700">
              {note.summary}
            </p>
          </>
        )}
      </div>
    );
  }