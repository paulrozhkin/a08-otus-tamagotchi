export const selectRestaurant = (restaurant) => {
    return {
        type: "SELECT_RESTAURANT",
        payload: restaurant
    }
}
