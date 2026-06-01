export async function coursesLoader({ request }) {
  const url = new URL(request.url);
  const searchQuery = url.searchParams.get("search") || "";
  const currentPage = parseInt(url.searchParams.get("page")) || 1;
  const entitiesPerPage = 15;

  const params = new URLSearchParams({
    SearchQuery: searchQuery,
    CurrentPage: currentPage.toString(),
    EntitiesPerPage: entitiesPerPage.toString(),
  });

  const res = await fetch(
    `https://localhost:7234/api/courses/courses?${params.toString()}`,
    {
      method: "GET",
    },
  );

  if (!res.ok) {
    throw new Response("Failed to load courses", {
      status: res.status,
      statusText: res.statusText,
    });
  }

  return res.json();
}
