import React, { createContext, useState, useEffect } from "react";

const API_BASE = "https://localhost:7234/api";

// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null); // only set on register/login
  const [loading, setLoading] = useState(true); // loading state for initial check

  useEffect(() => {
    refreshUser();
  }, []);

  async function login({ email, password }) {
    const res = await fetch(`${API_BASE}/User/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include", // important if backend sets HttpOnly cookie
      body: JSON.stringify({ email, password }),
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({ message: res.statusText }));
      throw new Error(err.message || "Login failed");
    }
    const data = await res.json(); // expect { userId, email, firstName, lastName, token, roles }
    setUser(data);
    return data;
  }

  async function register(payload) {
    const res = await fetch(`${API_BASE}/User/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(payload),
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({ message: res.statusText }));
      throw new Error(err.message || "Registration failed");
    }
    const data = await res.json(); // expect returned user DTO
    setUser(data);
    return data;
  }

  async function logout() {
    await fetch(`${API_BASE}/User/logout`, {
      method: "POST",
      credentials: "include",
    });
    setUser(null);
  }

  async function refreshUser() {
    try {
      const res = await fetch(`${API_BASE}/User/me`, {
        method: "GET",
        credentials: "include",
      });
      if (!res.ok) {
        setUser(null);
        return;
      }
      const data = await res.json();
      setUser(data);
    } catch {
      setUser(null);
    } finally {
      setLoading(false);
    }
  }

  // Re-hydrate user state from cookie on mount

  return (
    <AuthContext.Provider value={{ user, login, register, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}
