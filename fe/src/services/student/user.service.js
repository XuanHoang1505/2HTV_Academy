import axios from "../../customs/axios.customize";

const getProfileService = async () => {
  const URL_BACKEND = "/users/profile";

  const response = await axios.get(URL_BACKEND);

  return response;
};

const updateProfileService = async (formData) => {
  const URL_BACKEND = "/users/profile";

  const response = await axios.put(URL_BACKEND, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  return response;
};

export { getProfileService, updateProfileService };
