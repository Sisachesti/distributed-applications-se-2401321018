const API_BASE_URL = "https://localhost:7234/api/courses";

export async function enrollToCourse(courseId) {
  if (!courseId) throw new Error("courseId is required");

  const url = `${API_BASE_URL}/courses/${courseId}/enroll`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
    body: JSON.stringify({ courseId }),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Enroll request failed: ${res.status}`);
  }

  // return parsed response if any, otherwise boolean
  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function fetchEnrolledCourseDetails(courseId) {
  if (!courseId) throw new Error("courseId is required");

  const url = `${API_BASE_URL}/my-courses/${courseId}`;

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
    throw new Error(text || `Fetch enrolled course failed: ${res.status}`);
  }
  return res.json();
}

export async function fetchWatchLecture(courseId, lectureId) {
  const url = `${API_BASE_URL}/my-courses/${courseId}/${lectureId}`;

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
    throw new Error(text || `Fetch lecture failed: ${res.status}`);
  }

  return res.json();
}

export async function addCourse({ title, description, imageUrl }) {
  if (!title) throw new Error("Title is required");
  if (!description) throw new Error("Description is required");
  const courseData = { title, description, imageUrl };

  const url = `${API_BASE_URL}/add`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
    body: JSON.stringify(courseData),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Add course failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function editCourse({ title, description, imageUrl, courseId }) {
  if (!courseId) throw new Error("Course ID is required");
  if (!title) throw new Error("Title is required");
  if (!description) throw new Error("Description is required");
  const courseData = { title, description, imageUrl, courseId };

  const url = `${API_BASE_URL}/edit`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
    body: JSON.stringify(courseData),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Edit course failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}

export async function deleteCourse(courseId) {
  if (!courseId) throw new Error("courseId is required");

  const url = `${API_BASE_URL}/delete/${courseId}`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include", // send cookies for auth
    mode: "cors",
  });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    throw new Error(text || `Delete course failed: ${res.status}`);
  }

  try {
    return await res.json();
  } catch {
    return true;
  }
}
