import axios from "../../customs/axios.customize";

const orderFromCartService = async (userId, amount, courseIds) => {
  const URL_BACKEND = "/payment/create-payment";

  const response = await axios.post(URL_BACKEND, {
    userId,
    amount,
    courseIds,
  });

  return response;
};

export { orderFromCartService };
