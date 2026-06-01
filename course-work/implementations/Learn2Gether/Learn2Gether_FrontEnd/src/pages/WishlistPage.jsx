import React, { useState } from "react";
import { Link, useLoaderData } from "react-router-dom";
import { removeFromWishlist } from "../services/wishlistService";
import { useAuth } from "../hooks/useAuth";
import "../styles/WishlistPage.css";

export default function WishlistPage() {
  const loadedCourses = useLoaderData();
  const { user } = useAuth();
  const [wishlist, setWishlist] = useState(loadedCourses || []);
  const [isRemoving, setIsRemoving] = useState(false);
  const [error, setError] = useState(null);

  async function handleRemoveFromWishlist(courseId) {
    if (!user) {
      setError("You must be logged in to remove from wishlist");
      return;
    }

    try {
      setIsRemoving(true);
      setError(null);
      await removeFromWishlist(courseId);

      // Update local state
      setWishlist((prev) => prev.filter((c) => c.id !== courseId));
    } catch (err) {
      console.error("Failed to remove from wishlist:", err);
      setError(err.message || "Failed to remove from wishlist");
    } finally {
      setIsRemoving(false);
    }
  }

  if (!wishlist || wishlist.length === 0) {
    return (
      <main className="courses-page wishlist-page">
        <header className="courses-header">
          <h1 className="courses-title">My Wishlist</h1>
        </header>
        <section className="courses-grid">
          <div className="empty-message">
            <p>
              Your wishlist is empty. Start adding courses you&apos;re
              interested in!
            </p>
            <Link to="/courses" className="course-cta">
              Browse courses
            </Link>
          </div>
        </section>
      </main>
    );
  }

  return (
    <main className="courses-page wishlist-page">
      <header className="courses-header">
        <h1 className="courses-title">My Wishlist</h1>
        <p className="wishlist-subtitle">
          {wishlist.length} course{wishlist.length !== 1 ? "s" : ""} saved
        </p>
        {error && (
          <div
            style={{
              color: "#dc2626",
              fontSize: "14px",
              marginTop: "8px",
              padding: "8px 12px",
              background: "#fee",
              borderRadius: "6px",
            }}
          >
            {error}
          </div>
        )}
      </header>

      <section className="courses-grid" aria-live="polite">
        {wishlist.map((c) => (
          <article key={c.id} className="course-card wishlist-card">
            <img className="course-media" src={c.imageUrl} alt={c.title} />
            <button
              className="remove-wishlist-btn"
              onClick={() => handleRemoveFromWishlist(c.id)}
              aria-label={`Remove ${c.title} from wishlist`}
              title="Remove from wishlist"
              disabled={isRemoving}
            >
              ✕
            </button>
            <div className="course-body">
              <h3 className="course-title">{c.title}</h3>
              <p className="course-meta">
                {c.instructor} · {c.duration}
              </p>
              <div className="course-price-row">
                <span className="course-price">{c.price}</span>
                <span className="course-stats">
                  {c.students} students • {c.rating}★
                </span>
              </div>
              <div className="course-actions">
                <Link to={`/courses/${c.id}`} className="course-cta">
                  View course
                </Link>
              </div>
            </div>
          </article>
        ))}
      </section>
    </main>
  );
}
