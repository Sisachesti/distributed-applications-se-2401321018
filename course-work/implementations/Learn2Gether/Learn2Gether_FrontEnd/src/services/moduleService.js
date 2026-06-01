const API_BASE_URL = "https://localhost:7234/api/module";

export async function addModule({ courseId, title }) {
  if (!courseId) throw new Error("Course ID is required");
  if (!title) throw new Error("Title is required");

  const moduleData = {
    courseId,
    title: String(title),
  };

  const url = `${API_BASE_URL}/add`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify(moduleData),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Add module failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function editModule({ moduleId, title }) {
  if (!moduleId) throw new Error("Module ID is required");
  if (!title) throw new Error("Title is required");

  const moduleData = { moduleId, title };

  const url = `${API_BASE_URL}/edit`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify(moduleData),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Edit module failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function deleteModule(moduleId) {
  if (!moduleId) throw new Error("Module ID is required");

  const url = `${API_BASE_URL}/delete/${moduleId}`;

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
    throw new Error(text || `Delete module failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}
