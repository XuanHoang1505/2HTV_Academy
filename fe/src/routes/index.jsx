import { createBrowserRouter } from "react-router-dom";
import MainLayout from "../layouts/MainLayout";
import HomePage from "../pages/site/Home";
import CoursesPage from "../pages/site/Courses";
import AboutPage from "../pages/site/About";
import ContactPage from "../pages/site/Contact";
import LoginPage from "../pages/site/Login";
import RegisterPage from "../pages/site/Register";
import VerifyEmailPage from "../pages/site/VerifyEmail";
import AdminLayout from "../layouts/AdminLayout";
import DashboardAdminPage from "../pages/admin/Dashboard";
import CourseAdminPage from "../pages/admin/Course";
import OrderAdminPage from "../pages/admin/Order";
import StudentAdminPage from "../pages/admin/Student";
import CategoryAdminPage from "../pages/admin/Category";
import UserAdminPage from "../pages/admin/User";
import CourseDetailPage from "../pages/site/CourseDetail";
import EnrollmentPage from "../pages/site/Enrollment";
import EnrollmentDetailPage from "../pages/site/EnrollmentDetail";
import ProfilePage from "../pages/site/Profile";
import CourseDetailAdminPage from "../pages/admin/Course/CourseDetail";
import ProtectedRoute from "./ProtectedRoute";

const router = createBrowserRouter([
  {
    path: "/",
    element: <MainLayout />,
    children: [
      { index: true, element: <HomePage /> },
      { path: "khoa-hoc", element: <CoursesPage /> },
      { path: "khoa-hoc/:slug", element: <CourseDetailPage /> },
      {
        path: "khoa-hoc-cua-toi",
        element: (
          <ProtectedRoute requiredRole="Student">
            <EnrollmentPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "hoc-tap/:slug",
        element: (
          <ProtectedRoute requiredRole="Student">
            <EnrollmentDetailPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "ho-so",
        element: (
          <ProtectedRoute requiredRole="Student">
            <ProfilePage />
          </ProtectedRoute>
        ),
      },
      { path: "gioi-thieu", element: <AboutPage /> },
      { path: "lien-he", element: <ContactPage /> },
    ],
  },
  {
    path: "/admin",
    element: <AdminLayout />,

    children: [
      { index: true, element: <DashboardAdminPage /> },
      { path: "danh-muc-khoa-hoc", element: <CategoryAdminPage /> },
      { path: "khoa-hoc", element: <CourseAdminPage /> },
      { path: "khoa-hoc/:id", element: <CourseDetailAdminPage /> },
      { path: "don-hang", element: <OrderAdminPage /> },
      { path: "hoc-vien", element: <StudentAdminPage /> },
      { path: "nguoi-dung", element: <UserAdminPage /> },
    ],
  },
  {
    path: "/dang-nhap",
    element: <LoginPage />,
  },
  {
    path: "/dang-ky-tai-khoan",
    element: <RegisterPage />,
  },
  {
    path: "/xac-thuc-tai-khoan",
    element: <VerifyEmailPage />,
  },
]);

export default router;
