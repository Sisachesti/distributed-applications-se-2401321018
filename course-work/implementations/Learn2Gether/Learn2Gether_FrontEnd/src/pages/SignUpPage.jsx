import React from "react";
import "../styles/SignUpPage.css";
import SignUpForm from "../components/SignUpForm";

export default function SignUpPage({ onSubmit } = {}) {
  return (
    <main className="signup-page">
      <SignUpForm onSubmit={onSubmit} />
    </main>
  );
}
