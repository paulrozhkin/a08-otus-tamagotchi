import axios from "axios";


const {REACT_APP_API_URL} = process.env;

export const dishesService = {
    getPopularDishes
}

function getPopularDishes() {
    return axios.get(`${REACT_APP_API_URL}/dishes?pageNumber=1&pageSize=10`)
        .then(response => {
            return response.data.items
        })
}

