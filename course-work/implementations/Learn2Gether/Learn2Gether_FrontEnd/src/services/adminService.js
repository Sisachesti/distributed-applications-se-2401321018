const API_BASE_URL = "https://localhost:7234/api/admin";

export async function assignRoleToUser(userId, role) {
  if (!userId || !role) {
    throw new Error("userId and role are required");
  }

  const url = `${API_BASE_URL}/assign-role`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
    body: JSON.stringify({ userId, role }),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Failed to assign role: ${res.status}`);
  }

  // Return true on success or parse response
  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function removeRoleFromUser(userId, role) {
  if (!userId || !role) {
    throw new Error("userId and role are required");
  }

  const url = `${API_BASE_URL}/remove-role`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
    body: JSON.stringify({ userId, role }),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Failed to remove role: ${res.status}`);
  }

  // Return true on success or parse response
  try {
    return await res.json();
  } catch {
    return true;
  }
}
