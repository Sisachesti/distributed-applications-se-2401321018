export const instructorCourseDetailsLoader = async ({ params }) => {
  const { courseId } = params;

  const response = await fetch(
    `https://localhost:7234/api/instructor/management/${courseId}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
    },
  );

  if (!response.ok) {
    if (response.status === 404) {
      throw new Response("Course not found", { status: 404 });
    }
    throw new Response("Failed to fetch course details", {
      status: response.status,
    });
  }

  return await response.json();
};
