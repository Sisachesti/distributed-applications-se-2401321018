import { createBrowserRouter, RouterProvider } from "react-router-dom";
import RootLayout from "./pages/RootLayout";
import WelcomePage from "./pages/WelcomePage.jsx";
import SignInPage from "./pages/SignInPage.jsx";
import SignUpPage from "./pages/SignUpPage.jsx";
import CoursesPage from "./pages/CoursesPage.jsx";
import ErrorPage from "./pages/ErrorPage.jsx";
import CourseInfoPage from "./pages/CourseInfoPage.jsx";
import CourseEnrolledPage from "./pages/CourseEnrolledPage.jsx";
import ProfilePage from "./pages/ProfilePage.jsx";
import MyCoursesPage from "./pages/MyCoursesPage.jsx";
import WishlistPage from "./pages/WishlistPage.jsx";
import AdminManagementPage from "./pages/AdminManagementPage.jsx";
import InstructorManagementPage from "./pages/InstructorManagementPage.jsx";
import InstructorCourseManagementPage from "./pages/InstructorCourseManagementPage.jsx";
import { AuthProvider } from "./context/AuthContext.jsx";

import { coursesLoader } from "./loaders/coursesLoader.js";
import { courseDetailsLoader } from "./loaders/courseDetailsLoader.js";
import { myCoursesLoader } from "./loaders/myCoursesLoader.js";
import { wishlistCoursesLoader } from "./loaders/wishlistCoursesLoader.js";
import { instructorManagementLoader } from "./loaders/instructorManagementLoader.js";
import { instructorCourseDetailsLoader } from "./loaders/instructorCourseDetailsLoader.js";
import { adminManagementLoader } from "./loaders/adminManagementLoader.js";
import { welcomePageLoader } from "./loaders/welcomePageLoader.js";

function App() {
  const router = createBrowserRouter([
    {
      path: "/",
      element: <RootLayout />,
      errorElement: <ErrorPage />,
      children: [
        { index: true, element: <WelcomePage />, loader: welcomePageLoader },
        { path: "courses", element: <CoursesPage />, loader: coursesLoader },
        { path: "login", element: <SignInPage /> },
        { path: "sign-up", element: <SignUpPage /> },
        {
          path: "courses/:id",
          element: <CourseInfoPage />,
          loader: courseDetailsLoader,
        },
        {
          path: "my-courses/:id/:lectureId",
          element: <CourseEnrolledPage />,
        },
        {
          path: "my-courses/:id",
          element: <CourseEnrolledPage />,
        },
        { path: "profile", element: <ProfilePage /> },
        {
          path: "my-courses",
          element: <MyCoursesPage />,
          loader: myCoursesLoader,
        },
        {
          path: "wishlist",
          element: <WishlistPage />,
          loader: wishlistCoursesLoader,
        },
        {
          path: "admin",
          element: <AdminManagementPage />,
          loader: adminManagementLoader,
        },
        {
          path: "instructor",
          element: <InstructorManagementPage />,
          loader: instructorManagementLoader,
        },
        {
          path: "instructor/:courseId",
          element: <InstructorCourseManagementPage />,
          loader: instructorCourseDetailsLoader,
        },
        { path: "*", element: <ErrorPage /> },
      ],
    },
  ]);

  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  );
}

export default App;
