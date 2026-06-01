const API_BASE_URL = "https://localhost:7234/api/admin";

export async function adminManagementLoader() {
  const url = `${API_BASE_URL}/index`;

  const res = await fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Failed to fetch users: ${res.status}`);
  }

  return await res.json();
}
