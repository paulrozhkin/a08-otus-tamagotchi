import axios from "axios";


const {REACT_APP_API_URL} = process.env;

export const restaurantsService = {
    getRestaurants,
    getRestaurant
}

function getRestaurants() {
    return axios.get(`${REACT_APP_API_URL}/restaurants?pageNumber=1&pageSize=2147483647`)
        .then(response => {
            return response.data.items
        })
}


function getRestaurant(id) {
    return axios.get(`${REACT_APP_API_URL}/restaurants/${id}`)
        .then(response => {
            return response.data
        })
}
