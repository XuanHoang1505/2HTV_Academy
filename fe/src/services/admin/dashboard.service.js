import axios from "../../customs/axios.customize";

const getDashboardService = async (year) => {
  const response = await axios.get(`/dashboard?year=${year}`);
  return response;
};

export { getDashboardService };
