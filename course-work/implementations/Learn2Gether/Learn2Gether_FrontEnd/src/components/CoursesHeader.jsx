export default function CoursesHeader({ page, totalPages, go }) {
  return (
    <header className="courses-header">
      <h1 className="courses-title">All Courses</h1>
      <div className="courses-pager-top">
        <button
          className="pager-btn"
          onClick={() => go(page - 1)}
          disabled={page === 1}
          aria-label="Previous page"
        >
          ‹ Prev
        </button>
        <div className="pager-info">
          Page {page} of {totalPages}
        </div>
        <button
          className="pager-btn"
          onClick={() => go(page + 1)}
          disabled={page === totalPages}
          aria-label="Next page"
        >
          Next ›
        </button>
      </div>
    </header>
  );
}
