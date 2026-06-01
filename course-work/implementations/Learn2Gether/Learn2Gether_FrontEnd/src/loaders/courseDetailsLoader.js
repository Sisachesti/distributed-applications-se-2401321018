export async function courseDetailsLoader({ params }) {
  const { id } = params || {};

  if (!id) {
    throw new Response("Course id is missing in route params", {
      status: 400,
      statusText: "Bad Request",
    });
  }

  const res = await fetch(`https://localhost:7234/api/courses/courses/${id}`, {
    method: "GET",
    credentials: "include",
  });

  if (!res.ok) {
    throw new Response("Failed to load course details", {
      status: res.status,
      statusText: res.statusText,
    });
  }

  return res.json();
}
