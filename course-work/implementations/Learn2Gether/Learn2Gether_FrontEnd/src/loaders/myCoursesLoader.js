export async function myCoursesLoader() {
  const res = await fetch(
    `https://localhost:7234/api/courses/my-courses`,
    {
      method: "GET",
      credentials: "include",
    }
  );

  if (!res.ok) {
    throw new Response("Failed to load enrolled courses", {
      status: res.status,
      statusText: res.statusText,
    });
  }

  return res.json();
}
