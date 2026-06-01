import React from "react";
import "../styles/SignInPage.css";
import SignInForm from "../components/SignInForm";

export default function SignInPage({ onSubmit } = {}) {
  return (
    <main className="signin-page">
      <SignInForm onSubmit={onSubmit} />
    </main>
  );
}
