import React, { useState } from "react";
import { useLoaderData, useParams, useRevalidator } from "react-router-dom";
import { addModule, editModule, deleteModule } from "../services/moduleService";
import {
  addLecture,
  updateLecture,
  deleteLecture,
} from "../services/lectureService";
import "../styles/InstructorCourseManagementPage.css";

export default function InstructorCourseManagementPage() {
  const courseId = useParams();
  const course = useLoaderData();
  const revalidator = useRevalidator();

  const [viewMode, setViewMode] = useState("overview"); // 'overview', 'add-module', 'edit-module', 'add-lecture', 'edit-lecture'
  const [selectedModule, setSelectedModule] = useState(null);
  const [selectedLecture, setSelectedLecture] = useState(null);
  const [formData, setFormData] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState(null);

  const handleAddModule = () => {
    setFormData({ title: "" });
    setViewMode("add-module");
    setError(null);
  };

  const handleEditModule = (module) => {
    setSelectedModule(module);
    setFormData({ title: module.title, moduleId: module.moduleId });
    setViewMode("edit-module");
    setError(null);
  };

  const handleDeleteModule = async (moduleId) => {
    if (
      window.confirm(
        "Are you sure you want to delete this module? All lectures in this module will also be deleted.",
      )
    ) {
      try {
        await deleteModule(moduleId);
        revalidator.revalidate();
      } catch (err) {
        alert(err.message || "Failed to delete module");
      }
    }
  };

  const handleAddLecture = (moduleId) => {
    setSelectedModule(course.modules.find((m) => m.moduleId === moduleId));
    setFormData({ title: "", videoUrl: "", moduleId });
    setViewMode("add-lecture");
    setError(null);
  };

  const handleEditLecture = (lecture, moduleId) => {
    setSelectedLecture(lecture);
    setFormData({
      title: lecture.title,
      videoUrl: lecture.videoFile || "",
      lectureId: lecture.lectureId,
    });
    setViewMode("edit-lecture");
    setError(null);
  };

  const handleDeleteLecture = async (lectureId) => {
    if (window.confirm("Are you sure you want to delete this lecture?")) {
      try {
        await deleteLecture(lectureId);
        revalidator.revalidate();
      } catch (err) {
        alert(err.message || "Failed to delete lecture");
      }
    }
  };

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      if (viewMode === "add-module") {
        await addModule({ courseId: courseId.courseId, title: formData.title });
      } else if (viewMode === "edit-module") {
        await editModule({
          moduleId: formData.moduleId,
          title: formData.title,
        });
      } else if (viewMode === "add-lecture") {
        await addLecture({
          moduleId: formData.moduleId,
          title: formData.title,
          videoUrl: formData.videoUrl,
        });
      } else if (viewMode === "edit-lecture") {
        await updateLecture({
          lectureId: formData.lectureId,
          title: formData.title,
          videoUrl: formData.videoUrl,
        });
      }

      revalidator.revalidate();
      setViewMode("overview");
      setFormData({});
    } catch (err) {
      setError(err.message || "Operation failed");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCancel = () => {
    setViewMode("overview");
    setFormData({});
    setError(null);
  };

  const renderForm = () => {
    let title = "";
    let fields = [];

    if (viewMode === "add-module") {
      title = "Add New Module";
      fields = [
        { name: "title", label: "Module Title", type: "text", required: true },
      ];
    } else if (viewMode === "edit-module") {
      title = "Edit Module";
      fields = [
        { name: "title", label: "Module Title", type: "text", required: true },
      ];
    } else if (viewMode === "add-lecture") {
      title = `Add Lecture to "${selectedModule?.title}"`;
      fields = [
        { name: "title", label: "Lecture Title", type: "text", required: true },
        { name: "videoUrl", label: "Video URL", type: "url", required: false },
      ];
    } else if (viewMode === "edit-lecture") {
      title = "Edit Lecture";
      fields = [
        { name: "title", label: "Lecture Title", type: "text", required: true },
        { name: "videoUrl", label: "Video URL", type: "url", required: false },
      ];
    }

    return (
      <div className="form-container">
        <h2>{title}</h2>
        {error && <div className="error-message">{error}</div>}

        <form onSubmit={handleFormSubmit} className="management-form">
          {fields.map((field) => (
            <div key={field.name} className="form-group">
              <label htmlFor={field.name}>
                {field.label} {field.required && "*"}
              </label>
              <input
                type={field.type}
                id={field.name}
                name={field.name}
                value={formData[field.name] || ""}
                onChange={handleFormChange}
                required={field.required}
                disabled={isSubmitting}
                placeholder={`Enter ${field.label.toLowerCase()}`}
              />
            </div>
          ))}

          <div className="form-actions">
            <button
              type="button"
              onClick={handleCancel}
              disabled={isSubmitting}
            >
              Cancel
            </button>
            <button type="submit" disabled={isSubmitting}>
              {isSubmitting ? "Saving..." : "Save"}
            </button>
          </div>
        </form>
      </div>
    );
  };

  return (
    <div className="instructor-course-management-page">
      <div className="management-sidebar">
        <div className="sidebar-header">
          <h3>Course Content</h3>
          <button className="add-module-btn" onClick={handleAddModule}>
            + Add Module
          </button>
        </div>

        <div className="modules-list">
          {course?.modules?.map((module) => (
            <div key={module.moduleId} className="module-item">
              <div className="module-header">
                <span className="module-title">{module.title}</span>
                <div className="module-actions">
                  <button
                    className="action-btn add-btn"
                    onClick={() => handleAddLecture(module.moduleId)}
                    title="Add Lecture"
                  >
                    +
                  </button>
                  <button
                    className="action-btn edit-btn"
                    onClick={() => handleEditModule(module)}
                    title="Edit Module"
                  >
                    ✏️
                  </button>
                  <button
                    className="action-btn delete-btn"
                    onClick={() => handleDeleteModule(module.moduleId)}
                    title="Delete Module"
                  >
                    ✕
                  </button>
                </div>
              </div>

              <ul className="lectures-list">
                {module.lectures?.map((lecture) => (
                  <li key={lecture.lectureId} className="lecture-item">
                    <span className="lecture-title">{lecture.title}</span>
                    <div className="lecture-actions">
                      <button
                        className="action-btn edit-btn"
                        onClick={() =>
                          handleEditLecture(lecture, module.moduleId)
                        }
                        title="Edit Lecture"
                      >
                        ✏️
                      </button>
                      <button
                        className="action-btn delete-btn"
                        onClick={() => handleDeleteLecture(lecture.lectureId)}
                        title="Delete Lecture"
                      >
                        ✕
                      </button>
                    </div>
                  </li>
                ))}
                {(!module.lectures || module.lectures.length === 0) && (
                  <li className="empty-lectures">No lectures yet</li>
                )}
              </ul>
            </div>
          ))}

          {(!course?.modules || course.modules.length === 0) && (
            <div className="empty-state">
              <p>No modules yet. Click "+ Add Module" to get started.</p>
            </div>
          )}
        </div>
      </div>

      <div className="management-content">
        {viewMode === "overview" ? (
          <div className="overview-section">
            <h2>{course.title}</h2>
            <p className="course-description">{course.description}</p>
            <div className="course-stats">
              <div className="stat">
                <strong>Modules:</strong> {course.modules?.length || 0}
              </div>
              <div className="stat">
                <strong>Total Lectures:</strong>{" "}
                {course.modules?.reduce(
                  (sum, m) => sum + (m.lectures?.length || 0),
                  0,
                ) || 0}
              </div>
            </div>
            <div className="help-text">
              <h3>Getting Started</h3>
              <p>Use the sidebar to manage your course content:</p>
              <ul>
                <li>
                  Click <strong>"+ Add Module"</strong> to create a new module
                </li>
                <li>
                  Click <strong>"+"</strong> next to a module to add lectures
                </li>
                <li>
                  Click <strong>"✏️"</strong> to edit modules or lectures
                </li>
                <li>
                  Click <strong>"✕"</strong> to delete modules or lectures
                </li>
              </ul>
            </div>
          </div>
        ) : (
          renderForm()
        )}
      </div>
    </div>
  );
}
