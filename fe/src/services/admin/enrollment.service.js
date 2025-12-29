import axios from "../../customs/axios.customize";

const getEnrollmentCourse = async (courseId) => {
  const URL_BACKEND = `/enrollments/course/${courseId}/students`;

  const response = axios.get(URL_BACKEND);

  return response;
};

export { getEnrollmentCourse };
