export default function CoursesFooter({ page, totalPages, go }) {
  return (
    <footer className="courses-footer">
      <div className="pager-controls">
        <button
          className="pager-btn"
          onClick={() => go(1)}
          disabled={page === 1}
        >
          First
        </button>
        <button
          className="pager-btn"
          onClick={() => go(page - 1)}
          disabled={page === 1}
        >
          Prev
        </button>

        {/* numeric pages (show up to 7 pages centered) */}
        {Array.from({ length: totalPages }).map((_, idx) => {
          const p = idx + 1;
          // keep page buttons to a reasonable number
          const tooFar = Math.abs(p - page) > 3 && p !== 1 && p !== totalPages;
          if (tooFar) {
            // show first/last and nearby pages only
            return null;
          }
          return (
            <button
              key={p}
              className={"pager-page" + (p === page ? " active" : "")}
              onClick={() => go(p)}
              aria-current={p === page ? "page" : undefined}
            >
              {p}
            </button>
          );
        })}

        <button
          className="pager-btn"
          onClick={() => go(page + 1)}
          disabled={page === totalPages}
        >
          Next
        </button>
        <button
          className="pager-btn"
          onClick={() => go(totalPages)}
          disabled={page === totalPages}
        >
          Last
        </button>
      </div>
    </footer>
  );
}
