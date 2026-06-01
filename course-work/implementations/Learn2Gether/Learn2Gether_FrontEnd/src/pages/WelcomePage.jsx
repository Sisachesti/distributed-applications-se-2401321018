import TopCourses from "../components/TopCourses";
import Welcome from "../components/Welcome";
import "../styles/WelcomePage.css";

export default function WelcomePage() {
  return (
    <section className="welcome-page">
      <Welcome />
      <TopCourses />
    </section>
  );
}
