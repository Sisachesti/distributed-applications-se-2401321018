import { useState } from "react";
import { useLoaderData } from "react-router-dom";
import "../styles/AdminManagementPage.css";
import { assignRoleToUser, removeRoleFromUser } from "../services/adminService";

const availableRoles = ["Student", "Instructor", "Admin"];

function AdminManagementPage() {
  const loadedUsers = useLoaderData();
  const [users, setUsers] = useState(loadedUsers);

  const handleAddRole = async (userId, roleToAdd) => {
    try {
      await assignRoleToUser(userId, roleToAdd);

      // Update local state optimistically
      setUsers(
        users.map((user) => {
          if (user.userId === userId) {
            if (!user.roles.includes(roleToAdd)) {
              return { ...user, roles: [...user.roles, roleToAdd] };
            }
          }
          return user;
        }),
      );
    } catch (err) {
      alert(`Failed to add role: ${err.message}`);
      console.error("Error adding role:", err);
    }
  };

  const handleRemoveRole = async (userId, roleToRemove) => {
    try {
      const user = users.find((u) => u.userId === userId);

      // Ensure at least one role remains
      if (user && user.roles.length <= 1) {
        alert("User must have at least one role.");
        return;
      }

      await removeRoleFromUser(userId, roleToRemove);

      // Update local state optimistically
      setUsers(
        users.map((user) => {
          if (user.userId === userId) {
            return {
              ...user,
              roles: user.roles.filter((role) => role !== roleToRemove),
            };
          }
          return user;
        }),
      );
    } catch (err) {
      alert(`Failed to remove role: ${err.message}`);
      console.error("Error removing role:", err);
    }
  };

  return (
    <div className="admin-management-page">
      <div className="admin-container">
        <h1 className="admin-title">Admin Management Dashboard</h1>
        <div className="users-section">
          <div className="section-header">
            <h2>Users ({users.length})</h2>
          </div>
          {users.length === 0 ? (
            <p className="no-data">No users found.</p>
          ) : (
            <div className="table-container">
              <table className="admin-table">
                <thead>
                  <tr>
                    <th>Username</th>
                    <th>Email</th>
                    <th>Roles</th>
                  </tr>
                </thead>
                <tbody>
                  {users.map((user) => (
                    <tr key={user.userId}>
                      <td className="username">{user.username}</td>
                      <td>{user.email}</td>
                      <td>
                        <div className="roles-container">
                          <div className="roles-tags">
                            {(user.roles || ["Student"]).map((role) => (
                              <span key={role} className="role-tag">
                                {role}
                                <button
                                  className="remove-role-btn"
                                  onClick={() =>
                                    handleRemoveRole(user.userId, role)
                                  }
                                  title={`Remove ${role} role`}
                                >
                                  ×
                                </button>
                              </span>
                            ))}
                          </div>
                          <select
                            className="add-role-select"
                            onChange={(e) => {
                              if (e.target.value) {
                                handleAddRole(user.userId, e.target.value);
                                e.target.value = "";
                              }
                            }}
                            defaultValue=""
                          >
                            <option value="" disabled>
                              + Add Role
                            </option>
                            {availableRoles
                              .filter(
                                (role) => !(user.roles || []).includes(role),
                              )
                              .map((role) => (
                                <option key={role} value={role}>
                                  {role}
                                </option>
                              ))}
                          </select>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
        )
      </div>
    </div>
  );
}

export default AdminManagementPage;
