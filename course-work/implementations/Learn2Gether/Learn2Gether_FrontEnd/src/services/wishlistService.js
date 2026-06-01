const API_BASE_URL = "https://localhost:7234/api/wishlist";

export async function removeFromWishlist(courseId) {
  if (!courseId) throw new Error("courseId is required");

  const url = `${API_BASE_URL}/remove`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify({ CourseId: courseId }),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Remove from wishlist failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function addToWishlist(courseId) {
  if (!courseId) throw new Error("courseId is required");

  const url = `${API_BASE_URL}/add`;
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
    mode: "cors",
    body: JSON.stringify({ CourseId: courseId }),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Add to wishlist failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}
