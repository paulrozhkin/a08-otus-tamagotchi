import store from "../store";
import {login as storeLogin} from '../actions/login'
import {logout as storeLogout} from '../actions/logout'


const {REACT_APP_API_URL} = process.env;

export const userService = {
  login,
  logout
}

function login(login, password) {
  const requestOptions = {
    method: 'POST',
    headers: {'Content-Type': 'application/json'},
    body: JSON.stringify({username: login, password})
  };

  return fetch(`${REACT_APP_API_URL}/authenticate`, requestOptions)
    .then(handleResponse)
    .then(jwt => {
      // store user details and jwt token in local storage to keep user logged in between page refreshes
      store.dispatch(storeLogin(jwt))
      return jwt;
    });
}

function logout() {
  // remove user from local storage to log user out
  //localStorage.removeItem('jwt');
  store.dispatch(storeLogout())
}

function handleResponse(response) {
  return response.text().then(text => {
    const data = text && JSON.parse(text);
    if (!response.ok) {
      if (response.status === 401) {
        // auto logout if 401 response returned from api
        logout();
      }

      const error = (data && data.message) || response.statusText;
      return Promise.reject(error);
    }

    return data;
  });
}
