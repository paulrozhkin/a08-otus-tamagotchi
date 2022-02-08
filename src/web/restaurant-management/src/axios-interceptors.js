import axios from "axios";
import store from "./store";
const {REACT_APP_API_URL} = process.env;

axios.defaults.baseURL = REACT_APP_API_URL;

axios.interceptors.request.use(function (config) {
  const token = store.getState().jwt.token
  config.headers.Authorization =  token ? `Bearer ${token}` : '';
  return config;
});

export default axios
