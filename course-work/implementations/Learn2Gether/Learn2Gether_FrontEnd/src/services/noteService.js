const API_BASE_URL = "https://localhost:7234/api/note";

export async function saveNote(courseId, lectureId, content) {
  if (!courseId) throw new Error("courseId is required");
  if (!lectureId) throw new Error("lectureId is required");

  if (typeof content !== "string") throw new Error("content must be a string");

  const url = `${API_BASE_URL}/save/${lectureId}`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
    body: JSON.stringify({ content }),
  });
  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Save note failed: ${res.status}`);
  }

  // return parsed response if any, otherwise boolean
  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function deleteNote(noteId) {
  if (!noteId) throw new Error("noteId is required");

  const url = `${API_BASE_URL}/delete`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(noteId),
    credentials: "include",
    mode: "cors",
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Delete note failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function editNote(noteId, content) {
  if (!noteId) throw new Error("noteId is required");
  if (typeof content !== "string") throw new Error("content must be a string");

  const url = `${API_BASE_URL}/edit`;
  const body = { NoteId: noteId, Content: content };
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Edit note failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}
