import React, { useState, useMemo, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import FormField from "./FormField";
import { useAuth } from "../hooks/useAuth";

export default function SignUpForm({ onSubmit } = {}) {
  const navigate = useNavigate();
  const auth = useAuth();
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [username, setUsername] = useState("");
  const [role, setRole] = useState(""); // 'student' | 'teacher'
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [verify, setVerify] = useState("");
  const [touched, setTouched] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  const errors = useMemo(
    () => {
      const pwPattern = /(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/;
      const pwValid = password.length >= 6 && pwPattern.test(password);

      return {
        firstName:
          firstName.trim().length >= 3
            ? ""
            : "First name must be at least 3 characters.",
        lastName:
          lastName.trim().length >= 3
            ? ""
            : "Last name must be at least 3 characters.",
        username:
          username.trim().length >= 6
            ? (email.trim() &&
              username.trim().toLowerCase() === email.trim().toLowerCase()
                ? "Username must be different from email."
                : "")
            : "Username must be at least 6 characters.",
        role:
          role === "student" || role === "instructor"
            ? ""
            : "Please select a role.",
        email: /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)
          ? ""
          : "Please enter a valid email.",
        password: pwValid
          ? ""
          : "Password must be at least 6 characters and include uppercase, lowercase, and a number.",
        verify: verify === password ? "" : "Passwords do not match.",
      };
    },
    [firstName, lastName, username, role, email, password, verify],
  );

  const isValid = Object.values(errors).every((v) => v === "");

  async function registerUser(payload) {
    setIsSubmitting(true);
    setError("");

    try {
      const data = await auth.register({
        firstName: payload.firstName,
        lastName: payload.lastName,
        username: payload.username,
        role: payload.role,
        email: payload.email,
        password: payload.password,
        verifyPassword: payload.password,
      });

      setSuccess(true);

      // Call parent onSubmit if provided
      if (typeof onSubmit === "function") {
        onSubmit(data);
      }

      // Navigate to home after successful registration
      navigate("/");
    } catch (err) {
      setError(err.message || "Failed to register. Please try again.");
      console.error("Registration error:", err);
    } finally {
      setIsSubmitting(false);
    }
  }

  useEffect(() => {
    if (success) {
      // Optional: Redirect after successful registration
      // window.location.href = "/dashboard";
      // or use React Router: navigate("/dashboard");
    }
  }, [success]);

  function handleSubmit(e) {
    e.preventDefault();
    // mark all touched so errors show
    setTouched({
      firstName: true,
      lastName: true,
      username: true,
      role: true,
      email: true,
      password: true,
      verify: true,
    });
    if (!isValid) return;

    const payload = {
      firstName: firstName.trim(),
      lastName: lastName.trim(),
      username: username.trim(),
      role,
      email: email.trim(),
      password,
    };

    registerUser(payload);
  }

  function handleReset() {
    setFirstName("");
    setLastName("");
    setUsername("");
    setRole("");
    setEmail("");
    setPassword("");
    setVerify("");
    setTouched({});
  }

  return (
    <form className="signup-card" onSubmit={handleSubmit} noValidate>
      <h1 className="signup-title">Create account</h1>

      {error && (
        <div
          className="error-message"
          style={{ color: "red", marginBottom: "1rem" }}
        >
          {error}
        </div>
      )}

      {success && (
        <div
          className="success-message"
          style={{ color: "green", marginBottom: "1rem" }}
        >
          Account created successfully!
        </div>
      )}

      <div className="row-2">
        <FormField
          label="First name"
          name="firstName"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          onBlur={() => setTouched((t) => ({ ...t, firstName: true }))}
          error={touched.firstName ? errors.firstName : ""}
          required
        />

        <FormField
          label="Last name"
          name="lastName"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          onBlur={() => setTouched((t) => ({ ...t, lastName: true }))}
          error={touched.lastName ? errors.lastName : ""}
          required
        />
      </div>

      <FormField
        label="Username"
        name="username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        onBlur={() => setTouched((t) => ({ ...t, username: true }))}
        error={touched.username ? errors.username : ""}
        required
      />

      <fieldset className="field radios">
        <legend className="label-text">You are</legend>
        <div className="radio-row">
          <label className="radio">
            <input
              type="radio"
              name="role"
              value="student"
              checked={role === "student"}
              onChange={() => setRole("student")}
              onBlur={() => setTouched((t) => ({ ...t, role: true }))}
            />
            Student
          </label>
          <label className="radio">
            <input
              type="radio"
              name="role"
              value="instructor"
              checked={role === "instructor"}
              onChange={() => setRole("instructor")}
              onBlur={() => setTouched((t) => ({ ...t, role: true }))}
            />
            Instructor
          </label>
        </div>
        {touched.role && errors.role && (
          <div className="field-error">{errors.role}</div>
        )}
      </fieldset>

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

      <FormField
        label="Verify password"
        type="password"
        name="verify"
        value={verify}
        onChange={(e) => setVerify(e.target.value)}
        onBlur={() => setTouched((t) => ({ ...t, verify: true }))}
        error={touched.verify ? errors.verify : ""}
        required
      />

      <div className="actions">
        <button
          type="button"
          className="btn-reset"
          onClick={handleReset}
          disabled={
            isSubmitting ||
            (!firstName &&
              !lastName &&
              !username &&
              !role &&
              !email &&
              !password &&
              !verify)
          }
        >
          Reset
        </button>
        <button
          type="submit"
          className="btn-submit"
          disabled={!isValid || isSubmitting}
        >
          {isSubmitting ? "Creating account..." : "Create account"}
        </button>
      </div>
    </form>
  );
}
