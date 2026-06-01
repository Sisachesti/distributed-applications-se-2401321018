import React from "react";
import { useLoaderData, useNavigate } from "react-router-dom";
import CoursesHeader from "../components/CoursesHeader";
import CoursesMainSection from "../components/CoursesMainSection";
import CoursesFooter from "../components/CoursesFooter";
import CoursesSearchBar from "../components/CoursesSearchBar";
import "../styles/CoursesPage.css";

export default function CoursesPage() {
  const data = useLoaderData();
  const navigate = useNavigate();

  const courses = data?.courses || [];
  const currentPage = data?.currentPage || 1;
  const totalPages = data?.totalPages || 1;
  const searchQuery = data?.searchQuery || "";

  const go = (newPage) => {
    if (newPage < 1 || newPage > totalPages) return;

    const params = new URLSearchParams();
    if (searchQuery) {
      params.set("search", searchQuery);
    }
    params.set("page", newPage.toString());

    navigate(`/courses?${params.toString()}`);
  };

  const handleSearch = (query) => {
    const params = new URLSearchParams();
    if (query) {
      params.set("search", query);
    }
    params.set("page", "1");

    navigate(`/courses?${params.toString()}`);
  };

  return (
    <main className="courses-page">
      <CoursesSearchBar onSearch={handleSearch} currentSearch={searchQuery} />
      <CoursesHeader page={currentPage} totalPages={totalPages} go={go} />
      {courses.length === 0 && (
        <div className="no-courses-message">
          <h2>No courses found</h2>
        </div>
      )}
      <CoursesMainSection current={courses} />
      <CoursesFooter page={currentPage} totalPages={totalPages} go={go} />
    </main>
  );
}
