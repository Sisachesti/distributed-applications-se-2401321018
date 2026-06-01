import React from "react";
import { Link, useLoaderData } from "react-router-dom";

export default function TopCourses() {
  const courses = useLoaderData();

  const list =
    Array.isArray(courses) && courses.length ? courses.slice(0, 3) : [];

  return (
    <section className="tc-container" aria-labelledby="top-courses-title">
      <div className="tc-header">
        <h2 id="top-courses-title" className="tc-heading">
          Top Courses
        </h2>
        <Link to="courses" className="tc-view-all">
          View all
        </Link>
      </div>

      <div className="tc-grid">
        {list.map((c, idx) => (
          <article
            key={c.id}
            className="tc-card"
            aria-label={`${c.title} course`}
          >
            <img src={c.imageUrl} alt={c.title} className="tc-media" />
            <div className="tc-body">
              <h3 className="tc-title">{c.title}</h3>
              <div className="tc-meta">
                <span className="tc-instructor">{c.instructor}</span>
                <span className="tc-sep">·</span>
                <span className="tc-students">{c.students} students</span>
              </div>

              <div className="tc-actions">
                <Link
                  to={`/courses/${c.id}`}
                  className="tc-button"
                  aria-label={`Open course ${c.title}`}
                >
                  View
                </Link>
              </div>
            </div>

            <div className="tc-footer">
              <small className="tc-rank">Top {idx + 1}</small>
              <small className="tc-updated">Updated recently</small>
            </div>
          </article>
        ))}
      </div>
    </section>
  );
}
