import axios from "axios";


const {REACT_APP_API_URL} = process.env;

export const menuService = {
    getMenu
}

function getMenu(restaurantId) {
    return axios.get(`${REACT_APP_API_URL}/restaurants/${restaurantId}/menu?pageNumber=1&pageSize=2147483647`)
        .then(response => {
            return response.data.items
        })
}

