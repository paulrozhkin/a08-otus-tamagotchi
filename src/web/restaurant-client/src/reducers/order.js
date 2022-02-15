const initialOrder = {
    restaurant: null,
    menu: [],
    numberOfPersons: 0,
    visitTime: null,
    comment: null,
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
            let newMenu
            if (count > 0) {
                const menuItemList = menu.filter(item => item.menuItem.id === menuItem.id)
                if (menuItemList.length > 0) {
                    menuItemList[0].count = count
                    newMenu = [...menu]
                } else {
                    newMenu = [...menu, {menuItem, count}]
                }
            } else {
                newMenu = menu.filter(item => item.menuItem.id !== menuItem.id)
            }

            return {
                ...state,
                menu: newMenu,
                isMenuSet: newMenu.length > 0
            }
        case 'SET_CLIENT_INFO':
            return {
                ...state,
                numberOfPersons: rest.payload.numberOfPersons,
                visitTime: rest.payload.visitTime,
                comment: rest.payload.comment,
                isBookInfoSet: !!rest.payload
            }
        default:
            return state
    }
}

export default orderReducer
