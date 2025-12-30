import axios from "../../customs/axios.customize";

const getPurchaseByIdService = async (purchaseId) => {
  const URL_BACKEND = `/purchases/${purchaseId}`;

  const response = await axios.get(URL_BACKEND);

  return response;
};

const getPurchasesService = async (page, limit) => {
  const URL_BACKEND = `/purchases?page=${page || 1}&limit=${limit || 10}`;

  const response = await axios.get(URL_BACKEND);

  return response;
};

export { getPurchaseByIdService, getPurchasesService };
