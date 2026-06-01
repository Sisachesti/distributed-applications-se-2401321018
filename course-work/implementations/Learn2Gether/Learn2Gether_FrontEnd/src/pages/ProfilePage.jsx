import React from "react";
import "../styles/Profile.css";

export default function ProfilePage() {
  const user = {
    name: "John Doe",
    email: "john.doe@example.com",
  };

  const courses = [
    { id: 1, title: "React Basics" },
    { id: 2, title: "Advanced JavaScript" },
  ];

  return (
    <div className="profile-container">
      <div className="profile-card">
        <div className="profile-header">
          <div className="avatar">
            {user.name
              .split(" ")
              .map((n) => n[0])
              .slice(0, 2)
              .join("")}
          </div>
          <div className="user-info">
            <h2 className="user-name">{user.name}</h2>
            <p className="user-email">{user.email}</p>
            <div className="actions">
              <button className="edit-btn">Edit Profile</button>
            </div>
          </div>
        </div>

        <div className="profile-body">
          <h3>Enrolled Courses</h3>
          <ul className="courses-list">
            {courses.map((c) => (
              <li key={c.id} className="course-item">
                {c.title}
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}
