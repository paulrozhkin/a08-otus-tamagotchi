import {combineReducers} from "redux";
import jwtReducer from "./jwt";
import sidebar from "./sidebar";

const reducers = combineReducers({
  jwt: jwtReducer,
  changeState: sidebar
});


export default reducers
