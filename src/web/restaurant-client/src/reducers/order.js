const initialOrder = {
    restaurant: null,
    menu: [],
    numberOfPersons: 0,
    visitTime: null,
    comment: null,
    email: null,
    isRestaurantSet: false,
    isMenuSet: false,
    isBookInfoSet: false
}

const orderReducer = (state = initialOrder, {type, ...rest}) => {
    switch (type) {
        case 'SELECT_RESTAURANT':
            const restaurant = rest.payload
            return {
                ...state,
                restaurant: restaurant,
                isRestaurantSet: !!restaurant
            }
        case 'UPDATE_MENU':
            const menuItem = rest.payload.menuItem
            const count = rest.payload.count
            const menu = state.menu
            let newMenu = menu.filter(item => item.menuItem.id !== menuItem.id)

            if (count > 0) {
                newMenu = [...newMenu, {menuItem, count}]
            }
            console.log(newMenu)
            return {
                ...state,
                menu: newMenu,
                isMenuSet: newMenu.length > 0
            }
        case 'SET_CLIENT_INFO':
            return {
                ...state,
                numberOfPersons: rest.payload.numberOfPersons,
                visitTime: rest.payload.numberOfPersons,
                comment: rest.payload.numberOfPersons,
                email: rest.payload.numberOfPersons,
                isBookInfoSet: !!rest.payload
            }
        default:
            return state
    }
}

export default orderReducer
