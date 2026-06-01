import { NavLink, useNavigate } from "react-router-dom";
import "../styles/Navbar.css";
import { useAuth } from "../hooks/useAuth";

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const roles = user?.roles || [];
  const isStudent = roles.includes("Student");
  const isAdmin = roles.includes("Admin");
  const isInstructor = roles.includes("Instructor");

  async function handleLogout() {
    try {
      await logout();
      navigate("/login", { replace: true });
    } catch (err) {
      console.error("Logout failed", err);
    }
  }

  return (
    <nav className="navbar">
      <NavLink to="/" className="nav-logo">
        <img
          src="/favicon-64x64.png"
          alt="Learn2Gether"
          className="nav-logo-img"
        />
        Learn2Gether
      </NavLink>
      <ul className="nav-links">
        {isStudent && (
          <div className="nav-links-group">
            <li className="nav-item">
              <NavLink
                to="/my-courses"
                className={({ isActive }) =>
                  "nav-link" + (isActive ? " active" : "")
                }
              >
                My Courses
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink
                to="/wishlist"
                className={({ isActive }) =>
                  "nav-link nav-wishlist" + (isActive ? " active" : "")
                }
              >
                Wishlist
              </NavLink>
            </li>
          </div>
        )}
        {isInstructor && (
          <li className="nav-item">
            <NavLink
              to="/instructor"
              className={({ isActive }) =>
                "nav-link nav-signup" + (isActive ? " active" : "")
              }
            >
              Manage Your Courses
            </NavLink>
          </li>
        )}
        <li className="nav-item">
          <NavLink
            to="/courses"
            className={({ isActive }) =>
              "nav-link" + (isActive ? " active" : "")
            }
          >
            All Courses
          </NavLink>
        </li>

        {!user ? (
          <>
            <li className="nav-item">
              <NavLink
                to="/login"
                className={({ isActive }) =>
                  "nav-link nav-login" + (isActive ? " active" : "")
                }
              >
                Sign In
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink
                to="/sign-up"
                className={({ isActive }) =>
                  "nav-link nav-signup" + (isActive ? " active" : "")
                }
              >
                Sign Up
              </NavLink>
            </li>
          </>
        ) : (
          <>
            {isAdmin && (
              <li className="nav-item">
                <NavLink
                  to="/admin"
                  className={({ isActive }) =>
                    "nav-link nav-admin" + (isActive ? " active" : "")
                  }
                >
                  Admin
                </NavLink>
              </li>
            )}
            <li className="nav-item">
              <button className="nav-link" onClick={handleLogout}>
                Sign Out
              </button>
            </li>
          </>
        )}
      </ul>
    </nav>
  );
}
