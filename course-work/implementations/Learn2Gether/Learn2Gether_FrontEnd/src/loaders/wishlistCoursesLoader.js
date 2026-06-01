export async function wishlistCoursesLoader() {
  const res = await fetch(
    `https://localhost:7234/api/Wishlist/courses`,
    {
      method: "GET",
      credentials: "include",
    }
  );

  if (!res.ok) {
    throw new Response("Failed to load wishlist courses", {
      status: res.status,
      statusText: res.statusText,
    });
  }

  return res.json();
}
