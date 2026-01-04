import axios from "../../customs/axios.customize";

const createReview = async (dataReview) => {
  const URL_BACKEND = "/reviews";
  const response = await axios.post(URL_BACKEND, dataReview);

  return response;
};

const updateReview = async (id, dataReview) => {
  const URL_BACKEND = `/reviews/${id}`;
  const response = await axios.put(URL_BACKEND, dataReview);

  return response;
};

export { createReview, updateReview };
