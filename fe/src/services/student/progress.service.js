import axios from "../../customs/axios.customize";

const updateEnrollmentProgress = async (enrollmentId, lectureId) => {
  const URL_BACKEND = `/progresses?enrollmentId=${enrollmentId}&lectureId=${lectureId}`;

  const response = await axios.post(URL_BACKEND);

  return response;
};

export { updateEnrollmentProgress };