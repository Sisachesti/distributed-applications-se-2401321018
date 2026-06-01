import React from "react";
import { Link } from "react-router-dom";
import "../styles/ErrorPage.css";
import Navbar from "../components/Navbar";

export default function ErrorPage() {
  return (
    <main className="error-page">
      <Navbar />
      <div className="error-card">
        <div className="error-icon" aria-hidden="true">
          <svg
            className="error-icon__svg"
            width="84"
            height="84"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
            role="img"
            aria-hidden="true"
          >
            <circle cx="12" cy="12" r="10" fill="#bc4749" />
            <rect x="11" y="6.5" width="2" height="7" rx="1" fill="#fff" />
            <rect x="11" y="15.5" width="2" height="2" rx="1" fill="#fff" />
          </svg>
        </div>
        <h1 className="error-title">404</h1>
        <h2 className="error-sub">Page Not Found</h2>
        <p className="error-desc">
          Sorry — the page you are looking for does not exist or has been moved.
        </p>
        <div className="error-actions">
          <Link to="/" className="error-home">
            Go Home
          </Link>
        </div>
      </div>
    </main>
  );
}
