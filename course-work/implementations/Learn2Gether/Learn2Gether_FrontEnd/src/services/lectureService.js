const API_BASE_URL = "https://localhost:7234/api/lecture";

export async function addLecture({ moduleId, title, videoUrl }) {
  if (!moduleId) throw new Error("Module ID is required");
  if (!title) throw new Error("Title is required");

  const lectureData = { moduleId, title, videoUrl };

  const url = `${API_BASE_URL}/add`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify(lectureData),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Add lecture failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function updateLecture({ lectureId, title, videoUrl }) {
  if (!lectureId) throw new Error("Lecture ID is required");
  if (!title) throw new Error("Title is required");

  const lectureData = { lectureId, title, videoUrl };

  const url = `${API_BASE_URL}/update`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify(lectureData),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Update lecture failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function deleteLecture(lectureId) {
  if (!lectureId) throw new Error("Lecture ID is required");

  const url = `${API_BASE_URL}/delete/${lectureId}`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Delete lecture failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}
