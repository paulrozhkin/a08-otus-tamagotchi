const jwt = localStorage.getItem('jwt');
const initialStateJwt = jwt ? JSON.parse(jwt) : {}

const jwtReducer = (state = initialStateJwt, {type, ...rest}) => {
  switch (type) {
    case 'LOGIN':
      const jwtToken = rest.payload
      localStorage.setItem('jwt', JSON.stringify(jwtToken));
      return jwtToken
    case 'LOGOUT':
      localStorage.removeItem('jwt');
      return {}
    default:
      return state
  }
}

export default jwtReducer
