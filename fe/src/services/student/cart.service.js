import axios from "../../customs/axios.customize";

const getCardOfStudentService = async () => {
  const URL_BACKEND = "/carts";

  const response = await axios.get(URL_BACKEND);

  return response;
};

const addToCardService = async (courseId) => {
  const URL_BACKEND = "/carts/add";

  const response = await axios.post(URL_BACKEND, { courseId });

  return response;
};

const removeCourseFromCarte = async (courseId) => {
  const URL_BACKEND = `/carts/remove/${courseId}`;

  const response = await axios.delete(URL_BACKEND);

  return response;
};

export { getCardOfStudentService, addToCardService, removeCourseFromCarte };
