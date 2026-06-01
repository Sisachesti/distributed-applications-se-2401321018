import React, { useState } from "react";
import { useLoaderData, useNavigate, useRevalidator } from "react-router-dom";
import AddNewCourseModal from "../components/AddNewCourseModal";
import EditCourseModal from "../components/EditCourseModal";
import { deleteCourse } from "../services/courseService";
import "../styles/InstructorManagementPage.css";

export default function InstructorManagementPage() {
  const courses = useLoaderData();
  const navigate = useNavigate();
  const revalidator = useRevalidator();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedCourse, setSelectedCourse] = useState(null);

  const handleCourseClick = (courseId) => {
    navigate(`/instructor/${courseId}`);
  };

  const handleCreateCourse = () => {
    setIsModalOpen(true);
  };

  const handleCourseAdded = () => {
    revalidator.revalidate();
  };

  const handleCourseEdited = () => {
    revalidator.revalidate();
  };

  const handleEditClick = (e, course) => {
    e.stopPropagation();
    setSelectedCourse(course);
    setIsEditModalOpen(true);
  };

  const handleDeleteClick = async (e, courseId) => {
    e.stopPropagation();

    if (
      window.confirm(
        "Are you sure you want to delete this course? This action cannot be undone.",
      )
    ) {
      try {
        await deleteCourse(courseId);
        revalidator.revalidate();
      } catch (error) {
        alert(error.message || "Failed to delete course");
      }
    }
  };

  return (
    <main className="instructor-management-page">
      <div className="instructor-header">
        <h1>My Courses</h1>
        <button className="create-course-btn" onClick={handleCreateCourse}>
          + Create New Course
        </button>
      </div>

      {courses.length === 0 ? (
        <div className="no-courses-message">
          <h2>You haven't created any courses yet</h2>
          <p>Start sharing your knowledge by creating your first course!</p>
          <button
            className="create-course-btn-large"
            onClick={handleCreateCourse}
          >
            Create Your First Course
          </button>
        </div>
      ) : (
        <div className="courses-grid">
          {courses.map((course) => (
            <div
              key={course.id}
              className="course-card"
              onClick={() => handleCourseClick(course.id)}
            >
              <div className="course-image">
                <img src={course.imageUrl} alt={course.title} />
                <div className="course-overlay">
                  <span className="view-details">View Details</span>
                </div>
              </div>
              <div className="course-info">
                <h3 className="course-title">{course.title}</h3>
                <div className="course-stats">
                  <div className="stat-item">
                    <span className="stat-icon">👥</span>
                    <span className="stat-value">
                      {course.students} students
                    </span>
                  </div>
                </div>
                <div className="course-actions">
                  <button
                    className="edit-btn"
                    onClick={(e) => handleEditClick(e, course)}
                  >
                    Edit
                  </button>
                  <button
                    className="delete-btn"
                    onClick={(e) => handleDeleteClick(e, course.id)}
                  >
                    Delete
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <AddNewCourseModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onCourseAdded={handleCourseAdded}
      />

      <EditCourseModal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        onCourseEdited={handleCourseEdited}
        course={selectedCourse}
      />
    </main>
  );
}
