import axios from "axios";
import store from "./store";
import {logout as storeLogout} from "./actions/logout";

const {REACT_APP_API_URL} = process.env;

console.log(REACT_APP_API_URL)
axios.defaults.baseURL = REACT_APP_API_URL;

axios.interceptors.request.use(function (config) {
    const token = store.getState().account.token
    config.headers.Authorization = token ? `Bearer ${token}` : '';
    return config;
});

axios.interceptors.response.use(function (response) {
    return response;
}, function (error) {
    const status = error.response.status || 500;
    if (status === 401) {
        store.dispatch(storeLogout())
    }

    return error
});

export default axios
