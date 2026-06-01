const API_BASE_URL = "https://localhost:7234/api/answer";

export async function saveAnswer(questionId, title, content) {
  if (!questionId) throw new Error("questionId is required");
  if (typeof title !== "string") throw new Error("title must be a string");
  if (typeof content !== "string") throw new Error("content must be a string");

  const url = `${API_BASE_URL}/save/${questionId}`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify({ title, content }),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Save answer failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function editAnswer(answerId, title, content) {
  if (!answerId) throw new Error("answerId is required");
  if (typeof title !== "string") throw new Error("title must be a string");
  if (typeof content !== "string") throw new Error("content must be a string");

  const url = `${API_BASE_URL}/edit`;
  const body = { AnswerId: answerId, Title: title, Content: content };
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
    throw new Error(text || `Edit answer failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function deleteAnswer(answerId) {
  if (!answerId) throw new Error("answerId is required");

  const url = `${API_BASE_URL}/delete`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify(answerId),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Delete answer failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}
