import { useLoaderData, Link, useParams, useNavigate } from "react-router-dom";
import {
  enrollToCourse,
  fetchEnrolledCourseDetails,
} from "../services/courseService";
import { useAuth } from "../hooks/useAuth";
import "../styles/CourseInfoPage.css";

function Stars({ value = 0, max = 5 }) {
  const full = Math.round(Math.min(value, max));
  return (
    <span aria-hidden className="cp-stars">
      {"★".repeat(full) + "☆".repeat(max - full)}
    </span>
  );
}

function formatDuration(minutes = 0) {
  const hrs = Math.floor(minutes / 60);
  const mins = minutes % 60;
  if (hrs > 0) return `${hrs}h ${mins}m`;
  return `${mins}m`;
}

export default function CourseInfoPage() {
  const course = useLoaderData();
  const { id } = useParams();
  const navigate = useNavigate();
  const auth = useAuth();

  if (!course) {
    return (
      <main className="cp-page">
        <div className="cp-card">
          <h2 className="cp-missing">Course not found</h2>
          <p className="cp-desc">
            We could not find the course you were looking for.
          </p>
          <div className="cp-actions">
            <Link to="/courses" className="cp-back">
              Back to courses
            </Link>
          </div>
        </div>
      </main>
    );
  }

  return (
    <main className="cp-page">
      <article className="cp-card">
        <div className="cp-media-wrap">
          <img src={course.imageUrl} alt={course.title} className="cp-media" />
        </div>

        <div className="cp-body">
          <h1 className="cp-title">{course.title}</h1>
          <div className="cp-meta">
            <span className="cp-instructor">{course.instructor}</span>
            <span className="cp-sep">·</span>
            <span className="cp-rating">
              <Stars value={course.rating} />
            </span>
            <span className="cp-sep">·</span>
            <span className="cp-students">
              {course.studentsEnrolled} students
            </span>
          </div>

          <p className="cp-desc">{course.description}</p>

          <div className="cp-footer">
            <small className="cp-duration">
              Duration: {formatDuration(course.durationInMinutes)}
            </small>
            <div className="cp-actions">
              {!course.enrolled && auth.user && (
                <button
                  onClick={() => {
                    enrollToCourse(id);
                    navigate("/my-courses");
                  }}
                  className="cp-enroll"
                >
                  Enroll now
                </button>
              )}
              {course.enrolled && auth.user && (
                <button
                  onClick={() => {
                    navigate("/my-courses/" + id);
                    fetchEnrolledCourseDetails(id);
                  }}
                  className="cp-enroll cp-enrolled"
                >
                  Start Learning
                </button>
              )}
              <Link to="/courses" className="cp-back">
                Back to courses
              </Link>
            </div>
          </div>
        </div>
      </article>
    </main>
  );
}
