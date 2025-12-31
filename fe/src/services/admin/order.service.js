import axios from "../../customs/axios.customize";

const getAllOrders = async (currentPage, pageSize, filterStatus = null) => {
  let URL_BACKEND = `/purchases?limit=${pageSize}&page=${currentPage}`;

  if (filterStatus) {
    URL_BACKEND += `&status=${filterStatus}`;
  }

  const response = await axios.get(URL_BACKEND);

  return response;
};

const getOrderById = async (id) => {
  const response = await axios.get(`/purchases/${id}`);
  return response;
};

export { getAllOrders, getOrderById };
