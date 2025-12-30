import axios from "../../customs/axios.customize";

const getEnrollmentService = async () => {
  const URL_BACKEND = "/enrollments/my-enrollments";

  const response = await axios.get(URL_BACKEND);

  return response;
};

const getEnrollmentByIdService = async (enrollmentId) => {
  const URL_BACKEND = `/enrollments/${enrollmentId}`;

  const response = await axios.get(URL_BACKEND);

  return response;
};

export { getEnrollmentService, getEnrollmentByIdService };
