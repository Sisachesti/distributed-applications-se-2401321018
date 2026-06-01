import React, { useEffect, useRef, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  fetchEnrolledCourseDetails,
  fetchWatchLecture,
} from "../services/courseService.js";
import "../styles/CourseEnrolledPage.css";
import NotesSection from "../components/NotesSection.jsx";
import LectureNav from "../components/LectureNav.jsx";
import QuestionsAnswersSection from "../components/QuestionsAnswersSection.jsx";
import { useAuth } from "../hooks/useAuth";

const CourseEnrolledPage = () => {
  const { user } = useAuth();
  const { id, lectureId } = useParams();
  const navigate = useNavigate();
  const [course, setCourse] = useState(null);
  const [currentLecture, setCurrentLecture] = useState(null);
  const [expandedModules, setExpandedModules] = useState({});
  const [section, setSection] = useState();
  const [qaComments, setQaComments] = useState([]); // Questions with array of answers inside
  const [notes, setNotes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const videoRef = useRef(null);

  const currentUserId = user?.username;

  // Fetch course data and lecture data
  useEffect(() => {
    const loadCourseData = async () => {
      try {
        setLoading(true);
        setError(null);

        if (lectureId) {
          // Fetch specific lecture details
          const lectureData = await fetchWatchLecture(id, lectureId);
          setCourse(lectureData.courseDetails);

          const lecture = lectureData.courseDetails.modules
            .flatMap((m) => m.lectures)
            .find((lec) => lec.lectureId === lectureId);
          setCurrentLecture(lecture || null);

          const questionsAnswers = lectureData.questionsAnswers || [];
          setQaComments(questionsAnswers);

          const savedNotes = lectureData.notes || [];
          setNotes(savedNotes);
        } else {
          // Fetch course overview (modules and lectures list)
          const courseData = await fetchEnrolledCourseDetails(id);
          setCourse(courseData);

          // Navigate to first lecture if available
          if (
            courseData.modules?.length > 0 &&
            courseData.modules[0].lectures?.length > 0
          ) {
            const firstLecture = courseData.modules[0].lectures[0];
            navigate(`/my-courses/${id}/${firstLecture.lectureId}`, {
              replace: true,
            });
            return;
          }
        }
      } catch (err) {
        console.error("Failed to load course data:", err);
        setError(err.message || "Failed to load course data");
      } finally {
        setLoading(false);
      }
    };

    loadCourseData();
  }, [id, lectureId, navigate]);

  const handleNoteSaved = async () => {
    // Refresh the lecture data to get updated notes
    try {
      const lectureData = await fetchWatchLecture(id, lectureId);
      const savedNotes = lectureData.notes || [];
      setNotes(savedNotes);
    } catch (err) {
      console.error("Failed to refresh notes:", err);
    }
  };

  const handleQuestionSaved = async () => {
    // Refresh the lecture data to get updated questions and answers
    try {
      const lectureData = await fetchWatchLecture(id, lectureId);
      const questionsAnswers = lectureData.questionsAnswers || [];
      setQaComments(questionsAnswers);
    } catch (err) {
      console.error("Failed to refresh questions:", err);
    }
  };

  const toggleModule = (moduleId) => {
    setExpandedModules((prev) => ({
      ...prev,
      [moduleId]: !prev[moduleId],
    }));
  };

  const selectLecture = (lecture) => {
    navigate(`/my-courses/${id}/${lecture.lectureId}`);
  };

  const getTotalLectures = () => {
    if (!course?.modules) return 0;
    return course.modules.reduce((sum, mod) => sum + mod.lectures.length, 0);
  };

  if (loading) {
    return (
      <div className="course-enrolled-page">
        <div className="loading-state">Loading course data...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="course-enrolled-page">
        <div className="error-state">
          <h3>Error Loading Course</h3>
          <p>{error}</p>
          <button onClick={() => navigate("/my-courses")}>
            Back to My Courses
          </button>
        </div>
      </div>
    );
  }

  if (!course) {
    return (
      <div className="course-enrolled-page">
        <div className="error-state">
          <h3>Course Not Found</h3>
          <button onClick={() => navigate("/my-courses")}>
            Back to My Courses
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="course-enrolled-page">
      <div className="modules-sidebar">
        <h3 className="sidebar-title">Course Content</h3>
        <div className="modules-list">
          {course?.modules?.map((module) => (
            <div key={module.moduleId} className="module-item">
              <div
                className="module-header"
                onClick={() => toggleModule(module.moduleId)}
              >
                <span className="module-toggle">
                  {expandedModules[module.moduleId] ? "▼" : "►"}
                </span>
                <span className="module-title">{module.title}</span>
                <span className="module-count">{module.lectures.length}</span>
              </div>
              {expandedModules[module.moduleId] && (
                <ul className="lectures-list">
                  {module.lectures.map((lecture) => (
                    <li
                      key={lecture.lectureId}
                      className={`lecture-item ${
                        currentLecture?.lectureId === lecture.lectureId
                          ? "active"
                          : ""
                      }`}
                      onClick={() => selectLecture(lecture)}
                    >
                      <span className="lecture-title">{lecture.title}</span>
                      <span className="lecture-duration">
                        {lecture.duration}
                      </span>
                    </li>
                  ))}
                </ul>
              )}
            </div>
          ))}
        </div>
      </div>

      <div className="main-content">
        <div className="video-area" ref={videoRef}>
          {currentLecture && course && (
            <>
              <h2 className="course-title">{course.title}</h2>
              <h3 className="lecture-title-display">{currentLecture.title}</h3>
              <div className="video-player-wrapper">
                <video
                  className="main-video"
                  controls
                  src={`Learn2Gether_FrontEnd/public/videos/${currentLecture.videoFile}`}
                  type="video/mp4"
                  key={currentLecture.id}
                />
              </div>
              <div className="video-meta">
                Instructor: {course.instructor} • Duration: {course.duration} •
                Lessons: {getTotalLectures()}
              </div>
            </>
          )}
        </div>

        <LectureNav course={course} section={section} setSection={setSection} />

        <section className="section-content">
          {section === "qa" && (
            <QuestionsAnswersSection
              qaComments={qaComments}
              courseId={id}
              lectureId={lectureId}
              onQuestionSaved={handleQuestionSaved}
              currentUserId={currentUserId}
            />
          )}

          {section === "notes" && (
            <NotesSection
              notes={notes}
              courseId={id}
              lectureId={lectureId}
              onNoteSaved={handleNoteSaved}
            />
          )}

          {section === "info" && course && (
            <div className="info-section">
              <h3>Course Info</h3>
              <p className="course-desc">{course.description}</p>
              <ul className="course-meta-list">
                <li>
                  <strong>Instructor:</strong> {course.instructor}
                </li>
                <li>
                  <strong>Modules:</strong> {course.modules?.length || 0}
                </li>
                <li>
                  <strong>Lectures:</strong> {getTotalLectures()}
                </li>
              </ul>
            </div>
          )}
        </section>
      </div>
    </div>
  );
};

export default CourseEnrolledPage;
