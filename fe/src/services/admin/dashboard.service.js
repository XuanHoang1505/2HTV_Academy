import axios from "../../customs/axios.customize";

const getDashboardService = async () => {
  const response = await axios.get("/dashboard");
  return response;
};

export { getDashboardService };
