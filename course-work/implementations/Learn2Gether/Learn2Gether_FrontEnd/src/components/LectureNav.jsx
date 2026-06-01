export default function LectureNav({ section, setSection }) {
  return (
    <nav className="video-nav">
      <button
        className={section === "qa" ? "active" : ""}
        onClick={() => setSection("qa")}
      >
        Q/A
      </button>
      <button
        className={section === "notes" ? "active" : ""}
        onClick={() => setSection("notes")}
      >
        Notes
      </button>
      <button
        className={section === "info" ? "active" : ""}
        onClick={() => setSection("info")}
      >
        Course Info
      </button>
    </nav>
  );
}
