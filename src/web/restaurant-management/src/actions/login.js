export const login = (JWT) => {
  return {
        type: "LOGIN",
        payload: JWT
    }
}
