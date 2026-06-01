import React, { useState, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import FormField from "./FormField";
import { useAuth } from "../hooks/useAuth";

// Accept both `onSubmit` (backwards compat) and `onSuccess` (clearer name)
export default function SignInForm({ onSubmit, onSuccess } = {}) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [touched, setTouched] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState("");
  const navigate = useNavigate();
  const auth = useAuth();

  const errors = useMemo(
    () => ({
      email: !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)
        ? "Please enter a valid email."
        : "",
      password:
        password.length < 6 ? "Password must be at least 6 characters." : "",
    }),
    [email, password],
  );

  const isValid = !errors.email && !errors.password;

  async function handleSubmit(e) {
    e.preventDefault();
    setTouched({ email: true, password: true });
    setApiError("");
    if (!isValid) return;

    setIsLoading(true);
    try {
      const data = await auth.login({ email: email.trim(), password });

      // call parent callbacks (if provided)
      if (typeof onSubmit === "function") onSubmit(data);
      if (typeof onSuccess === "function") onSuccess(data);

      // Navigate to home or dashboard after successful login
      navigate("/");
    } catch (error) {
      setApiError(error.message || "An error occurred during login");
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <form className="signin-card" onSubmit={handleSubmit} noValidate>
      <h1 className="signin-title">Sign In</h1>

      {apiError && <div className="error-message">{apiError}</div>}

      <FormField
        label="Email"
        type="email"
        name="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        onBlur={() => setTouched((t) => ({ ...t, email: true }))}
        error={touched.email ? errors.email : ""}
        required
      />

      <FormField
        label="Password"
        type="password"
        name="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        onBlur={() => setTouched((t) => ({ ...t, password: true }))}
        error={touched.password ? errors.password : ""}
        required
      />

      <div className="actions">
        <button
          type="submit"
          className="btn-submit"
          disabled={!isValid || isLoading}
        >
          {isLoading ? "Signing In..." : "Sign In"}
        </button>
      </div>
    </form>
  );
}
