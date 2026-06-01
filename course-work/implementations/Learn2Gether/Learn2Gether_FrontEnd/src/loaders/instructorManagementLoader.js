export const instructorManagementLoader = async () => {
  const response = await fetch(
    "https://localhost:7234/api/instructor/management",
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
    },
  );

  if (!response.ok) {
    throw new Response("Failed to fetch courses", { status: response.status });
  }

  return await response.json();
};
