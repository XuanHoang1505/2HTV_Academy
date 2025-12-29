import axios from "../../customs/axios.customize";

const getAllCategoryService = async (
  currentPage,
  pageSize,
  searchText = ""
) => {
  let URL_BACKEND = `/categories?limit=${pageSize}&page=${currentPage}`;

  if (searchText && searchText.trim() != "") {
    URL_BACKEND += `&name=${searchText.trim()}`;
  }

  const response = await axios.get(URL_BACKEND);

  return response;
};

const createCategoryService = async (data) => {
  const URL_BACKEND = "/categories";

  const response = await axios.post(URL_BACKEND, data);
  return response;
};

const updateCategoryByIdService = async (id, data) => {
  const URL_BACKEND = `/categories`;

  const response = await axios.put(`${URL_BACKEND}/${id}`, data);

  return response;
};

const deleteCategoryByIdService = async (id) => {
  const URL_BACKEND = `/categories`;

  const response = await axios.delete(`${URL_BACKEND}/${id}`);

  return response;
};

export {
  getAllCategoryService,
  createCategoryService,
  updateCategoryByIdService,
  deleteCategoryByIdService,
};
