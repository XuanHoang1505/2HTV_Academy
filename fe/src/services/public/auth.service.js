import axios from "../../customs/axios.customize";

const loginService = async (userName, password) => {
  const URL_BACKEND = "/auth/login";
  const dataLogin = { userName, password };

  const response = await axios.post(URL_BACKEND, dataLogin);

  return response;
};

const registerService = async (email, fullName, password, confirmPassword) => {
  const URL_BACKEND = "/auth/register";
  const dataRegister = { email, fullName, password, confirmPassword };

  const response = await axios.post(URL_BACKEND, dataRegister);

  return response;
};

const verifyEmailService = async (otp, email) => {
  const URL_BACKEND = "/auth/verify-otp";
  const dataSend = { otp, email };

  const response = await axios.post(URL_BACKEND, dataSend);

  return response;
};

const resendVerificationCodeService = async (email) => {
  const URL_BACKEND = "/auth/resend-otp";
  const dataResend = { email };

  const response = await axios.post(URL_BACKEND, dataResend);

  return response;
};

export {
  loginService,
  registerService,
  verifyEmailService,
  resendVerificationCodeService,
};
