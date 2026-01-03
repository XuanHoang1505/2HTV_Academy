import axios from "axios";

const getCourseReviews = async (courseId) => {
  const URL_BACKEND = `/reviews/course/${courseId}`;
  const response = await axios.get(URL_BACKEND);

  return response;
};

export { getCourseReviews };
