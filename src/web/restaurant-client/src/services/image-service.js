const {REACT_APP_API_URL} = process.env;

export const imageService = {
    getUrlById
}

function getUrlById(id) {
    return `${REACT_APP_API_URL}/resources/${id}`
}

