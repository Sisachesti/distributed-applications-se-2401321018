import React, { useState } from "react";
import { Link, useLoaderData } from "react-router-dom";
import { addToWishlist, removeFromWishlist } from "../services/wishlistService";
import { useAuth } from "../hooks/useAuth";
import "../styles/MyCoursesPage.css";

export default function MyCoursesPage() {
  const enrolled = useLoaderData() || [];
  const { user } = useAuth();
  const [wishlistedCourses, setWishlistedCourses] = useState(() => {
    // Initialize with courses that are already in wishlist
    return new Set(enrolled.filter((c) => c.isInWishlist).map((c) => c.id));
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

  if (!enrolled || enrolled.length === 0) {
    return (
      <main className="courses-page">
        <header className="courses-header">
          <h1 className="courses-title">My Courses</h1>
        </header>
        <section className="courses-grid">
          <div className="empty-message">
            <p>You are not enrolled in any courses yet.</p>
            <Link to="/courses" className="course-cta">
              Browse courses
            </Link>
          </div>
        </section>
      </main>
    );
  }

  return (
    <main className="courses-page">
      <header className="courses-header">
        <h1 className="courses-title">My Courses</h1>
      </header>

      <section className="courses-grid" aria-live="polite">
        {enrolled.map((c) => {
          const isWishlisted = wishlistedCourses.has(c.id);
          const isLoading = loading === c.id;

          return (
            <article key={c.id} className="course-card">
              <img className="course-media" src={c.imageUrl} alt={c.title} />
              <button
                className="wishlist-star-btn"
                onClick={() => handleWishlistToggle(c.id, isWishlisted)}
                disabled={isLoading || !user}
                title={
                  isWishlisted ? "Remove from wishlist" : "Add to wishlist"
                }
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
                <div className="course-actions">
                  <Link to={`/courses/${c.id}`} className="course-cta">
                    View course
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
    </main>
  );
}
