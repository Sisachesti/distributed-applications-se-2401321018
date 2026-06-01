import { Link } from "react-router-dom";
import { useState } from "react";
import { addToWishlist, removeFromWishlist } from "../services/wishlistService";
import { useAuth } from "../hooks/useAuth";

export default function CoursesMainSection({ current = [] }) {
  const { user } = useAuth();
  const [wishlistedCourses, setWishlistedCourses] = useState(() => {
    // Initialize with courses that are already in wishlist
    return new Set(current.filter((c) => c.isInWishlist).map((c) => c.id));
  });
  const [loading, setLoading] = useState(null);

  const handleWishlistToggle = async (courseId, isCurrentlyWishlisted) => {
    if (!user) {
      alert("Please log in to add courses to your wishlist");
      return;
    }

    try {
      setLoading(courseId);

      if (isCurrentlyWishlisted) {
        await removeFromWishlist(courseId);
        setWishlistedCourses((prev) => {
          const updated = new Set(prev);
          updated.delete(courseId);
          return updated;
        });
      } else {
        await addToWishlist(courseId);
        setWishlistedCourses((prev) => new Set([...prev, courseId]));
      }
    } catch (error) {
      console.error("Failed to update wishlist:", error);
      alert(error.message || "Failed to update wishlist");
    } finally {
      setLoading(null);
    }
  };
  return (
    <section className="courses-grid" aria-live="polite">
      {(current || []).map((c) => {
        const isWishlisted = wishlistedCourses.has(c.id);
        const isLoading = loading === c.id;

        return (
          <article key={c.id} className="course-card">
            <img
              className="course-media"
              src={c.imageUrl}
              alt={c.title || "Course image"}
            />
            <button
              className="wishlist-star-btn"
              onClick={() => handleWishlistToggle(c.id, isWishlisted)}
              disabled={isLoading || !user}
              title={isWishlisted ? "Remove from wishlist" : "Add to wishlist"}
              aria-label={
                isWishlisted ? "Remove from wishlist" : "Add to wishlist"
              }
            >
              {isWishlisted ? "★" : "☆"}
            </button>
            <div className="course-body">
              <h3 className="course-title">{c.title}</h3>
              <p className="course-meta">
                {c.instructor} · {c.duration}
              </p>
              <p>
                {c.enrolled && <span className="enrolled-badge">Enrolled</span>}
              </p>
              <div className="course-actions">
                <Link
                  to={`/courses/${c.id}`}
                  className="course-cta"
                  aria-label={`Open course ${c.title}`}
                >
                  View
                </Link>
                <span className="course-stats">
                  {c.students} students • {c.rating}★
                </span>
              </div>
            </div>
          </article>
        );
      })}
    </section>
  );
}
