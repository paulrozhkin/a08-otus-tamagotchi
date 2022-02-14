export const updateMenu = (menuItem, count) => {
    return {
        type: "UPDATE_MENU",
        payload: {
            menuItem,
            count
        }
    }
}
