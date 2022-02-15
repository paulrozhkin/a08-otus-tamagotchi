import axios from "axios";
import store from "../store";


const {REACT_APP_API_URL} = process.env;

export const ordersService = {
    createOrder,
    getAccountOrders
}

function createOrder() {
    const orderInfo = store.getState().order
    const menuPositions = orderInfo.menu.map(x => ({id: x.menuItem.id, count: x.count}))
    const orderDto = {
        restaurantId: orderInfo.restaurant.id,
        menu: menuPositions,
        numberOfPersons: orderInfo.numberOfPersons,
        visitTime: orderInfo.visitTime,
        comment: orderInfo.comment,
    }

    return axios.post(`${REACT_APP_API_URL}/orders`, orderDto)
}

function getAccountOrders() {
    return axios.get(`${REACT_APP_API_URL}/orders/?pageNumber=1&pageSize=2147483647`)
        .then(response => {
            return response.data.items
        })
}
