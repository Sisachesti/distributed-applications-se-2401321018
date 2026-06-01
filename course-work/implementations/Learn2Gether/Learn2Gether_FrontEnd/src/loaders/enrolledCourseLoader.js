export async function enrolledCourseLoader({ params }) {
  const { id } = params || {};

  if (!id) {
    throw new Response("Course id is missing in route params", {
      status: 400,
      statusText: "Bad Request",
    });
  }

  const res = await fetch(
    `https://localhost:7234/api/courses/my-courses/${id}`,
    {
      method: "GET",
      credentials: "include",
    },
  );

  if (!res.ok) {
    throw new Response("Failed to load enrolled course", {
      status: res.status,
      statusText: res.statusText,
    });
  }

  return res.json();
}
