const jwtToken = localStorage.getItem('jwt');
const token = jwtToken && JSON.parse(jwtToken).token
const initialAccount = token ? createAccountFromJwtToken(token) : {}


const accountReducer = (state = initialAccount, {type, ...rest}) => {
    switch (type) {
        case 'LOGIN':
            const jwtToken = rest.payload
            localStorage.setItem('jwt', JSON.stringify(jwtToken));
            return createAccountFromJwtToken(jwtToken.token)
        case 'LOGOUT':
            localStorage.removeItem('jwt');
            return {}
        default:
            return state
    }
}

function getParsedJwt(token) {
    try {
        return JSON.parse(atob(token.split('.')[1]))
    } catch (error) {
        return undefined
    }
}

function createAccountFromJwtToken(token) {
    const account = getParsedJwt(token)
    return {
        id: account.nameid,
        username: account.unique_name,
        name: account.family_name,
        roles: account.role,
        expiration: account.exp,
        token
    }
}

export default accountReducer
