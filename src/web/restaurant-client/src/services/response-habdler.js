import {logout as storeLogout} from '../actions/logout'
import store from "../store";


export default function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                store.dispatch(storeLogout())
            }

            const error = {
                status: response.status,
                message:  (data && data.message) || response.statusText
            }
            return Promise.reject(error);
        }

        return data;
    });
}
