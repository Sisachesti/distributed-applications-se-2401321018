const API_BASE_URL = "https://localhost:7234/api/question";

export async function saveQuestion(courseId, lectureId, title, content) {
  if (!courseId) throw new Error("courseId is required");
  if (!lectureId) throw new Error("lectureId is required");

  if (typeof title !== "string") throw new Error("title must be a string");
  if (typeof content !== "string") throw new Error("content must be a string");

  const url = `${API_BASE_URL}/save/${lectureId}`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
    body: JSON.stringify({ title, content }),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Save question failed: ${res.status}`);
  }

  // return parsed response if any, otherwise boolean
  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function editQuestion(questionId, title, content) {
  if (!questionId) throw new Error("questionId is required");
  if (typeof title !== "string") throw new Error("title must be a string");
  if (typeof content !== "string") throw new Error("content must be a string");

  const url = `${API_BASE_URL}/edit`;
  const body = { QuestionId: questionId, Title: title, Content: content };
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
    throw new Error(text || `Edit question failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function deleteQuestion(questionId) {
  if (!questionId) throw new Error("questionId is required");

  const url = `${API_BASE_URL}/delete`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify(questionId),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Delete question failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}
