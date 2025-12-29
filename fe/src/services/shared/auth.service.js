import axios from "../../customs/axios.customize";

const getAccountService = async () => {
  const URL_BACKEND = "/auth/me";

  const response = await axios.get(URL_BACKEND);

  return response;
};

export { getAccountService };
