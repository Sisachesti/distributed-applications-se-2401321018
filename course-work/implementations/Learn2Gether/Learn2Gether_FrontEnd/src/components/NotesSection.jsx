import { useState } from "react";
import { saveNote, deleteNote, editNote } from "../services/noteService";

export default function NotesSection({
  notes,
  courseId,
  lectureId,
  onNoteSaved,
}) {
  const [noteContent, setNoteContent] = useState("");
  const [isSaving, setIsSaving] = useState(false);
  const [saveError, setSaveError] = useState(null);
  const [editingNoteId, setEditingNoteId] = useState(null);
  const [editingContent, setEditingContent] = useState("");

  const handleSaveNote = async () => {
    if (!noteContent.trim()) {
      setSaveError("Note content cannot be empty");
      return;
    }

    try {
      setIsSaving(true);
      setSaveError(null);
      await saveNote(courseId, lectureId, noteContent);
      setNoteContent("");

      // Call the parent callback to refresh notes
      if (onNoteSaved) {
        await onNoteSaved();
      }
    } catch (error) {
      console.error("Failed to save note:", error);
      setSaveError(error.message || "Failed to save note");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeleteNote = async (noteId) => {
    if (!noteId) return;
    try {
      setIsSaving(true);
      setSaveError(null);
      await deleteNote(noteId);

      if (onNoteSaved) await onNoteSaved();
    } catch (error) {
      console.error("Failed to delete note:", error);
      setSaveError(error.message || "Failed to delete note");
    } finally {
      setIsSaving(false);
    }
  };

  const handleStartEdit = (note) => {
    setEditingNoteId(note.id);
    setEditingContent(note.content || "");
    setSaveError(null);
  };

  const handleCancelEdit = () => {
    setEditingNoteId(null);
    setEditingContent("");
    setSaveError(null);
  };

  const handleSaveEdit = async (noteId) => {
    if (!editingContent.trim()) {
      setSaveError("Note content cannot be empty");
      return;
    }

    try {
      setIsSaving(true);
      setSaveError(null);
      await editNote(noteId, editingContent);
      setEditingNoteId(null);
      setEditingContent("");
      if (onNoteSaved) await onNoteSaved();
    } catch (error) {
      console.error("Failed to edit note:", error);
      setSaveError(error.message || "Failed to edit note");
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="notes-section">
      <h3 className="notes-header">Your Notes</h3>
      <div>
        <div className="notes-list">
          {Array.isArray(notes) &&
            (notes.length === 0 ? (
              <div className="empty">No notes yet.</div>
            ) : (
              notes.map((note) => (
                <div key={note.id} className="note-item">
                  <button
                    className="note-edit-btn note-action-btn"
                    title="Edit note"
                    onClick={() => handleStartEdit(note)}
                    disabled={isSaving}
                  >
                    ✎
                  </button>
                  <button
                    className="note-delete-btn note-action-btn"
                    title="Delete note"
                    onClick={() => handleDeleteNote(note.id)}
                    disabled={isSaving}
                  >
                    ×
                  </button>
                  <div className="note-meta">
                    <small>{note.createdAt}</small>
                  </div>
                  {editingNoteId === note.id ? (
                    <div>
                      <textarea
                        className="notes-textarea"
                        value={editingContent}
                        onChange={(e) => {
                          setEditingContent(e.target.value);
                          if (saveError) setSaveError(null);
                        }}
                        disabled={isSaving}
                      />
                      <div className="notes-actions">
                        <button
                          onClick={() => handleSaveEdit(note.id)}
                          disabled={isSaving || !editingContent.trim()}
                        >
                          {isSaving ? "Saving..." : "Save"}
                        </button>
                        <button onClick={handleCancelEdit} disabled={isSaving}>
                          Cancel
                        </button>
                      </div>
                    </div>
                  ) : (
                    <div className="note-text">{note.content}</div>
                  )}
                </div>
              ))
            ))}
        </div>
      </div>
      {saveError && (
        <div
          className="note-error"
          style={{ color: "#dc2626", fontSize: "14px", marginBottom: "8px" }}
        >
          {saveError}
        </div>
      )}
      <textarea
        className="notes-textarea"
        value={noteContent}
        onChange={(e) => {
          setNoteContent(e.target.value);
          if (saveError) setSaveError(null);
        }}
        placeholder="Write notes for this lecture..."
        disabled={isSaving}
      />
      <div className="notes-actions">
        <button
          onClick={handleSaveNote}
          disabled={isSaving || !noteContent.trim()}
        >
          {isSaving ? "Saving..." : "Save Notes"}
        </button>
        <button onClick={() => setNoteContent("")} disabled={isSaving}>
          Clear
        </button>
      </div>
    </div>
  );
}
