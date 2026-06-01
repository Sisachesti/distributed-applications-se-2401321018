export async function welcomePageLoader() {
  const res = await fetch(`https://localhost:7234/api/Courses/top-courses`, {
    method: "GET",
  });

  if (!res.ok) {
    throw new Response("Failed to load wishlist courses", {
      status: res.status,
      statusText: res.statusText,
    });
  }

  return res.json();
}
