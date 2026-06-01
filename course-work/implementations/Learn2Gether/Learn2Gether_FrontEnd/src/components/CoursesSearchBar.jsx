import { useState, useEffect } from "react";
import "../styles/CoursesSearchBar.css";

export default function CoursesSearchBar({ onSearch, currentSearch = "" }) {
  const [searchTerm, setSearchTerm] = useState(currentSearch);

  useEffect(() => {
    setSearchTerm(currentSearch);
  }, [currentSearch]);

  const handleSearch = () => {
    if (onSearch) {
      onSearch(searchTerm);
    }
  };

  const handleClear = () => {
    setSearchTerm("");
    if (onSearch) {
      onSearch("");
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      handleSearch();
    }
  };

  return (
    <div className="search-container">
      <input
        id="course-search-input"
        type="text"
        placeholder="Search courses by name or description..."
        className="search-input"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        onKeyPress={handleKeyPress}
      />
      <button className="action-btn search-btn" onClick={handleSearch}>
        Search
      </button>
      <button className="action-btn delete-btn" onClick={handleClear}>
        Clear
      </button>
    </div>
  );
}
